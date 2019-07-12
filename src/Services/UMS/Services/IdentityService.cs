using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using VDS.UMS.Common;
using VDS.UMS.Interfaces;
using VDS.UMS.Entities;
using VDS.UMS.Entities.ResponseModels;
using VDS.UMS.Settings;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using VDS.UMS.Logging;

namespace VDS.UMS.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly AppSettings _appSettings;
        private readonly LoggerAdapter<IdentityService> _logger;
        private readonly ClientHostName _clientHostName;

        public IdentityService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IMapper mapper,
            IEmailService emailService,
            IOptions<AppSettings> appSettings,
            LoggerAdapter<IdentityService> logger,
            IOptions<ClientHostName> clientHostName)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _appSettings = appSettings.Value ?? throw new ArgumentNullException(nameof(appSettings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _clientHostName = clientHostName.Value ?? throw new ArgumentNullException(nameof(appSettings));
        }

        public async Task<CreateUserResponse> Create(string firstName, string lastName, string email, string password, string hostname)
        {
            _logger.LogInformation("Create new User");

            ApplicationUser appUser = new ApplicationUser { Email = email, UserName = email, FirstName = firstName, LastName = lastName, JwtRole = JwtRole.User }; //  EmailConfirmed = true

            IdentityResult identityResult = await _userManager.CreateAsync(appUser, password);

            if (identityResult.Succeeded)
            {
                string confirmationToken = _userManager.GenerateEmailConfirmationTokenAsync(appUser).Result;

                string currentHost = $"https://{hostname}";

                string confirmUrl = $"https://{_clientHostName.Host}" + "emailconfirmation?userid={0}&code={1}";

                string callbackUrl = string.Format(confirmUrl, appUser.Id, confirmationToken);

                await _emailService.SendEmailAsync(email, "Confirm Email", callbackUrl);
            }

            CreateUserResponse result = _mapper.Map<CreateUserResponse>(identityResult);

            return result;
        }

        public async Task<bool> ConfirmEmail(string userId, string code)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                code = code.Replace(" ", "+");
                IdentityResult result = await _userManager.ConfirmEmailAsync(user, code);
                return result.Succeeded ? true : false;
            }

            return false;
        }

        public async Task<LoginResponse> Login(string userName, string password)
        {
            var identityResult = await _signInManager.PasswordSignInAsync(userName, password, false, lockoutOnFailure: false);

            if (identityResult.Succeeded)
            {
                var user = _userManager.Users.SingleOrDefault(x => x.UserName == userName);

                LoginResponse result = _mapper.Map<LoginResponse>(identityResult);

                result.Id = user?.Id;
                result.FirstName = user?.FirstName;
                result.LastName = user?.LastName;
                result.UserName = user?.UserName;
                result.JwtRole = user?.JwtRole;

                result.Token = GenerateJwtSecurityToken(user.Id, user.JwtRole);

                return result;
            }

            return _mapper.Map<LoginResponse>(identityResult);
        }

        private string GenerateJwtSecurityToken(string userId, string userRole)
        {
            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                        new Claim(ClaimTypes.Name, userId),
                        new Claim(ClaimTypes.Role, userRole)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

    }
}

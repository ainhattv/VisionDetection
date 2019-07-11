using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FVD.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FVD.Controllers
{
    [Authorize]
    public class BlobsController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IBlobService _blobService;

        public BlobsController(SignInManager<IdentityUser> signInManager,
                                 IBlobService blobService)
        {
            _signInManager = signInManager;
            _blobService = blobService;
        }

        public async Task<IActionResult> Index()
        {
            if (!ModelState.IsValid) return BadRequest();

            string userName = _signInManager.Context.User.Identity.Name;
            var user = await _signInManager.UserManager.FindByEmailAsync(userName);

            var result = await _blobService.GetBlobs(user.Id);

            return View(result);
        }


        [HttpPost]
        public async Task<IActionResult> UploadFiles(List<IFormFile> files)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();

                string userName = _signInManager.Context.User.Identity.Name;
                var user = await _signInManager.UserManager.FindByEmailAsync(userName);

                // Check valid User
                if (user == null) return BadRequest();

                await _blobService.UpLoadBlobs(Guid.Parse(user.Id), files);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return BadRequest();
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> IdentityFace(IFormFile file)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();

                return Ok(await _blobService.IdentityFace(file));
            }
            catch (Exception ex)
            {
                return BadRequest();
                throw ex;
            }
        }
    }
}
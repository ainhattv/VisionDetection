using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VDS.WPS.Controllers;
using VDS.WPS.Data;
using VDS.WPS.Interfaces;
using VDS.WPS.Logging;
using AutoMapper;
using VDS.WPS.Data.Entities;
using VDS.WPS.Models.Response;
using System.Linq;
using System.Collections.Generic;
using VDS.WPS.Models.Request;

namespace VDS.WPS.Services
{
    public class WorkPlaceService : IWorkPlaceService
    {
        private readonly LoggerAdapter<WorkPlaceService> _logger;
        private readonly WPSContext _context;
        private readonly IMapper _mapper;

        public WorkPlaceService(
            LoggerAdapter<WorkPlaceService> logger,
            WPSContext context,
            IMapper mapper
        )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<WorkPlaceResponseModel> GetWorkPlaceByUserId(Guid userId)
        {
            WorkPlace workPlace = await _context.WorkPlaces.SingleOrDefaultAsync(x => x.AuthorId == userId);
            return _mapper.Map<WorkPlaceResponseModel>(workPlace);
        }

        public async Task<IEnumerable<WorkPlaceResponseModel>> GetWorkPlaces()
        {
            IEnumerable<WorkPlace> workPlaces = await _context.WorkPlaces.ToListAsync();
            return _mapper.Map<IEnumerable<WorkPlaceResponseModel>>(workPlaces);
        }

        public async Task CreateWorkPlace(WorkPlaceRequestModel workPlace)
        {
            bool valid = await _context.WorkPlaces.AnyAsync(x => x.AuthorId == workPlace.AuthorId || x.AuthorEmail == workPlace.AuthorEmail);
            if (valid) throw new ArgumentException(nameof(workPlace));

            WorkPlace model = new WorkPlace()
            {
                Id = Guid.NewGuid(),
                CreatedDate = DateTime.UtcNow,
                Name = workPlace.Name,
                AuthorId = workPlace.AuthorId,
                AuthorEmail = workPlace.AuthorEmail,
                AuthorName = workPlace.AuthorName,
                IsActive = true
            };

            await _context.AddAsync(model);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateWorkPlace(Guid id, WorkPlaceRequestModel workPlace)
        {
            var validWP = _context.WorkPlaces.Where(x => x.Id == id);
            if (!await validWP.AnyAsync()) throw new ArgumentException(nameof(workPlace));

            WorkPlace wp = await validWP.SingleOrDefaultAsync();

            wp.Name = workPlace.Name;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteWorkPlace(Guid id)
        {
            var validWP = _context.WorkPlaces.Where(x => x.Id == id);
            if (!await validWP.AnyAsync()) throw new ArgumentException(nameof(id));

            WorkPlace wp = await validWP.SingleOrDefaultAsync();
            _context.WorkPlaces.Remove(wp);
            await _context.SaveChangesAsync();
        }
    }
}
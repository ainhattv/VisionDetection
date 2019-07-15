using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using VDS.BlobService.Adapters;
using VDS.BlobService.Data;
using VDS.BlobService.Data.Entities;
using VDS.BlobService.Interfaces;

namespace VDS.BlobService.Services
{
    public class ContainerService : IContainerService
    {
        private readonly IBlobAdapter _blobAdapter;
        private readonly BlobContext _context;
        public ContainerService(
            IBlobAdapter blobAdapter,
            BlobContext context)
        {
            _blobAdapter = blobAdapter ?? throw new System.ArgumentNullException(nameof(blobAdapter));
            _context = context ?? throw new System.ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Uri>> GetBlobs(Guid wpId, Guid userId)
        {
            IQueryable<BlobContainer> wpQR = _context.BlobContainers.Where(x => x.WorkPlaceId == wpId);
            if (!wpQR.Any()) throw new ArgumentException(nameof(wpId));

            IQueryable<BlobFolder> folderQR = _context.BlobFolders.Where(x => x.UserId == userId);
            if (!folderQR.Any()) throw new ArgumentException(nameof(userId));

            BlobContainer container = await wpQR.SingleOrDefaultAsync();
            BlobFolder folder = await folderQR.SingleOrDefaultAsync();

            return await _blobAdapter.GetBlobs(container.Id, folder.Id);
        }

        public async Task CreateContainer(Guid wpId)
        {
            Guid containerId = await _blobAdapter.CreateContainer();
            BlobContainer container = new BlobContainer()
            {
                Id = containerId,
                WorkPlaceId = wpId,
            };

            await _context.BlobContainers.AddAsync(container);
        }

        public async Task DeleteContainer(Guid wpId)
        {
            IQueryable<BlobContainer> qr = _context.BlobContainers.Where(x => x.WorkPlaceId == wpId);

            if (!qr.Any()) throw new ArgumentException(nameof(wpId));

            BlobContainer container = qr.SingleOrDefault();

            _context.BlobContainers.Remove(container);
            await _context.SaveChangesAsync();
        }

        public async Task<Uri> UploadContainerBlob(Guid wpId, Guid userId, IFormFile file)
        {
            BlobFolder folder = await CreateIfNotExist(wpId, userId);

            Uri result = await _blobAdapter.UploadContainerBlob(folder.BlobContainerId, folder.Id, file);

            return result;
        }

        public async Task DeleteBlob(Guid containerId, string blobPath)
        {
            await _blobAdapter.DeleteBlob(containerId, blobPath);
        }

        private async Task<BlobFolder> CreateIfNotExist(Guid wpId, Guid userId)
        {
            BlobContainer container = await _context.BlobContainers
                                                    .Include(x => x.BlobFolders)
                                                    .SingleOrDefaultAsync(x => x.WorkPlaceId == wpId);

            if (container == null) throw new ArgumentException(nameof(wpId));

            BlobFolder blobFolder = container.BlobFolders.SingleOrDefault(x => x.UserId == userId);

            if (blobFolder == null)
            {
                BlobFolder folder = new BlobFolder()
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                };

                await _context.BlobFolders.AddAsync(folder);
                container.BlobFolders.Add(folder);

                await _context.SaveChangesAsync();
                return folder;
            }
            else
            {
                return blobFolder;
            }
        }
    }
}
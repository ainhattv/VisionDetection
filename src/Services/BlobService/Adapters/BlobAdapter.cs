using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Options;
using VDS.BlobService.Settings;

namespace VDS.BlobService.Adapters
{
    public class BlobAdapter : IBlobAdapter
    {
        private readonly IOptions<BlobSettings> _blobSettings;
        protected CloudStorageAccount _storageAccount;
        protected CloudBlobClient _cloudBlobClient;

        public BlobAdapter(
            IOptions<BlobSettings> blobSettings
        )
        {
            _blobSettings = blobSettings ?? throw new ArgumentNullException(nameof(blobSettings));

            InitialBlobStorage();
        }

        public async Task<IEnumerable<Uri>> GetBlobs(Guid containerId, Guid folderId)
        {
            CloudBlobContainer container = _cloudBlobClient.GetContainerReference(containerId.ToString());

            if (!container.Exists()) throw new Exception($"Container not exist containerId: {containerId}");

            var resultSegment = await container.GetDirectoryReference(folderId.ToString()).ListBlobsSegmentedAsync(null);

            IList<Uri> uris = new List<Uri>();
            foreach (var item in resultSegment.Results)
            {
                uris.Add(item.Uri);
            }

            return uris;
        }

        public async Task<Guid> CreateContainer()
        {
            Guid containerId = Guid.NewGuid();
            CloudBlobContainer container = _cloudBlobClient.GetContainerReference(containerId.ToString());

            await container.CreateIfNotExistsAsync();

            return containerId;
        }

        public async Task CreateContainer(Guid containerId)
        {
            CloudBlobContainer container = _cloudBlobClient.GetContainerReference(containerId.ToString());

            await container.CreateIfNotExistsAsync();
        }

        public async Task DeleteContainer(Guid containerId)
        {
            CloudBlobContainer container = _cloudBlobClient.GetContainerReference(containerId.ToString());

            await container.DeleteIfExistsAsync();
        }

        public async Task<Uri> UploadContainerBlob(Guid containerId, Guid folderId, IFormFile file)
        {
            Stream stream = file.OpenReadStream();
            string filePath = Path.Combine(folderId.ToString(), Guid.NewGuid().ToString() + Path.GetExtension(file.FileName));

            CloudBlobContainer container = _cloudBlobClient.GetContainerReference(containerId.ToString());

            if (!container.Exists()) throw new Exception($"Container not exist containerId: {containerId}");

            // Set the permissions so the blobs are public.
            BlobContainerPermissions permissions = new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            };
            await container.SetPermissionsAsync(permissions);

            // UpLoad to Blob
            CloudBlockBlob cloudBlockBlob = container.GetBlockBlobReference(filePath);
            await cloudBlockBlob.UploadFromStreamAsync(stream);

            return cloudBlockBlob.Uri;
        }

        public async Task DeleteBlob(Guid containerId, string blobPath)
        {
            CloudBlobContainer container = _cloudBlobClient.GetContainerReference(containerId.ToString());

            if (!container.Exists()) throw new Exception($"Container not exist containerId: {containerId}");

            ICloudBlob blob = await container.GetBlobReferenceFromServerAsync(blobPath);

            await blob.DeleteIfExistsAsync();
        }

        #region Private Functions
        private async Task<string> SaveLocalFile(IFormFile file)
        {
            // Create a file in your local MyDocuments folder to upload to a blob.
            string localPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string localFileName = Guid.NewGuid().ToString();
            string sourceFile = Path.Combine(localPath, localFileName);

            if (file.Length > 0)
            {
                using (var stream = new FileStream(sourceFile, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }

            return sourceFile;
        }
        #endregion

        #region InitialBlob
        private void InitialBlobStorage()
        {
            InitialBlobAccount();
            InitialBlobClient();
        }

        private void InitialBlobAccount()
        {
            // Get Connection string
            string blobConn = _blobSettings.Value.BlobConnection;

            if (CloudStorageAccount.TryParse(blobConn, out _storageAccount))
            {
                // If the connection string is valid, proceed with operations against Blob
                // storage here.
                // ADD OTHER OPERATIONS HERE
            }
            else
            {
                throw new Exception("InitialBlobAccount failed!");
            }
        }

        private void InitialBlobClient()
        {
            _cloudBlobClient = _storageAccount.CreateCloudBlobClient();

            if (_cloudBlobClient == null) throw new Exception("InitialBlobClient failed!");
        }
        #endregion
    }
}
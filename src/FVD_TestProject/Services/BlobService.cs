using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FVD.Interfaces;
using FVD.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Options;

namespace FVD.Services
{
    public class BlobService : IBlobService
    {
        private readonly IOptions<BlobSettings> _blobSettings;
        private CloudStorageAccount _storageAccount;
        private CloudBlobClient _cloudBlobClient;
        private CloudBlobContainer _cloudBlobContainer;

        private readonly IVisionService _visionService;

        public BlobService(IOptions<BlobSettings> blobSettings,
                            SignInManager<IdentityUser> signInManager,
                            IVisionService visionService)
        {
            _blobSettings = blobSettings;
            _visionService = visionService;

            // Init BlobStorage
            InitialBlobStorage();
        }

        private void InitialBlobStorage()
        {
            InitialBlobAccount();
            InitialBlobClient();
            InitialBlobContainer();
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
                throw new System.Exception("InitialBlobAccount failed!");
            }
        }

        private void InitialBlobClient()
        {
            _cloudBlobClient = _storageAccount.CreateCloudBlobClient();

            if (_cloudBlobClient == null) throw new System.Exception("InitialBlobClient failed!");
        }

        private void InitialBlobContainer()
        {
            string containerId = _blobSettings.Value.ContainerId;

            _cloudBlobContainer = _cloudBlobClient.GetContainerReference(containerId);

            if (_cloudBlobContainer == null) throw new System.Exception("InitialBlobContainer failed!");

            // Set the permissions so the blobs are public.
            BlobContainerPermissions permissions = new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            };
            _cloudBlobContainer.SetPermissionsAsync(permissions);
        }

        public async Task<IEnumerable<string>> GetBlobs(string userid)
        {
            BlobResultSegment blobs = await _cloudBlobContainer.GetDirectoryReference(userid).ListBlobsSegmentedAsync(null);

            IList<string> strings = new List<string>();

            foreach (var blob in blobs.Results)
            {
                strings.Add(blob.StorageUri.PrimaryUri.ToString());
            }

            return strings;
        }

        public async Task<bool> UpLoadBlobs(Guid userId, List<IFormFile> files)
        {
            try
            {
                foreach (IFormFile file in files)
                {
                    await UpLoadBlob(userId, file);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }

        public async Task UpLoadBlob(Guid userId, IFormFile file)
        {
            // save local file
            string sourceFile = await SaveLocalFile(file);

            // Blob Path
            string fullPath = Path.Combine(userId.ToString(), Path.GetFileName(sourceFile) + ".jpg");

            // UpLoad to Blob
            CloudBlockBlob cloudBlockBlob = _cloudBlobContainer.GetBlockBlobReference(fullPath);
            await cloudBlockBlob.UploadFromFileAsync(sourceFile);
            await _visionService.AddPersonFaceByUserId(userId, sourceFile);

            File.Delete(sourceFile);
        }

        public async Task<IList<IdentifyResult>> IdentityFace(IFormFile file)
        {
            // save local file
            string sourceFile = await SaveLocalFile(file);

            return await _visionService.FaceIdentity(sourceFile);
        }

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

    }
}
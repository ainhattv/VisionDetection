using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FVD.Data;
using FVD.Interfaces;
using FVD.Settings;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Drawing;
using System.Linq;

namespace FVD.Services
{
    public class VisionService : IVisionService
    {
        private readonly IOptions<VisionSettings> _visionSettings;
        private FaceClient _faceClient;
        private readonly VisionDbContext _visionDbContext;

        public VisionService(IOptions<VisionSettings> visionSettings,
                            VisionDbContext visionDbContext)
        {
            _visionSettings = visionSettings;
            _visionDbContext = visionDbContext;

            InitialVision();
        }

        #region InitialVision
        private async void InitialVision()
        {
            string subscriptionKey = _visionSettings.Value.SubscriptionKey ?? throw new System.Exception("Cannot get settings");
            string endPoint = _visionSettings.Value.Endpoint ?? throw new System.Exception("Cannot get settings");

            _faceClient = new FaceClient(
                   new ApiKeyServiceClientCredentials(_visionSettings.Value.SubscriptionKey),
                   new System.Net.Http.DelegatingHandler[] { });

            _faceClient.Endpoint = _visionSettings.Value.Endpoint;

            await CreatePersonGroupIfNotExist(_visionSettings.Value.GroupId, _visionSettings.Value.GroupName);
        }

        private async Task CreatePersonGroupIfNotExist(string groupId, string groupName)
        {

            PersonGroup group = null;

            try
            {
                group = await _faceClient.PersonGroup.GetAsync(groupId);
            }
            catch
            {

            }

            // Check before Create
            if (group == null)
            {
                await _faceClient.PersonGroup.CreateAsync(groupId, groupName);
            }
        }
        #endregion

        #region Public service functions
        public async Task<bool> AddPersonFaceByUserId(Guid userId, string filePath)
        {
            try
            {
                string personGroupId = _visionSettings.Value.GroupId ?? throw new Exception("Null vision settings");

                FVD.Data.Entities.Person appPerson = await CreatePersonIfNotExist(personGroupId, userId, "None");

                if (appPerson != null)
                {
                    await AddPersonFace(personGroupId, appPerson.Id, filePath);
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch
            {
                return false;
            }

        }

        public async Task TrainExecute()
        {
            await _faceClient.PersonGroup.TrainAsync(_visionSettings.Value.GroupId);

            TrainingStatus status;

            do
            {
                status = await _faceClient.PersonGroup.GetTrainingStatusAsync(_visionSettings.Value.GroupId);

                await Task.Delay(3000);
            } while (status != null && (int)status.Status > 1);

        }

        public async Task<IList<IdentifyResult>> FaceIdentity(string filePath)
        {
            using (Stream s = File.OpenRead(filePath))
            {
                var faces = await _faceClient.Face.DetectWithStreamAsync(s);
                var faceIds = faces.Select(face => Guid.Parse(face.FaceId.ToString())).ToList();

                return await _faceClient.Face.IdentifyAsync(faceIds, _visionSettings.Value.GroupId);
            }
        }
        #endregion

        #region Private Functions



        // Uploads the image file and calls DetectWithStreamAsync.
        private async Task<IList<DetectedFace>> UploadAndDetectFaces(string imageFilePath)
        {
            // The list of Face attributes to return.
            IList<FaceAttributeType> faceAttributes =
                new FaceAttributeType[]
                {
                    FaceAttributeType.Gender, FaceAttributeType.Age,
                    FaceAttributeType.Smile, FaceAttributeType.Emotion,
                    FaceAttributeType.Glasses, FaceAttributeType.Hair
                };

            // Call the Face API.
            try
            {
                using (Stream imageFileStream = File.OpenRead(imageFilePath))
                {
                    // The second argument specifies to return the faceId, while
                    // the third argument specifies not to return face landmarks.
                    IList<DetectedFace> faceList =
                        await _faceClient.Face.DetectWithStreamAsync(
                            imageFileStream, true, false, faceAttributes);
                    return faceList;
                }
            }
            // Catch and display Face API errors.
            catch (APIErrorException f)
            {
                return new List<DetectedFace>();
                throw f;
            }
            // Catch and display all other errors.
            catch (Exception e)
            {
                return new List<DetectedFace>();
                throw e;
            }
        }

        private async Task<FVD.Data.Entities.Person> CreatePersonIfNotExist(string personGroupId, Guid userId, string personName)
        {
            FVD.Data.Entities.Person appPerson = await _visionDbContext.Persons.SingleOrDefaultAsync(x => x.UserId == userId);

            if (appPerson == null)
            {
                Person person = await _faceClient.PersonGroupPerson.CreateAsync(personGroupId, personName);

                FVD.Data.Entities.Person newPerson = new Data.Entities.Person()
                {
                    Id = person.PersonId,
                    Name = person.Name
                };

                await _visionDbContext.Persons.AddAsync(newPerson);

                await _visionDbContext.SaveChangesAsync();
                return newPerson;
            }
            else
            {
                return appPerson;
            }
        }

        private async Task AddPersonFace(string personGroupId, Guid personId, string filePath)
        {
            IList<DetectedFace> faceList = await UploadAndDetectFaces(filePath);

            if (faceList.Count > 0)
            {
                Rectangle section = new Rectangle(new Point(faceList[0].FaceRectangle.Left, faceList[0].FaceRectangle.Top),
                                                    new Size(faceList[0].FaceRectangle.Width, faceList[0].FaceRectangle.Height));

                // Get image as Bitmap  
                Bitmap src = Image.FromFile(filePath) as Bitmap;

                // Crop Image
                Bitmap image = CropImage(src, section);

                image.Save(filePath + ".jpg");
            }

            using (Stream stream = File.OpenRead(filePath + ".jpg"))
            {
                await _faceClient.PersonGroupPerson.AddFaceFromStreamAsync(personGroupId, personId, stream);
            }
        }

        private Bitmap CropImage(Bitmap source, Rectangle section)
        {
            // An empty bitmap which will hold the cropped image
            Bitmap bmp = new Bitmap(section.Width, section.Height);

            Graphics g = Graphics.FromImage(bmp);

            // Draw the given area (section) of the source image
            // at location 0,0 on the empty bitmap (bmp)
            g.DrawImage(source, 0, 0, section, GraphicsUnit.Pixel);

            return bmp;
        }


        #endregion
    }
}
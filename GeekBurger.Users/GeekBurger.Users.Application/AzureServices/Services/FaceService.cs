using GeekBurger.Users.Core.Configs;
using GeekBurger.Users.Core.Domains;
using Microsoft.ProjectOxford.Face;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.Users.Application.AzureServices.Services
{
    public class FaceService : IFaceService
    {
        private readonly FaceServiceClient _faceServiceClient;

        public FaceService()
        {
            _faceServiceClient = new FaceServiceClient(ConfigurationManager.Configuration["azureConfig:key1"], ConfigurationManager.Configuration["azureConfig:endpoint"]);
        }

        public async Task<User> DetectFaceAsync(string face)
        {
            var user = new User
            {
                FaceBase64 = face,
                InProcessing = false
            };

            try
            {
                using (var streamFace = new MemoryStream(Convert.FromBase64String(face)))
                {
                    var faceResult = await _faceServiceClient.DetectAsync(streamFace);
                    var detectedFace = faceResult.FirstOrDefault();
                    

                    user.AzureGuid = detectedFace.FaceId.ToString();
                    user.InProcessing = true;

                    var facePersisted = await _faceServiceClient.AddFaceToFaceListAsync(ConfigurationManager.Configuration["azureConfig:faceListId"], streamFace);

                    
                    //salvar usuário

                    streamFace.Flush();
                    streamFace.Close();
                    return user;
                }
            }
            catch (Exception ex)
            {
                //log

                return user;
            }
            
        }


        
    }
}

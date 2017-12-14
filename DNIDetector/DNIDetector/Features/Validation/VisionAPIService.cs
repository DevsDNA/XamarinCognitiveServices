[assembly: Xamarin.Forms.Dependency(typeof(DNIDetector.Features.Validation.VisionAPIService))]
namespace DNIDetector.Features.Validation
{
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Net.Http;
	using System.Net.Http.Headers;
	using System.Threading.Tasks;
	using DNIDetector.Models;
	using Newtonsoft.Json;
	using Plugin.Media.Abstractions;

	public class VisionAPIService : IVisionAPIService
	{
		public async Task<string> UploadPhotoAndGetIdAsync(MediaFile file)
		{
			HttpClient client = new HttpClient();
			client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "<YOURAPIKEY>");
			string requestParameters = "returnFaceId=true&returnFaceLandmarks=false&returnFaceAttributes=age,gender";
			string uri = $"https://westcentralus.api.cognitive.microsoft.com/face/v1.0/detect?{requestParameters}";

			BinaryReader binaryReader = new BinaryReader(file.GetStream());
			byte[] imgData = binaryReader.ReadBytes((int)file.GetStream().Length);

			using (ByteArrayContent content = new ByteArrayContent(imgData))
			{
				content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
				var response = await client.PostAsync(uri, content);

				string contentString = await response.Content.ReadAsStringAsync();

				var responseResult = JsonConvert.DeserializeObject<List<FaceModel>>(contentString).FirstOrDefault();

				if (responseResult != null)
					return responseResult.FaceId;
				else
				{
					await App.Current.MainPage.DisplayAlert("Ops...", "no se ha identificado ninguna cara...", "ok");
					return string.Empty;
				}
			}
		}

		public async Task<VerificationResultModel> VerifyPhotosAsync(string photoIdOne, string photoIdTwo)
		{
			HttpClient client = new HttpClient();
			client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "<YOURAPIKEY>");
			string uri = $"https://westcentralus.api.cognitive.microsoft.com/face/v1.0/verify";

			string strContent = $"{{\"faceId1\":\"{photoIdOne}\", \"faceId2\":\"{photoIdTwo}\"}}";

			using (StringContent content = new StringContent(strContent))
			{
				content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
				var response = await client.PostAsync(uri, content);

				string contentString = await response.Content.ReadAsStringAsync();

				var result = JsonConvert.DeserializeObject<VerificationResultModel>(contentString);

				return result;
			}
		}
	}
}

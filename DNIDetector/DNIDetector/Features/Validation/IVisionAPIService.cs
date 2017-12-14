namespace DNIDetector.Features.Validation
{
	using DNIDetector.Models;
	using Plugin.Media.Abstractions;
	using System.Threading.Tasks;

	public interface IVisionAPIService
    {
		Task<string> UploadPhotoAndGetIdAsync(MediaFile file);

		Task<VerificationResultModel> VerifyPhotosAsync(string photoIdOne, string photoIdTwo);
	}
}

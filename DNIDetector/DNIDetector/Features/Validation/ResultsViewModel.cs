namespace DNIDetector.Features.Validation
{
	using DNIDetector.Models;
	using System.Threading.Tasks;
	using Xamarin.Forms;

	public class ResultsViewModel : BindObject
    {
		private readonly IVisionAPIService visionService;

		private string dniPhotoId;
		private string facePhotoId;
		private VerificationResultModel result;

		public ResultsViewModel()
		{
			visionService = DependencyService.Get<IVisionAPIService>();
		}

		public string DniPhotoId
		{
			get => dniPhotoId;
			set => RaiseAndSetProperty(ref dniPhotoId, value);
		}

		public string FacePhotoId
		{
			get => facePhotoId;
			set => RaiseAndSetProperty(ref facePhotoId, value);
		}

		public VerificationResultModel Result
		{
			get => result;
			set => RaiseAndSetProperty(ref result, value);
		}

		public async Task GetResults()
		{
			Result = await visionService.VerifyPhotosAsync(dniPhotoId, FacePhotoId);
		}
    }
}

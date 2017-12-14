namespace DNIDetector.Features.Validation
{
	using DNIDetector.Models;
	using Plugin.Media;
	using Plugin.Media.Abstractions;
	using System.Threading.Tasks;
	using Xamarin.Forms;

	public class MainViewModel : BindObject
    {
		private readonly IVisionAPIService visionService;

		private MediaFile dniFile;
		private MediaFile faceFile;
		private string dniPhotoId;
		private string facePhotoId;

		private bool isUploadingDniPhoto = false;
		private bool isUploadingFacePhoto = false;

		private Command getDniPhotoCommand;
		private Command getFacePhotoCommand;
		private Command getResultsCommand;

		public MainViewModel()
		{
			visionService = DependencyService.Get<IVisionAPIService>();
			getDniPhotoCommand = new Command(async () => await GetDniPhotoExecuteAsync());
			getFacePhotoCommand = new Command(async () => await GetFacePhotoExecuteAsync());
			getResultsCommand = new Command(async () => await GetResultsExecuteAsync());
		}

		public ImageSource DniImage
		{
			get => ImageSource.FromStream(() => dniFile.GetStream());
		}

		public ImageSource FaceImage
		{
			get => ImageSource.FromStream(() => faceFile.GetStream());
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

		public bool IsUploadingDniPhoto
		{
			get => isUploadingDniPhoto;
			set => RaiseAndSetProperty(ref isUploadingDniPhoto, value);
		}

		public bool IsUploadingFacePhoto
		{
			get => isUploadingFacePhoto;
			set => RaiseAndSetProperty(ref isUploadingFacePhoto, value);
		}

		public Command GetDniPhotoCommand
		{
			get => getDniPhotoCommand;
		}

		public Command GetFacePhotoCommand
		{
			get => getFacePhotoCommand;
		}

		public Command GetResultsCommand
		{
			get => getResultsCommand;
		}

		private async Task GetDniPhotoExecuteAsync()
		{
			IsUploadingDniPhoto = true;
			dniFile = await TakePhotoAsync();
			RaiseProperty("DniImage");
			DniPhotoId = await visionService.UploadPhotoAndGetIdAsync(dniFile);
			IsUploadingDniPhoto = false;
		}

		private async Task GetFacePhotoExecuteAsync()
		{
			IsUploadingFacePhoto = true;
			faceFile = await TakePhotoAsync(CameraDevice.Front);
			RaiseProperty("FaceImage");
			FacePhotoId = await visionService.UploadPhotoAndGetIdAsync(faceFile);
			IsUploadingFacePhoto = false;
		}

		private async Task GetResultsExecuteAsync()
		{
			ResultsView view = new ResultsView();
			(view.BindingContext as ResultsViewModel).DniPhotoId = DniPhotoId;
			(view.BindingContext as ResultsViewModel).FacePhotoId = FacePhotoId;
			await (App.Current.MainPage as NavigationPage).PushAsync(view, true);
		}

		private async Task<MediaFile> TakePhotoAsync(CameraDevice camera = CameraDevice.Rear)
		{
			await CrossMedia.Current.Initialize();

			if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
			{
				await App.Current.MainPage.DisplayAlert("Sin camara", ":( No hay camaras disponibles", "OK");
				return null;
			}

			var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions()
			{
				Directory = "Sample",
				Name = "test.jpg",
				DefaultCamera = camera
			});

			return file;
		}
	}
}

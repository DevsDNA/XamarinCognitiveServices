namespace DNIDetector.Models
{
	using System.ComponentModel;
	using System.Runtime.CompilerServices;
	using Xamarin.Forms;

	public class BindObject : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected void RaiseAndSetProperty<T>(ref T storage, T value, [CallerMemberName]string propertyName = "")
		{
			storage = value;
			RaiseProperty(propertyName);
		}

		protected void RaiseProperty([CallerMemberName]string propertyName = "")
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			});
			
		}
	}
}

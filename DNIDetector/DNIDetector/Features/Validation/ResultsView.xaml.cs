using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DNIDetector.Features.Validation
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ResultsView : ContentPage
	{
		public ResultsView ()
		{
			InitializeComponent ();
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();

			await (BindingContext as ResultsViewModel).GetResults();
		}
	}
}
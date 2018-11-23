using LoggerTest.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LoggerTest.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LogView : ContentView
	{
		public LogView ()
		{
			InitializeComponent ();

            BindingContext = new LogViewModel();
		}
	}
}
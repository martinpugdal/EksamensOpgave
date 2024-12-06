using GUI_Admin.ViewModels;

namespace GUI_Admin.Pages
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();

            BindingContext = new MainViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindingContext = new MainViewModel();
        }
    }
}
using U3ActRegistroDeActividadesMaui.Views;

namespace U3ActRegistroDeActividadesMaui
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var splashPage = new U3ActRegistroDeActividadesMaui.Views.Splashpage();


            MainPage = new ContentPage
            {
                Content = splashPage
            };
        }

        //buscando otra manera
        protected override void OnStart()
        {
            Device.StartTimer(TimeSpan.FromSeconds(3), () =>
            {
                MainPage = new NavigationPage(new AgregarDepView());
                return false;
            });
        }
    }
}

using U3ActRegistroDeActividadesMaui.Services;
using U3ActRegistroDeActividadesMaui.Views;

namespace U3ActRegistroDeActividadesMaui
{
    public partial class App : Application
    {
        public static ActividadesService ActividadesService { get; set; } = new();

        public App()
        {
            InitializeComponent();

            var splashPage = new U3ActRegistroDeActividadesMaui.Views.Splashpage();


            MainPage = new ContentPage
            {
                Content = splashPage
            };
        }

        async void Sincronizador()
        {
            while (true)
            {
                await ActividadesService.GetActividades();
                Thread.Sleep(TimeSpan.FromSeconds(15));
            }
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

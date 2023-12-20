using Xamarin.Forms;
namespace IoTConfigurator
{
    public partial class App : Application
    {
        public App ()
        {
            InitializeComponent();
            XF.Material.Forms.Material.Init(this);
            MainPage = new NavigationPage(new HomePage());
        }

        protected override void OnStart ()
        {
        }

        protected override void OnSleep ()
        {
        }

        protected override void OnResume ()
        {
        }

    }
}


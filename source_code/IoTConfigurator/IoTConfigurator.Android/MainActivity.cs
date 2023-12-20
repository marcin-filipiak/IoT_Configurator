using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using AndroidX.Core.Content;
using Android;
using Android.Support.V4.App;
using Xamarin.Essentials;
using Android.Bluetooth;
using Android.Content;
using Android.Widget;
using Xamarin.Forms;
using ColorExtensions = Xamarin.Forms.Platform.Android.ColorExtensions;

namespace IoTConfigurator.Droid
{
    [Activity(Label = "IoT Configurator", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private readonly int REQUEST_BLUETOOTH = 123;
        private readonly int REQUEST_ENABLE_BT = 124;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetStatusBarColor(ColorExtensions.ToAndroid(Color.FromHex("#5C4798")));
            Platform.Init(this, savedInstanceState);
            Forms.Init(this, savedInstanceState);
            XF.Material.Droid.Material.Init(this, savedInstanceState);
            RequestBluetoothPermission();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            if (requestCode == REQUEST_BLUETOOTH)
            {
                if (grantResults[0] == Permission.Granted)
                {
                    if (!IsBluetoothEnabled())
                    {
                        var enableBtIntent = new Intent(BluetoothAdapter.ActionRequestEnable);
                        StartActivityForResult(enableBtIntent, REQUEST_ENABLE_BT);
                    }
                    else
                    {
                        LoadApplication(new App());
                    }
                }
                else
                {
                    AppInfo.ShowSettingsUI();

                    ShowException("App cannot work without Bluetooth permissions");
                }
            }

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode != REQUEST_ENABLE_BT) return;
            if (resultCode == Result.Ok)
            {
                LoadApplication(new App());
            }
            else
            {
                ShowException("App cannot work without Bluetooth off");
            }
        }
        void ShowException(string ExceptionText)
        {
            Toast.MakeText(this, ExceptionText, ToastLength.Short).Show();

            FinishAffinity();
        }

        private bool IsBluetoothEnabled()
        {
            var adapter = BluetoothAdapter.DefaultAdapter;
            if (adapter == null) return false;
            return adapter.IsEnabled;
        }

        void RequestBluetoothPermission()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.S)
            {
                const string permission = Manifest.Permission.BluetoothConnect;
                if (ContextCompat.CheckSelfPermission(this, permission) == (int)Permission.Granted)
                {
                    ProceedWithBluetoothConnection();
                }
                else
                {
                    var requiredPermissions = new string[] { Manifest.Permission.BluetoothConnect };
                    ActivityCompat.RequestPermissions(this, requiredPermissions, REQUEST_BLUETOOTH);
                }
            }
            else
            {
                ProceedWithBluetoothConnection();
            }
        }

        void ProceedWithBluetoothConnection()
        {
            if (!IsBluetoothEnabled())
            {
                var enableBtIntent = new Intent(BluetoothAdapter.ActionRequestEnable);
                StartActivityForResult(enableBtIntent, REQUEST_ENABLE_BT);
            }
            else
            {
                LoadApplication(new App());
            }
        }
    }
}

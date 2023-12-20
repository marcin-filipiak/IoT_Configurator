using System;
using System.Collections.Generic;
using System.Linq;
using Android.Bluetooth;
using IoTConfigurator.Models;
using ESP32FormGenerator.Services;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace IoTConfigurator
{
    public partial class HomePage : ContentPage
    {
        private ICollection<BluetoothDevice> devices;
        public HomePage()
        {
            InitializeComponent();
            SetPicker(BluetoothService.GetBondedDevices());
            BindingContext = new LoadingModel(false);
        }

        public void SetPicker(ICollection<BluetoothDevice> devices)
        {
            var resultList = new List<string>();
            this.devices = devices;
            foreach (var item in devices)
            {
                resultList.Add(item.Name);
            }
            picker.Choices = resultList;
        }

        async void Connect(object sender, EventArgs e)
        {
            try
            {
                BindingContext = new LoadingModel(true);
                string selectedDeviceName = picker.SelectedChoice.ToString();
                
                var item = devices.FirstOrDefault(n => n.Name == selectedDeviceName);
                var connectionResult = await BluetoothService.Connect(item);
                BindingContext = new LoadingModel(false);

                if(!BluetoothService._bluetoothAdapter.IsEnabled)
                {
                    var matAlert = await MaterialDialog.Instance.ConfirmAsync(message: "Bluetooth is disabled", 
                        title: "Error", 
                        confirmingText: "Enable bluetooth", 
                        dismissiveText: "Cancel");
                    if(matAlert.Value) BluetoothService.OpenBluetoothSettings();
                    return;
                }
                if (connectionResult)
                {
                    await Navigation.PushAsync(new MainPage(item));
                }
                else
                {
                    await MaterialDialog.Instance.AlertAsync("Cannot connect selected device", "Error", "Ok");
                }
            }
            catch (Exception ex)
            {
                BindingContext = new LoadingModel(false);
                await MaterialDialog.Instance.AlertAsync("Cannot connect selected device", "Error", "Ok");
            }
        }
    }
}
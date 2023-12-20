using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Android.Bluetooth;
using Android.Content;
using Android.Util;
using Java.Util;

namespace ESP32FormGenerator.Services
{
    public static class BluetoothService
    {
        private static BluetoothDevice _device;
        private static BluetoothSocket _socket;
        public static BluetoothAdapter _bluetoothAdapter = BluetoothAdapter.DefaultAdapter;

        public static ICollection<BluetoothDevice> GetBondedDevices()
        {
            BluetoothAdapter bluetoothAdapter = BluetoothAdapter.DefaultAdapter;
            if (bluetoothAdapter == null || !bluetoothAdapter.IsEnabled)
            {
                return null;
            }

            return bluetoothAdapter.BondedDevices; //Urzadzenia do Selecta
        }

        public static async Task<bool> Connect(BluetoothDevice device)
        {
            _device = device;
            bool socketState = false;
            BluetoothAdapter bluetoothAdapter = BluetoothAdapter.DefaultAdapter;
            if (bluetoothAdapter == null || !bluetoothAdapter.IsEnabled)
            {
                return false;
            }

            try
            { 
                socketState = await SocketOpen();
            }
            catch (Exception ex)
            {
                Log.Error("CONNECTION", ex.Message);
                await Console.Out.WriteLineAsync("ERROR " + ex.Message);
            }

            await Task.Delay(2000);
            return socketState;
        }

        private static async Task<bool> SocketOpen()
        {
            _socket = _device.CreateRfcommSocketToServiceRecord(UUID.FromString("00001101-0000-1000-8000-00805f9b34fb"));
            await _socket.ConnectAsync();
            return _socket.IsConnected; //todo: check if this is correct
        }

        public static async Task<bool> BluetoothCommand(string message)
        {
            try
            {
                await SocketOpen();
                await Task.Delay(2000);
                byte[] bytes = Encoding.UTF8.GetBytes(message);
                if (_socket.IsConnected && _socket.OutputStream != null)
                {
                    await _socket.OutputStream.WriteAsync(bytes, 0, bytes.Length);
                    Disconnect();
                    return true;
                }
                Disconnect();
                return false;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error " + ex.Message);
                Disconnect();
                return false;
            }
        }

        public static async Task<byte[]> BluetoothInput()
        {
            try
            {
                using (Stream inputStream = _socket.InputStream)
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead = await inputStream.ReadAsync(buffer, 0, buffer.Length);

                    if (bytesRead > 0)
                    {
                        byte[] dataReceived = new byte[bytesRead];
                        Array.Copy(buffer, dataReceived, bytesRead);

                        return dataReceived;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading data from Bluetooth: " + ex.Message);
            }

            return null;
        }

        private static void Disconnect()
        {
            if(_socket.IsConnected) _socket.Close();
        }
        
        public static void OpenBluetoothSettings()
        {
            Intent intentOpenBluetoothSettings = new Intent();
            intentOpenBluetoothSettings.SetAction(Android.Provider.Settings.ActionBluetoothSettings);
            intentOpenBluetoothSettings.AddFlags(ActivityFlags.NewTask);
            Android.App.Application.Context.StartActivity(intentOpenBluetoothSettings);
        }
    }
}
using System.Diagnostics;
using Android.Net.Wifi.P2p;

namespace DeltaShare.Platforms.Android.Service
{
    class WifiDirectListener : Java.Lang.Object, WifiP2pManager.IConnectionInfoListener
    {
        private readonly Action<WifiP2pInfo> _onConnectionInfoAvailable;

        public WifiDirectListener(Action<WifiP2pInfo> onConnectionInfoAvailable)
        {
            _onConnectionInfoAvailable = onConnectionInfoAvailable;
        }

        public void OnConnectionInfoAvailable(WifiP2pInfo info)
        {
            Debug.WriteLine($"Connection established. Group Owner: {info.GroupOwnerAddress}");
            _onConnectionInfoAvailable?.Invoke(info);
        }
    }
}

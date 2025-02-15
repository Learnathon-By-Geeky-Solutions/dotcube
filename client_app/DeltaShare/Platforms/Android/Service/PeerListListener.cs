using System.Diagnostics;
using Android.Net.Wifi.P2p;

namespace DeltaShare.Platforms.Android.Service
{
    public class PeerListListener : Java.Lang.Object, WifiP2pManager.IPeerListListener
    {
        private readonly Action<IList<WifiP2pDevice>> _onPeersAvailable;

        public PeerListListener(Action<IList<WifiP2pDevice>> onPeersAvailable)
        {
            _onPeersAvailable = onPeersAvailable;
        }

        public void OnPeersAvailable(WifiP2pDeviceList peerList)
        {
            var peers = new List<WifiP2pDevice>(peerList.DeviceList);
            Debug.WriteLine($"Discovered {peers.Count} peers.");
            _onPeersAvailable?.Invoke(peers);
        }
    }
}


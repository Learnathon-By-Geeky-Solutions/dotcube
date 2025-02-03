using System.Diagnostics;
using Android.Content;
using Android.Net.Wifi.P2p;
using DeltaShare.Service;

namespace DeltaShare.Platforms.Android.Service
{

    public class WifiDirectService : IWifiDirectService
    {
        private readonly WifiP2pManager _manager;
        private readonly WifiP2pManager.Channel _channel;
        private readonly Context _context;
        private WifiDirectReceiver _receiver;

        public WifiDirectService(Context context)
        {
            _context = context;
            _manager = (WifiP2pManager)_context.GetSystemService(Context.WifiP2pService);
            _channel = _manager.Initialize(_context, _context.MainLooper, null);
        }

        public void RegisterReceiver()
        {
            _receiver = new WifiDirectReceiver(_manager, _channel, peers =>
            {
                foreach (var peer in peers)
                {
                    Debug.WriteLine($"Found peer: {peer.DeviceName} ({peer.DeviceAddress})");
                }
            });

            IntentFilter filter = new();
            filter.AddAction(WifiP2pManager.WifiP2pStateChangedAction);
            filter.AddAction(WifiP2pManager.WifiP2pPeersChangedAction);
            filter.AddAction(WifiP2pManager.WifiP2pConnectionChangedAction);
            filter.AddAction(WifiP2pManager.WifiP2pThisDeviceChangedAction);

            _context.RegisterReceiver(_receiver, filter);
        }

        public void UnregisterReceiver()
        {
            if (_receiver != null)
            {
                _context.UnregisterReceiver(_receiver);
            }
        }

        public void DiscoverPeers()
        {
            Debug.WriteLine("Starting peer discovery...");
            var discoverListener = new PeerDiscover();
            discoverListener.OnSuccessAction += () => Debug.WriteLine("Peer discovery started!");
            discoverListener.OnFailureAction += reason => Debug.WriteLine($"Discovery failed: {reason}");

            _manager.DiscoverPeers(_channel, discoverListener);
        }

        public void ConnectToPeer(WifiP2pDevice device)
        {
            var config = new WifiP2pConfig { DeviceAddress = device.DeviceAddress };
            var listener = new PeerDiscover();
            listener.OnSuccessAction += () => Debug.WriteLine("Connected to peer!");
            listener.OnFailureAction += reason => Debug.WriteLine($"Connection failed: {reason}");

            _manager.Connect(_channel, config, listener);
        }

        public void Disconnect()
        {
            var listener = new PeerDiscover();
            listener.OnSuccessAction += () => Debug.WriteLine("Disconnected!");
            listener.OnFailureAction += reason => Debug.WriteLine($"Disconnect failed: {reason}");

            _manager.RemoveGroup(_channel, listener);
        }
    }
}

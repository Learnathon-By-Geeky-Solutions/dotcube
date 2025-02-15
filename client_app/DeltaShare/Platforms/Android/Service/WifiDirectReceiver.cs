using System.Diagnostics;
using Android.Content;
using Android.Net.Wifi.P2p;
using Android.Runtime;
using DeltaShare.Platforms.Android.Service;

[Register("deltashare.platforms.android.service.WifiDirectReceiver")]
public class WifiDirectReceiver : BroadcastReceiver
{
    private readonly WifiP2pManager _manager;
    private readonly WifiP2pManager.Channel _channel;
    private readonly Action<IList<WifiP2pDevice>> _onPeersUpdated;

    public WifiDirectReceiver(WifiP2pManager manager, WifiP2pManager.Channel channel, Action<IList<WifiP2pDevice>> onPeersUpdated)
    {
        _manager = manager;
        _channel = channel;
        _onPeersUpdated = onPeersUpdated;
    }

    public override void OnReceive(Context context, Intent intent)
    {
        string action = intent.Action;

        if (WifiP2pManager.WifiP2pStateChangedAction.Equals(action))
        {
            int state = intent.GetIntExtra(WifiP2pManager.ExtraWifiState, -1);
            Debug.WriteLine($"Wi-Fi Direct state changed: {state}");
        }
        else if (WifiP2pManager.WifiP2pPeersChangedAction.Equals(action))
        {
            Debug.WriteLine("Wi-Fi Direct peers changed. Requesting update.");
            _manager.RequestPeers(_channel, new PeerListListener(_onPeersUpdated));
        }
        else if (WifiP2pManager.WifiP2pConnectionChangedAction.Equals(action))
        {
            Debug.WriteLine("Wi-Fi Direct connection state changed.");
        }
        else if (WifiP2pManager.WifiP2pThisDeviceChangedAction.Equals(action))
        {
            Debug.WriteLine("Wi-Fi Direct this device changed.");
        }
    }
}

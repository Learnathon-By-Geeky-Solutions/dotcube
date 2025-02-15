using System.Diagnostics;
using Android.Net.Wifi.P2p;
using Android.Runtime;

namespace DeltaShare.Platforms.Android.Service
{
    [Register("deltashare/platforms/android/service/PeerDiscover")]
    public class PeerDiscover : Java.Lang.Object, WifiP2pManager.IActionListener
    {
        public event Action OnSuccessAction;
        public event Action<WifiP2pFailureReason> OnFailureAction;

        public PeerDiscover()
        {
        }

        public void OnSuccess()
        {
            Debug.WriteLine("Wifi direct action successful.");
            OnSuccessAction?.Invoke();
        }

        public void OnFailure([GeneratedEnum] WifiP2pFailureReason reason)
        {
            Debug.WriteLine($"Wifi direct action failed. Reason: {reason}");
            OnFailureAction?.Invoke(reason);
        }
    }
}

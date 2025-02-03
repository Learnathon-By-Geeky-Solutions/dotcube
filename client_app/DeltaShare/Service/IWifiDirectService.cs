
namespace DeltaShare.Service
{
    public interface IWifiDirectService
    {
        public void RegisterReceiver();
        public void UnregisterReceiver();
        public void DiscoverPeers();

        public void Disconnect();
    }
}

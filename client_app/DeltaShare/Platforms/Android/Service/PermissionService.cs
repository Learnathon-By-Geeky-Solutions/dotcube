using DeltaShare.Service;

namespace DeltaShare.Platforms.Android.Service
{
    public class PermissionService : IPermissionService
    {
        public async Task<Microsoft.Maui.ApplicationModel.PermissionStatus> RequestPermissions()
        {
            await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            await Permissions.RequestAsync<Permissions.NetworkState>();
            await Permissions.RequestAsync<Permissions.NearbyWifiDevices>();

            var locationPermissionStatus = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            var networkPermissionStatus = await Permissions.CheckStatusAsync<Permissions.NetworkState>();
            var nearbyPermissionStatus = await Permissions.CheckStatusAsync<Permissions.NearbyWifiDevices>();
            if (locationPermissionStatus != PermissionStatus.Granted ||
                networkPermissionStatus != PermissionStatus.Granted ||
                nearbyPermissionStatus != PermissionStatus.Granted)
            {
                return PermissionStatus.Denied;
            }
            return PermissionStatus.Granted;
        }
    }
}

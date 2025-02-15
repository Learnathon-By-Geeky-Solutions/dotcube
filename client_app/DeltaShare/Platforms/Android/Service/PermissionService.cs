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
            await Permissions.RequestAsync<Permissions.StorageRead>();
            await Permissions.RequestAsync<Permissions.StorageWrite>();
            await Permissions.RequestAsync<Permissions.Media>();

            var locationPermissionStatus = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            var networkPermissionStatus = await Permissions.CheckStatusAsync<Permissions.NetworkState>();
            var nearbyPermissionStatus = await Permissions.CheckStatusAsync<Permissions.NearbyWifiDevices>();
            var storageReadPermissionStatus = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
            var storageWritePermissionStatus = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
            var mediaPermissionStatus = await Permissions.CheckStatusAsync<Permissions.Media>();
            if (locationPermissionStatus != PermissionStatus.Granted ||
                networkPermissionStatus != PermissionStatus.Granted ||
                nearbyPermissionStatus != PermissionStatus.Granted ||
                storageReadPermissionStatus != PermissionStatus.Granted ||
                storageWritePermissionStatus != PermissionStatus.Granted ||
                mediaPermissionStatus != PermissionStatus.Granted)
            {
                return PermissionStatus.Denied;
            }
            return PermissionStatus.Granted;
        }
    }
}

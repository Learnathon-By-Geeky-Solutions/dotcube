using DeltaShare.Service;

namespace DeltaShare.Platforms.Windows.Service
{
    public class PermissionService : IPermissionService
    {
        public async Task<Microsoft.Maui.ApplicationModel.PermissionStatus> RequestPermissions()
        {
            await Permissions.RequestAsync<Permissions.NetworkState>();

            var networkPermissionStatus = await Permissions.CheckStatusAsync<Permissions.NetworkState>();
            return (networkPermissionStatus != PermissionStatus.Granted) ? PermissionStatus.Denied : PermissionStatus.Granted;
        }
    }
}

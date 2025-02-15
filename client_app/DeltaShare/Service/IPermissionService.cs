namespace DeltaShare.Service
{
    public interface IPermissionService
    {
        public Task<Microsoft.Maui.ApplicationModel.PermissionStatus> RequestPermissions();
    }
}

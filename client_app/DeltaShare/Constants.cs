namespace DeltaShare
{
    public static class Constants
    {
        // App API routes
        public const string NewClientPath = "/new-client";
        public const string ClientsSyncPath = "/clients-sync";
        public const string FilesSyncPath = "/files-sync";
        public const string NewFileMetadataPath = "/new-files";
        public const string FileDownloadPath = "/download-file";

        // App API form fields
        public const string PoolCreatorIpField = "PoolCreatorIpField";
        public const string UserJsonField = "UserJson";
        public const string AllUsersJsonField = "AllUsersJson";
        public const string AllFilesJsonField = "AllFilesJson";
        public const string UserFilesJsonField = "UserFilesJson";
        public const string FileUuidField = "FileUuid";

        // default values
        public const string DefaultUsername = "John Doe";

        // preferences keys
        public const string SettingsShowedKey = "IsSettingsShowed";
        public const string UsernameKey = "Username";
        public const string FullNameKey = "FullName";

        // pool creator values
        public const string Port = "9898";

        // window size
        public const int WindowWidth = 800;
        public const int WindowHeight = 800;
    }
}

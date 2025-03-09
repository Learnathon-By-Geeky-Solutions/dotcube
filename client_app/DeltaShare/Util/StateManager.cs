using System.Collections.ObjectModel;
using DeltaShare.Model;

namespace DeltaShare.Util
{
    public static class StateManager
    {
        public static bool IsSavedPoolCreatorIp { get; set; } = false;
        public static string PoolCreatorIpAddress { get; set; } = String.Empty;
        public static bool IsPoolCreator { get; set; } = false;
        public static string IpAddress { get; set; } = String.Empty;
        public static User CurrentUser { get; set; } = new("", "", "", "", false);
        public static ObservableCollection<User> PoolUsers { get; set; } = [];
        public static Dictionary<string, User> IpToUserMap { get; set; } = [];
        public static ObservableCollection<FileMetadata> PoolFiles { get; set; } = [];

        public static void InitMock()
        {
        }
    }
}

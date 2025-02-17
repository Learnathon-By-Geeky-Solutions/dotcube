using System.Collections.ObjectModel;
using DeltaShare.Model;

namespace DeltaShare.Util
{
    public class StateManager
    {
        public static string PoolCreatorIpAddress { get; set; } = String.Empty;
        public static bool IsPoolCreator { get; set; } = false;
        public static string IpAddress { get; set; } = String.Empty;
        public static ObservableCollection<User> PoolUsers { get; set; } = new();

        public static Dictionary<string, List<FileMetadata>> PoolFiles { get; set; }

        public static void InitMock()
        {
            PoolUsers = new ObservableCollection<User>([
                new User("John", "john@something.com", "john", "2.2.2.2", true),
                new User("Jane", "jane@something.com", "jane", "3.3.3.3", false),
                ]);
        }
    }
}

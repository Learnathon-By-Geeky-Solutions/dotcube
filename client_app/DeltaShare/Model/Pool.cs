namespace DeltaShare.Model
{
    class Pool
    {
        public override string ToString()
        {
            return $"dotpool://{SSID}/{Password}/{CreatorAddress}";
        }

        public Pool(string name, string ssid, string password, string creatorAddress)
        {
            Name = name;
            SSID = ssid;
            Password = password;
            CreatorAddress = creatorAddress;
        }
        public required string Name { get; set; }
        public required string SSID { get; set; }
        public required string Password { get; set; }
        public required string CreatorAddress { get; set; }
    }
}

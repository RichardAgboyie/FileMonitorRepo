namespace Entities.Entities
{
    public class PathSetting
    {
        public string SourceFolder { get; set; }
        public string DestinationFolder { get; set; }
        public SftpCredentials SftpCredentials { get; set; }
        public string LogPath { get; set; }
    }

    public class SftpCredentials
    {
        public string Sftp { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}

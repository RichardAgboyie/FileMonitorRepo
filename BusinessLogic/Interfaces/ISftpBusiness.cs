namespace BusinessLogic.Interfaces
{
    public interface ISftpBusiness
    {
        /// <summary>
        /// Monitor folder for files
        /// </summary>
        /// <returns>The list of files</returns>
        Task MonitorFolder();

        /// <summary>
        /// Monitor folder for files
        /// </summary>
        /// <returns>The list of files</returns>
        Task UploadFilesToServer(List<string> sourceFiles);
    }
}

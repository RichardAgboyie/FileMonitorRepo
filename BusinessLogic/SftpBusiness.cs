using BusinessLogic.Interfaces;
using Entities.Entities;
using Microsoft.Extensions.Options;
using Renci.SshNet;

namespace BusinessLogic
{
    public class SftpBusiness : ISftpBusiness
    {
        private readonly PathSetting _pathSetting;

        public SftpBusiness(IOptions<PathSetting> pathSetting)
        {
            _pathSetting = pathSetting.Value;
        }
        
        public async Task MonitorFolder()
        {

            try
            {
                Logger("Starting Monitor", LogType.Info);
                
                var sourceFolder = _pathSetting.SourceFolder;
                
                const string fileExtension = ".zip";

                if (Directory.Exists(sourceFolder))
                {
                   var  fileEntries = Directory.GetFiles(sourceFolder, "*" + fileExtension).ToList();

                    if (fileEntries.Any())
                    {
                        Logger($"{fileEntries.Count} Files Available for upload", LogType.Info);

                        await  UploadFilesToServer(fileEntries);
                        
                    }
                    else
                    {
                        Logger("No Files Available for Upload ", LogType.Info);
                    }
                }
                else
                {
                    Logger($"Directory {sourceFolder} does not exist ", LogType.Info);
                }

            }
            catch (Exception ex)
            {
                Logger(ex.Message, LogType.Error);
            }
        }


        public async Task UploadFilesToServer(List<string> sourceFiles)
        {
            
            try
            {
                using var sftpClient = new SftpClient(_pathSetting.SftpCredentials.Sftp,
                    _pathSetting.SftpCredentials.Port, _pathSetting.SftpCredentials.Username,
                    _pathSetting.SftpCredentials.Password);

                sftpClient.Connect();
                
                Logger("Connected to SFTP", LogType.Info);

                var workingDirectory = sftpClient.WorkingDirectory;

                sftpClient.ChangeDirectory(workingDirectory);
                
                Logger("Changed Working Directory", LogType.Info);

                if (sourceFiles.Any())
                {
                    Logger($"Moving {sourceFiles.Count}", LogType.Info);
                    
                    foreach (var file in sourceFiles)
                    {
                        Logger($"Uploading File {file}", LogType.Info);
                        
                        await using Stream stream = File.OpenRead(file);
                        
                        sftpClient.UploadFile(stream, workingDirectory + Path.GetFileName(file), Console.Write);
                    }
                }

                sftpClient.Disconnect();
                
                Logger("Disconnect to SFTP", LogType.Info);
            }
            catch (Exception ex)
            {
                Logger(ex.Message, LogType.Error);
            }

        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dirPath"></param>
        /// <returns></returns>
        private static Task<bool> DeleteFiles(string dirPath)
        {
            var di = new DirectoryInfo(dirPath);
            
            var arrFi = di.GetFiles("*.*");

            try
            {
                foreach (var fi in arrFi)
                {
                    File.Delete(di + "\\" + fi.Name);
                }

                return Task.FromResult(true);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }

        private  void Logger(string log, LogType logType)
        {
            if (!Directory.Exists(_pathSetting.LogPath))
            {
                Directory.CreateDirectory(_pathSetting.LogPath);
            }
            
            var path = logType switch
            {
                LogType.Info => $"{_pathSetting.LogPath}/info.txt",
                LogType.Error => $"{_pathSetting.LogPath}/Error.txt",
                _ => $"{_pathSetting.LogPath}/info.txt"
            };

           
            using var writeText = new StreamWriter(path);
            
            writeText.WriteLine($"{DateTime.UtcNow} : {log}");
        }
    }
}
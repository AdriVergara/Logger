using PCLStorage;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;

namespace LoggerTest
{
    public class SimpleLogger
    {
        private readonly string datetimeFormat; //Format of date

        private string logFilename = "logTest.log"; //Name of the log file
        private string FilePath { get; set; } //Path where the log file is 
        private double LogFileAntiquity { get; set; } //Path where the log file is 
        private string WindowsDirectoryPath { get; set; }

        private readonly string WindowsFolderName = "Logger";

        private DateTime OriginDate;

        //Only Used on UWP
        public async Task WindowsStorage()
        {
            IFolder rootFolder = FileSystem.Current.LocalStorage;

            WindowsDirectoryPath = rootFolder.Path;

            IFolder folder = await rootFolder.CreateFolderAsync(WindowsFolderName, CreationCollisionOption.OpenIfExists);
        }

        public SimpleLogger()
        {
            datetimeFormat = "dd-MM-yyyy HH:mm:ss";
            string path = string.Empty;

            //Check the OS of the running device
            if (Device.RuntimePlatform == Device.Android)
            {
                var assemblyPath = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
                path = Path.GetDirectoryName(assemblyPath); //EXTERNAL STORAGE // PATH --> "/storage/emulated/0/Android/data/com.companyname.LoggerTest/files/.__override__/<NameOfTheLogFile>.log"
                //path = Environment.GetFolderPath(Environment.SpecialFolder.Personal); //INTERNAL STORAGE: This saves the file on a internal folder impossible-to-acces manually from the device
            }
            //Check On Windows
            else if (Device.RuntimePlatform == Device.WPF || Device.RuntimePlatform == Device.UWP)
            {
                Task.Run(() => WindowsStorage()).Wait();

                path = WindowsDirectoryPath + "\\" + WindowsFolderName;
            }
            //Check On iOS
            else if (Device.RuntimePlatform == Device.iOS)
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            }

            //Path of (directory) file including the filename
            FilePath = Path.Combine(path, logFilename);

            CheckLogAntiquity();
        }

        //Deletes the log file. Does not create it again, because it will be automatically created when the program tries to write on the inexistent file.
        public void ResetLogFile()
        {
            File.Delete(FilePath);
        }

        //Checks the Log file antiquity. If it's bigger than 5 days, deletes it
        private void CheckLogAntiquity()
        {
            FileInfo fi = new FileInfo(FilePath);

            OriginDate = fi.CreationTime;
            var TodayDate = DateTime.Now;

            LogFileAntiquity = (TodayDate - OriginDate).TotalDays;

            if (LogFileAntiquity > 5)
            {
                ResetLogFile();
            }
        }

        public void Debug(string text)
        {
            WriteFormattedLog(LogLevel.DEBUG, text);
        }

        public void Error(string text)
        {
            WriteFormattedLog(LogLevel.ERROR, text);
        }


        public void Fatal(string text)
        {
            WriteFormattedLog(LogLevel.FATAL, text);
        }

        public void Info(string text)
        {
            WriteFormattedLog(LogLevel.INFO, text);
        }

        public void Trace(string text)
        {
            WriteFormattedLog(LogLevel.TRACE, text);
        }

        public void Warning(string text)
        {
            WriteFormattedLog(LogLevel.WARNING, text);
        }

        //Function to test if the logs were being correctly saved on the log file on iOS,
        //public void OpenLogFile()
        //{
        //    try
        //    {
        //        var lines = File.ReadLines(FilePath);

        //        foreach (var line in lines)
        //        {

        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        private void WriteLine(string text, bool append = true)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(FilePath, append, Encoding.UTF8))
                {
                    if (!string.IsNullOrEmpty(text))
                    {
                        writer.WriteLine(text);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private void WriteFormattedLog(LogLevel level, string text)
        {
            string pretext;
            switch (level)
            {
                case LogLevel.TRACE:
                    pretext = DateTime.Now.ToString(datetimeFormat) + " [TRACE]   ";
                    break;
                case LogLevel.INFO:
                    pretext = DateTime.Now.ToString(datetimeFormat) + " [INFO]    ";
                    break;
                case LogLevel.DEBUG:
                    pretext = DateTime.Now.ToString(datetimeFormat) + " [DEBUG]   ";
                    break;
                case LogLevel.WARNING:
                    pretext = DateTime.Now.ToString(datetimeFormat) + " [WARNING] ";
                    break;
                case LogLevel.ERROR:
                    pretext = DateTime.Now.ToString(datetimeFormat) + " [ERROR]   ";
                    break;
                case LogLevel.FATAL:
                    pretext = DateTime.Now.ToString(datetimeFormat) + " [FATAL]   ";
                    break;
                default:
                    pretext = "";
                    break;
            }
            WriteLine(pretext + text);
        }

        [Flags]
        private enum LogLevel
        {
            TRACE,
            INFO,
            DEBUG,
            WARNING,
            ERROR,
            FATAL
        }
    }
}

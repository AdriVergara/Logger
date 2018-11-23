using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace LoggerTest.ViewModels
{
    public class LogViewModel : BaseViewModel
    {
        public SimpleLogger simpleLogger { get; set; }

        public ICommand AddLogMessageCommand { get; set; }
        public ICommand ResetLogFileCommand { get; set; }

        public LogViewModel()
        {
            simpleLogger = new SimpleLogger();

            AddLogMessageCommand = new Command(async (Operation) => await ExecuteAddLogMessageCommand(Operation));
            ResetLogFileCommand = new Command(async () => await ExecuteResetLogFileCommand());
        }

        private async Task ExecuteResetLogFileCommand()
        {
            //Delete Log File
            simpleLogger.ResetLogFile();
        }

        private async Task ExecuteAddLogMessageCommand(object Operation) 
        {
            switch (Convert.ToInt32(Operation))
            {
                case 0:
                    simpleLogger.Trace("Trace Message");
                    break;

                case 1 :
                    simpleLogger.Info("Info message");
                    break;

                case 2:
                    simpleLogger.Debug("Debug Message");
                    break;

                case 3 :
                    simpleLogger.Warning("Warning message");
                    break;

                case 4 :
                    simpleLogger.Error("Error Message");
                    break;

                case 5:
                    simpleLogger.Fatal("Fatal error Message");
                    break;
            }
        }
    }
}

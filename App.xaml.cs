using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using System.IO;

namespace KyoshinQuickly
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            string dire = @$"{System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\KyoshinQuickly\Config";
            if (!Directory.Exists(dire))
            {
                Directory.CreateDirectory(dire);
            }

            dire = @$"{System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\KyoshinQuickly\Config\Point.txt";
            if (!File.Exists(dire))
            {
                File.WriteAllText(dire, "");
            }
            dire = @$"{System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\KyoshinQuickly\Config\TextColor.txt";
            if (!File.Exists(dire))
            {
                File.WriteAllText(dire, "0");
            }
            dire = @$"{System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\KyoshinQuickly\Config\Haikei_Color.txt";
            if (!File.Exists(dire))
            {
                File.WriteAllText(dire, "");
            }
            dire = @$"{System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\KyoshinQuickly\Config\Moji_Color.txt";
            if (!File.Exists(dire))
            {
                File.WriteAllText(dire, "");
            }

            dire = @$"{System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\KyoshinQuickly\Config\ver.txt";
            File.WriteAllText(dire, "1.0.0");
        }

        /// <summary>
        /// UI スレッドで発生した未処理例外を処理します。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            var exception = e.Exception as Exception;
            Debug.Print(e.Exception.Message);
            var dire = @"log\Error_UI.txt";
            File.WriteAllText(dire, $"UIスレッドエラー : \n{e.Exception.Message}\n\n{e.Exception.StackTrace}\n{e.Exception.Source}");
            if (ConfirmUnhandledException(exception, "UI スレッド"))
            {
                e.Handled = true;
            }
            else
            {
                //Environment.Exit(1);
            }
        }

        /// <summary>
        /// バックグラウンドタスクで発生した未処理例外を処理します。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            var exception = e.Exception.InnerException as Exception;
            Debug.Print(e.Exception.Message);
            var dire = @"log\Error_Background.txt";
            File.WriteAllText(dire, $"バックグラウンドタスクエラー : \n{e.Exception.Message}\n\n{e.Exception.StackTrace}\n{e.Exception.Source}");
            if (ConfirmUnhandledException(exception, "バックグラウンドタスク"))
            {
                e.SetObserved();
            }
            else
            {
                //Environment.Exit(1);
            }
        }

        /// <summary>
        /// 実行を継続するかどうかを選択できる場合の未処理例外を処理します。
        /// </summary>
        /// <param name="e">例外オブジェクト</param>
        /// <param name="sourceName">発生したスレッドの種別を示す文字列</param>
        /// <returns>継続することが選択された場合は true, それ以外は false</returns>
        bool ConfirmUnhandledException(Exception e, string sourceName)
        {
            var message = $"予期せぬエラーが発生しました。続けて発生する場合は開発者に報告してください。\nプログラムの実行を継続しますか？";
            var dire = @"log\Error_Unknown.txt";
            File.WriteAllText(dire, $"予期せぬエラー : \n{e.Message}\n\n{e.StackTrace}\n{e.Source}");
            if (e != null) message += $"\n({e.Message} @ {e.TargetSite.Name})";
            // Logger.Fatal($"未処理例外 ({sourceName})", e); // 適当なログ記録
            //var result = MessageBox.Show(message, $"未処理例外 ({sourceName})", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            var result = MessageBoxResult.Yes;
            return result == MessageBoxResult.Yes;
        }

        /// <summary>
        /// 最終的に処理されなかった未処理例外を処理します。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception;
            var message = $"予期せぬエラーが発生しました。続けて発生する場合は開発者に報告してください。";
            var dire = @"log\Error_Unknown2.txt";
            File.WriteAllText(dire, $"予期せぬエラー2 : \n{exception.Message}\n\n{exception.StackTrace}\n{exception.Source}");
            if (exception != null) message += $"\n({exception.Message} @ {exception.TargetSite.Name})";
            // Logger.Fatal("未処理例外", exception); // 適当なログ記録
            //MessageBox.Show(message, "未処理例外", MessageBoxButton.OK, MessageBoxImage.Stop);
            //Environment.Exit(1);
        }

        private System.Threading.Mutex mutex = new System.Threading.Mutex(false, "ApplicationName");

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // ミューテックスの所有権を要求
            if (!mutex.WaitOne(0, false))
            {
                // 既に起動しているため終了させる
                var a = MessageBox.Show("KyoshinQuickly は既に実行されています。\n\nサーバー負荷低減のため、\n同時に複数起動すること推奨されません。\nそれでも起動しますか？", "KyoshinQuickly", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (a != MessageBoxResult.Yes)
                {
                    mutex.Close();
                    mutex = null;
                    this.Shutdown();
                }
            }
        }
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            if (mutex != null)
            {
                mutex.ReleaseMutex();
                mutex.Close();
            }
            //Debug.Print(e.ApplicationExitCode.ToString());
        }
    }
}

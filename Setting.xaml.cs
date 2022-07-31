using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.Diagnostics;

namespace KyoshinQuickly
{
    /// <summary>
    /// Setting.xaml の相互作用ロジック
    /// </summary>
    public partial class Setting : Window
    {
        public Setting()
        {
            InitializeComponent();
        }
        private void Menu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var Index = Menu.SelectedIndex + 1;
            int iii = 1;
            while (true)
            {
                Grid tbox = MainGrid.Children.OfType<Grid>().FirstOrDefault(c => c.Name == $"B{iii}");
                if (tbox != null)
                {
                    if (iii == Index)
                    {
                        tbox.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        tbox.Visibility = Visibility.Hidden;
                    }
                    ListBoxItem tboxx = Menu.Items.OfType<ListBoxItem>().FirstOrDefault(c => c.Name == $"L{iii}");
                    if (tboxx != null)
                    {
                        if (iii == Index)
                        {
                            tboxx.Background = Brushes.White;
                            tboxx.Foreground = Brushes.Black;
                        }
                        else
                        {
                            tboxx.Background = Brushes.MidnightBlue;
                            tboxx.Foreground = Brushes.White;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
                iii++;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var HozonString = new string[0];

                var items = TourokuList.Items;
                foreach (var item in items)
                {
                    if (item != null)
                    {
                        Array.Resize(ref HozonString, HozonString.Length + 1);
                        HozonString[HozonString.Length - 1] = item.ToString();
                    }
                }
                if (HozonString.Length != 0)
                {
                    await File.WriteAllLinesAsync(@$"{System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\KyoshinQuickly\Config\Point.txt", HozonString);
                    MessageBox.Show("登録観測点リストを保存しました。\n設定は、再起動後に反映されます。", "KyoshinQuickly", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("登録観測点リストに観測点が登録されていません。", "KyoshinQuickly", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch
            {

            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                var item = KyoshinPointList.SelectedItem;
                if (item != null)
                {
                    TourokuList.Items.Add(item);
                    MessageBox.Show($"登録観測点リストに\n{item} \nを追加しました。", "KyoshinQuickly", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("強震モニタ観測点リストで、\n追加したい観測点を選択した状態で押してください。", "KyoshinQuickly", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch
            {

            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                var item = TourokuList.SelectedItem;
                if (item != null)
                {
                    TourokuList.Items.Remove(item);
                    MessageBox.Show($"登録観測点リストから\n{item} \nを削除しました。", "KyoshinQuickly", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("登録観測点リストで、\n削除したい観測点を選択した状態で押してください。", "KyoshinQuickly", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch
            {

            }
        }

        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ProcessStartInfo pi = new ProcessStartInfo()
            {
                FileName = "https://github.com/ingen084/KyoshinMonitorLib/blob/master/LICENSE",
                UseShellExecute = true,
            };

            Process.Start(pi);
        }

        private void TextBlock_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            ProcessStartInfo pi = new ProcessStartInfo()
            {
                FileName = "https://twitter.com/Ameuma773",
                UseShellExecute = true,
            };

            Process.Start(pi);
        }

        private async void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var iro = Haikeisyoku.Text;
            try
            {
                var a = (SolidColorBrush)new BrushConverter().ConvertFrom(iro);
            }
            catch
            {
                MessageBox.Show("正しい色を入力してください。", "KyoshinQuickly", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            await File.WriteAllTextAsync(@$"{System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\KyoshinQuickly\Config\Haikei_Color.txt", iro);
        }

        private async void Button_Click_4(object sender, RoutedEventArgs e)
        {
            var iro = Mojisyoku.Text;
            try
            {
                var a = (SolidColorBrush)new BrushConverter().ConvertFrom(iro);
            }
            catch
            {
                MessageBox.Show("正しい色を入力してください。", "KyoshinQuickly", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            await File.WriteAllTextAsync(@$"{System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\KyoshinQuickly\Config\Moji_Color.txt", iro);
        }
    }
}

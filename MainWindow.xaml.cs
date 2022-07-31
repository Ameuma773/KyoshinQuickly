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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime;
using System.Timers;
using KyoshinMonitorLib;
using KyoshinMonitorLib.Images;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.IO;

namespace KyoshinQuickly
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservationPoint[] points = ObservationPoint.LoadFromMpk("KyoshinMonitorPoints.mpk.lz4", true);
        public int KoushinJikoku = 0;
        public HttpClient client = new HttpClient();
        public DateTime dt = DateTime.Now;
        public DateTime dt2 = DateTime.Now;
        public string[] pos = File.ReadAllLines(@$"{System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\KyoshinQuickly\Config\Point.txt");
        public int[] UserPointInt = new int[0];
        public KyoshinQuickly.Setting setting = new Setting();
        string ver = File.ReadAllText(@$"{System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\KyoshinQuickly\Config\ver.txt");

        public SolidColorBrush backgroundColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#00000000");

        public string[] Japan = { "北海道", "青森県", "岩手県", "宮城県", "秋田県", "山形県", "福島県", "茨城県", "栃木県", "群馬県", "埼玉県", "千葉県", "東京都", "神奈川県", "新潟県", "富山県", "石川県", "福井県", "山梨県", "長野県", "岐阜県", "静岡県", "愛知県", "三重県", "滋賀県", "京都府", "大阪府", "兵庫県", "奈良県", "和歌山県", "鳥取県", "島根県", "岡山県", "広島県", "山口県", "徳島県", "香川県", "愛媛県", "高知県", "福岡県", "佐賀県", "長崎県", "熊本県", "大分県", "宮崎県", "鹿児島県", "沖縄県" };

        //コントロール系
        public TextBlock[] Control_Names = new TextBlock[0];
        public TextBlock[] Control_Numbers = new TextBlock[0];
        public TextBlock[] Control_JmaInts = new TextBlock[0];
        public Line[] Control_Lines = new Line[0];

        //震度配色
        public SolidColorBrush Int00 = new SolidColorBrush(Colors.MidnightBlue);
        public SolidColorBrush IntU = new SolidColorBrush(Colors.DarkCyan);
        public SolidColorBrush Int0 = new SolidColorBrush(Colors.DarkBlue);
        public SolidColorBrush Int1 = (SolidColorBrush)new BrushConverter().ConvertFrom("#0045B0");
        public SolidColorBrush Int2 = (SolidColorBrush)new BrushConverter().ConvertFrom("#1E7CEB");
        public SolidColorBrush Int3 = new SolidColorBrush(Colors.LimeGreen);
        public SolidColorBrush Int4 = new SolidColorBrush(Colors.Gold);
        public SolidColorBrush Int50 = new SolidColorBrush(Colors.Orange);
        public SolidColorBrush Int55 = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF7300");
        public SolidColorBrush Int60 = (SolidColorBrush)new BrushConverter().ConvertFrom("#DF0000");
        public SolidColorBrush Int65 = new SolidColorBrush(Colors.DarkRed);
        public SolidColorBrush Int7 = new SolidColorBrush(Colors.Purple);

        public SolidColorBrush IntString00 = System.Windows.Media.Brushes.White;
        public SolidColorBrush IntStringU = System.Windows.Media.Brushes.White;
        public SolidColorBrush IntString0 = System.Windows.Media.Brushes.White;
        public SolidColorBrush IntString1 = System.Windows.Media.Brushes.White;
        public SolidColorBrush IntString2 = System.Windows.Media.Brushes.White;
        public SolidColorBrush IntString3 = System.Windows.Media.Brushes.White;
        public SolidColorBrush IntString4 = System.Windows.Media.Brushes.White;
        public SolidColorBrush IntString50 = System.Windows.Media.Brushes.White;
        public SolidColorBrush IntString55 = System.Windows.Media.Brushes.White;
        public SolidColorBrush IntString60 = System.Windows.Media.Brushes.White;
        public SolidColorBrush IntString65 = System.Windows.Media.Brushes.White;
        public SolidColorBrush IntString7 = System.Windows.Media.Brushes.White;

        public MainWindow()
        {
            InitializeComponent();
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            GCSettings.LatencyMode = 0;


            string textcolor = File.ReadAllText(@$"{System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\KyoshinQuickly\Config\Moji_Color.txt");

            if (textcolor.Any())
            {
                try
                {
                    this.Resources["MojiColor"] = (SolidColorBrush)new BrushConverter().ConvertFrom(textcolor);
                }
                catch (Exception e)
                {
                    MessageBox.Show("文字色の設定が正しくありません。", "KyoshinQuickly", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.Resources["MojiColor"] = Brushes.White;
                }
            }

            string haikeicolor = File.ReadAllText(@$"{System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\KyoshinQuickly\Config\Haikei_Color.txt");

            if (haikeicolor.Any())
            {
                try
                {
                    this.Background = (SolidColorBrush)new BrushConverter().ConvertFrom(haikeicolor);
                }
                catch (Exception e)
                {
                    MessageBox.Show("文字色の設定が正しくありません。", "KyoshinQuickly", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.Background = Brushes.DimGray;
                }
            }

            Int00.Freeze();
            IntU.Freeze();
            Int0.Freeze();
            Int1.Freeze();
            Int2.Freeze();
            Int3.Freeze();
            Int4.Freeze();
            Int50.Freeze();
            Int55.Freeze();
            Int60.Freeze();
            Int65.Freeze();
            Int7.Freeze();

            IntString00.Freeze();
            IntStringU.Freeze();
            IntString0.Freeze();
            IntString1.Freeze();
            IntString2.Freeze();
            IntString3.Freeze();
            IntString4.Freeze();
            IntString50.Freeze();
            IntString55.Freeze();
            IntString60.Freeze();
            IntString65.Freeze();
            IntString7.Freeze();

            this.Title = $"KyoshinQuickly - Ver : {ver}";

            setting.Dispatcher.Invoke((Action)(() =>
            {
                setting.VerInfo.Text = $"Ver : {ver}";
            }));

            int k = 0;
            foreach (var okkkk in points)
            {
                foreach (var Point in pos)
                {
                    if (Point == $"{okkkk.Region} {okkkk.Name}")
                    {
                        Array.Resize(ref UserPointInt, UserPointInt.Length + 1);
                        UserPointInt[UserPointInt.Length - 1] = k;
                        break;
                    }
                }
                k++;
            }

            if (UserPointInt.Length == 0)
            {
                MessageBox.Show("強震モニタ観測点が一つも設定されていません。\n設定画面から設定してください。", "KyoshinQuickly", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            foreach (var id in UserPointInt)
            {
                CreateControl(id);
            }

            // タイマーの間隔(ミリ秒)
            Timer timer = new Timer(1000);

            // タイマーの処理
            timer.Elapsed += (sender, e) =>
            {
                KoushinJikoku++;

                if (KoushinJikoku >= 5)
                {
                    var timejson = "";
                    var timeurl = $"http://www.kmoni.bosai.go.jp/webservice/server/pros/latest.json";
                    try
                    {
                        timejson = client.GetStringAsync(timeurl).Result;
                    }
                    catch(Exception er)
                    {
                        Debug.Print(er.Message);
                    }

                    JObject jsontime = JObject.Parse(timejson);

                    if (jsontime == null)
                    {
                        return;
                    }
                    else
                    {
                        DateTime dateTime = DateTime.Parse(jsontime["latest_time"].ToString());
                        dt = dateTime;
                        dt2 = dateTime;
                    }
                    KoushinJikoku = 0;
                }
                else
                {
                    TimeSpan timeSpan1 = new TimeSpan(0, 0, 1);
                    dt = dt + timeSpan1;
                    dt2 = dt2 + timeSpan1;
                }

                if (Application.Current.Properties["Replay"] != null)
                {
                    string min = Application.Current.Properties["Replay"].ToString();
                    TimeSpan timeSpan = TimeSpan.Parse(min);
                    dt = dt2 - timeSpan;
                }
                else
                {
                    if (dt != dt2)
                    {
                        dt = dt2;
                    }
                }
                if (Application.Current.Properties["DevDay"] != null)
                {
                    string min = Application.Current.Properties["DevDay"].ToString();
                    TimeSpan timeSpan = new TimeSpan(int.Parse(min), 0, 0, 0);
                    dt = dt - timeSpan;
                }
                var viewtime = dt.ToString("yyyy/MM/dd HH:mm:ss");
                this.Dispatcher.Invoke((Action)(() =>
                {
                    NowTime.Text = viewtime;
                }));
            };
            timer.Start();

            //強震モニタ取得の頻度（1000ミリ秒=1秒）
            Timer KMoniTimer = new Timer(1000);

            KMoniTimer.Elapsed += (sender, e) =>
            {
                Task.Run(async () =>
                {
                    await GetKyoshinMonitor(dt);
                });
            };
            KMoniTimer.Start();
        }

        public async Task GetKyoshinMonitor(DateTime dateTime)
        {
            //強震モニタからデータ取得
            try
            {
                using var webApi = new WebApi();

                var result = await webApi.ParseScaleFromParameterAsync(points, dateTime);


                if (result.Data == null)
                {
                    Console.WriteLine("取得に失敗しました");
                    return;
                }

                int iii = 0;
                int num = 0;

                foreach (var point in result.Data)
                {
                    foreach (var PointNum in UserPointInt)
                    {
                        if (PointNum == iii)
                        {
                            var shindo = point.GetResultToIntensity();
                            this.Dispatcher.Invoke((Action)(() =>
                            {
                                Control_Names[num].Text = $"{point.ObservationPoint.Region} {point.ObservationPoint.Name}";
                                Control_Numbers[num].Text = String.Format("{0:0.0}", shindo);
                                System.Windows.Media.Color color = System.Windows.Media.Color.FromRgb(point.Color.R, point.Color.G, point.Color.B);
                                SolidColorBrush colorBrush = new SolidColorBrush(color);
                                Control_Lines[num].Stroke = colorBrush;
                            }));

                            if (shindo != null)
                            {
                                if ((shindo >= 0.0) && (shindo < 0.5))
                                {
                                    //震度0のうち、リアルタイム震度震度0.0以上
                                    this.Dispatcher.Invoke((Action)(() =>
                                    {
                                        Control_JmaInts[num].Text = "0";
                                        Control_JmaInts[num].Background = Int0;
                                        Control_JmaInts[num].Foreground = IntString0;
                                    }));
                                }
                                else if ((shindo >= 0.5) && (shindo < 1.5))
                                {
                                    //震度1
                                    this.Dispatcher.Invoke((Action)(() =>
                                    {
                                        Control_JmaInts[num].Text = "1";
                                        Control_JmaInts[num].Background = Int1;
                                        Control_JmaInts[num].Foreground = IntString1;
                                    }));
                                }
                                else if ((shindo >= 1.5) && (shindo < 2.5))
                                {
                                    //震度2
                                    this.Dispatcher.Invoke((Action)(() =>
                                    {
                                        Control_JmaInts[num].Text = "2";
                                        Control_JmaInts[num].Background = Int2;
                                        Control_JmaInts[num].Foreground = IntString2;
                                    }));
                                }
                                else if ((shindo >= 2.5) && (shindo < 3.5))
                                {
                                    //震度3
                                    this.Dispatcher.Invoke((Action)(() =>
                                    {
                                        Control_JmaInts[num].Text = "3";
                                        Control_JmaInts[num].Background = Int3;
                                        Control_JmaInts[num].Foreground = IntString3;
                                    }));
                                }
                                else if ((shindo >= 3.5) && (shindo < 4.5))
                                {
                                    //震度4
                                    this.Dispatcher.Invoke((Action)(() =>
                                    {
                                        Control_JmaInts[num].Text = "4";
                                        Control_JmaInts[num].Background = Int4;
                                        Control_JmaInts[num].Foreground = IntString4;
                                    }));
                                }
                                else if ((shindo >= 4.5) && (shindo < 5.0))
                                {
                                    //震度5弱
                                    this.Dispatcher.Invoke((Action)(() =>
                                    {
                                        Control_JmaInts[num].Text = "5-";
                                        Control_JmaInts[num].Background = Int50;
                                        Control_JmaInts[num].Foreground = IntString50;
                                    }));
                                }
                                else if ((shindo >= 5.0) && (shindo < 5.5))
                                {
                                    //震度5強
                                    this.Dispatcher.Invoke((Action)(() =>
                                    {
                                        Control_JmaInts[num].Text = "5+";
                                        Control_JmaInts[num].Background = Int55;
                                        Control_JmaInts[num].Foreground = IntString55;
                                    }));
                                }
                                else if ((shindo >= 5.5) && (shindo < 6.0))
                                {
                                    //震度6弱
                                    this.Dispatcher.Invoke((Action)(() =>
                                    {
                                        Control_JmaInts[num].Text = "6-";
                                        Control_JmaInts[num].Background = Int60;
                                        Control_JmaInts[num].Foreground = IntString60;
                                    }));
                                }
                                else if ((shindo >= 6.0) && (shindo < 6.5))
                                {
                                    //震度6強
                                    this.Dispatcher.Invoke((Action)(() =>
                                    {
                                        Control_JmaInts[num].Text = "6+";
                                        Control_JmaInts[num].Background = Int65;
                                        Control_JmaInts[num].Foreground = IntString65;
                                    }));
                                }
                                else if (shindo >= 6.5)
                                {
                                    //震度7
                                    this.Dispatcher.Invoke((Action)(() =>
                                    {
                                        Control_JmaInts[num].Text = "7";
                                        Control_JmaInts[num].Background = Int7;
                                        Control_JmaInts[num].Foreground = IntString7;
                                    }));
                                }
                                else
                                {
                                    //震度0未満
                                    this.Dispatcher.Invoke((Action)(() =>
                                    {
                                        Control_JmaInts[num].Text = "0";
                                        Control_JmaInts[num].Background = Int00;
                                        Control_JmaInts[num].Foreground = IntString00;
                                    }));
                                }
                            }
                            else
                            {
                                //震度情報なし（null）
                                this.Dispatcher.Invoke((Action)(() =>
                                {
                                    Control_JmaInts[num].Text = "?";
                                    Control_JmaInts[num].Background = IntU;
                                    Control_JmaInts[num].Foreground = IntStringU;
                                }));
                            }
                            num++;
                        }
                    }
                    iii++;
                }
                GC.Collect();
            }
            catch
            {

            }
        }

        public void ListList()
        {
            foreach (var Pref in Japan)
            {
                var aaa = new TreeViewItem();
                var str = Pref;
                aaa.Name = str;
                aaa.Header = str;
                setting.Dispatcher.Invoke((Action)(() =>
                {
                    setting.KyoshinPointList.Items.Add(aaa);
                }));
            }
            foreach (var point in points)
            {
                if (point.IsSuspended)
                {
                    continue;
                }
                string Pref = "";
                if (point.Region.Contains("北海道"))
                {
                    Pref = "北海道";
                }
                else if (point.Region.Contains("東京都"))
                {
                    Pref = "東京都";
                }
                else if (point.Region.Contains("長崎県"))
                {
                    Pref = "長崎県";
                }
                else
                {
                    Pref = point.Region;
                }
                setting.Dispatcher.Invoke((Action)(() =>
                {
                    TreeViewItem tbox = setting.KyoshinPointList.Items.OfType<TreeViewItem>().FirstOrDefault(c => c.Name == $"{Pref}");
                    if (tbox != null)
                    {
                        tbox.Items.Add($"{point.Region} {point.Name}");
                    }
                }));
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            setting.Show();
            setting.Activate();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ListList();
            GetUpdate();
        }

        public void CreateControl(int id)
        {
            var grid_ = new Grid();
            grid_.Name = $"Point{id}";
            grid_.Height = 155;
            grid_.Width = 600;
            grid_.Margin = new Thickness(5);
            grid_.VerticalAlignment = VerticalAlignment.Top;

            var border = new Border();
            border.BorderBrush = Brushes.Black;
            border.Background = backgroundColor;
            border.BorderThickness = new Thickness(2);
            border.CornerRadius = new CornerRadius(10);

            var tbox1 = new TextBlock();
            tbox1.Text = "強震モニタ観測点名";
            tbox1.Margin = new Thickness(5, 20, 0, 0);
            tbox1.Style = (Style)(this.Resources["Theme"]);
            tbox1.FontSize = 15;

            var tbox2_Name = new TextBlock();
            tbox2_Name.Name = $"P{id}_Name";
            tbox2_Name.Margin = new Thickness(155, 5, 0, 0);
            tbox2_Name.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/#Noto Sans CJK JP Bold");
            tbox2_Name.Text = "";
            tbox2_Name.Style = (Style)(this.Resources["Theme"]);
            tbox2_Name.FontSize = 30;

            var tbox3 = new TextBlock();
            tbox3.Text = "リアルタイム震度";
            tbox3.Margin = new Thickness(5, 100, 0, 0);
            tbox3.Style = (Style)(this.Resources["Theme"]);
            tbox3.FontSize = 20;

            var tbox4_Number = new TextBlock();
            tbox4_Number.Name = $"P{id}_Shindo";
            tbox4_Number.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/#Noto Sans CJK JP Bold");
            tbox4_Number.Text = "";
            tbox4_Number.Margin = new Thickness(180, 52, 0, 0);
            tbox4_Number.Style = (Style)(this.Resources["Theme"]);
            tbox4_Number.FontSize = 70;

            var tbox5 = new TextBlock();
            tbox5.Text = "気象庁震度階級";
            tbox5.Margin = new Thickness(340, 100, 0, 0);
            tbox5.Style = (Style)(this.Resources["Theme"]);
            tbox5.FontSize = 20;

            var tbox6_JmaInt = new TextBlock();
            tbox6_JmaInt.Text = "";
            tbox6_JmaInt.Style = (Style)(this.Resources["Theme"]);
            tbox6_JmaInt.FontSize = 80;
            tbox6_JmaInt.TextAlignment = TextAlignment.Center;
            tbox6_JmaInt.Width = 100;
            tbox6_JmaInt.Height = 100;
            tbox6_JmaInt.Margin = new Thickness(0, 0, 5, 5);
            tbox6_JmaInt.Background = Brushes.MidnightBlue;
            tbox6_JmaInt.HorizontalAlignment = HorizontalAlignment.Right;
            tbox6_JmaInt.VerticalAlignment = VerticalAlignment.Bottom;

            var line = new Line();
            line.Name = $"P{id}_Color";
            line.Stroke = Brushes.Black;
            line.X1 = 5;
            line.X2 = 320;
            line.Y1 = 143;
            line.Y2 = 143;
            line.StrokeThickness = 10;

            var MainGrid_ = new Grid();
            MainGrid_.Children.Add(tbox1);
            MainGrid_.Children.Add(tbox2_Name);
            MainGrid_.Children.Add(tbox3);
            MainGrid_.Children.Add(tbox4_Number);
            MainGrid_.Children.Add(tbox5);
            MainGrid_.Children.Add(tbox6_JmaInt);
            MainGrid_.Children.Add(line);

            border.Child = MainGrid_;

            grid_.Children.Add(border);
            stackpanel.Children.Add(grid_);

            Array.Resize(ref Control_Names, Control_Names.Length + 1);
            Control_Names[Control_Names.Length - 1] = tbox2_Name;
            Array.Resize(ref Control_Numbers, Control_Numbers.Length + 1);
            Control_Numbers[Control_Numbers.Length - 1] = tbox4_Number;
            Array.Resize(ref Control_JmaInts, Control_JmaInts.Length + 1);
            Control_JmaInts[Control_JmaInts.Length - 1] = tbox6_JmaInt;
            Array.Resize(ref Control_Lines, Control_Lines.Length + 1);
            Control_Lines[Control_Lines.Length - 1] = line;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            Process.Start("KyoshinQuickly.exe");
            Application.Current.Shutdown();
        }
        private async void GetUpdate()
        {
            try
            {
                var timeurl = $"https://ameuma773.github.io/EarthQuickly/KyoshinQuicklyUpdateInfo.txt";
                var NewVer = await client.GetStringAsync(timeurl);
                var NewVerTxt = NewVer.Trim();

                if (NewVerTxt != ver)
                {
                    //アップデートがある場合
                    setting.Dispatcher.Invoke((Action)(() =>
                    {
                        setting.NewVerInfo.Text = $"最新のバージョン {NewVerTxt} が公開されています";
                    }));
                    MessageBox.Show($"最新のバージョン {NewVerTxt} が公開されています。", "KyoshinQuickly", MessageBoxButton.YesNo, MessageBoxImage.Information);
                }
                else
                {
                    setting.Dispatcher.Invoke((Action)(() =>
                    {
                        setting.NewVerInfo.Text = "";
                    }));
                }
            }
            catch (Exception e)
            {
                setting.Dispatcher.Invoke((Action)(() =>
                {
                    setting.NewVerInfo.Text = $"更新の有無を取得できませんでした";
                }));
                MessageBox.Show($"最新のアップデートを取得できませんでした。\n{e.Message}", "KyoshinQuickly", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

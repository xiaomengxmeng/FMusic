using NAudio.Wave;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Color = System.Windows.Media.Color;
using ColorConverter = System.Windows.Media.ColorConverter;
using TagLib;

namespace FMusic
{
    /// <summary>
    /// Music.xaml 的交互逻辑git?
    /// </summary>
    public partial class Music : Window
    {
        const string pathFile = "D:\\SunTalk\\FMusic\\path\\";
        const string MusicFiles = "D:\\SunTalk\\FMusic\\MusicFiles\\";
        const string lrcpath = "D:\\SunTalk\\FMusic\\lrc\\";
        const string muscoverpath = "D:\\SunTalk\\FMusic\\music_cover\\";

        string lrccolor = "#00BFFF";
        private string[] Files = Directory.GetFiles(MusicFiles);//读取所有音乐路径
        List<string> musicfiles = new List<string>();
        List<string> lrcList = new List<string>(); //初始化歌单目录
        DispatcherTimer timer = new DispatcherTimer();//申明时间控件
        public bool mucply_flag = false; //初始化音乐播放标签
        public bool love_flag = false;//我的喜欢点击图标变化标签
        public string muPath;//初始化双击音乐列表播放路径
        public int mucpattern_flag = 1;//初始化音乐模式（1为顺序模式）
        public Music()
        {
            
            InitializeComponent();
           
            
        }
       


        private void LoadFiles()
        {
            MusicList.Items.Clear();

            //string[] Files = Directory.GetFiles(MusicFiles);

            foreach(string filePath in Files)
            {
                string fileName = System.IO.Path.GetFileName(filePath); //文件名带拓展名
                TreeViewItem mus = new TreeViewItem();
                string file = System.IO.Path.GetFileNameWithoutExtension(fileName);//文件名不带拓展名
                mus.Header = file;
                mus.Tag = fileName;
                mus.Name = MusicList.Name;
                MusicList.Items.Add(mus);

                mus.MouseDoubleClick += Mus_MouseDoubleClick;

                
            }
            loveupdate();
        }
        private void loveupdate()
        {
            //使用List类来初始化一个string列表
            List<string> musicFilePaths = new List<string>();
            //string[] musicFilePaths = new string[0]; // 初始化一个空的数组

            using (StreamReader reader = new StreamReader(pathFile + "lovePath.txt"))
            {
                string line;
                
                while ((line = reader.ReadLine()) != null) // 逐行读取文件内容
                {
                    musicFilePaths.Add(line); 
                    
                }
            }

            LoveList.Items.Clear();


            foreach (string filePath in musicFilePaths)
            {
                string fileName = System.IO.Path.GetFileName(filePath); //文件名带拓展名
                TreeViewItem mus = new TreeViewItem();
                string file = System.IO.Path.GetFileNameWithoutExtension(fileName);//文件名不带拓展名
                mus.Header = file;
                mus.Tag = fileName;
                mus.Name = LoveList.Name;
                LoveList.Items.Add(mus);

                mus.MouseDoubleClick += Mus_MouseDoubleClick;


            }
        }

        private void Mus_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem MucList = sender as TreeViewItem;
            muPath = MusicFiles + MucList.Tag;
            musicPlay(muPath, out lrcList);
            StartBtn.Background = new ImageBrush(new BitmapImage(new Uri("D:\\SunTalk\\FMusic\\img\\pause.png", UriKind.Relative)));
            StartBtn.Tag = "false";

        }

        private void musicPlay(string path, out List<string> mlrc)
        {
            string musicChat = "";
            //获取播放的音乐路径
            FileInfo file = new FileInfo(path);
            //根据路径找音乐名
            string musicName = file.Name.Substring(0, file.Name.LastIndexOf('.'));
            //把音乐名添加进去
            int size = 40;
            if (musicName.Length >= size)
            {
                string temp = musicName.Substring(0, size);
                
                MusName.Content = temp;
            }
            else
            {
                MusName.Content = musicName;
            }


            //添加音乐封面
            
            


            //FileInfo coverInfo = new FileInfo(muscoverpath + musicName + ".png");
            //if(coverInfo.Exists)
            //{
            //    Muscover.ImageSource = new BitmapImage(new Uri(muscoverpath + musicName + ".png", UriKind.Relative));
            //    Muscover.Stretch = Stretch.Fill;
            //}
            //else
            //{
            //    Muscover.ImageSource = new BitmapImage(new Uri("D:\\SunTalk\\FMusic\\img\\king.png", UriKind.Relative));
            //    Muscover.Stretch = Stretch.Fill;
            //}
            {
                //var cover = TagLib.File.Create(path);
                //var coverData = cover.Tag.Pictures[0].Data.Data;
                //这将使用Base64编码将“coverData”转换为字符串，并将其包装在“data:image/png;base64”URI中
                //。然后，将此URI传递给BitmapImage的构造函数，以将其用作图像源。
                //var uri = new System.Uri("data:image/png;base64," + Convert.ToBase64String(coverData));
                //Muscover.ImageSource = new BitmapImage(uri);

                //var song = TagLib.File.Create(path);




                var cover = TagLib.File.Create(path);
                if (cover != null)
                    {
                        var coverData = cover.Tag.Pictures[0].Data.Data;
                        //var tempFile = System.IO.File.Create(System.IO.Path.Combine(System.IO.Path.GetTempPath(), "temp.jpg"));
                        var imageSource = new BitmapImage();
                        imageSource.BeginInit();
                        imageSource.StreamSource = new MemoryStream(coverData);
                        imageSource.EndInit();
                        Muscover.ImageSource = imageSource;
                        var image = new BitmapImage();
                        using (var stream = new MemoryStream(coverData))
                        {
                            image.BeginInit();
                            image.CacheOption = BitmapCacheOption.OnLoad;
                            image.StreamSource = stream;
                            image.EndInit();
                        }
                        Muscover.ImageSource = image;

                    }
                    else
                    {
                        Muscover.ImageSource = new BitmapImage(new Uri("D:\\SunTalk\\FMusic\\img\\king.png", UriKind.Relative));
                        Muscover.Stretch = Stretch.Fill;
                    }


            }

                //找到音乐对应的歌词包括扩展名
                FileInfo info = new FileInfo(lrcpath + musicName + ".lrc");
            //判断是否存在
            if (info.Exists)
            {
                StreamReader reader = new StreamReader(lrcpath + musicName + ".lrc", Encoding.UTF8);
                //读取全部歌词
                musicChat = reader.ReadToEnd();

            }
            mlrc = musicChat.Split('\n').ToList();
           
            timer.Start();
            mediaElement.Source = new Uri(path, UriKind.Relative);
            mediaElement.Play();
            mucply_flag = true;
        }

        private void NewMethod(List<string> list, bool isDr)
        {
            foreach(string item in list)
            {
                FileInfo info = new FileInfo(item);
                bool n = false;
                foreach (TreeViewItem tree in MusicList.Items)
                {
                    if (tree.Header.ToString() == info.Name)
                        n = true; //判重
                }
                if (!n)
                {
                    TreeViewItem mus = new TreeViewItem();
                    string str = System.IO.Path.GetFileNameWithoutExtension(item);
                    mus.Header = str;
                    mus.Tag = str + System.IO.Path.GetExtension(item);
                    mus.Name = MusicList.Name;

                    MusicList.Items.Add(mus);
                    mus.MouseDoubleClick += Mus_MouseDoubleClick;

                    info.CopyTo(MusicFiles + info.Name, true);
                    string name = item.Substring(0, item.LastIndexOf('.'));
                    FileInfo lrcInfo = new FileInfo(name + ".lrc");
                    if (lrcInfo.Exists)
                    {
                        lrcInfo.CopyTo(lrcpath + lrcInfo.Name);
                    }
                }
            }
            Files = Directory.GetFiles(MusicFiles);

        }

        private void Mus_MouseDoubleClick1(object sender, MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadFiles();

            mediaElement.LoadedBehavior = MediaState.Manual;
            mediaElement.MediaOpened += MediaElement_MediaOpened;
            mediaElement.MediaEnded += Media_MediaEnded;

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;

        }
        private void Timer_Tick(object? sender, EventArgs e)
        {
            NowTime.Content = mediaElement.Position.ToString("hh':'mm':'ss");
            sliderPosition.Value = mediaElement.Position.TotalSeconds;   // 播放器进度条
            SetChat();
        }
        private void SetChat()
        {
            var item = shu.SelectedItem;
            TreeViewItem? Muclist = item as TreeViewItem;
            for (int i = 0; i < lrcList.Count; i++)
            {
                double nowt = mediaElement.Position.TotalMilliseconds;
                string geci = "";
                lbLrc.Text = geci;
                if (lrcList[i] != null)
                {
                    double current = GetLrcTime(i, out geci,1);
                    if (i < lrcList.Count - 1)
                    {
                        double next = GetLrcTime(i + 1, out string geci2,2);
                        lbLrc.Text = geci;
                        lbLrc2.Text = geci2;
                        if (current <= nowt && nowt < next)
                        {
                            lbLrc.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(lrccolor));
                            return;
                        }
                    }
                    else
                    {
                        lbLrc.Text = geci;
                    }
                }
                
            }

        }

        private double GetLrcTime(int index, out string chat,int flag)
        {
            double time = 0;
            chat = "";
            string[] everyChat = lrcList[index].Split(new char[] { '[', ']' });
            string pattern = @"^(\d{2}:){1,2}(\d{2}.\d{2})$";
            //string pattern = @"^(\d{2}:){1,2}(\d{2,3}.\d{2})$";
           // string pattern = @"^(\d{2}:\d{2}.)(\d{2})$";
          //  string pattern1 = @"^(\d{2}:\d{2}.)(\d{2})$";
            string pattern1 = @"^(\d{2})(:?\d{2}(.\d{2}))?$";
            try
            {
              //  if (Regex.IsMatch(everyChat[1], pattern)|| Regex.IsMatch(everyChat[1], pattern1))
                {
                    chat = everyChat[2];
                    string t = everyChat[1];
                    if (everyChat[1].Length <= 8)
                    {
                        t = "00:" + everyChat[1];
                    }
                    TimeSpan ts;
                    if (TimeSpan.TryParse(t, out ts))
                    {
                        time = ts.TotalMilliseconds;
                    }
                }
            }
            catch
            {
                if (flag == 1) 
                { 
                     chat = "未找到对应歌词";
                }
                else
                {
                    chat = " ";
                }
            }
            

            return time;
        }
       


        private void MediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
             TimeSpan time = mediaElement.NaturalDuration.TimeSpan;
            
            sliderPosition.Maximum = time.TotalSeconds;
            EndTime.Content = time.ToString("hh':'mm':'ss");
        }

        //结束根据播放模式，播放下一首歌
        private void Media_MediaEnded(object sender, RoutedEventArgs e)
        {
            int Music_currentcount = 0;
            string muPath = "";
            DirectoryInfo directory = new DirectoryInfo(MusicFiles);

            FileInfo[] files = directory.GetFiles();

            List<string> fileNames = new List<string>();
            for (int i = 0; i < files.Length; i++)
            {
                string name = files[i].Name;
                fileNames.Add(name);
                if (fileNames[i] == MusName.Content.ToString() + ".mp3"|| fileNames[i] == MusName.Content.ToString() + ".flac")
                    Music_currentcount = i;
            }
            Random random = new Random();
            int rand_number = random.Next(files.Length);
            if (mucpattern_flag == 2) //随机模式
            {
                muPath = MusicFiles + fileNames[rand_number];
                musicPlay(muPath, out lrcList);
            }
            else if (mucpattern_flag == 3)  //单曲循环
            {
                mediaElement.Stop();
                mediaElement.Play();
            }
            else if (mucpattern_flag == 1)  //顺序模式
            {
                if (Music_currentcount == files.Length - 1)
                    Music_currentcount = -1;
                muPath = MusicFiles + fileNames[Music_currentcount + 1];
                musicPlay(muPath, out lrcList);
            }
        }

       

        private void SetBackGround1_Click(Object sender, RoutedEventArgs e)
        {
            //水绿色
            title_bck.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffffff"));
            menu_bck.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E6755F"));
            shu.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8DB799"));
            mucic_bck.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8Db799"));
            lrccolor = "#8Db799";
        }
        private void SetBackGround2_Click(Object sender, RoutedEventArgs e)
        {
            //玉山背景
            title_bck.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
            menu_bck.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#719847"));
            shu.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F8F7F0"));
            mucic_bck.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F8F7F0"));
            lrccolor = "#F8F7F0";
        }
        private void SetBackGround3_Click(Object sender, RoutedEventArgs e)
        {
            //雪青背景
            title_bck.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
            menu_bck.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5f479a"));
            shu.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A59ACA"));
            mucic_bck.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A59ACA"));
            lrccolor = "#A59ACA";
        }
        private void Bckresrt_Click(Object sender, RoutedEventArgs e)
        {
            //还原背景
            title_bck.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#a1afc9"));
            menu_bck.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ed5736"));
            shu.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CFE0F7"));
            mucic_bck.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD9E4F1"));
            lrccolor = "#00BFFF";
        }
        private void Fontsizeup_Click(Object sender, RoutedEventArgs e)
        {
            shu.FontSize += 5;
        }
        private void Fontsizedown_Click(Object sender, RoutedEventArgs e)
        {
            shu.FontSize -= 5;
        }
        private void Fontback_Click(Object sender, RoutedEventArgs e)
        {
            shu.FontSize = 14;
        }
        //删除
        private void ContextMenu1_Click(Object sender, RoutedEventArgs e)
        {
            var item = shu.SelectedItem;
            MusicList.Items.Remove(item);

        }
        //删除文件
        private void ContextMenu2_Click(Object sender, RoutedEventArgs e)
        {
            var item = shu.SelectedItem;
            TreeViewItem Muclist = item as TreeViewItem;
            FileInfo info = new FileInfo(MusicFiles + Muclist.Tag);
            info.Delete();
            Muclist.Items.Remove(Muclist.Header);
        }
        //添加到喜欢
        private void ContextMenu3_Click(Object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)shu.SelectedItem;
            
            

            LoveJson(MusicFiles + item.Tag);
        }

        private void Audioview(object sender, MouseButtonEventArgs e)
        {
            //音频可视化
        }
        ImageBrush brush;   //声明brush
        private void DownBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            int Music_currentcount = 0;
            Border img = sender as Border;
            brush = img.Background as ImageBrush;

            var item = shu.SelectedItem;
            TreeViewItem Muclist = item as TreeViewItem;
            
            DirectoryInfo directory = new DirectoryInfo(MusicFiles);
            FileInfo[] files = directory.GetFiles();
            List<string> fileNames = new List<string>();
            for (int i = 0; i < files.Length; i++)
            {
                fileNames.Add(files[i].Name);
                if (fileNames[i] == MusName.Content.ToString() + ".mp3" || fileNames[i] == MusName.Content.ToString() + ".flac")
                    Music_currentcount = i;
            }
            Random random = new Random();
            int rand_number = random.Next(files.Length);
            if (img.Name == "UpBtn")
            {
                StartBtn.Background = new ImageBrush(new BitmapImage(new Uri("D:\\SunTalk\\FMusic\\img\\pause.png", UriKind.Relative)));
                StartBtn.Tag = "false";

                if (mucpattern_flag == 2) //随机模式
                {
                    muPath = muPath + fileNames[rand_number];
                    musicPlay(muPath, out lrcList);
                }
                else if (mucpattern_flag == 3) //单曲循环
                {
                    mediaElement.Stop();
                    mediaElement.Play();
                }
                else if (mucpattern_flag == 1) //判断是否当前播放是否为播放列表第一个，实现列表循环播放
                {
                    if (Music_currentcount == 0)
                        Music_currentcount = files.Length;
                    muPath = MusicFiles + fileNames[Music_currentcount - 1];
                    musicPlay(muPath, out lrcList);
                }
            }
            else if (img.Name == "StartBtn")
            {
                if (img.Tag.ToString() == "true")
                {
                    img.Tag = "false";
                    mediaElement.Play();
                    img.Background = new ImageBrush(new BitmapImage(new Uri("D:\\SunTalk\\FMusic\\img\\pause.png", UriKind.Relative)));
                }
                else if (img.Tag.ToString() == "false")
                {
                    mediaElement.Pause();
                    img.Tag = "true";
                    img.Background = new ImageBrush(new BitmapImage(new Uri("D:\\SunTalk\\FMusic\\img\\play.png", UriKind.Relative)));
                }
            }
            else if (img.Name == "DownBtn")
            {
                StartBtn.Background = new ImageBrush(new BitmapImage(new Uri("D:\\SunTalk\\FMusic\\img\\pause.png", UriKind.Relative)));
                StartBtn.Tag = "false";

                if (mucpattern_flag == 2) //随机模式
                {
                    muPath = MusicFiles + fileNames[rand_number];
                    musicPlay(muPath, out lrcList);
                }
                else if (mucpattern_flag == 3)  //单曲循环
                {
                    mediaElement.Stop();
                    mediaElement.Play();
                }
                else if (mucpattern_flag == 1)  //顺序模式
                {
                    if (Music_currentcount == files.Length - 1)
                        Music_currentcount = -1;
                    muPath = MusicFiles + fileNames[(Music_currentcount + 1)];
                    musicPlay(muPath, out lrcList);
                }
            }
        }
        private void DownBtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Border? img = sender as Border;
            if (img.Name == "StartBtn")
            {
                return;

            }
            img.Background = brush;
        }
        private void import_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "(*.mp3,*.flac)|*.mp3;*.flac;";
            dialog.Multiselect = true;
            dialog.InitialDirectory = "D:\\SunTalk\\FMusic";

            DialogResult result = dialog.ShowDialog();

            if(result==System.Windows.Forms.DialogResult.OK)
            {
                NewMethod(dialog.FileNames.ToList(), true);
            }
        }
        private void exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void clear_Click(object sender, RoutedEventArgs e)
        {
            MusicList.Items.Clear();
        }
        private void Love_Click(object sender, RoutedEventArgs e)
        {
            
            string lovepath = muPath;  //要添加到love的文件路径

            LoveJson (lovepath);
           

        }

        private void LoveJson(string lovepath)
        {
            // 将音乐文件路径列表转换为字符串
            string String = lovepath;
            bool lovecheck = false;
            foreach(string file in Files)
            {
                if (file == String)
                {
                    lovecheck = true;
                }
            }
            // 将字符串写入文件中
            if(lovecheck)
                using (StreamWriter writer = new StreamWriter(pathFile+"lovePath.txt",true))
                {
                    // 将新的文本内容追加到文件末尾，并保留原有的换行符
                    writer.WriteLine(String);//写入到文件流中。 WriteLine 与前面的换行
                    writer.Flush();//强制将缓冲区中的数据写入到文件流中，以确保数据被完整地写入文件。
                    writer.Close();
                }
            loveupdate();
        }

        private void sliderPosition_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaElement.Position=TimeSpan.FromSeconds(sliderPosition.Value);
        }
        //音乐模式选择
        private void Mucpattern_Click(object sender, RoutedEventArgs e)
        {
            if (mucpattern_flag == 1)
            {
                pattern_txt.Source = new BitmapImage(new Uri("D:\\SunTalk\\FMusic\\img\\shuffle.png"));  //随机模式
                mucpattern_flag = 2;
            }
            else if (mucpattern_flag == 2)
            {
                pattern_txt.Source = new BitmapImage(new Uri("D:\\SunTalk\\FMusic\\img\\play-once.png"));  //循环模式
                mucpattern_flag = 3;
            }
            else if (mucpattern_flag == 3)
            {
                pattern_txt.Source = new BitmapImage(new Uri("D:\\SunTalk\\FMusic\\img\\play-cycle.png"));  //顺序模式
                mucpattern_flag = 1;
            }
        }
        //静音按键
        private void Mute_Click(object sender, RoutedEventArgs e)
        {
            valu.Value= 0;
        }
        private void LrcBackGround1_Click(Object sender, RoutedEventArgs e)
        {
            lrccolor = "#8B0000";// 暗红色：
        }
        private void LrcBackGround2_Click(Object sender, RoutedEventArgs e)
        {
            lrccolor = "#FFC0CB";//粉红色
        }
        private void LrcBackGround3_Click(Object sender, RoutedEventArgs e)
        {
            lrccolor = "#FFA500";//橙色
        }
        private void lrcBckresrt_Click(object sender,RoutedEventArgs e)
        {
            lrccolor = "#00BFFF";//青绿色：#00BFFF
        }
    }
}

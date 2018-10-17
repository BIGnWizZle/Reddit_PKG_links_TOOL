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
using System.Windows.Threading;

namespace Reddit_PKG_links_TOOL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow AppWindow;

        public MainWindow()
        {
            InitializeComponent();
            AppWindow = this;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() => { Workers.getJSON(); }), DispatcherPriority.ContextIdle, null);

            //Thread thread = new Thread(() => { Workers.getJSON(); });
            //thread.Start();


        }

        private void GetButtonINFO(object sender, RoutedEventArgs e)
        {

            StackPanel stackpanel = (StackPanel)((Button)sender).Content;
            Image pic = (Image)stackpanel.Children[0];
            TextBlock TITLE = (TextBlock)stackpanel.Children[1];
            TextBlock TITLEcut = (TextBlock)stackpanel.Children[2];
            TextBlock Reaction = (TextBlock)stackpanel.Children[3];
            Download DownloadFORM = new Download();
            DownloadFORM.ItemTitle = TITLE.Text;
            DownloadFORM.TITLEcut = TITLEcut.Text;
            DownloadFORM.Description = Reaction.Text;
            DownloadFORM.PIC = pic.Source;
            DownloadFORM.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //DownloadControl window = new DownloadControl();
            //window.Show();
        }
    }
}

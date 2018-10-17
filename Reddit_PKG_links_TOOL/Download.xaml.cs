using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Reddit_PKG_links_TOOL
{
    /// <summary>
    /// Interaction logic for Download.xaml
    /// </summary>
    public partial class Download : Window
    {
        public Download()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            MessageBox.Show(Button1.Tag.ToString());
        }

       
        public string ItemTitle
        {
            get { return label1.Content.ToString(); }
            set { label1.Content = value; }
        }
        public string Description
        {
            get { return richTextBox1.ToString(); }
            set { richTextBox1.AppendText(value); }
        }
        public string TITLEcut
        {
            get { return label2.Content.ToString(); }
            set { label2.Content = value; }
        }
        public System.Windows.Media.ImageSource PIC
        {
            //get { return pictureBox1.; }
            set
            {
                try
                {
                    Image1.Source = value;
                }
                catch { }
            }
        }

        public void getlinks()
        {
            List<string> test = new List<string>();
            String[] lines =
        richTextBox1.ToString().Split(new[] { Environment.NewLine }
                                          , StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                try
                {
                    string ToBeParsed = line.Trim();
                    var result = (ToBeParsed.Length > 20) && Regex.IsMatch(ToBeParsed, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
                    if (result.ToString() == "True")
                    {
                        byte[] data = Convert.FromBase64String(line);

                        test.Add(Encoding.UTF8.GetString(data));
                    }
                }
                catch
                { //NO BASE64}
                }
            }
            Button1.Content = ("FOUND LINKS: " + test.Count());
            Button1.Tag = String.Join(",", test);

        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
           
                getlinks();
            
        }
    }
}

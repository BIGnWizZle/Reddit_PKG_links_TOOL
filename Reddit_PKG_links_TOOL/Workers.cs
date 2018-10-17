using Newtonsoft.Json.Linq;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Reddit_PKG_links_TOOL
{
    class Workers
    {
        public class TodoItem
        {
            public string PKG_PIC { get; set; }
            public string PKG_TITLE { get; set; }
            public string PKG_CUSA { get; set; }
            public string PKG_TITLEcut { get; set; }
            public string PKG_Download { get; set; }
        }
        public static class Globals
        {
            public static string lastvalue = "None";
        }

        public static string RemoveBetween(string s, char begin, char end)
        {
            Regex regex = new Regex(string.Format("\\{0}.*?\\{1}", begin, end));
            return regex.Replace(s, string.Empty);
        }

        public static void getJSON()
        {
            //download PkgLinks json
            var json = "";
            JObject JsonObject;
            using (WebClient wc = new WebClient())
            {
                if (Globals.lastvalue == "None")
                {
                    json = wc.DownloadString("https://www.reddit.com/r/PkgLinks/new.json?sort=new&limit=" + MainWindow.AppWindow.Results.Text);
                    JsonObject = JObject.Parse(json);


                }
                else
                {
                    json = wc.DownloadString("https://www.reddit.com/r/PkgLinks/new.json?after=" + Globals.lastvalue + "&limit=" + MainWindow.AppWindow.Results.Text);
                    JsonObject = JObject.Parse(json);
                }
            }
            Globals.lastvalue = JsonObject["data"]["children"].Last()["data"]["name"].ToString();


            //results contains posts 
            var results = JsonObject["data"]["children"].Children();
            foreach (JObject Child in results)
            {
                List<Workers.TodoItem> items = new List<Workers.TodoItem>();

                string PKG_Header = (string)Child["data"]["title"];
                string PKG_Content = (string)Child["data"]["selftext_html"];
                //detect if CUSA
                string CUSA = Get_PKG_CUSA(PKG_Header);

                MatchCollection matchList = Regex.Matches(PKG_Header, @"\[.*?\]|\(.*?\)");
                //var list = matchList.Cast<Match>().Select(match => match.Value).ToList();

                string Brackets = String.Join(",  ", matchList.Cast<Match>().Select(match => match.Value).ToList().ToArray());
                //MessageBox.Show(box);
                if (CUSA != "NOT FOUND")
                {
                    items.Add(new TodoItem() { PKG_PIC = Get_PKG_PIC(CUSA), PKG_TITLE = Get_PKG_TITLE(PKG_Header), PKG_TITLEcut = Brackets, PKG_Download = HTMLdecode(PKG_Content) });
                    MainWindow.AppWindow.ItemDisplay.Items.Add(items);
                    //MainWindow.AppWindow.ItemDisplay.UpdateLayout();


                }

                //no CUSA = ps2,theme,TOOL, ...
                else
                {
                    var PS2 = new[] { "ps2", "slus", "sles", "scus" };

                    //PS2
                    if (PS2.Any(PKG_Header.ToLower().Contains))
                    {
                        items.Add(new TodoItem() { PKG_PIC = AppDomain.CurrentDomain.BaseDirectory + @"\Thumbs\Notfoundps2.jpg", PKG_TITLE = PKG_Header });

                        {
                            MainWindow.AppWindow.ItemDisplay1.Items.Add(items);
                        }

                    }
                    //theme, tool , ..
                    else
                    {
                        items.Add(new TodoItem() { PKG_PIC = AppDomain.CurrentDomain.BaseDirectory + @"\Thumbs\Notfound1.jpg", PKG_TITLE = PKG_Header });

                        {
                            MainWindow.AppWindow.ItemDisplay2.Items.Add(items);


                        }
                    }
                }
            }
        }


        public static string HTMLdecode(string Parameter)
        {
            //WebUtility.HtmlEncode();
            //WebUtility.HtmlDecode();

            string uno = WebUtility.HtmlDecode(Parameter);
            return Regex.Replace(uno, "<.*?>", String.Empty);
        }

        public static string Get_PKG_PIC(string Parameter) //GET IMAGE FROM PS_STORE
        {
            string localfolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"\Thumbs\" + Parameter + ".png");
            if (File.Exists(localfolder))
            {
                return localfolder;
            }
            else
            {
                try
                {
                    using (WebClient wc = new WebClient())
                    {
                        JObject jObject = JObject.Parse(wc.DownloadString("https://store.playstation.com/store/api/chihiro/00_09_000/titlecontainer/US/en/999/" + Parameter + "_00"));
                        //the result of the htmlwork
                        string image = (string)jObject.SelectToken("images[2].url").ToString();

                        WebClient webClient = new WebClient();
                        webClient.DownloadFile(image, localfolder);
                      

                        return localfolder;
                    }
                }
                catch
                {

                    return AppDomain.CurrentDomain.BaseDirectory + @"\Thumbs\Notfound1.jpg";

                }
            }

        }

        public static string Get_PKG_CUSA(string Parameter) //GET PKG CUSA ID
        {
            string outputString, CUSA;

            //check if header contains CUSA
            int ix = Parameter.IndexOf("CUSA");
            if (ix != -1)
            {
                try
                {
                    outputString = Parameter.Substring(ix, 10);

                }
                catch
                {

                    outputString = Parameter.Substring(ix, 9);

                }
                Regex re = new Regex("[^A-Za-z0-9]");
                CUSA = re.Replace(outputString, string.Empty);
                //MessageBox.Show(CUSA);
                return CUSA;
            }

            //no CUSA = ps2,theme,TOOL, ...
            else
            {
                return "NOT FOUND";
            }
        }

        public static string Get_PKG_TITLE(string Parameter) //GET PKG CLEAN TITLE
        {
            string header = Parameter;
            header = RemoveBetween(header, '(', ')');
            header = RemoveBetween(header, '[', ']');


            return header;
        }

        public static void getITems() //BUTTON.CLICK
        { }

    }
}


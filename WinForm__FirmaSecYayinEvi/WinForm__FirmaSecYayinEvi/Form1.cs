using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForm__FirmaSecYayinEvi
{
    public partial class Form1 : Form
    {

        public class FirmModel
        {
            public String Name;
            public String URL;
            public String EmailAddress;
        }

        public int[] intervals = new int[5] { 15000, 12000, 8000, 10000, 13000 };
        public List<FirmModel> firms = new List<FirmModel>();

        int page = 1;
        int firmCount = 1;

        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }


        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (textBox1.Text.Length == 0)
                    textBox1.Text = "http://www.firmasec.com/firma/ara/yay%C4%B1nevi/istanbul/" + page;

                webBrowser1.Navigate(textBox1.Text);


                //webBrowser1.Navigate(url);
                webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(ListPageOpened);
            }
        }

        public void NavigateListPage()
        {
                webBrowser1.Navigate(textBox1.Text);
                webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(ListPageOpened);

        }

        public void NavigateDetailPage()
        {
            System.Threading.Thread.Sleep(intervals[firmCount % 5]);
            firmCount--;
            textBox1.Text = firms[firmCount].URL;
            webBrowser1.Navigate(firms[firmCount].URL);
            webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(DetailPageOpened);
        }

        private void DetailPageOpened(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
             var spans = webBrowser1.Document.GetElementsByTagName("span");

             foreach (HtmlElement span in spans)
             {
                 if (span.GetAttribute("className").Contains("marev"))
                 {
                     firms[firmCount].EmailAddress = span.InnerText;
                 }
             }

             if ((firmCount) == 0)
             {
                 richTextBox1.Text = JsonConvert.SerializeObject(firms);

             }
             else
             {

                 NavigateDetailPage();
             }


            webBrowser1.DocumentCompleted -= new WebBrowserDocumentCompletedEventHandler(DetailPageOpened);

        }

        private void ListPageOpened(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

            var divs = webBrowser1.Document.GetElementsByTagName("div");
            HtmlElement paginationDiv = null;
            bool isKeepGoing = true; ;

            foreach (HtmlElement div in divs)
            {
                //if (div.GetAttribute("className").Contains("pagination"))
                //{
                //    paginationDiv = div;
                //    continue;

                //}

                if (div.GetAttribute("className") == "list-item0")
                {

                    try
                    {
                        firms.Add(new FirmModel() { Name = div.GetElementsByTagName("A")[0].GetAttribute("title"), URL = div.GetElementsByTagName("A")[0].GetAttribute("href") });

                    }
                    catch (Exception)
                    {
                        isKeepGoing = false;
                    }
                    //HtmlElementCollection links = div.Children.GetElementsByName("a");
                    //foreach (HtmlElement link in links)
                    //{

                    //    firms.Add(new FirmModel() { Name = link.GetAttribute("title"), URL = link.GetAttribute("href") });
                    //}
                }
            }

            #region pagination for next page

            //if (paginationDiv != null)
            //{
            //    HtmlElementCollection links = paginationDiv.GetElementsByTagName("A");
            //    foreach (HtmlElement link in links)
            //    {

            //        firms.Add(new FirmModel() { Name = link.GetAttribute("title"), URL = link.GetAttribute("href") });
            //    }
            //}
            #endregion

            //HtmlElement tcknAraButon = webBrowser1.Document.GetElementById("btnGiris");
            //if (tcknAraButon != null)
            //{
            //    tcknAraButon.InvokeMember("click");

            //    while (webBrowser1.ReadyState != WebBrowserReadyState.Complete)
            //    {
            //        Application.DoEvents();
            //    }


            //    var asss = webBrowser1.Document;
            //}

            //webBrowser1.DocumentCompleted -= new WebBrowserDocumentCompletedEventHandler(GirisCompleted);
            //webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(b_DocumentCompleted2);

            

            webBrowser1.DocumentCompleted -= new WebBrowserDocumentCompletedEventHandler(ListPageOpened);

            isKeepGoing = false;
            if (isKeepGoing)
            {
                System.Threading.Thread.Sleep(intervals[firmCount % 5]);
                textBox1.Text = textBox1.Text.Replace("/" + page, "/" + (++page));
                NavigateListPage();
            }
            else
            {
                firmCount = firms.Count;
                NavigateDetailPage();
                //richTextBox1.Text = string.Join<string>("*", firms.Select(x => x.Name + "-" + x.URL + "-" + x.EmailAddress));
            }

        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            // Better use the e parameter to get the url.
            // ... This makes the method more generic and reusable.
            this.Text = e.Url.ToString() + " loaded";
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

    }
}

//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace WindowsFormsApplication1
//{
//    public partial class Form1 : Form
//    {
//        public Form1()
//        {
//            InitializeComponent();
//        }

//        private void Form1_Load(object sender, EventArgs e)
//        {

        
//        }

//        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
//        {
//            // Set text while the page has not yet loaded.
//            this.Text = "Navigating";
//        }

//        private void webBrowser1_DocumentCompleted(object sender,
//            WebBrowserDocumentCompletedEventArgs e)
//        {
//            // Better use the e parameter to get the url.
//            // ... This makes the method more generic and reusable.
//            this.Text = e.Url.ToString() + " loaded";
//        }

//        private void webBrowser1_DocumentCompleted_1(object sender, WebBrowserDocumentCompletedEventArgs e)
//        {

//        }

//        private void textBox1_TextChanged(object sender, EventArgs e)
//        {
            
//        }

//        private void textBox1_KeyDown(object sender, KeyEventArgs e)
//        {
//            if (e.KeyCode == Keys.Enter)
//            {
//                if(textBox1.Text.Length ==0)
//                    textBox1.Text = "http://www.firmasec.com/firma/ara/yay%C4%B1nevi/istanbul/1";
                                
//                //webBrowser1.Navigate("http://www.firmasec.com/firma/ara/yay%C4%B1nevi/istanbul/1");
//                webBrowser1.Navigate(textBox1.Text);
//            }
//        }


//    }
//}


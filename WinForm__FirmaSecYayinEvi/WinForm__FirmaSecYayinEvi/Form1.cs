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

        public int[] intervals = new int[5] { 58000, 53000, 42000, 46000, 55000 };
        public List<FirmModel> firms = new List<FirmModel>();
        public List<FirmModel> myFirms = new List<FirmModel>();

        int startPage = 1;
        int endPage = 5;
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
                firms = new List<FirmModel>();
                myFirms = new List<FirmModel>();

                startPage = Int32.Parse(txtStartPage.Text);
                endPage = Int32.Parse(txtEndPage.Text);

                if (textBox1.Text.Length == 0)
                    textBox1.Text = "http://www.firmasec.com/firma/ara/yay%C4%B1nevi/istanbul/" + startPage;

                webBrowser1.Navigate(textBox1.Text);


                //webBrowser1.Navigate(url);
                webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(ListPageOpened);
            }
        }

        public void NavigateListPage()
        {
            if ((startPage+1) <= endPage)
            {
                textBox1.Text = textBox1.Text.Replace("/" + startPage, "/" + (++startPage));
                System.Threading.Thread.Sleep(intervals[(firmCount + startPage) % 5]);
                webBrowser1.Navigate(textBox1.Text);
                webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(ListPageOpened);
            }
            else
            {
                firmCount = firms.Count;
                NavigateDetailPage();
            }
        }

        public void NavigateDetailPage()
        {
            try
            {
                System.Threading.Thread.Sleep(intervals[firmCount % 5]);
                textBox1.Text = firms[firmCount - 1].URL;
                webBrowser1.Navigate(textBox1.Text);
                webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(DetailPageOpened);
            }
            catch (Exception)
            {
                richTextBox1.Text += textBox1.Text;
                richTextBox1.Text = JsonConvert.SerializeObject(myFirms);
                webBrowser1.DocumentCompleted -= new WebBrowserDocumentCompletedEventHandler(DetailPageOpened);
            }
        }

        private void DetailPageOpened(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            var pList = GetElementByClassName(webBrowser1, "p", "item-details-left-p2");
            foreach (HtmlElement p in pList)
            {
                var sList = GetElementByClassName(p, "span", "marev");
                if (sList.Count > 0)
                {
                    firms[firmCount].EmailAddress = StringHelper.ReverseString(sList[0].InnerText.Trim());
                    myFirms.Add(firms[firmCount]);
                }
            }
            //var spans = webBrowser1.Document.GetElementsByTagName("p");

            //foreach (HtmlElement span in spans)
            //{
            //    if (span.GetAttribute("className").Contains("marev"))
            //    {
            //        firms[firmCount].EmailAddress = span.InnerText;
            //    }
            //}

            if ((firmCount--) == 0)
            {
                richTextBox1.Text += textBox1.Text;
                richTextBox1.Text = JsonConvert.SerializeObject(myFirms);
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
            bool isKeepGoing = false;

            //HtmlElement paginationDiv = null;


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
                        isKeepGoing = true;
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

            if (isKeepGoing)
            {
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
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }


        private List<HtmlElement> GetElementByClassName(WebBrowser browser, string tagName, string className)
        {
            List<HtmlElement> list = new List<HtmlElement>();
            var tagElements = browser.Document.GetElementsByTagName(tagName);
            foreach (HtmlElement tagElement in tagElements)
            {
                if (tagElement.GetAttribute("className").Contains(className))
                {
                    list.Add(tagElement);
                }
            }

            return list;
        }


        private List<HtmlElement> GetElementByClassName(HtmlElement element, string tagName, string className)
        {
            List<HtmlElement> list = new List<HtmlElement>();
            var tagElements = element.GetElementsByTagName(tagName);

            foreach (HtmlElement tagElement in tagElements)
            {
                if (tagElement.GetAttribute("className").Contains(className))
                {
                    list.Add(tagElement);
                }
            }

            return list;
        }

        static class StringHelper
        {
            public static string ReverseString(string s)
            {
                char[] arr = s.ToCharArray();
                Array.Reverse(arr);
                return new string(arr);
            }
        }

        private void richTextBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void txtEndPage_TextChanged(object sender, EventArgs e)
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


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
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
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


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

using System.Net;
using System.IO;

namespace StockTrack
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string stockVal;
        string date;
        string symbol;
        float quantity;

        public MainWindow()
        {
            InitializeComponent();
            textBlock.Foreground = Brushes.White;
            textBlocka.Foreground = Brushes.White;
            textBlockb.Foreground = Brushes.White;
            textBlockc.Foreground = Brushes.White;
            textBlock2_Changedisplay1.Foreground = Brushes.White;
            textBlockc_Prev.Foreground = Brushes.White;
            textBlock_PortName.Foreground = Brushes.White;
            textBlock_Value.Foreground = Brushes.White;
            textBlock1.Foreground = Brushes.White;



        }
        // Regular tracking of single stock
        public void button_Click(object sender, RoutedEventArgs e)
        {
            string url = "";
            if (textBox.Text != "") url += textBox.Text + "+";
            if (url != "")
            {
            // first url for general sum of the day
                url = url.Substring(0, url.Length - 1);
                const string base_url =
                    "http://download.finance.yahoo.com/d/quotes.csv?s=@&f=sl1d1t1c1ghnp";
                url = base_url.Replace("@", url);

            }
            try
            {
                //grab web response
                string result = GetWebResponse(url);
                Console.WriteLine(result.Replace("\\r\\\n", "\r\n"));

                //set the prices
                string[] lines = result.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                textBlock1.Text = decimal.Parse(lines[0].Split(',')[1]).ToString("C3");

                stockVal = decimal.Parse(lines[0].Split(',')[1]).ToString();
                date = (lines[0].Split('"')[3]).ToString();
                

                string j = decimal.Parse(lines[0].Split(',')[4]).ToString();
                if (j[0] == '-')
                {
                    textBlock2.Text = j + " %";
                    textBlock2.Foreground = Brushes.Red;
                }
                else
                {
                    textBlock2.Text = j + " %";
                    textBlock2.Foreground = Brushes.LightGreen;
                }

                //high set color to green
                textBlock3.Foreground = Brushes.LightGreen;
                textBlock3.Text = decimal.Parse(lines[0].Split(',')[6]).ToString("C3");

                //low set color to red
                textBlock4.Foreground = Brushes.Red;
                textBlock4.Text = decimal.Parse(lines[0].Split(',')[5]).ToString("C3");

                //stock full name
                textBlock5.Foreground = Brushes.White;
                textBlock5.Text = (lines[0].Split('"')[7]).ToString();
                symbol = (lines[0].Split('"')[1]).ToString();

                //previous close
                textBlock4_Close.Text = decimal.Parse(lines[0].Split(',')[8]).ToString("C3");



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Read Error");
            }
        }
        private string GetWebResponse(string url)
        {
            WebClient web_client = new WebClient();
            Stream response = web_client.OpenRead(url);
            using (StreamReader stream_reader = new StreamReader(response))
            {
                string result = stream_reader.ReadToEnd();
                stream_reader.Close();
                return result;
            }
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }
        private void textBox_Enter(object sender, TextChangedEventArgs e)
        { 


        }
       //buy stocks
        public void button_Buy_Click_1(object sender, RoutedEventArgs e)
        {
            string name = textBox_NameSearch.Text;
            string title;
            const string tit = @"C:\Users\T4NG1\Desktop\stocktracker Test\NAME.txt";
            title = tit.Replace("NAME", name);
            int buy;
            int.TryParse(BuyBox.Text, out buy);
            
            try
            {
                string[] row = System.IO.File.ReadAllLines(title);
                if (symbol == (row[0].Split(',')[1]).ToString() || symbol == "")
                {
                    if (buy > 0)
                    {
                        quantity += buy;
                    }

                    //creates text file 
                    string text = stockVal + "," + symbol + "," + quantity;
                    System.IO.File.WriteAllText(title, text);

                    //append the document with a new line for each bought stock type
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Read Error");
            }
        }
        //sell stocks
        public void button_Sell_Click(object sender, RoutedEventArgs e)
        {
            string name = textBox_NameSearch.Text;
            string title;
            const string tit = @"C:\Users\T4NG1\Desktop\stocktracker Test\NAME.txt";
            title = tit.Replace("NAME", name);
            int sell;
            int.TryParse(SellBox.Text, out sell);

            try
            {
                string[] row = System.IO.File.ReadAllLines(title);
                if (symbol == (row[0].Split(',')[1]).ToString())
                {
                    if (quantity >= sell)
                    {
                        quantity -= sell;
                    }
                    //creates text file 
                    string text = stockVal + "," + symbol + "," + quantity;
                    System.IO.File.WriteAllText(title, text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Read Error");
            }
        }

        //  search function/update function
        public void button1_Copy1_Click(object sender, RoutedEventArgs e)
        {
            //finds file with text regarding stocks
            string name = textBox_NameSearch.Text;
            string title;
            const string tit = @"C:\Users\T4NG1\Desktop\stocktracker Test\NAME.txt";
            title = tit.Replace("NAME", name);
            try
            {
                //reads it
                string[] row = System.IO.File.ReadAllLines(title);
                //then prints out stock symbols to each appropriate box
                textBlocka_userstock1.Text = (row[0].Split(',')[1]).ToString();

                //prints overall net loss or gain % to appropriate box
                textBlock2_Changedisplay1.Text = (row[0].Split(',')[2]).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Read Error");
            }
        }
        //creates a file
        public void button_Create_Click(object sender, RoutedEventArgs e)
        {
            string name = textBox_NameSearch.Text;
            string title;
            const string tit = @"C:\Users\T4NG1\Desktop\stocktracker Test\NAME.txt";
            title = tit.Replace("NAME", name);
            System.IO.File.WriteAllText(title, ",,");
        }

        private void textBox1_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }
        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}

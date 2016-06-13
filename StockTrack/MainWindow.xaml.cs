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
        public string stockVal;
        public string date;
        public string symbol;
        public float quantity;

        public MainWindow()
        {
            InitializeComponent();
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

                
                string[] lines = result.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
               
                //set the prices
                textBlock1.Foreground = Brushes.White;
                textBlock1.Text = decimal.Parse(lines[0].Split(',')[1]).ToString("C3");
                
                //**********save stock value/date for portfolio use***********************
                stockVal = decimal.Parse(lines[0].Split(',')[1]).ToString();
                date = (lines[0].Split('"')[3]).ToString();
                

                string sign = decimal.Parse(lines[0].Split(',')[4]).ToString();
                if (sign[0] == '-')
                {
                    textBlock2.Text = sign + " %";
                    textBlock2.Foreground = Brushes.Red;
                }
                else
                {
                    textBlock2.Text = sign + " %";
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
                // save symbol for portfolio
                symbol = (lines[0].Split('"')[1]).ToString();


                //previous close
                textBlock4_Close.Foreground = Brushes.Yellow;
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

       //buy stocks
        public void button_Buy_Click_1(object sender, RoutedEventArgs e)
        {
            string name = textBox_NameSearch.Text;
            string title;
            const string tit = @"C:\Users\T4NG1\Desktop\stocktracker Test\NAME.txt";
            title = tit.Replace("NAME", name);
            int buy;
            int.TryParse(BuyBox.Text, out buy);
            symbol = symbol.ToUpper();
            string previous = System.IO.File.ReadAllText(title);

            try
            {
                string[] row = System.IO.File.ReadAllLines(title);
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(title))
                {
                    foreach (string line in row)
                    {
                        // If the line contains the stock symbol, write the line to the file.
                        if (!line.Contains(symbol))
                        {
                            if (buy > 0)
                            {
                                
                                quantity += buy;
                                string text = (stockVal + ',' + symbol + ',' + quantity);
                                file.WriteLine(previous + text);
                                quantity = 0;
                                break;
                            }
                            else
                            {
                                string no = "Invalid input";
                                MessageBox.Show(no, "Read Error");
                            }
                        }
                        else if (line.Contains(symbol))
                        {
                            if (buy > 0)
                            {
                                quantity += buy;
                                string now = ( stockVal + ',' + symbol + ',' + quantity);
                                file.WriteLine(now + previous);
                                quantity = 0;
                                break;
                            }
                            else
                            {
                                string no = "Invalid input";
                                MessageBox.Show(no, "Read Error");
                            }
                        }
                    }
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
            int.TryParse(BuyBox.Text, out sell);
            symbol = symbol.ToUpper();
            string previous = System.IO.File.ReadAllText(title);

            try
            {
                string[] row = System.IO.File.ReadAllLines(title);
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(title))
                {
                    foreach (string line in row)
                    {
                        // If the line contains the stock symbol, write the line to the file.
                        if (!line.Contains(symbol))
                        {
                            if (sell > 0)
                            {

                                quantity -= sell;
                                string text = (stockVal + ',' + symbol + ',' + quantity);
                                file.WriteLine(previous + text);
                                quantity = 0;
                                break;
                            }
                            else
                            {
                                string no = "Invalid input";
                                MessageBox.Show(no, "Read Error");
                            }
                        }
                        else if (line.Contains(symbol))
                        {
                            if (sell > 0)
                            {
                                quantity -= sell;
                                string now = (stockVal + ',' + symbol + ',' + quantity);
                                file.WriteLine(now + previous);
                                quantity = 0;
                                break;
                            }
                            else
                            {
                                string no = "Invalid input";
                                MessageBox.Show(no, "Read Error");
                            }
                        }
                    }
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
            decimal sum = 0;
            decimal Nsum = 0;
            decimal first_buy;
            decimal first_buy1;
            try
            {
                // for loop so that all lines are read 
                Portfolio port = new Portfolio();
                port.add();

                string[] row = System.IO.File.ReadAllLines(title);
                for (int counter = 1; counter <= row.Length; counter++)
                {
                    decimal symq1;
                    decimal symq2;
                    string sym = (row[counter].Split(',')[1]).ToString();
                    string sym2 = (row[counter + 1].Split(',')[1]).ToString();
                    symq1 = decimal.Parse(row[counter].Split(',')[2]);
                    symq2 = decimal.Parse(row[counter + 1].Split(',')[2]);
                    // is the symbol in the middle the same as the next line?
                    if (sym == sym2)
                    {
                        if (sum == 0)
                        {
                            sum += symq2 + symq1;
                        }
                        else
                        {
                            sum += symq2;
                        }
                        //print the summation of stocks bought and the symbol + initial price
                        textBlocka_userstock1.Text = sym;
                        textBlock2_Changedisplay1.Text = Convert.ToString(sum);
                        first_buy = decimal.Parse(row[counter].Split(',')[0]);
                        textBlock6.Text = Convert.ToString(first_buy);

                    }
                    else if (sym != sym2)
                    {
                        if (Nsum == 0)
                        {
                            Nsum += symq2;

                            textBlockb_userstock2.Text = sym2;
                            textBlock3_Changedisplay2.Text = Convert.ToString(Nsum);
                            first_buy1 = decimal.Parse(row[counter+1].Split(',')[0]);
                            textBlock7.Text = Convert.ToString(first_buy1);
                        }
                    }
                }
            }



            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "LOST");
            }
        }
        //creates a file
        public void button_Create_Click(object sender, RoutedEventArgs e)
        {
            string name = textBox_NameSearch.Text;
            string title;
            const string tit = @"C:\Users\T4NG1\Desktop\stocktracker Test\NAME.txt";
            title = tit.Replace("NAME", name);
            System.IO.File.WriteAllText(title,"\n");
        }

        private void textBox1_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }
        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void textBox_Enter(object sender, TextChangedEventArgs e)
        {


        }
    }
}

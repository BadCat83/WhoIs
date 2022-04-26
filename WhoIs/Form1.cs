using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WhoIs
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        bool self_address = true;
        IPAddress ipAdr;

        void parseInfo()
        {
            string line = "http://ipwhois.app/xml/";

            using (WebClient wc = new WebClient())
                line = !self_address ? wc.DownloadString(line + textBox1.Text) : wc.DownloadString(line);

            Match ip_address = Regex.Match(line, "<ip>(.*?)</ip>");
            Match country = Regex.Match(line, "<country>(.*?)</country>");
            Match region = Regex.Match(line, "<region>(.*?)</region>");
            Match city = Regex.Match(line, "<city>(.*?)</city>");
            Match organization = Regex.Match(line, "<org>(.*?)</org>");

            label6.Text = ip_address.Groups[1].Value;
            label7.Text = country.Groups[1].Value;
            label8.Text = region.Groups[1].Value;
            label9.Text = organization.Groups[1].Value;
            label10.Text = city.Groups[1].Value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int count = textBox1.Text.Split('.').Length - 1;

            if(self_address)
            {
                parseInfo();
            }
            else
            {
                if (count < 3 || !IPAddress.TryParse(textBox1.Text, out ipAdr))
                {
                    MessageBox.Show("Не корректный IP адрес", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
                else
                {
                    parseInfo();
                }
            }
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            self_address = false;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!Char.IsDigit(number) && number != 8 && e.KeyChar != 46)
                e.Handled = true;
        }
    }
}

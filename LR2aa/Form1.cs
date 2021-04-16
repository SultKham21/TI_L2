using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Windows.Forms;

namespace protect_inf_LR1
{
    public partial class Form1 : Form
    {
        char[] characters = new char[] { 'A', 'B', 'C', 'D', 'E', 
                                         'F', 'G', 'H', 'I', 'J',
                                         'K', 'L', 'M', 'N', 'O', 
                                         'P', 'Q', 'R', 'S', 'T', 
                                         'U', 'V', 'W', 'X', 'Y', 
                                         'Z', ' '};

        public Form1()
        {
            InitializeComponent();
        }

        private void buttonShifr_Click(object sender, EventArgs evnt)
        {
            if ((tbn_p.Text.Length > 0) && (tbn_q.Text.Length > 0))
            {
                long p = Convert.ToInt64(tbn_p.Text);
                long q = Convert.ToInt64(tbn_q.Text);

                if (CheckSimple(p) && CheckSimple(q))
                {
                    string strin = "";
                    StreamReader stre = new StreamReader("ent.txt");

                    while (!stre.EndOfStream)
                    {
                        strin += stre.ReadLine();
                    }

                    stre.Close();
                    strin = strin.ToUpper();
                    long n = p * q;
                    long fi = (p - 1) * (q - 1);
                    long d = CalcD(fi);
                    long e = CalcE(d, fi);
                    List<string> result = RSAE(strin, e, n);
                    StreamWriter stwr = new StreamWriter("on.txt");

                    foreach (string item in result)
                        stwr.WriteLine(item);
                    stwr.Close();

                    textBox_d.Text = d.ToString();
                    textBox_n.Text = n.ToString();

                    Process.Start("on.txt");
                }
                else
                    MessageBox.Show("p and q - must be simple");
            }
            else
                MessageBox.Show("no p and q");
        }

        private void buttonDeshiphr(object sender, EventArgs e)
        {
            if ((textBox_d.Text.Length > 0) && (textBox_n.Text.Length > 0))
            {
                long d = Convert.ToInt64(textBox_d.Text);
                long n = Convert.ToInt64(textBox_n.Text);

                List<string> input = new List<string>();

                StreamReader sr = new StreamReader("on.txt");

                while (!sr.EndOfStream)
                {
                    input.Add(sr.ReadLine());
                }

                sr.Close();

                string result = RSAD(input, d, n);

                StreamWriter sw = new StreamWriter("ow.txt");
                sw.WriteLine(result);
                sw.Close();

                Process.Start("ow.txt");
            }
            else
                MessageBox.Show("you must enter secret key");
        }

        private bool CheckSimple(long n)
        {
            if (n < 2)
                return false;

            if (n == 2)
                return true;

            for (long i = 2; i < n; i++)
                if (n % i == 0)
                    return false;

            return true;
        }

        private long CalcD(long al)
        {
            long d = al - 1;

            for (long i = 2; i <= al; i++)
                if ((al % i == 0) && (d % i == 0))
                {
                    d--;
                    i = 1;
                }

            return d;
        }

        private long CalcE(long d, long al)
        {
            long e = 10;

            while (true)
            {
                if ((e * d) % al == 1)
                    break;
                else
                    e++;
            }

            return e;
        }

        private List<string> RSAE(string s, long eg, long n)
        {
            List<string> result = new List<string>();
            BigInteger big;

            for (int i = 0; i < s.Length; i++)
            {
                int index = Array.IndexOf(characters, s[i]);
                big = new BigInteger(index);
                big = BigInteger.Pow(big, (int)eg);
                BigInteger nt = new BigInteger((int)n);
                big = big % nt;
                result.Add(big.ToString());
            }

            return result;
        }

       

        private string RSAD(List<string> input, long d, long n)
        {
            string result = "";
            BigInteger big;

            foreach (string item in input)
            {
                big = new BigInteger(Convert.ToDouble(item));
                big = BigInteger.Pow(big, (int)d);
                BigInteger nt = new BigInteger((int)n);
                big = big % nt;
                int index = Convert.ToInt32(big.ToString());
                result += characters[index].ToString();
            }

            return result;
        }
    }
}

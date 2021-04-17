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
                    long r = p * q;
                    long fi = (p - 1) * (q - 1);
                    long e = CalcE(fi);

                    TempValuesGcd temp = CalcDEuclid(fi, e);
                    long d = temp.Y;
                    if (d < 0)
                    {
                        d += fi;
                    }


                    List<string> result = RSAE(strin, d, r);
                    StreamWriter stwr = new StreamWriter("on.txt");

                    foreach (string item in result)
                        stwr.WriteLine(item);
                    stwr.Close();

                    textBox_d.Text = d.ToString();
                    textBox_r.Text = r.ToString();

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
            if ((textBox_d.Text.Length > 0) && (textBox_r.Text.Length > 0))
            {
                long d = Convert.ToInt64(textBox_d.Text);
                long r = Convert.ToInt64(textBox_r.Text);

                List<string> input = new List<string>();

                StreamReader sr = new StreamReader("on.txt");

                while (!sr.EndOfStream)
                {
                    input.Add(sr.ReadLine());
                }

                sr.Close();

                string result = RSAD(input, d, r);

                StreamWriter sw = new StreamWriter("ow.txt");
                sw.WriteLine(result);
                sw.Close();

                Process.Start("ow.txt");
            }
            else
                MessageBox.Show("you must enter secret key");
        }

        private bool CheckSimple(long pq)
        {
            if (pq < 2)
                return false;

            if (pq == 2)
                return true;

            for (long i = 2; i < pq; i++)
                if (pq % i == 0)
                    return false;

            return true;
        }

        private long CalcE(long fip)
        {
            long d = fip - 1;

            for (long i = 2; i <= fip; i++)
                if ((fip % i == 0) && (d % i == 0))
                {
                    d--;
                    i = 1;
                }

            return d;
        }

        private TempValuesGcd CalcDEuclid(long a, long b)
        {

            if (b == 0)
            {
                return new TempValuesGcd(b, 1, 0);
            }
            else
            {
                var tmp = CalcDEuclid(b, a % b);
                var d = tmp.D;
                var y = tmp.X - tmp.Y * (a / b);
                var x = tmp.Y;
                return new TempValuesGcd(d, x, y);
            }

        }

        private List<string> RSAE(string s, long eg, long n)
        {
            List<string> result = new List<string>();
            BigInteger big, big1;

            for (int i = 0; i < s.Length; i++)
            {
                int index = Array.IndexOf(characters, s[i]);
                big = new BigInteger(index);
                big = Power(big, (int)eg, (int)n);
                BigInteger nt = new BigInteger((int)n);
                big = big % nt;
                result.Add(big.ToString());
            }

            return result;
        }

        private static BigInteger Power(BigInteger x, long y, long rp)
        {
            if (y == 0) return 1;

            var z = Power(x, y / 2, rp);

            if (y % 2 == 0)
                return (z * z) % rp;
            else
                return (x * z * z) % rp;
        }



        private string RSAD(List<string> input, long d, long n)
        {
            string result = "";
            BigInteger big;

            foreach (string item in input)
            {
                big = new BigInteger(Convert.ToDouble(item));
                big = Power(big, (int)d, (int)n);
                BigInteger nt = new BigInteger((int)n);
                big = big % nt;
                int index = Convert.ToInt32(big.ToString());
                result += characters[index].ToString();
            }

            return result;
        }

        private class TempValuesGcd
        {
            public TempValuesGcd(long d, long x, long y)
            {
                D = d;
                X = x;
                Y = y;
            }

            public long D { get; }
            public long X { get; }
            public long Y { get; }
        }

    }
}

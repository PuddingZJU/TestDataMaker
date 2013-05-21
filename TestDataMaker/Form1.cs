using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
namespace TestDataMaker
{
    public partial class Form1 : Form
    {
        TextBox[] tblist = new TextBox[10];
        Random ran;
        public Form1()
        {
            InitializeComponent();

            tblist[0] = textBox1;
            tblist[1] = textBox2;
            tblist[2] = textBox3;
            tblist[3] = textBox4;
            tblist[4] = textBox5;
            tblist[5] = textBox6;
            tblist[6] = textBox7;
            tblist[7] = textBox8;
            tblist[8] = textBox9;
            tblist[9] = textBox10;
            ran = new Random((int)(DateTime.Now.Ticks & 0xffffffffL) | (int)(DateTime.Now.Ticks >> 32));
            dimensionbox.SelectedIndex = 9;
            weakbox.SelectedIndex = 9;

            for (int i = 0; i < tblist.Length; i++)
            {
                tblist[i].Text = ran.Next(10000).ToString();
                tblist[i].Enabled = true;
            }
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            int[] thr ;
            int datacnt;
            int weaknum;
            double error;
            try
            {
                thr = new int[Convert.ToInt32(dimensionbox.Text)];
                datacnt = Convert.ToInt32(databox.Text);
                weaknum = Convert.ToInt32(weakbox.SelectedItem);
                error = Convert.ToDouble(ErrorBox.Text);
                error /= 100.0;
            }
            catch (Exception ice)
            {
                MessageBox.Show(ice.ToString());
                return;
            }
            for (int i = 0; i < thr.Length; i++)
            {
                try
                {
                    thr[i] = Convert.ToInt32(tblist[i].Text);
                    if (thr[i] <= 0)
                    {
                        MessageBox.Show((i + 1).ToString() + "号阈值有误！请输入小于10000的正整数");
                        return;
                    }
                    if (thr[i] > 10000)
                    {
                        MessageBox.Show((i + 1).ToString() + "号阈值过大！请输入小于10000的正整数");
                        return;
                    }
                }
                catch (Exception ice)
                {
                    MessageBox.Show((i + 1) + "号阈值填写错误！\n" + ice.ToString());
                    return;
                }
            }
            StreamWriter sw = new StreamWriter(@"D:\alldata.txt");
            string testdat="";
            int cntID=0;
            for (int i = 0; i < datacnt*error; i++,cntID++)
            {

                string line = (cntID + 1).ToString()+"\t1";
                testdat += (cntID + 201).ToString()+"\t1";
                for (int j = 0; j < thr.Length; j++)
                {
                    string tmp = "\t"+ran.Next(thr[j], thr[j] * 2).ToString();
                    line+=tmp;
                    testdat += tmp;
                }
                sw.WriteLine(line);
                testdat += "\r\n";
            }
            for (int i = 0; i < datacnt * (1-error); i++,cntID++)
            {

                string line = (cntID + 1).ToString() + "\t10";
                testdat += (cntID + 201).ToString() + "\t10";
                for (int j = 0; j < thr.Length; j++)
                {
                    string tmp = "\t" + ran.Next(thr[j], thr[j] * 2).ToString();
                    line += tmp;
                    testdat += tmp;
                }
                sw.WriteLine(line);
                testdat += "\r\n";

            }
            for (int i = 0; i < datacnt * (1 - error); i++,cntID++)
            {

                string line = (cntID + 1).ToString() + "\t1";
                testdat += (cntID + 201).ToString() + "\t1";
                for (int j = 0; j < thr.Length; j++)
                {
                    string tmp = "\t" + ran.Next(0, thr[j]).ToString();
                    line += tmp;
                    testdat += tmp;
                }
                sw.WriteLine(line);
                testdat += "\r\n";

            }
            for (int i = 0; i < datacnt*error; i++,cntID++)
            {
                string line = (cntID + 1).ToString() + "\t10";
                testdat += (cntID + 201).ToString() + "\t10";
                for (int j = 0; j < thr.Length; j++)
                {
                   string tmp = "\t" + ran.Next(0, thr[j]).ToString();
                    line += tmp;
                    testdat += tmp;
                }
                sw.WriteLine(line);
                testdat += "\r\n";

            }
            sw.Write(testdat);
            sw.Close();
            sw = new StreamWriter(@"D:\config.txt");
            sw.WriteLine("200\t399\t"+weakbox.Text);
            sw.Close();
            //MessageBox.Show("ok");
            Process app = new System.Diagnostics.Process();
            ProcessStartInfo startInfo = new ProcessStartInfo(); 
            startInfo.FileName= @"D:\Documents\Visual Studio 2012\Projects\Adaboost_V\Release\Adaboost_V.exe";
            startInfo.Arguments = @"D:\alldata.txt D:\config.txt";
            app.StartInfo = startInfo;
            app.Start();
        }

        private void dimensionbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int dim = Convert.ToInt32(dimensionbox.SelectedItem);
            for (int i = 0; i < 10; i++)
            {
                tblist[i].Enabled = i < dim;
                tblist[i].Text = i < dim ? tblist[i].Text : "";
            }
        }

        private void weakbox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void resetbutton_Click(object sender, EventArgs e)
        {

            dimensionbox.SelectedIndex = 9;
            weakbox.SelectedIndex = 9;
            
            for (int i = 0; i < tblist.Length; i++)
            {
                tblist[i].Text = ran.Next(10000).ToString();
                tblist[i].Enabled = true;
            }
        }
    }
}

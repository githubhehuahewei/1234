using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
//download by http://www.codesc.net
namespace Mail
{
    public partial class mailone : Form
    {
        public mailone()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (fjr.Text.Length == 0 || sjr.Text.Length == 0)
            {
                MessageBox.Show("请填写Email地址", "填写不完整", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                return;
            }
            try
            {
                YanZheng(fjr.Text, sjr.Text);   //验证邮件格式
            }
            catch (EmailErrorException ex)
            {
                MessageBox.Show(ex.Message, "Email格式错误", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                return;
            }

            string fjrtxt = fjr.Text;
            string mmtxt = mm.Text;
            string sjrtxt = sjr.Text;
            string zttxt = zt.Text;
            string fjtxt = fj.Text;
            string nrtxt = nr.Text;

            string[] fasong = fjrtxt.Split('@');
            string[] fs = fasong[1].Split('.');

            //发送
            SmtpClient client = new SmtpClient("smtp." + fs[0].ToString().Trim() + ".com");   //设置邮件协议
            client.UseDefaultCredentials = false;//这一句得写前面
            client.DeliveryMethod = SmtpDeliveryMethod.Network; //通过网络发送到Smtp服务器
            client.Credentials = new NetworkCredential(fasong[0], mmtxt); //通过用户名和密码 认证

            MailMessage mmsg = new MailMessage(new MailAddress(fjrtxt), new MailAddress(sjrtxt)); //发件人和收件人的邮箱地址
            mmsg.Subject = zttxt;      //邮件主题
            mmsg.SubjectEncoding = Encoding.UTF8;   //主题编码
            mmsg.Body = nrtxt;         //邮件正文
            mmsg.BodyEncoding = Encoding.UTF8;      //正文编码
            mmsg.IsBodyHtml = true;    //设置为HTML格式           
            mmsg.Priority = MailPriority.High;   //优先级

            if (fj.Text.Trim() != "")
            {
                mmsg.Attachments.Add(new Attachment(fj.Text));//增加附件
            }
            try
            {
                client.Send(mmsg);
                MessageBox.Show("邮件已发成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }  
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fj.Text = openFileDialog1.FileName;   //得到附件的地址
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool YanZheng(string fmail, string smail)
        {
            string[] subStrings = smail.Split('@');
            string[] subStrings1 = fmail.Split('@');
            if (subStrings.Length != 2 || subStrings1.Length != 2)
            {
                throw new EmailErrorException();
            }
            else
            {
                int index = subStrings[1].IndexOf(".");
                int index1 = subStrings1[1].IndexOf(".");
                if (index <= 0 || index1 <= 0)
                {
                    throw new EmailErrorException();
                }

                if (subStrings[1][subStrings[1].Length - 1] == '.' || subStrings1[1][subStrings[1].Length - 1] == '.')
                {
                    throw new EmailErrorException();
                }
            }

            return true;
        }
    }
}
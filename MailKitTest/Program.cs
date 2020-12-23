using System;

using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using MailKit.Net.Imap;

namespace MailKitTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Leo Cheng", "nixiaozi01@126.com")); // Leo Cheng 会显示成对方的发件人，后面加上发件邮箱
            message.To.Add(new MailboxAddress("Mrs. Lei", "1341534547@qq.com")); // Mrs.Lei 是发件方自己定义的收件人(不会在收件人方显示) ， 后面加上收件邮箱
            message.Subject = "How you doin'?";  // 邮件的主体

            message.Body = new TextPart("plain") // 邮件的正文 plain 表示纯文本
            {
                Text = @"Hey Chandler,

I just wanted to let you know that Monica and I were going to go play some paintball, you in?

-- Joey"
            };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.126.com", 465, true); // 客户端要连接的 smtp服务器和端口 126 参照 http://wap.126.com/xm/static/html/126_android_2.html

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate("nixiaozi01@126.com", "SSTQDRCILQUPBDQB"); // 登录可以使用 用户名就是邮箱， 这个密码就是这个授权码。

                client.Send(message);
                client.Disconnect(true);
            }


            
        }
    }
}

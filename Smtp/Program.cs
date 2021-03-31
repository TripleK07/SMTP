using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Smtp
{
    class Program
    {
        static void Main(string[] args)
        {
            String logoPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\Files\mabLogo.png";
            String file = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\Files\Daily Position.xlsx";

            Console.WriteLine(logoPath);
            Console.WriteLine(file);

            using (SmtpClient client = new SmtpClient()
            {
                Host = "smtp.office365.com",
                Port = 587,
                UseDefaultCredentials = false, // This require to be before setting Credentials property
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential("khantkoko@mabbank.com", "triplek@Mab"), // you must give a full email address for authentication 
                TargetName = "STARTTLS/smtp.office365.com", // Set to avoid MustIssueStartTlsFirst exception
                EnableSsl = true // Set to avoid secure connection exception
            })
            {
                String body = @"Dear Mr.Testing, 
                                <br><div style='margin-left: 40px'>This is a test run from auto mail sending program.</div> 
                                <div>I hope everything is fine and well formatted.</div>";


                String footer = @"<br> <b> Best Regards </b>,
                                <br> Khant Ko Ko 
                                <br> Technology, Data & Information Department 
                                <br><br> <font style='color:#691C32'> <b> Myanma Apex Bank Ltd. </b> </font>
                                <br> T: +95 (1) 8398811~19 (Ext: 138)
                                <br> M: +95 (9) 250 909 303
                                <br> E: <a href='mailto:khantkoko@mabbank.com'>khantkoko@mabbank.com</a>
                                <br> W: <a href='https://www.mabbank.com'>www.mabbank.com</a>
                                <br> A: No.321, Bldg. 2, 1st floor, Botahtaung Office Park (BOP), Botahtaung Tsp, Yangon, Myanmar(11161) <br>";

                String mailcontent = body + footer;
                Attachment attachedFile = new Attachment(file);

                MailMessage message = new MailMessage()
                {
                    From = new MailAddress("khantkoko@mabbank.com"), // sender must be a full email address
                    Subject = "Sample email sending from c#",
                    IsBodyHtml = true,
                    AlternateViews = { GetAlternativeViewAsBody(mailcontent, logoPath) }, //add footer company logo
                    BodyEncoding = System.Text.Encoding.UTF8,
                    SubjectEncoding = System.Text.Encoding.UTF8,
                    Attachments = { attachedFile },
                };

                String recipients = "howl@mit.com.mm";
                var toAddresses = recipients.Split(',');
                foreach (var to in toAddresses)
                {
                    message.To.Add(to.Trim());
                }

                try
                {
                    client.Send(message);
                    Console.WriteLine("Sent");
                    Console.Read();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    Console.Read();
                }
            }
        }

        private static AlternateView GetAlternativeViewAsBody(String htmlContent, String filePath)
        {
            LinkedResource res = new LinkedResource(filePath, "image/jpeg"); //content type is important
            res.ContentId = Guid.NewGuid().ToString();
            
            string htmlBody = htmlContent + "<br>" + @"<img src='cid:" + res.ContentId + @"'/>"; //mail content + logo

            AlternateView alternateView = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);
            alternateView.LinkedResources.Add(res);
            return alternateView;
        }

        public static AlternateView attachLogo(String imgPath)
        {
            Bitmap b = new Bitmap(imgPath);
            ImageConverter ic = new ImageConverter();
            Byte[] ba = (Byte[])ic.ConvertTo(b, typeof(Byte[]));
            MemoryStream logo = new MemoryStream(ba);
            LinkedResource footerImg = new LinkedResource(logo, "image/jpeg");
            footerImg.ContentId = "companyLogo";
            AlternateView foot = AlternateView.CreateAlternateViewFromString("<img src=cid:companylogo>", null, MediaTypeNames.Text.Html);
            foot.LinkedResources.Add(footerImg);
            return foot;
        }
    }
}

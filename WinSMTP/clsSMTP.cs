using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace WinSMTP
{
    public class clsSMTP
    {
        public void sendMail(List<String> attachments)
        {
            String logoPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\logo.png";
            //String file = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\Files\Daily Position.xlsx";

            using (SmtpClient client = new SmtpClient()
            {
                Host = "smtp.office365.com",
                Port = 587,
                UseDefaultCredentials = false, // This require to be before setting Credentials property
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(clsHelper.sender, clsHelper.senderPassword), // you must give a full email address for authentication 
                TargetName = "STARTTLS/smtp.office365.com", // Set to avoid MustIssueStartTlsFirst exception
                EnableSsl = true // Set to avoid secure connection exception
            })
            {
                String body = "<pre>" + clsHelper.body + "</pre>";


                String footer = @"<br> Technology, Data & Information Department 
                                <br><br> <font style='color:#691C32'> <b> Myanma Apex Bank Ltd. </b> </font>
                                <br> T: +95 (1) 8398811~19 (Ext: 138)
                                <br> M: +95 (9) 250 909 303
                                <br> E: <a href='" + clsHelper.sender + "'>" + clsHelper.sender + @"</a>
                                <br> W: <a href='https://www.mabbank.com'>www.mabbank.com</a>
                                <br> A: No.321, Bldg. 2, 1st floor, Botahtaung Office Park (BOP), Botahtaung Tsp, Yangon, Myanmar(11161) <br>";

                String mailcontent = body + footer;
                //Attachment attachedFile = new Attachment(file);

                MailMessage message = new MailMessage()
                {
                    From = new MailAddress(clsHelper.sender), // sender must be a full email address
                    Subject = clsHelper.subject,
                    IsBodyHtml = true,
                    AlternateViews = { GetAlternativeViewAsBody(mailcontent, logoPath) }, //add footer company logo
                    BodyEncoding = System.Text.Encoding.UTF8,
                    SubjectEncoding = System.Text.Encoding.UTF8,
                    //Attachments = { attachedFile },
                };

                linkAttachmentFiles(message, attachments);

                String recipients = clsHelper.recipients;
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

        private void linkAttachmentFiles(MailMessage mail, List<String> attachments)
        {
            foreach (String item in attachments)
            {
                mail.Attachments.Add(new Attachment(item));
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

    }
}

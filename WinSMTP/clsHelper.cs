using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace WinSMTP
{
    public static class clsHelper
    {
        public static String sender = "";
        public static String senderPassword = "";
        public static String recipients = "";
        public static String subject = "";
        public static String body = "";
        public static String attachmentPath = "";

        private static String configExeName = "\\WinSMTP.exe";
        private static String configName = "\\WinSMTP.exe.config";

        private static Configuration getConfiguration()
        {
            return ConfigurationManager.OpenExeConfiguration(Application.StartupPath + configExeName);
        }
        public static void writeConfigVariables()
        {
            Configuration config = getConfiguration();
            config.AppSettings.Settings.Remove("sender");
            config.AppSettings.Settings.Remove("senderPassword");
            config.AppSettings.Settings.Remove("recipients");
            config.AppSettings.Settings.Remove("subject");
            config.AppSettings.Settings.Remove("body");
            config.AppSettings.Settings.Remove("attachmentPath");

            config.AppSettings.Settings.Add("sender", sender);
            config.AppSettings.Settings.Add("senderPassword", clsEncryption.Encrypt(senderPassword, "mab-bank-mufi-auto-email"));  //key must has 24 characters
            config.AppSettings.Settings.Add("recipients", recipients);
            config.AppSettings.Settings.Add("subject", subject);
            config.AppSettings.Settings.Add("body", body);
            config.AppSettings.Settings.Add("attachmentPath", attachmentPath);

            config.Save(ConfigurationSaveMode.Modified);
        }

        public static void readConfigVariables()
        {
            Configuration config = getConfiguration();
            if (config.AppSettings.Settings.Count > 0)
            {
                sender = config.AppSettings.Settings["sender"].Value;
                senderPassword = clsEncryption.Decrypt(config.AppSettings.Settings["senderPassword"].Value, "mab-bank-mufi-auto-email"); //key must has 24 characters
                recipients = config.AppSettings.Settings["recipients"].Value;
                subject = config.AppSettings.Settings["subject"].Value;
                body = config.AppSettings.Settings["body"].Value;
                attachmentPath = config.AppSettings.Settings["attachmentPath"].Value;
            }
        }

        public static List<String> getFilesFromAttachmentPath()
        {
            List<String> files = new List<string>();

            if (attachmentPath == "" || attachmentPath == null)
                readConfigVariables();

            if (Directory.Exists(attachmentPath))
            {
                files = Directory.GetFiles(attachmentPath).ToList();
            }

            return files;
        }

        public static void WriteErrorLog(Exception ex)
        {
            string filePath = Application.StartupPath + "\\ErrorLog\\";

            DirectoryInfo info = new DirectoryInfo(filePath);
            if (!info.Exists)
            {
                info.Create();
            }
            filePath = filePath + "ErrorLog" + System.DateTime.Now.ToString("ddMMyyyy") + ".txt";

            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(
                    "Date :" + DateTime.Now.ToString() +
                    Environment.NewLine + "Message :" + ex.Message +
                    Environment.NewLine + "StackTrace :" + ex.StackTrace + "" + Environment.NewLine);
                writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
            }
        }

        public static void WriteErrorLog(String message)
        {
            string filePath = Application.StartupPath + "\\ErrorLog\\";

            DirectoryInfo info = new DirectoryInfo(filePath);
            if (!info.Exists)
            {
                info.Create();
            }
            filePath = filePath + "ErrorLog" + System.DateTime.Now.ToString("ddMMyyyy") + ".txt";

            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(
                        "Date :" + DateTime.Now.ToString() +
                        Environment.NewLine + "Message :" + message +
                        Environment.NewLine);
                writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
            }
        }

    }
}

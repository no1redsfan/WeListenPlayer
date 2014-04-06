using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using EAGetMail; //add EAGetMail namespace
using WeListenPlayer.APIClasses;

namespace WeListenPlayer.PurchaseEmailHandler
{
    class PurchaseEmailHandler
    {
 
        private static PurchaseEmail ConvertMailToHtml(string fileName)
        {
            var currentHtMLEmail = new PurchaseEmail();
            try
            {
                int pos = fileName.LastIndexOf(".");
                string mainName = fileName.Substring(0, pos);
                string htmlName = mainName + ".htm";

                string htmlFolder = mainName;
                if (!File.Exists(htmlName))
                {
                    // We haven't generate the html for this email, generate it now.
                   currentHtMLEmail = _GenerateHtmlForEmail(htmlName, fileName, htmlFolder, currentHtMLEmail);

                }

                //Console.WriteLine("Please open {0} to browse your email", htmlName);
            }
            catch (Exception ep)
            {
               MessageBox.Show(ep.Message);
            }
            return currentHtMLEmail;
        }

        private static string _FormatHtmlTag(string src)
        {
            src = src.Replace(">", "&gt;");
            src = src.Replace("<", "&lt;");
            return src;
        }

        private static PurchaseEmail _GenerateHtmlForEmail(string htmlName, string emlFile, string tempFolder, PurchaseEmail currentHtMLEmail)
        {
            Mail oMail = new Mail("TryIt");
            oMail.Load(emlFile, false);

            if (oMail.IsEncrypted)
            {
                try
                {
                    // This email is encrypted, we decrypt it by user default certificate.
                    oMail = oMail.Decrypt(null);
                }
                catch (Exception ep)
                {
                    MessageBox.Show(ep.Message);
                    oMail.Load(emlFile, false);
                }
            }

            if (oMail.IsSigned)
            {
                try
                {
                    // This email is digital signed.
                    EAGetMail.Certificate cert = oMail.VerifySignature();
                    //Console.WriteLine("This email contains a valid digital signature.");
                }
                catch (Exception ep)
                {
                    MessageBox.Show(ep.Message);
                }
            }

            // Parse html body
            string html = oMail.HtmlBody;
            var hdr = new StringBuilder();

            // Parse sender
            hdr.Append("<font face=\"Courier New,Arial\" size=2>");
            hdr.Append("<b>From:</b> " + _FormatHtmlTag(oMail.From.ToString()) + "<br>");

            // Parse to
            MailAddress[] addrs = oMail.To;
            int count = addrs.Length;
            if (count > 0)
            {
                hdr.Append("<b>To:</b> ");
                for (int i = 0; i < count; i++)
                {
                    hdr.Append(_FormatHtmlTag(addrs[i].ToString()));
                    if (i < count - 1)
                    {
                        hdr.Append(";");
                    }
                }
                hdr.Append("<br>");
            }

            // Parse cc
            addrs = oMail.Cc;

            count = addrs.Length;
            if (count > 0)
            {
                hdr.Append("<b>Cc:</b> ");
                for (int i = 0; i < count; i++)
                {
                    hdr.Append(_FormatHtmlTag(addrs[i].ToString()));
                    if (i < count - 1)
                    {
                        hdr.Append(";");
                    }
                }
                hdr.Append("<br>");
            }

            hdr.Append(String.Format("<b>Subject:</b>{0}<br>\r\n", _FormatHtmlTag(oMail.Subject)));

            // Parse attachments and save to local folder
            Attachment[] atts = oMail.Attachments;
            count = atts.Length;
            if (count > 0)
            {
                if (!Directory.Exists(tempFolder))
                    Directory.CreateDirectory(tempFolder);

                hdr.Append("<b>Attachments:</b>");
                for (int i = 0; i < count; i++)
                {
                    Attachment att = atts[i];

                    // this attachment is in OUTLOOK RTF format, decode it here.
                    if (String.Compare(att.Name, "winmail.dat") == 0)
                    {
                        Attachment[] tatts = null;
                        try
                        {
                            tatts = Mail.ParseTNEF(att.Content, true);
                        }
                        catch (Exception ep)
                        {
                            Console.WriteLine(ep.Message);
                            continue;
                        }

                        int y = tatts.Length;
                        for (int x = 0; x < y; x++)
                        {
                            Attachment tatt = tatts[x];
                            string tattname = String.Format("{0}\\{1}", tempFolder, tatt.Name);
                            tatt.SaveAs(tattname, true);
                            hdr.Append(
                            String.Format("<a href=\"{0}\" target=\"_blank\">{1}</a> ",
                                tattname, tatt.Name));
                        }
                        continue;
                    }

                    string attname = String.Format("{0}\\{1}", tempFolder, att.Name);
                    att.SaveAs(attname, true);
                    hdr.Append(String.Format("<a href=\"{0}\" target=\"_blank\">{1}</a> ",
                            attname, att.Name));
                    if (att.ContentID.Length > 0)
                    {

                        // Show embedded images.
                        html = html.Replace("cid:" + att.ContentID, attname);
                    }
                    else if (String.Compare(att.ContentType, 0, "image/", 0,
                                "image/".Length, true) == 0)
                    {

                        // show attached images.
                        html = html + String.Format("<hr><img src=\"{0}\">", attname);
                    }
                }
            }

            var reg = new Regex("(<meta[^>]*charset[ \t]*=[ \t\"]*)([^<> \r\n\"]*)", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            html = reg.Replace(html, "$1utf-8");
            if (!reg.IsMatch(html))
            {
                hdr.Insert(0, "<meta HTTP-EQUIV=\"Content-Type\" Content=\"text-html; charset=utf-8\">");
            }

            // write html to file
            html = hdr.ToString() + "<hr>" + html;


            //Save the html file to the html directory
            var newHtmlLocation = Directory.GetCurrentDirectory();
            newHtmlLocation = String.Format("{0}\\htmlInbox", newHtmlLocation);
            string newFileName = htmlName.Split('\\').Last();
            htmlName = newHtmlLocation + "\\" + newFileName;

            // Parse out song title
            string pattern = "<div style=\"font-family: verdana,arial,helvetica,sans-serif; font-weight: bold;\">(.*)</div>";
            MatchCollection matches = Regex.Matches(html, pattern);

            string songTitle = matches[0].Groups[1].ToString();

            currentHtMLEmail.fileName = newFileName;
            currentHtMLEmail.htmlPath = htmlName;
            currentHtMLEmail.title = songTitle;

            // save html file
            var fs = new FileStream(htmlName, FileMode.Create, FileAccess.Write, FileShare.None);
            byte[] data = System.Text.UTF8Encoding.UTF8.GetBytes(html);
            fs.Write(data, 0, data.Length);
            fs.Close();
            oMail.Clear();

            return currentHtMLEmail;
        }

        public List<PurchaseEmail> CheckEmail()
        {
            string curpath = Directory.GetCurrentDirectory();
            string mailbox = String.Format("{0}\\inbox", curpath);
            string htmlMailbox = String.Format("{0}\\htmlInbox", curpath);

            // If the .eml and .htm folders do not exist, create it.
            if (!Directory.Exists(mailbox))
            {
                Directory.CreateDirectory(mailbox);
            }
            if (!Directory.Exists(htmlMailbox))
            {
                Directory.CreateDirectory(htmlMailbox);
            }

            // Gmail IMAP4 server is "imap.gmail.com"
            MailServer oServer = new MailServer("imap.gmail.com",
                        "chriswelistendj@gmail.com", "**Jessica8", ServerProtocol.Imap4);
            MailClient oClient = new MailClient("TryIt");

            // Set SSL connection,
            oServer.SSLConnection = true;

            // Set 993 IMAP4 port
            oServer.Port = 993;

            try
            {
                oClient.Connect(oServer);
                MailInfo[] infos = oClient.GetMailInfos();
                for (int i = 0; i < infos.Length; i++)
                {
                    MailInfo info = infos[i];
                    //Console.WriteLine("Index: {0}; Size: {1}; UIDL: {2}", info.Index, info.Size, info.UIDL);

                    // Download email from GMail IMAP4 server
                    Mail oMail = oClient.GetMail(info);

                    if (oMail.From.ToString().Contains("digital-noreply@amazon.com"))
                    {
                        // Generate an email file name based on date time.
                        System.DateTime d = System.DateTime.Now;
                        var cur = new System.Globalization.CultureInfo("en-US");
                        string sdate = d.ToString("yyyyMMddHHmmss", cur);
                        string fileName = String.Format("{0}\\{1}{2}{3}.eml", mailbox, sdate, d.Millisecond.ToString("d3"), i);

                        // Save email to local disk
                        oMail.SaveAs(fileName, true);
                    }

                    // Mark email as deleted in GMail account.
                    oClient.Delete(info);

                }

                // Quit and purge emails marked as deleted from Gmail IMAP4 server.
                oClient.Quit();
            }
            catch (Exception ep)
            {
                MessageBox.Show(ep.Message);
            }

            // Get all *.eml files in specified folder and parse it one by one.
            string[] files = Directory.GetFiles(mailbox, "*.eml");

            var purchaseList = new List<PurchaseEmail>();

            for (int i = 0; i < files.Length; i++)
            {
                var convertedMessage = ConvertMailToHtml(files[i]);
                if (System.IO.File.Exists(files[i]))
                {
                    // Use a try block to catch IOExceptions, to 
                    // handle the case of the file already being 
                    // opened by another process. 
                    try
                    {
                        System.IO.File.Delete(files[i]);
                    }
                    catch (System.IO.IOException e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
                purchaseList.Add(convertedMessage);


            }

            return purchaseList;
        }
    }
}

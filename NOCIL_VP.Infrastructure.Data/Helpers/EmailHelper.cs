using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NOCIL_VP.API.Logging;
using NOCIL_VP.Domain.Core.Dtos;
using System.Net.Mail;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Hosting;

namespace NOCIL_VP.Infrastructure.Data.Helpers
{
    public class EmailHelper
    {
        private readonly IConfiguration _config;
        //global Variables
        string hostName;
        string SMTPEmail;
        string SMTPEmailPassword;
        int SMTPPort;
        string siteURL;
        string siteLink;

        public EmailHelper(IConfiguration config)
        {
            _config = config;
        }

        #region Initialize Variables
        public void SetValues()
        {
            hostName = _config.GetValue<string>("SMTPDetails:Host");
            SMTPEmail = _config.GetValue<string>("SMTPDetails:Email");
            SMTPEmailPassword = _config.GetValue<string>("SMTPDetails:Password");
            SMTPPort = Int32.Parse(_config.GetValue<string>("SMTPDetails:Port"));
            siteURL = _config.GetValue<string>("SMTPDetails:SiteURL");
            siteLink = _config.GetValue<string>("SMTPDetails:SiteLink");
        }

        public MailMessage CreateMailMessage(string toEmail, string subject, string sb)
        {
            MailMessage reportEmail = new MailMessage(SMTPEmail, toEmail, subject, sb.ToString());
            reportEmail.BodyEncoding = UTF8Encoding.UTF8;
            reportEmail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            reportEmail.IsBodyHtml = true;
            return reportEmail;
        }

        public SmtpClient CreateSmtpClient()
        {
            SmtpClient client = new SmtpClient();
            client.Port = Convert.ToInt32(SMTPPort);
            client.Host = hostName;
            client.EnableSsl = _config.GetValue<bool>("SMTPDetails:EnableSsl");
            client.Timeout = 60000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = _config.GetValue<bool>("SMTPDetails:UseDefaultCredentials");
            client.Credentials = new System.Net.NetworkCredential(SMTPEmail.Trim(), SMTPEmailPassword.Trim());
            return client;
        }
        #endregion

        #region Sending mail
        public async Task SendMailToVendors(SendMail sendMail)
        {
            LogWritter.WriteErrorLog($"SendMailToVendors - {JsonConvert.SerializeObject(sendMail)}");
            try
            {
                SetValues();
                string subject = "Vendor Onboarding";
                string mailBody = "";
                var qParams = new
                {
                    Form_Id = sendMail.Form_Id,
                    V_Id = sendMail.Vendor_Type_Id,
                    Status = ""
                };
                string jsonString = JsonConvert.SerializeObject(qParams);
                string encodedJson = HttpUtility.UrlEncode(jsonString);

                if (sendMail.ToEmail != null)
                {
                    mailBody = readHtmlString("WelcomeVendor.html");
                    mailBody = mailBody.Replace("{recepient}", sendMail.Username).Replace("{registerUrl}", siteURL + encodedJson);
                }
                else
                {
                    throw new Exception("To Email not found!!!");
                }
                SmtpClient client = CreateSmtpClient();
                MailMessage mailMessage = CreateMailMessage(sendMail.ToEmail, subject, mailBody);
                await client.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task SendApprovalRequestMail(ApprovalMailInfo approvalMailInfo)
        {
            try
            {
                SetValues();
                string subject = "Vendor Onboarding";
                string mailBody = "";
                if (approvalMailInfo.ToEmail != null)
                {
                    mailBody = readHtmlString("ApproveRequest.html");
                    mailBody = mailBody.Replace("{recepient}", approvalMailInfo.UserName)
                        .Replace("{formId}", approvalMailInfo.Form_Id.ToString())
                        .Replace("{email}", approvalMailInfo.Email)
                        .Replace("{mobile}", approvalMailInfo.Mobile_Number)
                        .Replace("{url}", siteLink);
                }
                else
                {
                    throw new Exception("To Email not found!!!");
                }
                SmtpClient client = CreateSmtpClient();
                MailMessage mailMessage = CreateMailMessage(approvalMailInfo.ToEmail, subject, mailBody);
                await client.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task SendRejectionInfoMail(RejectionMailInfo rejectionMailInfo)
        {
            try
            {
                SetValues();
                string subject = "Vendor Onboarding";
                string mailBody = "";
                var qParams = new
                {
                    Form_Id = rejectionMailInfo.Form_Id,
                    V_Id = rejectionMailInfo.Vendor_Type_Id,
                    Status = "Rejected"
                };
                string jsonString = JsonConvert.SerializeObject(qParams);
                string encodedJson = HttpUtility.UrlEncode(jsonString);



                if (rejectionMailInfo.ToEmail != null)
                {
                    mailBody = readHtmlString("RejectionInfoToVendor.html");
                    mailBody = mailBody.Replace("{recepient}", rejectionMailInfo.Username)
                        .Replace("{reason}", rejectionMailInfo.Reason)
                        .Replace("{url}", siteURL + encodedJson);
                }
                else
                {
                    throw new Exception("To Email not found!!!");
                }
                SmtpClient client = CreateSmtpClient();
                MailMessage mailMessage = CreateMailMessage(rejectionMailInfo.ToEmail, subject, mailBody);
                await client.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task SendApprovalInfoMail(ApprovalInfo approvalInfo)
        {
            try
            {
                SetValues();
                string subject = "Vendor Onboarding";
                string mailBody = "";
                if (approvalInfo.ToEmail != null)
                {
                    mailBody = readHtmlString("ApprovalInfo.html");
                    mailBody = mailBody.Replace("{recepient}", approvalInfo.UserName)
                        .Replace("{role}", approvalInfo.ApprovedBy);
                }
                else
                {
                    throw new Exception("To Email not found!!!");
                }
                SmtpClient client = CreateSmtpClient();
                MailMessage mailMessage = CreateMailMessage(approvalInfo.ToEmail, subject, mailBody);
                await client.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        public string readHtmlString(string fileName)
        {
            string htmlContent;
            try
            {
                string absolutePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot/EmailTemplates", fileName);
                if (!File.Exists(absolutePath)) { absolutePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/EmailTemplates", fileName); }
                htmlContent = File.ReadAllText(absolutePath);
            }
            catch (IOException ex)
            {
                throw new Exception($"Error reading HTML file: {ex.Message}");
            }

            return htmlContent;
        }
    }
}

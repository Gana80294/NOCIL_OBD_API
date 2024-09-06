using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NOCIL_VP.API.Logging;
using NOCIL_VP.Domain.Core.Dtos;
using System.Net.Mail;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Hosting;
using NOCIL_VP.Domain.Core.Configurations;
using Microsoft.Extensions.Options;

namespace NOCIL_VP.Infrastructure.Data.Helpers
{
    public class EmailHelper
    {
        private readonly SmtpSetting _smtpSettings;


        public EmailHelper(IOptions<SmtpSetting> smtp)
        {
            _smtpSettings = smtp.Value;
        }

        #region Initialize Variables

        public MailMessage CreateMailMessage(string toEmail, string subject, string sb)
        {
            MailMessage reportEmail = new MailMessage(_smtpSettings.Email, toEmail, subject, sb.ToString());
            reportEmail.BodyEncoding = UTF8Encoding.UTF8;
            reportEmail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            reportEmail.IsBodyHtml = true;
            return reportEmail;
        }

        public SmtpClient CreateSmtpClient()
        {
            SmtpClient client = new SmtpClient();
            client.Port = Convert.ToInt32(_smtpSettings.Port);
            client.Host = _smtpSettings.Host;
            client.EnableSsl = _smtpSettings.EnableSsl;
            client.Timeout = 60000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = _smtpSettings.UseDefaultCredentials;
            client.Credentials = new System.Net.NetworkCredential(_smtpSettings.Email.Trim(), _smtpSettings.Password.Trim());
            return client;
        }
        #endregion

        #region Sending mail
        public async Task SendMailToVendors(SendMail sendMail)
        {
            LogWritter.WriteErrorLog($"SendMailToVendors - {JsonConvert.SerializeObject(sendMail)}");
            try
            {
                string subject = "Vendor Onboarding";
                string mailBody = "";
                var qParams = new
                {
                    Form_Id = sendMail.Form_Id,
                    V_Id = sendMail.Vendor_Type_Id,
                    Status = "Initiated"
                };
                string jsonString = JsonConvert.SerializeObject(qParams);
                string encodedJson = HttpUtility.UrlEncode(jsonString);

                if (sendMail.ToEmail != null)
                {
                    mailBody = readHtmlString("WelcomeVendor.html");
                    mailBody = mailBody.Replace("{recepient}", sendMail.Username).Replace("{registerUrl}", _smtpSettings.SiteURL + encodedJson);
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
        public async Task SendWelcomeMailToVendors(WecomeApprovedVendorMail mail)
        {
            LogWritter.WriteErrorLog($"SendMailToVendors - {JsonConvert.SerializeObject(mail)}");
            try
            {
                string subject = "NOCIL - Vendor Onboarding";
                string mailBody = "";


                if (mail.ToEmail != null)
                {
                    mailBody = readHtmlString("WelcomeApprovedVendor.html");
                    mailBody = mailBody.Replace("{recepient}", mail.Recepient)
                        .Replace("{registerUrl}", _smtpSettings.SiteLink)
                        .Replace("{username}", mail.Username)
                        .Replace("{password}", mail.Password);
                }
                else
                {
                    throw new Exception("To Email not found!!!");
                }
                SmtpClient client = CreateSmtpClient();
                MailMessage mailMessage = CreateMailMessage(mail.ToEmail, subject, mailBody);
                await client.SendMailAsync(mailMessage);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task SendApprovalRequestMail(ApprovalMailInfo approvalMailInfo)
        {
            try
            {
                string subject = "Vendor Onboarding";
                string mailBody = "";
                if (approvalMailInfo.ToEmail != null)
                {
                    mailBody = readHtmlString("ApproveRequest.html");
                    mailBody = mailBody.Replace("{recepient}", approvalMailInfo.UserName)
                        .Replace("{formId}", approvalMailInfo.Form_Id.ToString())
                        .Replace("{email}", approvalMailInfo.Email)
                        .Replace("{mobile}", approvalMailInfo.Mobile_Number)
                        .Replace("{url}", _smtpSettings.SiteLink);
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
        public async Task SendRejectionInfoMailToVendor(RejectionMailInfoToVendor rejectionMailInfo)
        {
            try
            {
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
                        .Replace("{url}", _smtpSettings.SiteURL + encodedJson);
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
        public async Task SendRejectionMailInfoToBuyers(List<RejectionMailInfoToBuyer> rejectionMailInfo)
        {
            try
            {
                string subject = "Vendor Onboarding";
                string mailBody = "";
                foreach (var info in rejectionMailInfo)
                {
                    if (info.ToEmail != null)
                    {
                        mailBody = readHtmlString("RejectionInfoToBuyers.html");
                        mailBody = mailBody.Replace("{recepient}", info.Recepient)
                            .Replace("{reason}", info.Reason)
                            .Replace("{rejectedby}", info.RejectedBy)
                            .Replace("{formId}", info.Form_Id.ToString())
                            .Replace("{vendorName}", info.VendorName)
                            .Replace("{vendorMail}", info.VendorMail)
                            .Replace("{vendorMobile}", info.VendorMobile);
                    }
                    SmtpClient client = CreateSmtpClient();
                    MailMessage mailMessage = CreateMailMessage(info.ToEmail, subject, mailBody);
                    await client.SendMailAsync(mailMessage);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task SendApprovalInfoMail(ApprovalInfo approvalInfo)
        {
            try
            {
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
        
        public async Task SendVendorCreationNotification(VendorCreationNotification mail)
        {
            try
            {
                string subject = "Vendor Onboarding";
                string mailBody = "";
                if (mail.ToMail != null)
                {
                    mailBody = readHtmlString("VendorCreationNotification.html");
                    mailBody = mailBody.Replace("{recepient}", mail.Recepient)
                        .Replace("{vendorname}", mail.VendorName)
                        .Replace("{vendormail}", mail.VendorMail)
                        .Replace("{vendormobile}", mail.VendorMobile)
                        .Replace("{vendorcode}", mail.VendorCode);
                }
                else
                {
                    throw new Exception("To Email not found!!!");
                }
                SmtpClient client = CreateSmtpClient();
                MailMessage mailMessage = CreateMailMessage(mail.ToMail, subject, mailBody);
                await client.SendMailAsync(mailMessage);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task SendRequestForEditMailToVendors(RequestForEditMailToVendor requestMail)
        {
            try
            {
                string subject = "Requesting For Edit";
                string mailBody = "";
                if (requestMail.ToEmail != null)
                {
                    mailBody = readHtmlString("ApproveRequest.html");
                    mailBody = mailBody.Replace("{recepient}", requestMail.Username)
                        .Replace("{formId}", requestMail.Form_Id.ToString())
                        .Replace("{email}", requestMail.ToEmail);
                }
                else
                {
                    throw new Exception("To Email not found!!!");
                }

                SmtpClient client = CreateSmtpClient();
                MailMessage mailMessage = CreateMailMessage(requestMail.ToEmail, subject, mailBody);
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

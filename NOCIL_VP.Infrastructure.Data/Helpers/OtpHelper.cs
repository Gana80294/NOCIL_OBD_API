using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NOCIL_VP.Domain.Core.Configurations;
using NOCIL_VP.Domain.Core.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NOCIL_VP.Infrastructure.Data.Helpers
{
    public class OtpHelper
    {
        private readonly OtpSetting _otpSettings;

        //string otpRequestApi;
        //string otpValidateApi;
        //string customer_Key;
        //string apiKey;
        //string tokenKey;
        //string cookies;

        public OtpHelper(IOptions<OtpSetting> config)
        {
            _otpSettings = config.Value;
            //otpRequestApi = config.GetValue<string>("OtpDetails:OtpRequestApi");
            //otpValidateApi = config.GetValue<string>("OtpDetails:OtpValidateApi");
            //customer_Key = _config.GetValue<string>("OtpDetails:CustomerKey");
            //apiKey = _config.GetValue<string>("OtpDetails:ApiKey");
            //tokenKey = _config.GetValue<string>("OtpDetails:TokenKey");
            //cookies = _config.GetValue<string>("OtpDetails:Cookie");
        }


        public async Task<dynamic> SendOTP(string mobileNo)
        {

            var jsonPayload = new
            {
                customerKey = _otpSettings.CustomerKey,
                phone = mobileNo,
                authType = "SMS"
            };
            var jsonResponse = await CallMiniOrangeAPI(jsonPayload, _otpSettings.OtpRequestApi);
            string status = jsonResponse.status;
            if (status == "SUCCESS")
            {
                var res = new
                {
                    TxId = jsonResponse.txId,
                    Message = jsonResponse.message,
                };
                return res;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> VerifyOtp(VerifyOtp otp)
        {
            var jsonPayload = new
            {
                txId = otp.TxId,
                token = otp.Otp,
                authType = "SMS"
            };

            var jsonResponse = await CallMiniOrangeAPI(jsonPayload, _otpSettings.OtpValidateApi);
            string status = jsonResponse.status;
            if (status == "SUCCESS") return true;
            else { throw new Exception(jsonResponse.message); };
        }




        private string CalculateSHA512(string input)
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] hashBytes = sha512.ComputeHash(Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            }
        }

        private void FormRequestHeader(HttpClient httpClient)
        {

            string currentTimeInMillis = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            string stringToHash = _otpSettings.CustomerKey + currentTimeInMillis + _otpSettings.ApiKey;
            string hashValue = CalculateSHA512(stringToHash);

            // Setting the headers
            httpClient.DefaultRequestHeaders.Add("Customer-Key", _otpSettings.CustomerKey);
            httpClient.DefaultRequestHeaders.Add("Timestamp", currentTimeInMillis);
            httpClient.DefaultRequestHeaders.Add("Authorization", hashValue);
            httpClient.DefaultRequestHeaders.Add("Cookie", _otpSettings.Cookie);
        }

        private async Task<dynamic> CallMiniOrangeAPI(object jsonPayload, string url)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                this.FormRequestHeader(httpClient);

                // Setting the request content
                HttpContent content = new StringContent(JsonConvert.SerializeObject(jsonPayload), Encoding.UTF8, "application/json");

                // Calling the rest API
                HttpResponseMessage response = await httpClient.PostAsync(url, content);

                // If invalid response is received, throwing a Runtime Exception
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Invalid response received from authentication server. HTTP error code: {response.StatusCode}");
                }

                // If a valid response is received, get the JSON response
                string result = await response.Content.ReadAsStringAsync();
                dynamic jsonResponse = JsonConvert.DeserializeObject(result);
                return jsonResponse;
            }
        }
    }
}

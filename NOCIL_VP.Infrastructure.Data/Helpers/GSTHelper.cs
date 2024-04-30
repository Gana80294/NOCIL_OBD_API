﻿using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NOCIL_VP.Domain.Core.Dtos.Response;
using NOCIL_VP.Domain.Core.Entities.Registration.CommonData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Infrastructure.Data.Helpers
{
    public class GSTHelper
    {
        private readonly IConfiguration _config;
        string apiKey;
        string requestApi;
        public GSTHelper(IConfiguration config)
        {
            _config = config;
        }

        public void SetValues()
        {
            this.requestApi = _config.GetValue<string>("GSTINDetails:RequestApi");
            this.apiKey = _config.GetValue<string>("GSTINDetails:apikey");
        }

        public async Task<dynamic> GetGstDetails(string gstin)
        {
            try
            {
                SetValues();
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("accept", "application/json");
                    client.DefaultRequestHeaders.Add("apikey", apiKey);

                    HttpResponseMessage response = await client.GetAsync(requestApi + gstin);

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        var detail = JsonConvert.DeserializeObject<dynamic>(json);
                        return detail;
                    }
                    else
                    {
                        throw new Exception("Failed to retrieve data. Status code: " + response.StatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public TaxPayerDetails FormAddressData(IrisApiSuccessResponse data)
        {
            TaxPayerDetails details = new TaxPayerDetails();
            details.Name = data.name;
            DateTime regDate;
            if (DateTime.TryParseExact(data.registrationDate, "yyyy-mm-dd", null, System.Globalization.DateTimeStyles.None, out regDate))
            {
                details.RegistrationDate = regDate;
            }
            else
            {
                details.RegistrationDate = (DateTime?)null;
            }
            details.Addresses = new List<string>();
            details.Addresses.Add(BuildAddressDataFromObject(data.pradr));
            foreach (var item in data.adadr)
            {
                var adAddress = BuildAddressDataFromObject(item);
                details.Addresses.Add(adAddress);
            }
            return details;
        }

        public string BuildAddressDataFromObject(IrisApiAddress item)
        {
            var add = "";
            add += !string.IsNullOrEmpty(item.flno) ? item.flno+", " : "";
            add += !string.IsNullOrEmpty(item.bno) ? item.bno + ", " : "";
            add += !string.IsNullOrEmpty(item.bnm) ? item.bnm + ", " : "";
            add += !string.IsNullOrEmpty(item.st) ? item.st + ", " : "";
            add += !string.IsNullOrEmpty(item.loc) ? item.loc + ", " : "";
            add += !string.IsNullOrEmpty(item.district) ? item.district + ", " : "";
            add += !string.IsNullOrEmpty(item.stcd) ? item.stcd + ", " : "";
            add += !string.IsNullOrEmpty(item.pncd) ? item.pncd : "";
            return add.Trim();
        }
    }
}

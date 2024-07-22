using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NOCIL_VP.Domain.Core.Configurations;
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
        private readonly GstSetting _gstSettings;


        public GSTHelper(IOptions<GstSetting> gst)
        {
            _gstSettings = gst.Value;
        }


        public async Task<dynamic> GetGstDetails(string gstin)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("accept", "application/json");
                    client.DefaultRequestHeaders.Add("apikey", _gstSettings.apikey);

                    HttpResponseMessage response = await client.GetAsync(_gstSettings.RequestApi + gstin);

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
            details.Name = data.tradename;
            DateTime regDate;
            if (DateTime.TryParseExact(data.registrationDate, "yyyy-mm-dd", null, System.Globalization.DateTimeStyles.None, out regDate))
            {
                details.RegistrationDate = regDate;
            }
            else
            {
                details.RegistrationDate = (DateTime?)null;
            }
            details.Addresses = [BuildAddressDataFromObject(data.pradr)];
            foreach (var item in data.adadr)
            {
                var adAddress = BuildAddressDataFromObject(item);
                details.Addresses.Add(adAddress);
            }
            return details;
        }

        public TaxPayerAddress BuildAddressDataFromObject(IrisApiAddress item)
        {
            var add = new TaxPayerAddress()
            {
                House_No = item.bno,
                Street_2 = item.st,
                Street_3 = item.loc,
                Street_4 = $"{item.bnm},{item.flno}".Trim(),
                Street_5 = "",
                City = item.city,
                District = item.district,
                Postal_Code = item.pncd,
            };
            return add;
        }
    }
}

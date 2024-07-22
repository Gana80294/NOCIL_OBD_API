using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Dtos.Response
{
    public class IrisApiSuccessResponse
    {
        public string status_code { get; set; }
        public string gstin { get; set; }
        public string fetchTime { get; set; }
        public string name { get; set; }
        public string tradename { get; set; }
        public string registrationDate { get; set; }
        public string center { get; set; }
        public string state { get; set; }
        public string center_cd { get; set; }
        public string state_cd { get; set; }
        public string constitution { get; set; }
        public string type { get; set; }
        public string status { get; set; }
        public string lastUpdateDate { get; set; }
        public string cancellationDate { get; set; }
        public string einvoiceStatus { get; set; }
        public string[] nature { get; set; }
        public IrisApiAddress pradr { get; set; }
        public List<IrisApiAddress> adadr { get; set; }
    }

    public class IrisApiAddress
    {
        public string bnm { get; set; }
        public string st { get; set; }
        public string loc { get; set; }
        public string bno { get; set; }
        public string stcd { get; set; }
        public string flno { get; set; }
        public string lt { get; set; }
        public string lg { get; set; }
        public string pncd { get; set; }
        public string ntr { get; set; }
        public string district { get; set; }
        public string city { get; set; }
        public string locality { get; set; }
        public string geocodelvl { get; set; }
        public string landMark { get; set; }

    }

    public class TaxPayerDetails
    {
        public string Name { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public List<TaxPayerAddress> Addresses { get; set; }
    }

    public class TaxPayerAddress
    {
        public string House_No { get; set; }
        public string Street_2 { get; set; }
        public string Street_3 { get; set; }
        public string Street_4 { get; set; }
        public string Street_5 { get; set; }
        public string City { get; set; }
        public string Postal_Code { get; set; }
        public string District { get; set; }
        public string Country_Code { get; set; }
        public string Region_Id { get; set; }
    }
}

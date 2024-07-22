using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NOCIL_VP.API.Logging;
using NOCIL_VP.Domain.Core.Configurations;
using NOCIL_VP.Domain.Core.Entities.Registration.CommonData;
using SAP_VENDOR_CREATE_SERVICE;
using System.ServiceModel;
using System.Text;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;

namespace NOCIL_VP.Infrastructure.Data.Helpers
{


    public class VendorItem
    {
        public string COMPANY_CODE { get; set; }
        public string PURCHASING_ORG { get; set; }
        public string ACCOUNT_GRP { get; set; }
        public string NAME1 { get; set; }
        public string NAME2 { get; set; }
        public string NAME3 { get; set; }
        public string SEARCH_TERM { get; set; }
        public string STREET_2 { get; set; }
        public string STREET_3 { get; set; }
        public string STREET_HOUSE_NUMBER { get; set; }
        public string STREET_4 { get; set; }
        public string STREET_5 { get; set; }
        public string CITY { get; set; }
        public string POSTAL_CODE { get; set; }
        public string DISTRICT { get; set; }
        public string COUNTRY { get; set; }
        public string REGION { get; set; }
        public string LANGUAGE { get; set; }
        public string TELEPHONE { get; set; }
        public string TEL_EXTENS { get; set; }
        public string FAX { get; set; }
        public string MOBILE_PHONE { get; set; }
        public string E_MAIL { get; set; }
        public string TAX_NUMBER_3 { get; set; }
        public string INDUSTRY { get; set; }
        public string INITIATORS_NAME { get; set; }
        public string PAN_NUMBER { get; set; }
        public string GST_VEN_CLASS { get; set; }
        public string FIRST_NAME { get; set; }
        public string CP_NAME { get; set; }
        public string RECON_ACCOUNT { get; set; }
        public string ORDER_CURRENCY { get; set; }
        public string INCOTERMS { get; set; }
        public string INCOTERMS_TEXT { get; set; }
        public string SCHEMA_GROUP_VENDOR { get; set; }
        public string GR_BASED_INV_VERIF { get; set; }
        public string SRV_BASED_INV_VERIF { get; set; }
        public string VENDOR_CODE { get; set; }
        public string MESSAGE { get; set; }
    }

    public class VendorItemDto
    {
        public string COMPANY_CODE { get; set; }
        public string PURCHASING_ORG { get; set; }
        public string ACCOUNT_GRP { get; set; }
        public string NAME1 { get; set; }
        public string NAME2 { get; set; }
        public string NAME3 { get; set; }
        public string SEARCH_TERM { get; set; }
        public List<Address> ADDRESSES { get; set; }
        public List<Contact> CONTACTS { get; set; }
        public string LANGUAGE { get; set; }
        public string TAX_NUMBER_3 { get; set; }
        public string INDUSTRY { get; set; }
        public string INITIATORS_NAME { get; set; }
        public string PAN_NUMBER { get; set; }
        public string GST_VEN_CLASS { get; set; }
        public string RECON_ACCOUNT { get; set; }
        public string ORDER_CURRENCY { get; set; }
        public string INCOTERMS { get; set; }
        public string INCOTERMS_TEXT { get; set; }
        public string SCHEMA_GROUP_VENDOR { get; set; }
        public string GR_BASED_INV_VERIF { get; set; }
        public string SRV_BASED_INV_VERIF { get; set; }
        public string VENDOR_CODE { get; set; }
        public string MESSAGE { get; set; }
    }


    #region Vendor Create

    public class VendorService
    {
        private AppSetting _appSetting;
        private ztestClient _soapClient;

        public VendorService(IOptions<AppSetting> config)
        {
            _appSetting = config.Value;
            var binding = new BasicHttpBinding();
            var endpoint = new EndpointAddress("http://nlpveccdev.NOCIL.LOCAL:8000/sap/bc/srt/rfc/sap/ztest/400/ztest/test");
            _soapClient = new ztestClient(binding, endpoint);
        }

        public async Task<ZMM_RFC_VENDOR_CREATEResponse> CreateVendorAsync(VendorItem request)
        {
            try
            {
                var soapRequest = new ZMM_RFC_VENDOR_CREATERequest
                {
                    ZMM_RFC_VENDOR_CREATE = new ZMM_RFC_VENDOR_CREATE
                    {
                        IM_VENDOR_DATA =
                        [
                            new ZMM_ST_VENDOR_DATA
                            {
                                COMPANY_CODE = request.COMPANY_CODE,
                                PURCHASING_ORG = request.PURCHASING_ORG,
                                ACCOUNT_GRP = request.ACCOUNT_GRP,
                                NAME1 = request.NAME1,
                                NAME2 = request.NAME2,
                                NAME3 = request.NAME3,
                                SEARCH_TERM = request.SEARCH_TERM,
                                STREET_2 = request.STREET_2,
                                STREET_3 = request.STREET_3,
                                STREET_HOUSE_NUMBER = request.STREET_HOUSE_NUMBER,
                                STREET_4 = request.STREET_4,
                                STREET_5 = request.STREET_5,
                                CITY = request.CITY,
                                POSTAL_CODE = request.POSTAL_CODE,
                                DISTRICT = request.DISTRICT,
                                COUNTRY = request.COUNTRY,
                                REGION = request.REGION,
                                LANGUAGE = request.LANGUAGE.Substring(0,1),
                                TELEPHONE = request.TELEPHONE,
                                TEL_EXTENS = request.TEL_EXTENS,
                                FAX = request.FAX,
                                MOBILE_PHONE = request.MOBILE_PHONE,
                                E_MAIL = request.E_MAIL,
                                TAX_NUMBER_3 = string.IsNullOrEmpty(request.TAX_NUMBER_3) ? "NA" : request.TAX_NUMBER_3,
                                INDUSTRY = request.INDUSTRY,
                                INITIATORS_NAME = request.INITIATORS_NAME,
                                PAN_NUMBER = request.PAN_NUMBER,
                                GST_VEN_CLASS = request.GST_VEN_CLASS,
                                FIRST_NAME = request.FIRST_NAME,
                                CP_NAME = request.CP_NAME,
                                RECON_ACCOUNT = request.RECON_ACCOUNT,
                                ORDER_CURRENCY = request.ORDER_CURRENCY,
                                INCOTERMS=request.INCOTERMS,
                                INCOTERMS_TEXT = request.INCOTERMS_TEXT,
                                SCHEMA_GROUP_VENDOR = request.SCHEMA_GROUP_VENDOR,
                                GR_BASED_INV_VERIF = request.GR_BASED_INV_VERIF,
                                SRV_BASED_INV_VERIF = request.SRV_BASED_INV_VERIF,
                                VENDOR_CODE = "",
                                MESSAGE = ""
                            }
                        ]
                    }
                };

                string xmlRequest = GetSoapXmlPayload(soapRequest.ZMM_RFC_VENDOR_CREATE.IM_VENDOR_DATA[0]);
                LogWritter.WriteErrorLog(new string('*', 10) + "XML PAYLOAD" + new string('*', 10));
                LogWritter.WriteErrorLog(xmlRequest);
                LogWritter.WriteErrorLog(JsonConvert.SerializeObject(soapRequest.ZMM_RFC_VENDOR_CREATE));
                LogWritter.WriteErrorLog(new string('*', 10) + "XML PAYLOAD" + new string('*', 10));

                var response = await _soapClient.ZMM_RFC_VENDOR_CREATEAsync(soapRequest.ZMM_RFC_VENDOR_CREATE);

                LogWritter.WriteErrorLog(new string('*', 10) + "XML RESPONSE START" + new string('*', 10));
                LogWritter.WriteErrorLog("Response Json :" + JsonConvert.SerializeObject(response.ZMM_RFC_VENDOR_CREATEResponse));
                LogWritter.WriteErrorLog(new string('*', 10) + "XML RESPONSE END" + new string('*', 10));
                return response.ZMM_RFC_VENDOR_CREATEResponse;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }

        private string GetSoapXmlPayload(ZMM_ST_VENDOR_DATA request)
        {

            // Create the XML document
            XmlDocument doc = new XmlDocument();

            // Create the root element
            XmlElement envelope = doc.CreateElement("soapenv", "Envelope", "http://schemas.xmlsoap.org/soap/envelope/");
            doc.AppendChild(envelope);

            // Add the namespace for the urn
            XmlAttribute xmlnsAttr = doc.CreateAttribute("xmlns:urn");
            xmlnsAttr.Value = "urn:sap-com:document:sap:rfc:functions";
            envelope.Attributes.Append(xmlnsAttr);


            // Create the header element
            XmlElement header = doc.CreateElement("soapenv", "Header", "http://schemas.xmlsoap.org/soap/envelope/");
            envelope.AppendChild(header);

            // Create the body element
            XmlElement body = doc.CreateElement("soapenv", "Body", "http://schemas.xmlsoap.org/soap/envelope/");
            envelope.AppendChild(body);

            // Create the ZMM_RFC_VENDOR_CREATE element
            XmlElement vendorCreate = doc.CreateElement("urn", "ZMM_RFC_VENDOR_CREATE", "urn:sap-com:document:sap:rfc:functions");
            body.AppendChild(vendorCreate);

            // Create the IM_VENDOR_DATA element
            XmlElement imVendorData = doc.CreateElement("IM_VENDOR_DATA");
            vendorCreate.AppendChild(imVendorData);

            // Create the item element
            XmlElement item = doc.CreateElement("item");
            imVendorData.AppendChild(item);

            // Add the vendor data to the item element
            AddVendorData(doc, item, request);

            return doc.OuterXml;
        }

        private string GetSoap12XmlPayload(VendorItem request)
        {

            // Create the XML document
            XmlDocument doc = new XmlDocument();

            // Create the root element
            XmlElement envelope = doc.CreateElement("soap", "Envelope", "http://www.w3.org/2003/05/soap-envelope");
            doc.AppendChild(envelope);

            // Add the namespace for the urn
            XmlAttribute xmlnsAttr = doc.CreateAttribute("xmlns:urn");
            xmlnsAttr.Value = "urn:sap-com:document:sap:rfc:functions";
            envelope.Attributes.Append(xmlnsAttr);


            // Create the header element
            XmlElement header = doc.CreateElement("soap", "Header", "http://schemas.xmlsoap.org/soap/envelope/");
            envelope.AppendChild(header);

            // Create the body element
            XmlElement body = doc.CreateElement("soap", "Body", "http://schemas.xmlsoap.org/soap/envelope/");
            envelope.AppendChild(body);

            // Create the ZMM_RFC_VENDOR_CREATE element
            XmlElement vendorCreate = doc.CreateElement("urn", "ZMM_RFC_VENDOR_CREATE", "urn:sap-com:document:sap:rfc:functions");
            body.AppendChild(vendorCreate);

            // Create the IM_VENDOR_DATA element
            XmlElement imVendorData = doc.CreateElement("IM_VENDOR_DATA");
            vendorCreate.AppendChild(imVendorData);

            // Create the item element
            XmlElement item = doc.CreateElement("item");
            imVendorData.AppendChild(item);

            // Add the vendor data to the item element
            //AddVendorData(doc, item, request);

            return doc.OuterXml;
        }

        private void AddVendorData(XmlDocument doc, XmlElement item, ZMM_ST_VENDOR_DATA vendorItem)
        {
            // Add the vendor data elements
            AddElement(doc, item, "COMPANY_CODE", vendorItem.COMPANY_CODE);
            AddElement(doc, item, "PURCHASING_ORG", vendorItem.PURCHASING_ORG);
            AddElement(doc, item, "ACCOUNT_GRP", vendorItem.ACCOUNT_GRP);
            AddElement(doc, item, "NAME1", vendorItem.NAME1);
            AddElement(doc, item, "NAME2", vendorItem.NAME2);
            AddElement(doc, item, "NAME3", vendorItem.NAME3);
            AddElement(doc, item, "SEARCH_TERM", vendorItem.SEARCH_TERM);
            AddElement(doc, item, "STREET_2", vendorItem.STREET_2);
            AddElement(doc, item, "STREET_3", vendorItem.STREET_3);
            AddElement(doc, item, "STREET_HOUSE_NUMBER", vendorItem.STREET_HOUSE_NUMBER);
            AddElement(doc, item, "STREET_4", vendorItem.STREET_4);
            AddElement(doc, item, "STREET_5", vendorItem.STREET_5);
            AddElement(doc, item, "CITY", vendorItem.CITY);
            AddElement(doc, item, "POSTAL_CODE", vendorItem.POSTAL_CODE);
            AddElement(doc, item, "DISTRICT", vendorItem.DISTRICT);
            AddElement(doc, item, "COUNTRY", vendorItem.COUNTRY);
            AddElement(doc, item, "REGION", vendorItem.REGION);
            AddElement(doc, item, "LANGUAGE", vendorItem.LANGUAGE);
            AddElement(doc, item, "TELEPHONE", vendorItem.TELEPHONE);
            AddElement(doc, item, "TEL_EXTENS", vendorItem.TEL_EXTENS);
            AddElement(doc, item, "FAX", vendorItem.FAX);
            AddElement(doc, item, "MOBILE_PHONE", vendorItem.MOBILE_PHONE);
            AddElement(doc, item, "E_MAIL", vendorItem.E_MAIL);
            AddElement(doc, item, "TAX_NUMBER_3", vendorItem.TAX_NUMBER_3);
            AddElement(doc, item, "INDUSTRY", vendorItem.INDUSTRY);
            AddElement(doc, item, "INITIATORS_NAME", vendorItem.INITIATORS_NAME);
            AddElement(doc, item, "PAN_NUMBER", vendorItem.PAN_NUMBER);
            AddElement(doc, item, "GST_VEN_CLASS", vendorItem.GST_VEN_CLASS);
            AddElement(doc, item, "FIRST_NAME", vendorItem.FIRST_NAME);
            AddElement(doc, item, "CP_NAME", vendorItem.CP_NAME);
            AddElement(doc, item, "RECON_ACCOUNT", vendorItem.RECON_ACCOUNT);
            AddElement(doc, item, "ORDER_CURRENCY", vendorItem.ORDER_CURRENCY);
            AddElement(doc, item, "INCOTERMS", vendorItem.INCOTERMS);
            AddElement(doc, item, "INCOTERMS_TEXT", vendorItem.INCOTERMS_TEXT);
            AddElement(doc, item, "SCHEMA_GROUP_VENDOR", vendorItem.SCHEMA_GROUP_VENDOR);
            AddElement(doc, item, "GR_BASED_INV_VERIF", vendorItem.GR_BASED_INV_VERIF);
            AddElement(doc, item, "SRV_BASED_INV_VERIF", vendorItem.SRV_BASED_INV_VERIF);
            AddElement(doc, item, "VENDOR_CODE", vendorItem.VENDOR_CODE);
            AddElement(doc, item, "MESSAGE", vendorItem.MESSAGE);
            // Add more vendor data elements as needed
        }

        private void AddElement(XmlDocument doc, XmlElement parent, string name, string value)
        {
            XmlElement element = doc.CreateElement(name);
            element.InnerText = value;
            parent.AppendChild(element);
        }

        private string GetInnerText(XmlDocument doc, string xPath, XmlNamespaceManager nsmgr)
        {
            var node = doc.SelectSingleNode(xPath, nsmgr);
            if (node != null) return node.InnerText;
            return null;
        }
    }



    #endregion


}


using AutoMapper;
using NOCIL_VP.Domain.Core.Dtos.Registration;
using NOCIL_VP.Domain.Core.Dtos.Registration.CommonData;
using NOCIL_VP.Domain.Core.Dtos.Registration.Domestic;
using NOCIL_VP.Domain.Core.Dtos.Registration.Evaluation;
using NOCIL_VP.Domain.Core.Dtos.Registration.Transport;
using NOCIL_VP.Domain.Core.Entities.Registration.Attachments;
using NOCIL_VP.Domain.Core.Entities.Registration.CommonData;
using NOCIL_VP.Domain.Core.Entities.Registration.Domestic;
using NOCIL_VP.Domain.Core.Entities.Registration.Evaluation;
using NOCIL_VP.Domain.Core.Entities.Registration.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Infrastructure.Data.Mapper
{
    public class RegistrationProfile : Profile
    {
        public RegistrationProfile()
        {
            CreateMap<VendorPersonalData_Dto, VendorPersonalData>().ReverseMap();
            CreateMap<VendorOrganizationProfile_Dto, VendorOrganizationProfile>().ReverseMap();
            CreateMap<TransportVendorPersonalData_Dto,TransportVendorPersonalData>().ReverseMap();
            CreateMap<TankerDetail_Dto, TankerDetail>().ReverseMap();
            CreateMap<VehicleDetails,VehicleDetails_Dto>().ReverseMap();
            CreateMap<TechnicalProfile_Dto, TechnicalProfile>().ReverseMap();
            CreateMap<Subsideries_Dto, Subsideries>().ReverseMap();
            CreateMap<MajorCustomer_Dto, MajorCustomer>().ReverseMap();
            CreateMap<CommercialProfile_Dto, CommercialProfile>().ReverseMap();
            CreateMap<Bank_Detail_Dto, Bank_Detail>().ReverseMap();
            CreateMap<Address_Dto, Address>().ReverseMap();
            CreateMap<Contact_Dto, Contact>().ReverseMap();
            CreateMap<VendorBranch_Dto, VendorBranch>().ReverseMap();
            CreateMap<ProprietorOrPartner_Dto, ProprietorOrPartner>().ReverseMap();
            CreateMap<AnnualTurnOver_Dto, AnnualTurnOver>().ReverseMap();
            CreateMap<AttachmentDto, Attachment>().ReverseMap();
            CreateMap<NocilRelatedEmployeeDto, NocilRelatedEmployee>().ReverseMap();
            CreateMap<VendorGradeDto, VendorGrade>().ReverseMap();
            CreateMap<AdditionalFields_Dto, AdditionalFields>().ReverseMap();
        }
    }
}

using AutoMapper;
using Microsoft.Extensions.Configuration;
using NOCIL_VP.Domain.Core.Dtos.Registration.Evaluation;
using NOCIL_VP.Domain.Core.Dtos.Response;
using NOCIL_VP.Domain.Core.Entities;
using NOCIL_VP.Domain.Core.Entities.Registration.Evaluation;
using NOCIL_VP.Infrastructure.Interfaces.Repositories.Registration.Evaluation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Infrastructure.Data.Repositories.Registration.Evaluation
{
    public class VendorGradeRepository : Repository<VendorGrade>, IVendorGradeRepository
    {
        private readonly VpContext _dbContext;
        private readonly IMapper _mapper;

        public VendorGradeRepository(VpContext context, IMapper mapper) : base(context)
        {
            _dbContext = context;
            _mapper = mapper;
        }


        public async Task<ResponseMessage> AddVendorGrade(VendorGradeDto vendorGrade)
        {
            Add(this._mapper.Map<VendorGrade>(vendorGrade));
            await SaveAsync();
            return new ResponseMessage()
            {
                Status = (int)HttpStatusCode.OK,
                Error = "",
                Message = "Vendor grade added successfully"
            };
        }

        public async Task<ResponseMessage> UpdateVendorGrade(VendorGradeDto vendorGrade)
        {
            Update(this._mapper.Map<VendorGrade>(vendorGrade));
            await SaveAsync();
            return new ResponseMessage()
            {
                Status = (int)HttpStatusCode.OK,
                Error = "",
                Message = "Vendor grade updated successfully"
            };
        }

        public VendorGrade GetVendorGradeById(int formId)
        {
            var res = this._dbContext.VendorGrades.FirstOrDefault(x => x.FormId == formId);
            return res;
        }
    }
}

using NOCIL_VP.Domain.Core.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Infrastructure.Data.Helpers
{
    public class ResponseWritter
    {
        public static ResponseMessage response;

        public static ResponseMessage WriteSuccessResponse(string Mesage)
        {
            response = new ResponseMessage();
            response.Message = Mesage;
            response.Status = 200;
            response.Error = "";
            return response;
        }

        public static ResponseMessage WriteErrorResponse(string Mesage)
        {
            response = new ResponseMessage();
            response.Message = "";
            response.Status = 400;
            response.Error = Mesage;
            return response;
        }
    }
}

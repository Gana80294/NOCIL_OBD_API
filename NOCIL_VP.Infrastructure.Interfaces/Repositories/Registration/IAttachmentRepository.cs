using NOCIL_VP.Domain.Core.Dtos.Registration;
using NOCIL_VP.Domain.Core.Dtos.Response;
using NOCIL_VP.Domain.Core.Entities.Registration.Attachments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Infrastructure.Interfaces.Repositories.Registration
{
    public interface IAttachmentRepository:IRepository<Attachment>
    {
        Task<AttachmentDto> AddAttachment(Attachment attachment);
        Task<AttachmentDto> UpdateAttachment(Attachment attachment);
        Task<string> SaveFileToLocalFolder(byte[] fileContent, string fileName);
        Task<AttachmentResponse> GetAttachmentById(int attachmentId);
        Task<bool> DeleteAttachmentById(int id);
    }
}

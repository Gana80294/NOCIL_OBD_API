using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NOCIL_VP.Domain.Core.Configurations;
using NOCIL_VP.Domain.Core.Dtos.Registration;
using NOCIL_VP.Domain.Core.Dtos.Response;
using NOCIL_VP.Domain.Core.Entities;
using NOCIL_VP.Domain.Core.Entities.Registration.Attachments;
using NOCIL_VP.Infrastructure.Data.Helpers;
using NOCIL_VP.Infrastructure.Interfaces.Repositories.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Infrastructure.Data.Repositories.Registration
{
    public class AttachmentRepository : Repository<Attachment>, IAttachmentRepository
    {
        private VpContext _dbContext;
        private AppSetting _appSettings;
        private IMapper _mapper;

        public AttachmentRepository(VpContext dbContext, IOptions<AppSetting> config, IMapper mapper) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
            this._appSettings = config.Value;
        }

        public async Task<AttachmentDto> AddAttachment(Attachment attachment)
        {
            try
            {
                var newAttachment = Add(attachment);
                await SaveAsync();
                return this._mapper.Map<AttachmentDto>(newAttachment);
                //return ResponseWritter.WriteSuccessResponse("Attachment Added Successfully.");
            }
            catch (Exception ex)
            {
                File.Delete(attachment.File_Path);
                throw ex;
            }

        }

        public async Task<AttachmentDto> UpdateAttachment(Attachment attachment)
        {
            try
            {
                var oldDoc = this._dbContext.Attachments.FirstOrDefault(x => x.Attachment_Id == attachment.Attachment_Id);
                if(oldDoc != null) { File.Delete(oldDoc.File_Path); }
                Update(attachment);
                await SaveAsync();
                return this._mapper.Map<AttachmentDto>(attachment);
            }
            catch (Exception ex)
            {
                File.Delete(attachment.File_Path);
                throw ex;
            }
        }

        public async Task<string> SaveFileToLocalFolder(byte[] fileContent, string fileName)
        {
            CreateFolder();
            await File.WriteAllBytesAsync(Path.Combine(_appSettings.AttachmentFolderPath, fileName), fileContent);
            return Path.Combine(_appSettings.AttachmentFolderPath, fileName);
        }

        public async Task<AttachmentResponse> GetAttachmentById(int attachmentId)
        {
            var attachment = (from tb in _dbContext.Attachments
                              where tb.Attachment_Id == attachmentId
                              select new AttachmentResponse
                              {
                                  FileName = tb.File_Name,
                                  DocType = tb.File_Type,
                                  Extension = tb.File_Extension,
                                  FilePath = tb.File_Path
                              }).FirstOrDefault();
            if (attachment != null)
            {
                attachment.FileContent = await GetAttachmentByPath(attachment.FilePath);
                return attachment;
            }
            else
            {
                throw new FileNotFoundException("File not found");
            }
        }

        public async Task<bool> DeleteAttachmentById(int id)
        {
            var attchment = GetById(id);
            if (attchment != null)
            {
                Remove(attchment);
                File.Delete(attchment.File_Path);
                Save();
                return true;
            }
            else
            {
                throw new Exception("Unable to find the document");
            }
        }

        #region Local folder functions

        public void CreateFolder()
        {
            if (!Directory.Exists(_appSettings.AttachmentFolderPath))
            {
                Directory.CreateDirectory(_appSettings.AttachmentFolderPath);
            }
        }

        public async Task<byte[]> GetAttachmentByPath(string path)
        {
            byte[] file = await File.ReadAllBytesAsync(path);
            return file;
        }

        #endregion
    }
}

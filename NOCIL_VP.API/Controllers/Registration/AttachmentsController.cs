using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NOCIL_VP.Domain.Core.Dtos.Registration;
using NOCIL_VP.Domain.Core.Entities.Registration.Attachments;
using NOCIL_VP.Infrastructure.Data.Helpers;
using NOCIL_VP.Infrastructure.Interfaces.Repositories.Registration;

namespace NOCIL_VP.API.Controllers.Registration
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AttachmentsController : ControllerBase
    {
        private IAttachmentRepository _attachmentRepository;
        private ModelValidator _validator;
        private IMapper _mapper;

        public AttachmentsController(IAttachmentRepository attachmentRepository, IMapper mapper)
        {
            this._attachmentRepository = attachmentRepository;
            this._mapper = mapper;
            this._validator = new ModelValidator();
        }


        [HttpPost]
        public async Task<IActionResult> AttachFiles()
        {
            try
            {
                var request = Request;
                IFormFileCollection postedfiles = request.Form.Files;
                AttachmentDto attachment = JsonConvert.DeserializeObject<AttachmentDto>(request.Form["AttachmentDetails"]);
                var currentDateTime = DateTime.Now;
                attachment.File_Name = attachment.Form_Id.ToString() + "_" +
                    attachment.File_Type + "_" + currentDateTime.ToString().Replace(":", "").Replace("/", "") + "." + postedfiles[0].FileName.Split('.')[^1];

                using (Stream st = postedfiles[0].OpenReadStream())
                {
                    using (BinaryReader br = new BinaryReader(st))
                    {
                        byte[] fileBytes = br.ReadBytes((Int32)st.Length);
                        if (fileBytes.Length > 0)
                        {
                            attachment.File_Path = await _attachmentRepository.SaveFileToLocalFolder(fileBytes, attachment.File_Name);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(attachment.File_Path))
                {
                    var errors = this._validator.ValidateModel(attachment);
                    if (errors != null && errors.Count > 0)
                    {
                        System.IO.File.Delete(attachment.File_Path);
                        throw new ArgumentException(string.Join(", ", errors));
                    }
                    else
                    {
                        var attach = this._mapper.Map<Attachment>(attachment);
                        var res = new AttachmentDto();
                        if (attachment.Attachment_Id != 0)
                        {
                            res = await this._attachmentRepository.UpdateAttachment(attach);
                        }
                        else
                        {
                            res = await _attachmentRepository.AddAttachment(attach);
                        }
                        return Ok(res);
                    }
                }
                else return BadRequest("File not saved due to some error");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetAttachmentById(int attachmentId)
        {
            return Ok(await this._attachmentRepository.GetAttachmentById(attachmentId));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAttachmentById(int id)
        {
            var status = await _attachmentRepository.DeleteAttachmentById(id);
            if (status) return Ok(ResponseWritter.WriteSuccessResponse("File deleted successfully"));
            else throw new Exception("Unable to delete the file");
        }
    }
}

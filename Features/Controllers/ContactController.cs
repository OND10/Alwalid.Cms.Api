using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Features.Contact.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace CMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {

        private readonly IEmailSender _emailSender;
        public ContactController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }


        [HttpPost("sendEmail")]
        public async Task<Result<EmailResponseDto>>EmailPosting(EmailRequestDto request)
        {
            var result = _emailSender.SendEmailAsync(request.Email, request.Subject, request.Description);

            var response = new EmailResponseDto
            {
                Description = request.Description,
                Subject = request.Subject,
            };

            if (result.Status == TaskStatus.Faulted)
            {
                return await Result<EmailResponseDto>.SuccessAsync(response, "Faild in sending form", false);
            }
            return await Result<EmailResponseDto>.SuccessAsync(response, "Email is sent Successfully", true);
        }      

    }
}

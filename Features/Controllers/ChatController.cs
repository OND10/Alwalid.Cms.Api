using Alwalid.Cms.Api.Features.Conversation.Dtos;
using Alwalid.Cms.Api.Features.Conversation.Services;
using Alwalid.Cms.Api.Features.Message.Dtos;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Alwalid.Cms.Api.Features.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IConversationService _conversationService;

        public ChatController(IConversationService conversationService)
        {
            _conversationService = conversationService;
        }

        [HttpPost("message")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostMessage(ChatRequestDto request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            try
            {
                await _conversationService.HandleUserMessageAsync(request.ConversationId, userId, request.Message);

               return Accepted();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return StatusCode(500, "An internal server error occurred.");
            }
        }

        [HttpGet("{conversationId:guid}")]
        public async Task<IActionResult> GetConversationHistory(Guid conversationId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var conversation = await _conversationService.GetConversationByIdAsync(conversationId, userId);
            if (conversation == null) return NotFound();

            return Ok(conversation);
        }
    }
}

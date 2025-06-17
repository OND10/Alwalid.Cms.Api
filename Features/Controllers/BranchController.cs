using Microsoft.AspNetCore.Mvc;
using Alwalid.Cms.Api.Features.Branch.Commands.AddBranch;
using Alwalid.Cms.Api.Features.Branch.Commands.UpdateBranch;
using Alwalid.Cms.Api.Features.Branch.Commands.DeleteBranch;
using Alwalid.Cms.Api.Features.Branch.Queries.GetAllBranches;
using Alwalid.Cms.Api.Features.Branch.Queries.GetBranchById;
using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;

namespace Alwalid.Cms.Api.Features.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BranchController : ControllerBase
    {
        private readonly ICommandHandler<AddBranchCommand, Result<int>> _addBranchHandler;
        private readonly ICommandHandler<UpdateBranchCommand, Result<bool>> _updateBranchHandler;
        private readonly ICommandHandler<DeleteBranchCommand, Result<bool>> _deleteBranchHandler;
        private readonly IQueryHandler<GetAllBranchesQuery, Result<List<GetAllBranchesResponse>>> _getAllBranchesHandler;
        private readonly IQueryHandler<GetBranchByIdQuery, Result<GetBranchByIdResponse>> _getBranchByIdHandler;

        public BranchController(
            ICommandHandler<AddBranchCommand, Result<int>> addBranchHandler,
            ICommandHandler<UpdateBranchCommand, Result<bool>> updateBranchHandler,
            ICommandHandler<DeleteBranchCommand, Result<bool>> deleteBranchHandler,
            IQueryHandler<GetAllBranchesQuery, Result<List<GetAllBranchesResponse>>> getAllBranchesHandler,
            IQueryHandler<GetBranchByIdQuery, Result<GetBranchByIdResponse>> getBranchByIdHandler)
        {
            _addBranchHandler = addBranchHandler;
            _updateBranchHandler = updateBranchHandler;
            _deleteBranchHandler = deleteBranchHandler;
            _getAllBranchesHandler = getAllBranchesHandler;
            _getBranchByIdHandler = getBranchByIdHandler;
        }

        [HttpPost]
        public async Task<IActionResult> AddBranch([FromBody] AddBranchRequest request)
        {
            var command = new AddBranchCommand(request);
            var result = await _addBranchHandler.HandleAsync(command);
            
            if (result.IsSuccess)
                return Ok(result.Value);
            
            return BadRequest(result.Error);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBranch(int id, [FromBody] UpdateBranchRequest request)
        {
            var command = new UpdateBranchCommand(id, request);
            var result = await _updateBranchHandler.HandleAsync(command);
            
            if (result.IsSuccess)
                return Ok(result.Value);
            
            return BadRequest(result.Error);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBranch(int id)
        {
            var command = new DeleteBranchCommand(id);
            var result = await _deleteBranchHandler.HandleAsync(command);
            
            if (result.IsSuccess)
                return Ok(result.Value);
            
            return BadRequest(result.Error);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBranches()
        {
            var query = new GetAllBranchesQuery();
            var result = await _getAllBranchesHandler.HandleAsync(query);
            
            if (result.IsSuccess)
                return Ok(result.Value);
            
            return BadRequest(result.Error);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBranchById(int id)
        {
            var query = new GetBranchByIdQuery(id);
            var result = await _getBranchByIdHandler.HandleAsync(query);
            
            if (result.IsSuccess)
                return Ok(result.Value);
            
            return NotFound(result.Error);
        }
    }
} 
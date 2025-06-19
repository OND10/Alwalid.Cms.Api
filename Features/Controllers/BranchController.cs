using Microsoft.AspNetCore.Mvc;
using Alwalid.Cms.Api.Features.Branch.Commands.AddBranch;
using Alwalid.Cms.Api.Features.Branch.Commands.UpdateBranch;
using Alwalid.Cms.Api.Features.Branch.Commands.DeleteBranch;
using Alwalid.Cms.Api.Features.Branch.Queries.GetAllBranches;
using Alwalid.Cms.Api.Features.Branch.Queries.GetBranchById;
using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Features.Branch.Dtos;

namespace Alwalid.Cms.Api.Features.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BranchController : ControllerBase
    {
        private readonly ICommandHandler<AddBranchCommand, BranchResponseDto> _addBranchHandler;
        private readonly ICommandHandler<UpdateBranchCommand, Entities.Branch> _updateBranchHandler;
        private readonly ICommandHandler<DeleteBranchCommand, bool> _deleteBranchHandler;
        private readonly IQueryHandler<GetAllBranchesQuery, IEnumerable<Entities.Branch>> _getAllBranchesHandler;
        private readonly IQueryHandler<GetBranchByIdQuery, Entities.Branch> _getBranchByIdHandler;

        public BranchController(
            ICommandHandler<AddBranchCommand, BranchResponseDto> addBranchHandler,
            ICommandHandler<UpdateBranchCommand, Entities.Branch> updateBranchHandler,
            ICommandHandler<DeleteBranchCommand, bool> deleteBranchHandler,
            IQueryHandler<GetAllBranchesQuery, IEnumerable<Entities.Branch>> getAllBranchesHandler,
            IQueryHandler<GetBranchByIdQuery, Entities.Branch> getBranchByIdHandler)
        {
            _addBranchHandler = addBranchHandler;
            _updateBranchHandler = updateBranchHandler;
            _deleteBranchHandler = deleteBranchHandler;
            _getAllBranchesHandler = getAllBranchesHandler;
            _getBranchByIdHandler = getBranchByIdHandler;
        }

        [HttpPost]
        public async Task<IActionResult> AddBranch([FromBody] BranchRequestDto request, CancellationToken cancellationToken)
        {
            var command = new AddBranchCommand
            {
                Request = request
            };
            var result = await _addBranchHandler.Handle(command, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data);

            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBranch(int id, [FromBody] UpdateBranchRequestDto request, CancellationToken cancellationToken)
        {
            var command = new UpdateBranchCommand
            {
                Address = request.Address,
                City = request.City,
                CountryId = request.CountryId,
                Id = request.Id
            };
            var result = await _updateBranchHandler.Handle(command, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data);

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBranch(int id, CancellationToken cancellationToken)
        {
            var command = new DeleteBranchCommand
            {
                Id = id
            };
            var result = await _deleteBranchHandler.Handle(command, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data);

            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBranches(CancellationToken cancellationToken)
        {
            var query = new GetAllBranchesQuery();
            var result = await _getAllBranchesHandler.Handle(query, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data);

            return BadRequest();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBranchById(int id, CancellationToken cancellationToken)
        {
            var query = new GetBranchByIdQuery
            {
                Id = id
            };
            var result = await _getBranchByIdHandler.Handle(query, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data);

            return NotFound();
        }
    }
}
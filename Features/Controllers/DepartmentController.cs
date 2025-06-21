using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Alwalid.Cms.Api.Features.Department.Commands.AddDepartment;
using Alwalid.Cms.Api.Features.Department.Commands.UpdateDepartment;
using Alwalid.Cms.Api.Features.Department.Commands.DeleteDepartment;
using Alwalid.Cms.Api.Features.Department.Queries.GetAllDepartments;
using Alwalid.Cms.Api.Features.Department.Queries.GetDepartmentById;
using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.Department.Dtos;
using Alwalid.Cms.Api.Common.Handler;

namespace Alwalid.Cms.Api.Features.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ODataController
    {
        private readonly ICommandHandler<AddDepartmentCommand, DepartmentResponseDto> _addDepartmentHandler;
        private readonly ICommandHandler<UpdateDepartmentCommand, DepartmentResponseDto> _updateDepartmentHandler;
        private readonly ICommandHandler<DeleteDepartmentCommand, bool> _deleteDepartmentHandler;
        private readonly IQueryHandler<GetAllDepartmentsQuery, IEnumerable<DepartmentResponseDto>> _getAllDepartmentsHandler;
        private readonly IQueryHandler<GetDepartmentByIdQuery, DepartmentResponseDto> _getDepartmentByIdHandler;

        public DepartmentController(
            ICommandHandler<AddDepartmentCommand, DepartmentResponseDto> addDepartmentHandler,
            ICommandHandler<UpdateDepartmentCommand, DepartmentResponseDto> updateDepartmentHandler,
            ICommandHandler<DeleteDepartmentCommand, bool> deleteDepartmentHandler,
            IQueryHandler<GetAllDepartmentsQuery, IEnumerable<DepartmentResponseDto>> getAllDepartmentsHandler,
            IQueryHandler<GetDepartmentByIdQuery, DepartmentResponseDto> getDepartmentByIdHandler)
        {
            _addDepartmentHandler = addDepartmentHandler;
            _updateDepartmentHandler = updateDepartmentHandler;
            _deleteDepartmentHandler = deleteDepartmentHandler;
            _getAllDepartmentsHandler = getAllDepartmentsHandler;
            _getDepartmentByIdHandler = getDepartmentByIdHandler;
        }

        [HttpPost]
        public async Task<IActionResult> AddDepartment([FromBody] DepartmentRequestDto request, CancellationToken cancellationToken)
        {
            var command = new AddDepartmentCommand 
            {
                Request = request,
            };
            var result = await _addDepartmentHandler.Handle(command, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data);

            return BadRequest(result.Message);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(int id, [FromBody] DepartmentRequestDto request, CancellationToken cancellationToken)
        {
            var command = new UpdateDepartmentCommand 
            {
                Id = id,
                Request = request,
            };
            var result = await _updateDepartmentHandler.Handle(command, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data);

            return BadRequest(result.Message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id, CancellationToken cancellationToken)
        {
            var command = new DeleteDepartmentCommand
            {
                Id = id,
            };
            var result = await _deleteDepartmentHandler.Handle(command, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data);

            return BadRequest(result.Message);
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetAllDepartments(CancellationToken cancellationToken)
        {
            var query = new GetAllDepartmentsQuery();
            var result = await _getAllDepartmentsHandler.Handle(query, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data.AsQueryable());

            return BadRequest(result.Message);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartmentById(int id, CancellationToken cancellationToken)
        {
            var query = new GetDepartmentByIdQuery{ Id = id};
            var result = await _getDepartmentByIdHandler.Handle(query, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Data);

            return NotFound(result.Message);
        }
    }
}
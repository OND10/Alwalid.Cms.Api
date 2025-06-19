//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.OData.Query;
//using Microsoft.AspNetCore.OData.Routing.Controllers;
//using Alwalid.Cms.Api.Features.Department.Commands.AddDepartment;
//using Alwalid.Cms.Api.Features.Department.Commands.UpdateDepartment;
//using Alwalid.Cms.Api.Features.Department.Commands.DeleteDepartment;
//using Alwalid.Cms.Api.Features.Department.Queries.GetAllDepartments;
//using Alwalid.Cms.Api.Features.Department.Queries.GetDepartmentById;
//using Alwalid.Cms.Api.Abstractions.Messaging;

//namespace Alwalid.Cms.Api.Features.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class DepartmentController : ODataController
//    {
//        private readonly ICommandHandler<AddDepartmentCommand, Result<int>> _addDepartmentHandler;
//        private readonly ICommandHandler<UpdateDepartmentCommand, Result<bool>> _updateDepartmentHandler;
//        private readonly ICommandHandler<DeleteDepartmentCommand, Result<bool>> _deleteDepartmentHandler;
//        private readonly IQueryHandler<GetAllDepartmentsQuery, Result<List<GetAllDepartmentsResponse>>> _getAllDepartmentsHandler;
//        private readonly IQueryHandler<GetDepartmentByIdQuery, Result<GetDepartmentByIdResponse>> _getDepartmentByIdHandler;

//        public DepartmentController(
//            ICommandHandler<AddDepartmentCommand, Result<int>> addDepartmentHandler,
//            ICommandHandler<UpdateDepartmentCommand, Result<bool>> updateDepartmentHandler,
//            ICommandHandler<DeleteDepartmentCommand, Result<bool>> deleteDepartmentHandler,
//            IQueryHandler<GetAllDepartmentsQuery, Result<List<GetAllDepartmentsResponse>>> getAllDepartmentsHandler,
//            IQueryHandler<GetDepartmentByIdQuery, Result<GetDepartmentByIdResponse>> getDepartmentByIdHandler)
//        {
//            _addDepartmentHandler = addDepartmentHandler;
//            _updateDepartmentHandler = updateDepartmentHandler;
//            _deleteDepartmentHandler = deleteDepartmentHandler;
//            _getAllDepartmentsHandler = getAllDepartmentsHandler;
//            _getDepartmentByIdHandler = getDepartmentByIdHandler;
//        }

//        [HttpPost]
//        public async Task<IActionResult> AddDepartment([FromBody] AddDepartmentRequest request)
//        {
//            var command = new AddDepartmentCommand(request);
//            var result = await _addDepartmentHandler.HandleAsync(command);
            
//            if (result.IsSuccess)
//                return Ok(result.Value);
            
//            return BadRequest(result.Error);
//        }

//        [HttpPut("{id}")]
//        public async Task<IActionResult> UpdateDepartment(int id, [FromBody] UpdateDepartmentRequest request)
//        {
//            var command = new UpdateDepartmentCommand(id, request);
//            var result = await _updateDepartmentHandler.HandleAsync(command);
            
//            if (result.IsSuccess)
//                return Ok(result.Value);
            
//            return BadRequest(result.Error);
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteDepartment(int id)
//        {
//            var command = new DeleteDepartmentCommand(id);
//            var result = await _deleteDepartmentHandler.HandleAsync(command);
            
//            if (result.IsSuccess)
//                return Ok(result.Value);
            
//            return BadRequest(result.Error);
//        }

//        [HttpGet]
//        [EnableQuery]
//        public async Task<IActionResult> GetAllDepartments()
//        {
//            var query = new GetAllDepartmentsQuery();
//            var result = await _getAllDepartmentsHandler.HandleAsync(query);
            
//            if (result.IsSuccess)
//                return Ok(result.Value.AsQueryable());
            
//            return BadRequest(result.Error);
//        }

//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetDepartmentById(int id)
//        {
//            var query = new GetDepartmentByIdQuery(id);
//            var result = await _getDepartmentByIdHandler.HandleAsync(query);
            
//            if (result.IsSuccess)
//                return Ok(result.Value);
            
//            return NotFound(result.Error);
//        }
//    }
//} 
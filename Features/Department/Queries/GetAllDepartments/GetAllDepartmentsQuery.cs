using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.Department.Dtos;

namespace Alwalid.Cms.Api.Features.Department.Queries.GetAllDepartments
{
    public class GetAllDepartmentsQuery : IQuery<IEnumerable<DepartmentResponseDto>>
    {
        // No parameters needed for getting all departments
    }
} 
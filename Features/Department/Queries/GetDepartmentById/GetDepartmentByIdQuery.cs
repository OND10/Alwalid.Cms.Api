using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.Department.Dtos;

namespace Alwalid.Cms.Api.Features.Department.Queries.GetDepartmentById
{
    public class GetDepartmentByIdQuery : IQuery<DepartmentResponseDto>
    {
        public int Id { get; set; }
    }
} 
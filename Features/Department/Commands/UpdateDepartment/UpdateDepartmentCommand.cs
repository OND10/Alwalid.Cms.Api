using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.Department.Dtos;

namespace Alwalid.Cms.Api.Features.Department.Commands.UpdateDepartment
{
    public class UpdateDepartmentCommand : ICommand<DepartmentResponseDto>
    {
        public int Id { get; set; }
        public DepartmentRequestDto Request { get; set; } = new();
    }
} 
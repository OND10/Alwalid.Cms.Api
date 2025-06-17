using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.Department.Dtos;

namespace Alwalid.Cms.Api.Features.Department.Commands.AddDepartment
{
    public class AddDepartmentCommand : ICommand<DepartmentResponseDto>
    {
        public DepartmentRequestDto Request { get; set; } = new();
    }
} 
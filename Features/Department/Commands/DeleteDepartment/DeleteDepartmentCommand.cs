using Alwalid.Cms.Api.Abstractions.Messaging;

namespace Alwalid.Cms.Api.Features.Department.Commands.DeleteDepartment
{
    public class DeleteDepartmentCommand : ICommand<bool>
    {
        public int Id { get; set; }
    }
} 
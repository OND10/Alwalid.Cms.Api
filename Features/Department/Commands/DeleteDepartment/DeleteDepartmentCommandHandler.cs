using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Features.Department;

namespace Alwalid.Cms.Api.Features.Department.Commands.DeleteDepartment
{
    public class DeleteDepartmentCommandHandler : ICommandHandler<DeleteDepartmentCommand, bool>
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DeleteDepartmentCommandHandler(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<Result<bool>> Handle(DeleteDepartmentCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // Check if department exists
                if (!await _departmentRepository.ExistsAsync(command.Id))
                {
                    return await Result<bool>.FaildAsync(false, "Department not found.");
                }

                // Delete department
                var isDeleted = await _departmentRepository.DeleteAsync(command.Id);

                if (isDeleted)
                {
                    return await Result<bool>.SuccessAsync(true, "Department deleted successfully.");
                }
                else
                {
                    return await Result<bool>.FaildAsync(false, "Failed to delete department.");
                }
            }
            catch (Exception ex)
            {
                return await Result<bool>.FaildAsync(false, $"Error deleting department: {ex.Message}");
            }
        }
    }
} 
using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Entities;
using Alwalid.Cms.Api.Features.Department;
using Alwalid.Cms.Api.Features.Department.Dtos;

namespace Alwalid.Cms.Api.Features.Department.Commands.UpdateDepartment
{
    public class UpdateDepartmentCommandHandler : ICommandHandler<UpdateDepartmentCommand, DepartmentResponseDto>
    {
        private readonly IDepartmentRepository _departmentRepository;

        public UpdateDepartmentCommandHandler(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<Result<DepartmentResponseDto>> Handle(UpdateDepartmentCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // Check if department exists
                var existingDepartment = await _departmentRepository.GetByIdAsync(command.Id);
                if (existingDepartment == null)
                {
                    return await Result<DepartmentResponseDto>.FaildAsync(false, "Department not found.");
                }

                // Validate unique constraints
                if (await _departmentRepository.ExistsInBranchAsync(command.Request.BranchId, command.Request.EnglishName, command.Request.ArabicName, command.Id))
                {
                    return await Result<DepartmentResponseDto>.FaildAsync(false, "Department already exists in this branch with the same name.");
                }

                // Update department
                existingDepartment.EnglishName = command.Request.EnglishName;
                existingDepartment.ArabicName = command.Request.ArabicName;
                existingDepartment.BranchId = command.Request.BranchId;

                var updatedDepartment = await _departmentRepository.UpdateAsync(existingDepartment);

                // Map to response DTO
                var responseDto = new DepartmentResponseDto
                {
                    Id = updatedDepartment.Id,
                    EnglishName = updatedDepartment.EnglishName,
                    ArabicName = updatedDepartment.ArabicName,
                    BranchId = updatedDepartment.BranchId,
                    BranchCity = updatedDepartment.Branch?.City ?? string.Empty,
                    CountryName = updatedDepartment.Branch?.Country?.Name ?? string.Empty,
                    ProductsCount = updatedDepartment.Products?.Count ?? 0
                };

                return await Result<DepartmentResponseDto>.SuccessAsync(responseDto, "Department updated successfully.");
            }
            catch (Exception ex)
            {
                return await Result<DepartmentResponseDto>.FaildAsync(false, $"Error updating department: {ex.Message}");
            }
        }
    }
} 
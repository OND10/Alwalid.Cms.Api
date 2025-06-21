using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Entities;
using Alwalid.Cms.Api.Features.Department;
using Alwalid.Cms.Api.Features.Department.Dtos;

namespace Alwalid.Cms.Api.Features.Department.Commands.AddDepartment
{
    public class AddDepartmentCommandHandler : ICommandHandler<AddDepartmentCommand, DepartmentResponseDto>
    {
        private readonly IDepartmentRepository _departmentRepository;

        public AddDepartmentCommandHandler(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<Result<DepartmentResponseDto>> Handle(AddDepartmentCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // Validate unique constraints
                if (await _departmentRepository.ExistsInBranchAsync(command.Request.BranchId, command.Request.EnglishName, command.Request.ArabicName) is true)
                {
                    return await Result<DepartmentResponseDto>.FaildAsync(false, "Department already exists in this branch with the same name.");
                }

                // Create new department
                var department = new Entities.Department
                {
                    EnglishName = command.Request.EnglishName,
                    ArabicName = command.Request.ArabicName,
                    BranchId = command.Request.BranchId
                };

                var createdDepartment = await _departmentRepository.CreateAsync(department);

                var mappedResponse = new DepartmentResponseDto
                {
                    ArabicName = createdDepartment.ArabicName,
                    EnglishName = createdDepartment.EnglishName,
                    BranchId = createdDepartment.BranchId,
                };

                return await Result<DepartmentResponseDto>.SuccessAsync(mappedResponse, "Department created successfully.", true);
            }
            catch (Exception ex)
            {
                return await Result<DepartmentResponseDto>.FaildAsync(false, $"Error creating department: {ex.Message}");
            }
        }
    }
} 
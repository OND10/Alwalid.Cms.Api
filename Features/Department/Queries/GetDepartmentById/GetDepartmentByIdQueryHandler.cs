using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Entities;
using Alwalid.Cms.Api.Features.Department;
using Alwalid.Cms.Api.Features.Department.Dtos;

namespace Alwalid.Cms.Api.Features.Department.Queries.GetDepartmentById
{
    public class GetDepartmentByIdQueryHandler : IQueryHandler<GetDepartmentByIdQuery, DepartmentResponseDto?>
    {
        private readonly IDepartmentRepository _departmentRepository;

        public GetDepartmentByIdQueryHandler(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<Result<DepartmentResponseDto?>> Handle(GetDepartmentByIdQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var department = await _departmentRepository.GetByIdAsync(query.Id);
                
                if (department == null)
                {
                    return await Result<DepartmentResponseDto?>.FaildAsync(false, "Department not found.");
                }

                var responseDto = new DepartmentResponseDto
                {
                    Id = department.Id,
                    EnglishName = department.EnglishName,
                    ArabicName = department.ArabicName,
                    BranchId = department.BranchId,
                    BranchCity = department.Branch?.City ?? string.Empty,
                    CountryName = department.Branch?.Country?.Name ?? string.Empty,
                    ProductsCount = department.Products?.Count ?? 0
                };

                return await Result<DepartmentResponseDto?>.SuccessAsync(responseDto, "Department retrieved successfully.");
            }
            catch (Exception ex)
            {
                return await Result<DepartmentResponseDto?>.FaildAsync(false, $"Error retrieving department: {ex.Message}");
            }
        }
    }
} 
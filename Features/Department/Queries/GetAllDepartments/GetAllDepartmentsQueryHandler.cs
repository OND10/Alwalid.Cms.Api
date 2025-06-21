using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Entities;
using Alwalid.Cms.Api.Features.Department;
using Alwalid.Cms.Api.Features.Department.Dtos;

namespace Alwalid.Cms.Api.Features.Department.Queries.GetAllDepartments
{
    public class GetAllDepartmentsQueryHandler : IQueryHandler<GetAllDepartmentsQuery, IEnumerable<DepartmentResponseDto>>
    {
        private readonly IDepartmentRepository _departmentRepository;

        public GetAllDepartmentsQueryHandler(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<Result<IEnumerable<DepartmentResponseDto>>> Handle(GetAllDepartmentsQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var departments = await _departmentRepository.GetAllAsync();
                
                var responseDtos = departments.Select(department => new DepartmentResponseDto
                {
                    Id = department.Id,
                    EnglishName = department.EnglishName,
                    ArabicName = department.ArabicName,
                    BranchId = department.BranchId,
                    BranchCity = department.Branch?.City ?? string.Empty,
                    CountryName = department.Branch?.Country?.Name ?? string.Empty,
                    ProductsCount = department.Products?.Count ?? 0
                });

                return await Result<IEnumerable<DepartmentResponseDto>>.SuccessAsync(responseDtos, "Departments retrieved successfully.", true);
            }
            catch (Exception ex)
            {
                return await Result<IEnumerable<DepartmentResponseDto>>.FaildAsync(false, $"Error retrieving departments: {ex.Message}");
            }
        }
    }
} 
using FluentValidation;

namespace Alwalid.Cms.Api.Features.Category.Dtos
{
    public class CategoryRequestDto
    {
        public string? EnglishName { get; set; }
        public string? ArabicName { get; set; }
        public int DepartmentId { get; set; }
        public string MarketName { get; set; } = default!;

    }

    public class CategoryRequestDtoValidator : AbstractValidator<CategoryRequestDto>
    {
        public CategoryRequestDtoValidator()
        {

            RuleFor(x => x.EnglishName)
                   .NotEmpty().WithMessage("??? ????? ????? ??????????? ?????")
                   .MaximumLength(80).WithMessage("??? ??? ???? ????? ??????????? ?? 80 ?????");

            RuleFor(x => x.ArabicName).Length(0, 80);
            RuleFor(x => x.ArabicName).NotNull();

        }
    }
}
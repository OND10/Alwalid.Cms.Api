using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Common.Helper.Implementation;
using Alwalid.Cms.Api.Common.Helper.Interface;
using Alwalid.Cms.Api.Data;
using Alwalid.Cms.Api.Email;
using Alwalid.Cms.Api.Entities;
using Alwalid.Cms.Api.Features.Branch;
using Alwalid.Cms.Api.Features.Branch.Commands.AddBranch;
using Alwalid.Cms.Api.Features.Branch.Commands.DeleteBranch;
using Alwalid.Cms.Api.Features.Branch.Commands.UpdateBranch;
using Alwalid.Cms.Api.Features.Branch.Dtos;
using Alwalid.Cms.Api.Features.Branch.Queries.GetAllBranches;
using Alwalid.Cms.Api.Features.Branch.Queries.GetBranchById;
using Alwalid.Cms.Api.Features.Category;
using Alwalid.Cms.Api.Features.Category.Commands.AddCategory;
using Alwalid.Cms.Api.Features.Category.Commands.DeleteCategory;
using Alwalid.Cms.Api.Features.Category.Commands.SoftDeleteCategory;
using Alwalid.Cms.Api.Features.Category.Commands.UpdateCategory;
using Alwalid.Cms.Api.Features.Category.Dtos;
using Alwalid.Cms.Api.Features.Category.Queries.GetActiveCategories;
using Alwalid.Cms.Api.Features.Category.Queries.GetAllCategories;
using Alwalid.Cms.Api.Features.Category.Queries.GetCategoryById;
using Alwalid.Cms.Api.Features.Country;
using Alwalid.Cms.Api.Features.Country.Commands.AddCountry;
using Alwalid.Cms.Api.Features.Country.Commands.DeleteCountry;
using Alwalid.Cms.Api.Features.Country.Commands.UpdateCountry;
using Alwalid.Cms.Api.Features.Country.Dtos;
using Alwalid.Cms.Api.Features.Country.Queries.GetAllCountries;
using Alwalid.Cms.Api.Features.Country.Queries.GetCountryById;
using Alwalid.Cms.Api.Features.Currency;
using Alwalid.Cms.Api.Features.Currency.Commands.AddCurrency;
using Alwalid.Cms.Api.Features.Currency.Commands.DeleteCurrency;
using Alwalid.Cms.Api.Features.Currency.Commands.UpdateCurrency;
using Alwalid.Cms.Api.Features.Currency.Dtos;
using Alwalid.Cms.Api.Features.Currency.Queries.GetActiveCurrencies;
using Alwalid.Cms.Api.Features.Currency.Queries.GetAllCurrencies;
using Alwalid.Cms.Api.Features.Currency.Queries.GetByCode;
using Alwalid.Cms.Api.Features.Currency.Queries.GetByName;
using Alwalid.Cms.Api.Features.Currency.Queries.GetBySymbol;
using Alwalid.Cms.Api.Features.Currency.Queries.GetCurrencyById;
using Alwalid.Cms.Api.Features.Currency.Queries.GetTotalCount;
using Alwalid.Cms.Api.Features.Department;
using Alwalid.Cms.Api.Features.Department.Commands.AddDepartment;
using Alwalid.Cms.Api.Features.Department.Commands.DeleteDepartment;
using Alwalid.Cms.Api.Features.Department.Commands.UpdateDepartment;
using Alwalid.Cms.Api.Features.Department.Dtos;
using Alwalid.Cms.Api.Features.Department.Queries.GetAllDepartments;
using Alwalid.Cms.Api.Features.Department.Queries.GetDepartmentById;
using Alwalid.Cms.Api.Features.Product;
using Alwalid.Cms.Api.Features.Product.Commands.AddProduct;
using Alwalid.Cms.Api.Features.Product.Commands.DeleteProduct;
using Alwalid.Cms.Api.Features.Product.Commands.SoftDeleteProduct;
using Alwalid.Cms.Api.Features.Product.Commands.UpdateProduct;
using Alwalid.Cms.Api.Features.Product.Commands.UpdateStock;
using Alwalid.Cms.Api.Features.Product.Dtos;
using Alwalid.Cms.Api.Features.Product.Queries.GetActiveProducts;
using Alwalid.Cms.Api.Features.Product.Queries.GetAllProducts;
using Alwalid.Cms.Api.Features.Product.Queries.GetLowStockProducts;
//using Alwalid.Cms.Api.Features.Product.Queries.Get;
using Alwalid.Cms.Api.Features.Product.Queries.GetProductById;
using Alwalid.Cms.Api.Features.Product.Queries.SearchProducts;
using Alwalid.Cms.Api.Features.ProductImage;
using Alwalid.Cms.Api.Features.ProductImage.Commands.AddProductImage;
using Alwalid.Cms.Api.Features.ProductImage.Commands.DeleteProductImage;
using Alwalid.Cms.Api.Features.ProductImage.Commands.UpdateProductImage;
using Alwalid.Cms.Api.Features.ProductImage.Dtos;
using Alwalid.Cms.Api.Features.ProductImage.Queries.GetAllProductImages;
using Alwalid.Cms.Api.Features.ProductImage.Queries.GetByProductId;
using Alwalid.Cms.Api.Features.ProductImage.Queries.GetProductImageById;
using Alwalid.Cms.Api.Features.ProductStatistic;
using Alwalid.Cms.Api.Features.ProductStatistic.Commands.AddProductStatistic;
using Alwalid.Cms.Api.Features.ProductStatistic.Commands.DeleteProductStatistic;
using Alwalid.Cms.Api.Features.ProductStatistic.Commands.UpdateProductStatistic;
using Alwalid.Cms.Api.Features.ProductStatistic.Commands.IncrementViewCount;
using Alwalid.Cms.Api.Features.ProductStatistic.Dtos;
using Alwalid.Cms.Api.Features.ProductStatistic.Queries.GetAllProductStatistics;
using Alwalid.Cms.Api.Features.ProductStatistic.Queries.GetByDateRange;
using Alwalid.Cms.Api.Features.ProductStatistic.Queries.GetByProductId;
using Alwalid.Cms.Api.Features.ProductStatistic.Queries.GetProductStatisticById;
using Alwalid.Cms.Api.Features.ProductStatistic.Queries.GetTopSellingProducts;
using Alwalid.Cms.Api.Features.ProductStatistic.Queries.GetMostViewedProducts;
using Alwalid.Cms.Api.Features.Settings;
using Alwalid.Cms.Api.Features.Settings.Commands.AddSettings;
using Alwalid.Cms.Api.Features.Settings.Commands.DeleteSettings;
using Alwalid.Cms.Api.Features.Settings.Commands.UpdateDefaultCurrency;
using Alwalid.Cms.Api.Features.Settings.Commands.UpdateDefaultLanguage;
using Alwalid.Cms.Api.Features.Settings.Commands.UpdateMaintenanceMode;
using Alwalid.Cms.Api.Features.Settings.Commands.UpdateSettings;
using Alwalid.Cms.Api.Features.Settings.Dtos;
using Alwalid.Cms.Api.Features.Settings.Queries.GetAllSettings;
using Alwalid.Cms.Api.Features.Settings.Queries.GetMainSettings;
using Alwalid.Cms.Api.Features.Settings.Queries.GetSettingsById;
using Microsoft.AspNetCore.Identity.UI.Services;
using Alwalid.Cms.Api.Features.Feedback;
using Alwalid.Cms.Api.Features.Feedback.Commands.AddFeedback;
using Alwalid.Cms.Api.Features.Feedback.Commands.UpdateFeedback;
using Alwalid.Cms.Api.Features.Feedback.Commands.DeleteFeedback;
using Alwalid.Cms.Api.Features.Feedback.Dtos;
using Alwalid.Cms.Api.Features.Feedback.Queries.GetAllFeedbacks;
using Alwalid.Cms.Api.Features.Feedback.Queries.GetFeedbackById;
using Alwalid.Cms.Api.Features.Feedback.Queries.GetActiveFeedbacks;
using Alwalid.Cms.Api.Features.Feedback.Queries.GetByRating;
using Alwalid.Cms.Api.Features.Feedback.Queries.GetByPosition;
using Alwalid.Cms.Api.Features.Feedback.Queries.GetFeedbackStats;
using Alwalid.Cms.Api.Features.Gemini.Commands.GenerateContent;
using Alwalid.Cms.Api.Abstractions;
using Alwalid.Cms.Api.Common;
using Alwalid.Cms.Api.Services;
using Alwalid.Cms.Api.Events;
using Alwalid.Cms.Api.Features.Notification.Services;
using Alwalid.Cms.Api.Features.Category.Events;
using Alwalid.Cms.Api.Features.Category.Handlers;
using FluentValidation;
using System;
using Alwalid.Cms.Api.Features.Conversation.Services;

namespace Alwalid.Cms.Api.Extensions
{
    public static class AddApplicationExtension
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {


            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<ICacheService, MemoryCacheService>();
            services.AddScoped<IDispatcher, Dispatcher>();
            services.AddSingleton<IEventPublisher, EventPublisher>();
            services.AddScoped<IDomainEventHandler<CategoryCreatedEvent>, CategoryCreatedHandler>();
            services.AddScoped<INotificationService, ConsoleNotificationService>();


            // Register repositories
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IBranchRepository, BranchRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductImageRepository, ProductImageRepository>();
            services.AddScoped<IProductStatisticRepository, ProductStatisticRepository>();
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddScoped<ISettingsRepository, SettingsRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<IFeedbackRepository, FeedbackRepository>();

            // Register Country handlers
            services.AddScoped<ICommandHandler<AddCountryCommand, CountryResponseDto>, AddCountryCommandHandler>();
            services.AddScoped<ICommandHandler<UpdateCountryCommand, CountryResponseDto>, UpdateCountryCommandHandler>();
            services.AddScoped<ICommandHandler<DeleteCountryCommand, bool>, DeleteCountryCommandHandler>();
            services.AddScoped<IQueryHandler<GetAllCountriesQuery, IEnumerable<CountryResponseDto>>, GetAllCountriesQueryHandler>();
            services.AddScoped<IQueryHandler<GetCountryByIdQuery, CountryResponseDto>, GetCountryByIdQueryHandler>();

            // Register Branch handlers
            services.AddScoped<ICommandHandler<AddBranchCommand, BranchResponseDto>, AddBranchCommandHandler>();
            services.AddScoped<ICommandHandler<UpdateBranchCommand, Branch>, UpdateBranchCommandHandler>();
            services.AddScoped<ICommandHandler<DeleteBranchCommand, bool>, DeleteBranchCommandHandler>();
            services.AddScoped<IQueryHandler<GetAllBranchesQuery, IEnumerable<Branch>>, GetAllBranchesQueryHandler>();
            services.AddScoped<IQueryHandler<GetBranchByIdQuery, Branch>, GetBranchByIdQueryHandler>();

            // Register Department handlers
            services.AddScoped<ICommandHandler<AddDepartmentCommand, DepartmentResponseDto>, AddDepartmentCommandHandler>();
            services.AddScoped<ICommandHandler<UpdateDepartmentCommand, DepartmentResponseDto>, UpdateDepartmentCommandHandler>();
            services.AddScoped<ICommandHandler<DeleteDepartmentCommand, bool>, DeleteDepartmentCommandHandler>();
            services.AddScoped<IQueryHandler<GetAllDepartmentsQuery, IEnumerable<DepartmentResponseDto>>, GetAllDepartmentsQueryHandler>();
            services.AddScoped<IQueryHandler<GetDepartmentByIdQuery, DepartmentResponseDto>, GetDepartmentByIdQueryHandler>();

            // Register Category handlers
            services.AddScoped<ICommandHandler<AddCategoryCommand, CategoryResponseDto>, AddCategoryCommandHandler>();
            services.AddScoped<ICommandHandler<UpdateCategoryCommand, CategoryResponseDto>, UpdateCategoryCommandHandler>();
            services.AddScoped<ICommandHandler<DeleteCategoryCommand, bool>, DeleteCategoryCommandHandler>();
            services.AddScoped<ICommandHandler<SoftDeleteCategoryCommand, bool>, SoftDeleteCategoryCommandHandler>();
            services.AddScoped<IQueryHandler<GetAllCategoriesQuery, IEnumerable<CategoryResponseDto>>, GetAllCategoriesQueryHandler>();
            services.AddScoped<IQueryHandler<GetCategoryByIdQuery, CategoryResponseDto>, GetCategoryByIdQueryHandler>();
            services.AddScoped<IQueryHandler<GetActiveCategoriesQuery, IEnumerable<CategoryResponseDto>>, GetActiveCategoriesQueryHandler>();

            // Register Product handlers
            services.AddScoped<ICommandHandler<AddProductCommand, ProductResponseDto>, AddProductCommandHandler>();
            services.AddScoped<ICommandHandler<UpdateProductCommand, ProductResponseDto>, UpdateProductCommandHandler>();
            services.AddScoped<ICommandHandler<DeleteProductCommand, bool>, DeleteProductCommandHandler>();
            services.AddScoped<ICommandHandler<SoftDeleteProductCommand, bool>, SoftDeleteProductCommandHandler>();
            services.AddScoped<ICommandHandler<UpdateStockCommand, bool>, UpdateStockCommandHandler>();
            services.AddScoped<IQueryHandler<GetAllProductsQuery, IEnumerable<ProductResponseDto>>, GetAllProductsQueryHandler>();
            services.AddScoped<IQueryHandler<GetProductByIdQuery, ProductResponseDto>, GetProductByIdQueryHandler>();
            services.AddScoped<IQueryHandler<GetActiveProductsQuery, IEnumerable<ProductResponseDto>>, GetActiveProductsQueryHandler>();
            services.AddScoped<IQueryHandler<SearchProductsQuery, IEnumerable<ProductResponseDto>>, SearchProductsQueryHandler>();
            services.AddScoped<IQueryHandler<GetLowStockProductsQuery, IEnumerable<ProductResponseDto>>, GetLowStockProductsQueryHandler>();
            //services.AddScoped<IQueryHandler<GetOutOfStockProductsQuery, IEnumerable<ProductResponseDto>>, GetOutOfStockProductsQueryHandler>();

            // Register ProductImage handlers
            services.AddScoped<ICommandHandler<AddProductImageCommand, ProductImageResponseDto>, AddProductImageCommandHandler>();
            services.AddScoped<ICommandHandler<UpdateProductImageCommand, ProductImageResponseDto>, UpdateProductImageCommandHandler>();
            services.AddScoped<ICommandHandler<DeleteProductImageCommand, bool>, DeleteProductImageCommandHandler>();
            services.AddScoped<IQueryHandler<GetAllProductImagesQuery, IEnumerable<ProductImageResponseDto>>, GetAllProductImagesQueryHandler>();
            services.AddScoped<IQueryHandler<GetProductImageByIdQuery, ProductImageResponseDto?>, GetProductImageByIdQueryHandler>();
            services.AddScoped<IQueryHandler<GetByProductIdQuery, IEnumerable<ProductImageResponseDto>>, GetByProductIdQueryHandler>();

            // Register ProductStatistic handlers
            services.AddScoped<ICommandHandler<AddProductStatisticCommand, ProductStatisticResponseDto>, AddProductStatisticCommandHandler>();
            services.AddScoped<ICommandHandler<UpdateProductStatisticCommand, ProductStatisticResponseDto>, UpdateProductStatisticCommandHandler>();
            services.AddScoped<ICommandHandler<DeleteProductStatisticCommand, bool>, DeleteProductStatisticCommandHandler>();
            services.AddScoped<ICommandHandler<IncrementViewCountCommand, ProductStatisticResponseDto>, IncrementViewCountCommandHandler>();
            services.AddScoped<IQueryHandler<GetAllProductStatisticsQuery, IEnumerable<ProductStatisticResponseDto>>, GetAllProductStatisticsQueryHandler>();
            services.AddScoped<IQueryHandler<GetProductStatisticByIdQuery, ProductStatisticResponseDto>, GetProductStatisticByIdQueryHandler>();
            services.AddScoped<IQueryHandler<GetByProductIdForStatisticQuery, IEnumerable<ProductStatisticResponseDto>>, GetByProductIdForStatisticQueryHandler>();
            services.AddScoped<IQueryHandler<GetByDateRangeQuery, IEnumerable<ProductStatisticResponseDto>>, GetByDateRangeQueryHandler>();
            services.AddScoped<IQueryHandler<GetTopSellingProductsQuery, IEnumerable<ProductStatisticResponseDto>>, GetTopSellingProductsQueryHandler>();
            services.AddScoped<IQueryHandler<GetMostViewedProductsQuery, IEnumerable<ProductStatisticResponseDto>>, GetMostViewedProductsQueryHandler>();

            // Register Feedback handlers
            services.AddScoped<ICommandHandler<AddFeedbackCommand, FeedbackResponseDto>, AddFeedbackCommandHandler>();
            services.AddScoped<ICommandHandler<UpdateFeedbackCommand, FeedbackResponseDto>, UpdateFeedbackCommandHandler>();
            services.AddScoped<ICommandHandler<DeleteFeedbackCommand, bool>, DeleteFeedbackCommandHandler>();
            services.AddScoped<IQueryHandler<GetAllFeedbacksQuery, IEnumerable<FeedbackResponseDto>>, GetAllFeedbacksQueryHandler>();
            services.AddScoped<IQueryHandler<GetFeedbackByIdQuery, FeedbackResponseDto>, GetFeedbackByIdQueryHandler>();
            services.AddScoped<IQueryHandler<GetActiveFeedbacksQuery, IEnumerable<FeedbackResponseDto>>, GetActiveFeedbacksQueryHandler>();
            services.AddScoped<IQueryHandler<GetByRatingQuery, IEnumerable<FeedbackResponseDto>>, GetByRatingQueryHandler>();
            services.AddScoped<IQueryHandler<GetByPositionQuery, IEnumerable<FeedbackResponseDto>>, GetByPositionQueryHandler>();
            services.AddScoped<IQueryHandler<GetFeedbackStatsQuery, FeedbackStatsDto>, GetFeedbackStatsQueryHandler>();

            // Register Currency handlers
            services.AddScoped<ICommandHandler<AddCurrencyCommand, CurrencyResponseDto>, AddCurrencyCommandHandler>();
            services.AddScoped<ICommandHandler<UpdateCurrencyCommand, CurrencyResponseDto>, UpdateCurrencyCommandHandler>();
            services.AddScoped<ICommandHandler<DeleteCurrencyCommand, bool>, DeleteCurrencyCommandHandler>();
            services.AddScoped<IQueryHandler<GetAllCurrenciesQuery, IEnumerable<CurrencyResponseDto>>, GetAllCurrenciesQueryHandler>();
            services.AddScoped<IQueryHandler<GetCurrencyByIdQuery, CurrencyResponseDto>, GetCurrencyByIdQueryHandler>();
            services.AddScoped<IQueryHandler<GetByCodeQuery, CurrencyResponseDto>, GetByCodeQueryHandler>();
            services.AddScoped<IQueryHandler<GetByNameQuery, CurrencyResponseDto>, GetByNameQueryHandler>();
            services.AddScoped<IQueryHandler<GetBySymbolQuery, CurrencyResponseDto>, GetBySymbolQueryHandler>();
            services.AddScoped<IQueryHandler<GetActiveCurrenciesQuery, IEnumerable<CurrencyResponseDto>>, GetActiveCurrenciesQueryHandler>();
            services.AddScoped<IQueryHandler<GetTotalCountQuery, int>, GetTotalCountQueryHandler>();

            // Register Settings handlers
            services.AddScoped<ICommandHandler<AddSettingsCommand, SettingsResponseDto>, AddSettingsCommandHandler>();
            services.AddScoped<ICommandHandler<UpdateSettingsCommand, SettingsResponseDto>, UpdateSettingsCommandHandler>();
            services.AddScoped<ICommandHandler<DeleteSettingsCommand, bool>, DeleteSettingsCommandHandler>();
            services.AddScoped<ICommandHandler<UpdateMaintenanceModeCommand, bool>, UpdateMaintenanceModeCommandHandler>();
            services.AddScoped<ICommandHandler<UpdateDefaultLanguageCommand, bool>, UpdateDefaultLanguageCommandHandler>();
            services.AddScoped<ICommandHandler<UpdateDefaultCurrencyCommand, bool>, UpdateDefaultCurrencyCommandHandler>();
            services.AddScoped<IQueryHandler<GetAllSettingsQuery, IEnumerable<SettingsResponseDto>>, GetAllSettingsQueryHandler>();
            services.AddScoped<IQueryHandler<GetSettingsByIdQuery, SettingsResponseDto?>, GetSettingsByIdQueryHandler>();
            services.AddScoped<IQueryHandler<GetMainSettingsQuery, SettingsResponseDto?>, GetMainSettingsQueryHandler>();


            services.AddHttpClient<GenerateContentCommandHandler>();
            services.AddScoped<ICommandHandler<GenerateContentCommand, string>, GenerateContentCommandHandler>();


            //Validators
            services.AddScoped<IValidator<CategoryRequestDto>, CategoryRequestDtoValidator>();



            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IConversationService, ConversationService>();

            //Registering External Services
            services.AddTransient<IEmailSender, EmailSender>();
            return services;
        }
    }
}
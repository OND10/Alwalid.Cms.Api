using Alwalid.Cms.Api.Abstractions.Messaging;

namespace Alwalid.Cms.Api.Features.ProductStatistic.Commands.DeleteProductStatistic
{
    public class DeleteProductStatisticCommand : ICommand<bool>
    {
        public int Id { get; set; }
    }
} 
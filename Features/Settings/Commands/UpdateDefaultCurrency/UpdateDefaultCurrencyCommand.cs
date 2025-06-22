using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.Settings.Dtos;

namespace Alwalid.Cms.Api.Features.Settings.Commands.UpdateDefaultCurrency
{
    public class UpdateDefaultCurrencyCommand : ICommand<bool>
    {
        public int Id { get; set; }
       public DefaultCurrencyRequestDto Request {  get; set; }
    }
}
using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Features.Settings.Dtos;

namespace Alwalid.Cms.Api.Features.Settings.Commands.UpdateDefaultLanguage
{
    public class UpdateDefaultLanguageCommand : ICommand<bool>
    {
        public int Id { get; set; }
       public DefaultLanguageRequestDto Request {  get; set; }
    }
}
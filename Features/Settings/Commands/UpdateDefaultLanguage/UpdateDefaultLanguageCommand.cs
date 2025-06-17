using Alwalid.Cms.Api.Abstractions.Messaging;

namespace Alwalid.Cms.Api.Features.Settings.Commands.UpdateDefaultLanguage
{
    public class UpdateDefaultLanguageCommand : ICommand<bool>
    {
        public int Id { get; set; }
        public string DefaultLanguage { get; set; } = string.Empty;
    }
}
using Alwalid.Cms.Api.Abstractions.Messaging;

namespace Alwalid.Cms.Api.Features.Gemini.Commands.GenerateContent
{
    public class GenerateContentCommand : ICommand<string>
    {
         public string Prompt {  get; set; }
    }
}

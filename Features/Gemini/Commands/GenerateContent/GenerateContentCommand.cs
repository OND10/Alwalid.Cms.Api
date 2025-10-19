using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Entities;

namespace Alwalid.Cms.Api.Features.Gemini.Commands.GenerateContent
{
    public class GenerateContentCommand : ICommand<string>
    {
         public string Prompt {  get; set; }
        public IEnumerable<Entities.Message> history { get; set; }
    }
}

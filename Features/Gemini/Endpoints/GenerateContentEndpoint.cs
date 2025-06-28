using Alwalid.Cms.Api.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Alwalid.Cms.Api.Abstractions.Messaging;
using Alwalid.Cms.Api.Common.Handler;
using Alwalid.Cms.Api.Features.Gemini.Commands.GenerateContent;
using Alwalid.Cms.Api.Middleware;
using Alwalid.Cms.Api.Attributes;

namespace ProductAPI.VSA.Features.Gemini.Endpoints
{
    public class GenerateContentEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/gemini/", async (
                [FromBody] GeminiPromptRequestDto request,
                ICommandHandler<GenerateContentCommand, string> handler,
                CancellationToken cancellationToken) =>
            {

                var command = new GenerateContentCommand
                {
                    Prompt = request.Prompt
                };

                var result = await handler.Handle(command, cancellationToken);

                return await Result<string>.SuccessAsync(result.Data, "Here you go", true);
            }).WithMetadata(new EnableRateLimitingAttribute());
        }
    }

    public class GeminiPromptRequestDto
    {
        public string Prompt { get; set;}
    }
}

using SwaggerThemes;

namespace Presentation.Extensions;

internal static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseSwaggerWithUi(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(Theme.Gruvbox);

        return app;
    }
}

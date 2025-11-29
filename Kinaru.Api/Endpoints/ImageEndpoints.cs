using Kinaru.Api.Services.Interfaces;

namespace Kinaru.Api.Endpoints;

public static class ImageEndpoints
{
    public static void MapImageEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/images").WithTags("Images");

        group.MapPost("/upload", async (
            IFormFile file,
            IImageService service) =>
        {
            try
            {
                var imageUrl = await service.UploadImageAsync(file);
                return Results.Ok(new { url = imageUrl });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        })
        .DisableAntiforgery() // Required for file uploads in some cases
        .RequireAuthorization()
        .WithName("UploadImage")
        .WithOpenApi();
    }
}

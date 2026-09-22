using IdentityService.Application.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Api.Controllers;

[ApiController]
[Route("api/identity")]
public class IdentityController(LoginUseCase loginUseCase) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest("Email and password are required.");
        }

        var result = await loginUseCase.ExecuteAsync(request, cancellationToken);

        return result.Match(
            loginResult => Ok(loginResult),
            errors => Problem(
                statusCode: StatusCodes.Status401Unauthorized,
                title: errors.First().Description));
    }
}
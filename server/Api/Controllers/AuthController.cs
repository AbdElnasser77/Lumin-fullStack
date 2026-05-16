using Microsoft.AspNetCore.Mvc;
using server.Application.DTOs.Auth;
using server.Application.Interfaces;

namespace server.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;

    public AuthController(IAuthService auth) => _auth = auth;

    [HttpPost("signup")]
    public async Task<ActionResult<UserResponse>> SignUp([FromBody] SignUpRequest request, CancellationToken ct)
    {
        var user = await _auth.SignUpAsync(request, ct);
        return Ok(user);
    }

    [HttpPost("signin")]
    public async Task<ActionResult<AuthResponse>> SignIn([FromBody] SignInRequest request, CancellationToken ct)
    {
        var response = await _auth.SignInAsync(request, ct);
        return Ok(response);
    }

    [HttpPost("confirm-email")]
    public async Task<ActionResult<AuthResponse>> ConfirmEmail([FromBody] ConfirmEmailRequest request, CancellationToken ct)
    {
        var response = await _auth.ConfirmEmailAsync(request, ct);
        return Ok(response);
    }

    [HttpPost("send-email-confirmation")]
    public async Task<IActionResult> SendEmailConfirmation([FromBody] SendEmailConfirmationRequest request, CancellationToken ct)
    {
        await _auth.SendEmailConfirmationAsync(request, ct);
        return Ok(new { message = "A confirmation code has been sent to this email." });
    }

    [HttpPost("forgot-password")]
    public async Task<ActionResult<ForgotPasswordResponse>> ForgotPassword([FromBody] ForgotPasswordRequest request, CancellationToken ct)
    {
        var response = await _auth.ForgotPasswordAsync(request, ct);
        return Ok(response);
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request, CancellationToken ct)
    {
        await _auth.ResetPasswordAsync(request, ct);
        return Ok(new { message = "Password updated successfully." });
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<AuthResponse>> Refresh([FromBody] RefreshTokenRequest request, CancellationToken ct)
    {
        var response = await _auth.RefreshTokenAsync(request, ct);
        return Ok(response);
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Application.DTOs.Auth;
using server.Application.DTOs.Profile;
using server.Application.Interfaces;

namespace server.Api.Controllers;

[ApiController]
[Route("api/profile")]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly IProfileService _profile;

    public ProfileController(IProfileService profile) => _profile = profile;

    private Guid CurrentUserId => Guid.Parse(User.FindFirst("sub")!.Value);

    [HttpGet]
    public async Task<ActionResult<UserResponse>> GetProfile(CancellationToken ct)
    {
        var profile = await _profile.GetProfileAsync(CurrentUserId, ct);
        return Ok(profile);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request, CancellationToken ct)
    {
        await _profile.UpdateProfileAsync(CurrentUserId, request, ct);
        return Ok(new { message = "Profile updated." });
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request, CancellationToken ct)
    {
        await _profile.ChangePasswordAsync(CurrentUserId, request, ct);
        return Ok(new { message = "Password changed." });
    }

    [HttpPost("request-email-change")]
    public async Task<IActionResult> RequestEmailChange([FromBody] RequestEmailChangeRequest request, CancellationToken ct)
    {
        await _profile.RequestEmailChangeAsync(CurrentUserId, request, ct);
        return Ok(new { message = "Verification code sent to your new email address." });
    }

    [HttpPost("confirm-email-change")]
    public async Task<IActionResult> ConfirmEmailChange([FromBody] ConfirmEmailChangeRequest request, CancellationToken ct)
    {
        await _profile.ConfirmEmailChangeAsync(CurrentUserId, request, ct);
        return Ok(new { message = "Email updated." });
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAccount([FromBody] DeleteAccountRequest request, CancellationToken ct)
    {
        await _profile.DeleteAccountAsync(CurrentUserId, request, ct);
        return Ok(new { message = "Account deleted." });
    }
}

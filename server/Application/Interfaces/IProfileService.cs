using server.Application.DTOs.Auth;
using server.Application.DTOs.Profile;

namespace server.Application.Interfaces;

public interface IProfileService
{
    Task<UserResponse> GetProfileAsync(Guid userId, CancellationToken ct = default);
    Task UpdateProfileAsync(Guid userId, UpdateProfileRequest request, CancellationToken ct = default);
    Task ChangePasswordAsync(Guid userId, ChangePasswordRequest request, CancellationToken ct = default);
    Task RequestEmailChangeAsync(Guid userId, RequestEmailChangeRequest request, CancellationToken ct = default);
    Task ConfirmEmailChangeAsync(Guid userId, ConfirmEmailChangeRequest request, CancellationToken ct = default);
    Task DeleteAccountAsync(Guid userId, DeleteAccountRequest request, CancellationToken ct = default);
}

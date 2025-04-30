using Refit;

namespace DeltaShare.Service;

public interface IUserProfileService
{
    [Headers("Authorization: Bearer")]
    [Get("/profile")]
    Task<ApiResponse<Dictionary<string, string>>> GetProfileClaims();
}

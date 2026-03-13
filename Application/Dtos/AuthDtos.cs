namespace Application.Dtos;

	public record RegisterRequestDto(
		string UserName,
		string Email,
		string PhoneNumber,
		string Password,
		string ConfirmPassword,
		string AccountType

	);

	public record VerifyOtpRequestDto(
		string Email,
		string Otp
	);

	public record LoginRequestDto(
		string Email,
		string Password
	);
	public record LoginResponseDto(
		string AccessToken,
		string RefreshToken,
		DateTime RefreshTokenExpiry,
		string AccountType
	);

	public record RefreshTokenRequestDto(
		string AccessToken,
		string RefreshToken
	);

	public record ResendOtpRequestDto(
		string Email
	);
	public record ForgotPasswordRequestDto(
		string Email
	);

	public record ResetPasswordRequestDto(
    string Email,
    string Otp,
    string NewPassword,
    string ConfirmPassword
);




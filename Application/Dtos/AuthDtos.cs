namespace Application.Dtos;

	public record RegisterRequestDto(
		string UserName,
		string Email,
		string PhoneNumber,
		string GoogleId,
		string Password,
		string ConfirmPassword,
		string AccountType

	);

	public record VerityOtpRequestDto(
		string Email,
		string Otp
	);

	public record LoginRequestDto(
		string Email,
		string Password
	);
	public record LoginResponseDto(
		string AccessToken,
		DateTime Expiration
	);

	public record ResendOtpRequestDto(
		string Email
	);

	public record RefreshTokenRequestDto(
		string RefreshToken
	);

	public record GoogleLoginRequestDto(
		string GoogleToken,
		string Email
	);


	public record ForgotPasswordRequestDto(
		string Email
	);
	public record ResetPasswordRequestDto(
		string Email,
		string ResetToken,
		string NewPassword
	);





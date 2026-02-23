namespace Application.Dtos;
	public record RegisterV1RequestDto(
		string UserName,
		string Email,
		string GoogleId,
		string AccountType
	);

	public record RegisterV2RequestDto(
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
		string GoogleToken
	);

	public record FinalRegisterRequestDto(
		string VerificationToken,
		string Password,
		string ConfirmPassword
	);

	public record ForgotPasswordRequestDto(
		string Email
	);
	public record ResetPasswordRequestDto(
		string ResetToken,
		string NewPassword
	);





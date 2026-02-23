using FluentValidation;
using Application.Dtos;

namespace Application.Validators;

public static class ValidationExtensions
{
    public static IRuleBuilderOptions<T, string> ValidUserName<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("username is required.")
            .MinimumLength(3).WithMessage("username must be at least 3 characters long.");
    }

    public static IRuleBuilderOptions<T, string> ValidEmail<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Email is required.")
            . Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("A valid email is required.");
    }

    public static IRuleBuilderOptions<T, string> ValidPhoneNumber<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^(\+959|09)\d{7,9}$").WithMessage("A valid phone number is required.");
    }

    public static IRuleBuilderOptions<T, string> StrongPassword<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Password is required.")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$")
            .WithMessage("Password must be at least 8 characters long and contain uppercase, lowercase, digit, and special character.");
    }

    public static IRuleBuilderOptions<T, string> ValidOtp<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("OTP is required.")
            .Length(6).WithMessage("OTP must be 6 digits.")
            .Matches("^[0-9]+$").WithMessage("OTP must be numeric.");
    }
}

public class RegisterV1RequestDtoValidator 
    : AbstractValidator<RegisterV1RequestDto>
{
    public RegisterV1RequestDtoValidator()
    {
        RuleFor(x => x.UserName).ValidUserName();
        RuleFor(x => x.Email).ValidEmail();
        RuleFor(x => x.AccountType).IsInEnum();

        When(x => !string.IsNullOrWhiteSpace(x.GoogleId), () =>
        {
            RuleFor(x => x.GoogleId!)
                .MinimumLength(10)
                .WithMessage("GoogleId must be at least 10 characters.");
        });
    }
}

public class RegisterV2RequestDtoValidator 
    : AbstractValidator<RegisterV2RequestDto>
{
    public RegisterV2RequestDtoValidator()
    {
        RuleFor(x => x.UserName).ValidUserName();
        RuleFor(x => x.Email).ValidEmail();
        RuleFor(x => x.PhoneNumber).ValidPhoneNumber();
        RuleFor(x => x.AccountType).IsInEnum();

        When(x => !string.IsNullOrWhiteSpace(x.GoogleId), () =>
        {
            RuleFor(x => x.GoogleId!)
                .MinimumLength(10)
                .WithMessage("GoogleId must be at least 10 characters.");
        });
    }
}

public class VerifyOtpRequestDtoValidator 
    : AbstractValidator<VerityOtpRequestDto>
{
    public VerifyOtpRequestDtoValidator()
    {
        RuleFor(x => x.Email).ValidEmail();
        RuleFor(x => x.Otp).ValidOtp();
    }
}

public class ResendOtpRequestDtoValidator 
    : AbstractValidator<ResendOtpRequestDto>
{
    public ResendOtpRequestDtoValidator()
    {
        RuleFor(x => x.Email).ValidEmail();
    }
}

public class FinalRegisterRequestDtoValidator 
    : AbstractValidator<FinalRegisterRequestDto>
{
    public FinalRegisterRequestDtoValidator()
    {
        RuleFor(x => x.VerificationToken)
            .NotEmpty().WithMessage("Verification token is required.");

        RuleFor(x => x.Password).StrongPassword();

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password)
            .WithMessage("Passwords do not match.");
    }
}

public class LoginRequestDtoValidator 
    : AbstractValidator<LoginRequestDto>
{
    public LoginRequestDtoValidator()
    {
        RuleFor(x => x.Email).ValidEmail();

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}
public class ForgotPasswordRequestDtoValidator 
    : AbstractValidator<ForgotPasswordRequestDto>
{
    public ForgotPasswordRequestDtoValidator()
    {
        RuleFor(x => x.Email).ValidEmail();
    }
}

public class ResetPasswordRequestDtoValidator 
    : AbstractValidator<ResetPasswordRequestDto>
{
    public ResetPasswordRequestDtoValidator()
    {
        RuleFor(x => x.ResetToken)
            .NotEmpty().WithMessage("Reset token is required.");

        RuleFor(x => x.NewPassword).StrongPassword();
    }
}
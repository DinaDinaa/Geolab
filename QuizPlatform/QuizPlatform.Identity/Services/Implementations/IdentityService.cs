﻿using QuizPlatform.Identity.Exceptions;
using QuizPlatform.Identity.Models;
using QuizPlatform.Identity.Requests;
using QuizPlatform.Identity.Services.Abstractions;
using QuizPlatform.MessageSender.Abstractions.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Authentication;

namespace QuizPlatform.Identity.Services.Implementations;

public sealed class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<IdentityService> _logger;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ITokenService tokenService,
        IEmailSender emailSender,
        ILogger<IdentityService> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _emailSender = emailSender;
        _logger = logger;
    }

    public async Task<string> AuthenticateAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            throw new AuthenticationException();
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, true);

        if (!result.Succeeded)
        {
            _logger.LogWarning("login failed. result: {@Result}", result);
            throw new AuthenticationException();
        }

        return _tokenService.GenerateTokenFor(user);
    }

    public async Task<string?> RegisterAsync(RegisterRequest request)
    {
        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (result.Succeeded)
        {
            if (_userManager.Options.SignIn.RequireConfirmedAccount)
            {
                var code = _userManager.GenerateTwoFactorTokenAsync(user, "Email");
                await SendOtp(request.Email, code.Result);
                return null;
            }

            return _tokenService.GenerateTokenFor(user);
        }

        throw new IdentityException(result.Errors);
    }

    public async Task ConfirmEmail(ConfirmEmailRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            throw new AuthenticationExceptions();
        }

        var isValid = await _userManager.VerifyTwoFactorTokenAsync(user, "Email", request.Otp);
        if (!isValid)
        {
            throw new AuthenticationExceptions();
        }

        user.EmailConfirmed = true;
        await _userManager.UpdateAsync(user);
    }

    public async Task<string> ChangePasswordAsync(ChangePasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            throw new AuthenticationExceptions();
        }

        if (!await _userManager.CheckPasswordAsync(user, request.CurrentPassword))
        {
            throw new AuthenticationExceptions();
        }

        var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        if (result.Succeeded)
        {
            return _tokenService.GenerateTokenFor(user);
        }

        throw new IdentityException(result.Errors);
    }

    public async Task ResetPasswordAsync(ResetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            throw new AuthenticationExceptions();
        }

        var otp = await _userManager.GenerateTwoFactorTokenAsync(user, "ResetPassword");
        await SendOtp(request.Email, otp);
    }

    public async Task NewPasswordAsync(NewPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            throw new AuthenticationExceptions();
        }

        var isValid = await _userManager.VerifyTwoFactorTokenAsync(user, "ResetPassword", request.Otp);
        if (!isValid)
        {
            throw new AuthenticationExceptions();
        }

        if (!string.IsNullOrEmpty(user.PasswordHash))
        {
            var removePasswordResult = await _userManager.RemovePasswordAsync(user);
            if (!removePasswordResult.Succeeded)
            {
                throw new ChangePasswordException(removePasswordResult.Errors, "Failed to remove the old password");
            }
        }

        var addPasswordResult = await _userManager.AddPasswordAsync(user, request.NewPassword);
        if (!addPasswordResult.Succeeded)
        {
            throw new ChangePasswordException(addPasswordResult.Errors, "Failed to remove the old password");
        }
    }

    private async Task SendOtp(string email, string otp)
    {
        await _emailSender.SendEmailAsync(email, subject: "Otp", otp);
    }
}
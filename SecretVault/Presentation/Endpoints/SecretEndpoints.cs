using Microsoft.AspNetCore.Authorization;
using SecretVault.Application.Interfaces;
using SecretVault.Application.Models;
using SecretVault.Domain.Entities;
using System.Security.Claims;

namespace SecretVault.Presentation.Endpoints
{
    public static class SecretEndpoints
    {
        public static void MapSecretEndpoints(this WebApplication app, IEncryptionService encryptionService)
        {
            //Create Secret
            app.MapPost("/secrets", async (Secret secret, ISecretService secretService) =>
            {
                var createdSecret = await secretService.CreateSecretAsync(secret);
                return Results.Ok(createdSecret);
            }).RequireAuthorization("AdminPolicy");
             

            //Get Secret for a key
            app.MapGet("/secrets/{key}", async (string key, ISecretService secretService) =>
            {
                var secret = await secretService.GetSecretAsync(key);
                return secret != null ? Results.Ok(secret) : Results.NotFound();
            }).RequireAuthorization("AdminOrOperatorPolicy");

            //List existing users
            app.MapGet("/getsecrets", async (ISecretService secretService) =>
            {
                var secrets = await secretService.GetAllSecrets();

                var result = secrets.Select(sc => new { sc.Id, sc.Key });
                return Results.Ok(result);
            }).RequireAuthorization("AdminPolicy");

            //Create Token
            app.MapPost("/login",
                async (LoginModel login, IUserService userService, ITokenService tokenService) =>
                {
                    var user = await userService.Authenticate(login.Username, login.Password);
                    if (user == null || user.Id==0)
                    {
                        return Results.Unauthorized();
                    }
                    var token =  tokenService.GenerateToken(user.Username, user.Role);

                    return Results.Ok(new { token });

                    ;
                }).AllowAnonymous();

            //create/update Users
            app.MapPost("/users", async (User user, IUserService userService,
                IAuthorizationService authorizationService, ClaimsPrincipal currentUser) =>
            {

                var authorizationResult = await authorizationService.AuthorizeAsync(currentUser, "AdminPolicy");
                if (!authorizationResult.Succeeded)
                {
                    return Results.Forbid();
                }
                try
                {

                    var existingUser = await userService.AuthenticateUserExist(user.Username);
                    if (existingUser != null)
                    {
                        var updatedUser = await userService.UpdateUserPassword(user.Username, user.Password);
                        return Results.Ok(updatedUser);
                    }
                    else
                    {
                        var createdUser = await userService.CreateUser(user);
                        return Results.Ok(createdUser);
                    }

                }
                catch (ArgumentException ex)
                {

                    return Results.BadRequest(new { message = ex.Message });
                }
            }).RequireAuthorization("AdminPolicy");

            //List existing users
            app.MapGet("/getusers", async (IUserService userService) =>
            {
                var users = await userService.GetAllUsers();
                var result = users.Select(user => new { user.Username, user.Role });
                return Results.Ok(result);
            }).RequireAuthorization("AdminPolicy");

            //Generate Encrypt text
            app.MapPost("/configure/encrypt", (TextToEncripty TextRequest) =>
            {
                var encryptedText = encryptionService.Encrypt(TextRequest.PlainText);
                return Results.Ok(encryptedText);
            }).AllowAnonymous();
        }
    }

}

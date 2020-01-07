using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using QNZOA.Data;
using SIGOA.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace QNZOA.AdminUI.Services
{
    public class QNZAuthenticationStateProvider : AuthenticationStateProvider
    {
        private ISessionStorageService _sessionStorageService;
        private IUserService _userService;

        public bool IsAuthenticated { get; set; }

        public  QNZAuthenticationStateProvider(ISessionStorageService sessionStorageService, IUserService userService)
        {
            _sessionStorageService = sessionStorageService;
            _userService = userService;
            IsAuthenticated = false;
        }



        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            ClaimsIdentity identity;
            if (IsAuthenticated)
            {
                var username = await _sessionStorageService.GetItemAsync<string>("username");
                var passwordHash = await _sessionStorageService.GetItemAsync<string>("passwordHash");
                var checkUser = await _userService.GetUserByUsernameAsync(username);

                if (checkUser == null || passwordHash == null)
                {
                    identity = new ClaimsIdentity();
                    var user = new ClaimsPrincipal(identity);
                    return await System.Threading.Tasks.Task.FromResult(new AuthenticationState(user));

                }
                else
                {

                    //var salt = Convert.FromBase64String(checkUser.SecurityStamp);
                    //var pwdHash = EncryptionHelper.HashPasswordWithSalt(Password, salt);

                    if (checkUser.PasswordHash == passwordHash)
                    {

                        identity = new ClaimsIdentity(new[]
                        {
                    new Claim(ClaimTypes.Sid, checkUser.Id.ToString()),
                    new Claim("RealName", checkUser.RealName??"无"),
                    new Claim(ClaimTypes.Name, checkUser.Username),
                    new Claim(ClaimTypes.Email, checkUser.Email)
                }, "apiauth_type");

                        var user = new ClaimsPrincipal(identity);

                        return await System.Threading.Tasks.Task.FromResult(new AuthenticationState(user));
                    }
                    else
                    {
                        identity = new ClaimsIdentity();
                        var user = new ClaimsPrincipal(identity);
                        return await System.Threading.Tasks.Task.FromResult(new AuthenticationState(user));
                    }

                }
            }
            else
            {
                identity = new ClaimsIdentity();
                var user = new ClaimsPrincipal(identity);
                return await System.Threading.Tasks.Task.FromResult(new AuthenticationState(user));
            }
          
          

        }

        public async System.Threading.Tasks.Task MarkUserAsAuthenticatedAsync(string username, string password)
        {
        

            ClaimsIdentity identity;
         
            var checkUser = await _userService.GetUserByUsernameAsync(username);

            if (checkUser == null)
            {
                identity = new ClaimsIdentity();
                var user = new ClaimsPrincipal(identity);
                NotifyAuthenticationStateChanged(System.Threading.Tasks.Task.FromResult(new AuthenticationState(user)));

            }
            else
            {

                var salt = Convert.FromBase64String(checkUser.SecurityStamp);
                var pwdHash = EncryptionHelper.HashPasswordWithSalt(password, salt);

                if (checkUser.PasswordHash == pwdHash)
                {
                    await _sessionStorageService.SetItemAsync("username", username);
                    await _sessionStorageService.SetItemAsync("passwordHash", pwdHash);
                    IsAuthenticated = true;

                    identity = new ClaimsIdentity(new[]
                    {
                    new Claim(ClaimTypes.Sid, checkUser.Id.ToString()),
                    new Claim("RealName", checkUser.RealName??"无"),
                    new Claim(ClaimTypes.Name, checkUser.Username),
                    new Claim(ClaimTypes.Email, checkUser.Email)
                }, "apiauth_type");

                    var user = new ClaimsPrincipal(identity);

                    NotifyAuthenticationStateChanged(System.Threading.Tasks.Task.FromResult(new AuthenticationState(user)));
                }
                else
                {
                    identity = new ClaimsIdentity();
                    var user = new ClaimsPrincipal(identity);
                    NotifyAuthenticationStateChanged(System.Threading.Tasks.Task.FromResult(new AuthenticationState(user)));
                }

            }


        }



        public async System.Threading.Tasks.Task MarkUserAsAuthenticated2Async(string username, string passwordHash)
        {
        
            ClaimsIdentity identity;

            var checkUser = await _userService.GetUserByUsernameAsync(username);

            if (checkUser == null)
            {
                identity = new ClaimsIdentity();
                var user = new ClaimsPrincipal(identity);
                NotifyAuthenticationStateChanged(System.Threading.Tasks.Task.FromResult(new AuthenticationState(user)));

            }
            else
            {

                if (checkUser.PasswordHash == passwordHash)
                {
                    await _sessionStorageService.SetItemAsync("username", username);
                    await _sessionStorageService.SetItemAsync("passwordHash", passwordHash);
                    IsAuthenticated = true;

                    identity = new ClaimsIdentity(new[]
                    {
                    new Claim(ClaimTypes.Sid, checkUser.Id.ToString()),
                    new Claim("RealName", checkUser.RealName??"无"),
                    new Claim(ClaimTypes.Name, checkUser.Username),
                    new Claim(ClaimTypes.Email, checkUser.Email)
                }, "apiauth_type");

                    var user = new ClaimsPrincipal(identity);

                    NotifyAuthenticationStateChanged(System.Threading.Tasks.Task.FromResult(new AuthenticationState(user)));
                }
                else
                {
                    identity = new ClaimsIdentity();
                    var user = new ClaimsPrincipal(identity);
                    NotifyAuthenticationStateChanged(System.Threading.Tasks.Task.FromResult(new AuthenticationState(user)));
                }

            }


        }

    }
}

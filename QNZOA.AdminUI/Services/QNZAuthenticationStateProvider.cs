using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using QNZOA.Data;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace QNZOA.AdminUI.Services
{
    public class QNZAuthenticationStateProvider : AuthenticationStateProvider
    {

        //public static bool IsAuthenticated { get; set; }
        //public static bool IsAuthenticating { get; set; }

        //public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        //{
        //    ClaimsIdentity identity;

        //    if (IsAuthenticating)
        //    {
        //        return null;
        //    }
        //    else if (IsAuthenticated)
        //    {
        //        identity = new ClaimsIdentity(new List<Claim>
        //                {
        //                    new Claim(ClaimTypes.Name, "TestUser")

        //                }, "WebApiAuth");
        //    }
        //    else
        //    {
        //        identity = new ClaimsIdentity();
        //    }

        //    return await System.Threading.Tasks.Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)));
        //}

        //public void NotifyAuthenticationStateChanged()
        //{
        //    NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        //}


        private ISessionStorageService _sessionStorageService;
        public bool IsAuthenticated { get; set; }

        public QNZAuthenticationStateProvider(ISessionStorageService sessionStorageService)
        {
            _sessionStorageService = sessionStorageService;
            IsAuthenticated = false;
        }



        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {

            if (IsAuthenticated)
            {
                var checkUser = await _sessionStorageService.GetItemAsync<User>("currentUser");
                var identity = new ClaimsIdentity(new[]
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
                var identity = new ClaimsIdentity();
                var user = new ClaimsPrincipal(identity);
                return await System.Threading.Tasks.Task.FromResult(new AuthenticationState(user));
            }


        }

        public void MarkUserAsAuthenticatedAsync(User currentUser)
        {
            IsAuthenticated = true;

            var identity = new ClaimsIdentity(new[]
              {
                    new Claim(ClaimTypes.Sid, currentUser.Id.ToString()),
                    new Claim("RealName", currentUser.RealName??"无"),
                    new Claim(ClaimTypes.Name, currentUser.Username),
                    new Claim(ClaimTypes.Email, currentUser.Email)
                }, "apiauth_type");

            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(System.Threading.Tasks.Task.FromResult(new AuthenticationState(user)));

        }




    }
}

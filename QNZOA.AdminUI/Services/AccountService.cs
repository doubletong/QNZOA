using Blazored.LocalStorage;
using QNZOA.Model;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace QNZOA.AdminUI.Services
{
    public class AccountService:IAccountService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        public AccountService(HttpClient httpClient, ILocalStorageService localStorage/*, IHttpClientFactory httpClientFactory*/)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            //_httpClientFactory = httpClientFactory;
        }
        public async Task<ReturnVM> LoginAsync(LoginIM loginIM)
        {
            //var client = _httpClientFactory.CreateClient("aa");
            var url = $"{_httpClient.BaseAddress}Account";

            var content = new StringContent(JsonSerializer.Serialize(loginIM).ToString(), Encoding.UTF8, "application/json");
            var request = await _httpClient.PostAsync(url, content);
         
            string result = await request.Content.ReadAsStringAsync();

            if (request.StatusCode == System.Net.HttpStatusCode.OK)
            {
                
                var userInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<UserLoginVM>(result);
    
                await _localStorage.SetItemAsync("username", userInfo.Username);
                await _localStorage.SetItemAsync("token", userInfo.Token);
                await _localStorage.SetItemAsync("photo", userInfo.PhotoUrl);
                await _localStorage.SetItemAsync("realname", userInfo.RealName);
               
                return new ReturnVM {Status=true, Message="登录成功"};
            }
            else
            {
               
                return Newtonsoft.Json.JsonConvert.DeserializeObject<ReturnVM>(result); 
            }

        }
       

    }
}

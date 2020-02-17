using AutoMapper;
using AutoMapper.QueryableExtensions;
using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Microsoft.EntityFrameworkCore;
using QNZOA.Data;
using QNZOA.Model;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace QNZOA.AdminUI.Services
{
    public class AccountService:IAccountService
    {
        //private readonly HttpClient _httpClient;
        private ISessionStorageService _sessionStorageService;
        private readonly SIGOAContext _db;
        private readonly IMapper _mapper;
        public AccountService(/*HttpClient httpClient,*/ ISessionStorageService sessionStorageService, SIGOAContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            //_httpClient = httpClient;
            _sessionStorageService = sessionStorageService;
            //_httpClientFactory = httpClientFactory;
        }
        //public async Task<ReturnVM> LoginAsync(LoginIM loginIM)
        //{
        //    //var client = _httpClientFactory.CreateClient("aa");
        //    var url = $"{_httpClient.BaseAddress}Account";

        //    var content = new StringContent(JsonSerializer.Serialize(loginIM).ToString(), Encoding.UTF8, "application/json");
        //    var request = await _httpClient.PostAsync(url, content);

        //    string result = await request.Content.ReadAsStringAsync();

        //    if (request.StatusCode == System.Net.HttpStatusCode.OK)
        //    {

        //        var userInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<UserLoginVM>(result);

        //        await _localStorage.SetItemAsync("username", userInfo.Username);
        //        await _localStorage.SetItemAsync("token", userInfo.Token);
        //        await _localStorage.SetItemAsync("photo", userInfo.PhotoUrl);
        //        await _localStorage.SetItemAsync("realname", userInfo.RealName);

        //        return new ReturnVM { Status = true, Message = "登录成功" };
        //    }
        //    else
        //    {

        //        return Newtonsoft.Json.JsonConvert.DeserializeObject<ReturnVM>(result);
        //    }

        //}

        public async Task<IEnumerable<UserForSelectVM>> GetUsersForSelectAsync()
        {
            return await _db.Users.OrderBy(d => d.Username)
                .ProjectTo<UserForSelectVM>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }


    }
}

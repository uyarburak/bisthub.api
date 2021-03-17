using BistHub.Api.Common;
using BistHub.Api.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;

namespace BistHub.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : Controller
    {
        private readonly FirebaseConfig _firebaseConfig;
        public AuthController(IOptions<FirebaseConfig> options)
        {
            _firebaseConfig = options.Value;
        }
        [HttpPost]
        public async Task<ActionResult> GetToken([FromForm] LoginInfo loginInfo)
        {
            string uri = $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={_firebaseConfig.ApiKey}";
            using HttpClient client = new HttpClient();
            FireBaseLoginInfo fireBaseLoginInfo = new FireBaseLoginInfo
            {
                Email = loginInfo.Username,
                Password = loginInfo.Password
            };
            var result = await client.PostAsJsonAsync<FireBaseLoginInfo, GoogleToken>(uri, fireBaseLoginInfo);
            if (result == null)
                return BadRequest();
            Token token = new Token
            {
                token_type = "Bearer",
                access_token = result.idToken,
                id_token = result.idToken,
                expires_in = int.Parse(result.expiresIn),
                refresh_token = result.refreshToken
            };
            return Ok(token);
        }
    }

    public class LoginInfo
    {
        public string Username { get; set; }
        public string Password { get; set; }

    }

    class FireBaseLoginInfo
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool ReturnSecureToken { get; set; } = true;

    }

    class GoogleToken
    {
        public string kind { get; set; }
        public string localId { get; set; }
        public string email { get; set; }
        public string displayName { get; set; }
        public string idToken { get; set; }
        public bool registered { get; set; }
        public string refreshToken { get; set; }
        public string expiresIn { get; set; }
    }


    class Token
    {
        internal string refresh_token;

        public string token_type { get; set; }
        public int expires_in { get; set; }
        public int ext_expires_in { get; set; }
        public string access_token { get; set; }
        public string id_token { get; set; }
    }
}

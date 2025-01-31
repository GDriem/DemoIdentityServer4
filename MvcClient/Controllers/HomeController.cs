using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

public class HomeController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HomeController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public IActionResult Index()
    {
        return View();
    }

    [AllowAnonymous]
    public IActionResult Login(string returnUrl = "/")
    {
        // Puedes pasar el returnUrl si deseas redirigir al usuario a una página específica tras el login
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }
    // Acción POST que inicia el proceso de autenticación
    [HttpPost]
    [AllowAnonymous]
    public IActionResult LoginPost(string returnUrl = "/")
    {
        // Aquí el valor por defecto "/" se usa si no se especifica returnUrl,
        // lo que garantiza que RedirectUri no sea nulo.
        return Challenge(new AuthenticationProperties { RedirectUri = returnUrl }, "oidc");
    }
    public IActionResult Logout()
    {
        return SignOut(new AuthenticationProperties { RedirectUri = "/" },
            CookieAuthenticationDefaults.AuthenticationScheme,
            OpenIdConnectDefaults.AuthenticationScheme);
    }

    [Authorize]
    public async Task<IActionResult> SecureData()
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");

        var client = _httpClientFactory.CreateClient();
        client.SetBearerToken(accessToken);

        var response = await client.GetAsync("https://localhost:7108/api/products");

        if (!response.IsSuccessStatusCode)
        {
            return Content($"Error: {response.StatusCode}");
        }

        var content = await response.Content.ReadAsStringAsync();
        return Content(content, "application/json");
    }
    [AllowAnonymous]
    [Route("home/error")]
    public IActionResult Error(string errorId)
    {
        
        ViewData["ErrorId"] = errorId;
        return View();
    }
}

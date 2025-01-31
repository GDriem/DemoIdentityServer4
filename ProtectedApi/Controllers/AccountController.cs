using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

/// <summary>
/// Controller for handling account-related actions such as login and logout.
/// </summary>
[AllowAnonymous]
public class AccountController : Controller
{
    /// <summary>
    /// Displays the login view.
    /// </summary>
    /// <param name="returnUrl">The URL to return to after login.</param>
    /// <returns>The login view.</returns>
    public IActionResult Login(string returnUrl = "/")
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    /// <summary>
    /// Handles the login post request.
    /// </summary>
    /// <param name="returnUrl">The URL to return to after login.</param>
    /// <returns>A challenge result to initiate the OpenID Connect authentication.</returns>
    [HttpPost]
    public IActionResult LoginPost(string returnUrl = "/")
    {
        return Challenge(new AuthenticationProperties { RedirectUri = returnUrl }, "oidc");
    }

    /// <summary>
    /// Handles the logout request.
    /// </summary>
    /// <returns>A sign-out result to log the user out and redirect to the home page.</returns>
    public IActionResult Logout()
    {
        return SignOut(new AuthenticationProperties { RedirectUri = "/" },
            CookieAuthenticationDefaults.AuthenticationScheme,
            OpenIdConnectDefaults.AuthenticationScheme);
    }
}

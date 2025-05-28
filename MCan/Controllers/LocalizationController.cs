using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

public class LocalizationController : Controller
{
    [HttpPost]
    public IActionResult SetLanguage(string culture, string returnUrl = "/")
    {
        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddYears(1),
                IsEssential = true  // Make sure cookie works even if consent not given
            }
        );

        return LocalRedirect(returnUrl);
    }
}

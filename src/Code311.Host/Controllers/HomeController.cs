using Microsoft.AspNetCore.Mvc;

namespace Code311.Host.Controllers;

public sealed class HomeController : Controller
{
    public IActionResult Index() => View();
}

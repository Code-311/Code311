using Microsoft.AspNetCore.Mvc;

namespace Code311.Host.Controllers;

public sealed class ComponentsDemoController : Controller
{
    public IActionResult Forms() => View();
    public IActionResult Navigation() => View();
    public IActionResult Layout() => View();
    public IActionResult Feedback() => View();
    public IActionResult Data() => View();
    public IActionResult Media() => View();
}

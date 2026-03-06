using Microsoft.AspNetCore.Mvc;

namespace Code311.Host.Controllers;

public sealed class ArchitectureController : Controller
{
    public IActionResult About() => View();
}

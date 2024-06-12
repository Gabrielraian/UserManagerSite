using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using UserManagerSite.MVC.Models;

namespace UserManagerSite.MVC.Controllers;

public class RolesController : Controller
{
    private readonly ILogger<RolesController> _logger;

    public RolesController(ILogger<RolesController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Add()
    {
        return View();
    }
}
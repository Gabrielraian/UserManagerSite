using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using UserManagerSite.MVC.Models;

namespace UserManagerSite.MVC.Controllers;

public class UsersController : Controller
{
    private readonly ILogger<UsersController> _logger;

    public UsersController(ILogger<UsersController> logger)
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
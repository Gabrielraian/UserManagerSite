using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UserManagerSite.MVC.Data;
using UserManagerSite.MVC.Models;
using UserManagerSite.MVC.ViewModels;

namespace UserManagerSite.MVC.Controllers;

public class RolesController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<RolesController> _logger;

    public RolesController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int? id, string? role)
    {
        var client = _httpClientFactory.CreateClient();
        var url = "http://localhost:60702/api/Role?";

        if (id.HasValue)
        {
            url += $"id={id.Value}&";
        }
        if (!string.IsNullOrEmpty(role))
        {
            url += $"role={role}&";
        }

        // Removendo o último '&' da URL, se houver
        url = url.TrimEnd('&');

        var response = await client.GetAsync(url);
        var roles = new List<RoleDTO>();

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            roles = JsonConvert.DeserializeObject<List<RoleDTO>>(json);
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Erro ao buscar roles.");
        }

        return View(roles);
    }

    [HttpPost]
    public async Task<IActionResult> Add(RoleViewModel viewModel)
    {
        var client = _httpClientFactory.CreateClient();
        HttpResponseMessage response;

        try
        {
            // Verificar se o nome do role é obrigatório
            if (string.IsNullOrEmpty(viewModel.Role.role))
            {
                ModelState.AddModelError(string.Empty, "O nome do role é obrigatório.");
                return View(viewModel);
            }

            var existingRolesResponse = await client.GetAsync("http://localhost:60702/api/Role");
            
            var role = new Role
            {
                role = viewModel.Role.role,
                Users = null
            };

            if (viewModel.Role.id != 0 && viewModel.Role.id != null) // Atualização (PUT)
            {
                role.id = viewModel.Role.id;
                var jsonRole = JsonConvert.SerializeObject(role, Formatting.Indented);
                var content = new StringContent(jsonRole, Encoding.UTF8, "application/json");
                
                var apiUrl = $"http://localhost:60702/api/Role/{role.id}";
                response = await client.PutAsync(apiUrl, content);
            }
            else
            {
                var jsonRole = JsonConvert.SerializeObject(role, Formatting.Indented);
                var content = new StringContent(jsonRole, Encoding.UTF8, "application/json");
                var apiUrl = "http://localhost:60702/api/Role";
                response = await client.PostAsync(apiUrl, content);
            }

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, $"Erro na API: {errorMessage}");
                return View(viewModel);
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Erro: {ex.Message}");
            return View(viewModel);
        }
    }

    [HttpGet]
    public IActionResult Add()
    {
        var viewModel = new RoleViewModel();
        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync($"http://localhost:60702/api/Role/{id}");

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var role = JsonConvert.DeserializeObject<RoleDTO>(json);
            var viewModel = new RoleViewModel
            {
                Role = role
            };

            return View(viewModel);
        }
        else
        {
            return NotFound();
        }
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var client = _httpClientFactory.CreateClient();
        var apiUrl = $"http://localhost:60702/api/Role/{id}";

        var response = await client.DeleteAsync(apiUrl);

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Index");
        }
        else
        {
            // Handle error scenario here (e.g., show error message)
            return RedirectToAction("Index"); // Placeholder, you can change this as per your application's error handling needs
        }
    }

}
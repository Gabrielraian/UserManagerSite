using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using UserManagerSite.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UserManagerSite.MVC.Data;
using UserManagerSite.MVC.ViewModels;
using System.Text;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using NuGet.Protocol;

namespace UserManagerSite.MVC.Controllers;

public class UsersController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public UsersController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int? id, string? name, int? roleId)
    {
        var client = _httpClientFactory.CreateClient();

        var url = "http://localhost:60702/api/User?";
        if (id.HasValue)
        {
            url += $"id={id.Value}&";
        }
        if (!string.IsNullOrEmpty(name))
        {
            url += $"name={name}&";
        }
        if (roleId.HasValue)
        {
            url += $"roleId={roleId.Value}&";
        }
        var response = await client.GetAsync(url.TrimEnd('&'));

        var users = new List<UserDTO>();
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            users = JsonConvert.DeserializeObject<List<UserDTO>>(json);
        }

        // Fetch roles
        response = await client.GetAsync("http://localhost:60702/api/Role");
        var roles = new List<RoleDTO>();
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            roles = JsonConvert.DeserializeObject<List<RoleDTO>>(json);
        }

        var viewModel = new UsersViewModel
        {
            Users = users,
            Roles = roles
        };

        return View(viewModel);
    }

    public async Task<IActionResult> Add()
    {
        var roles = await GetRolesFromApi();

        var viewModel = new UserRoleViewModel
        {
            User = new UserDTO(),
            Roles = roles
        };

        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var apiUrl = $"http://localhost:60702/api/User/{id}";
        var client = _httpClientFactory.CreateClient();

        var response = await client.GetAsync(apiUrl);

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<UserDTO>(json);
            var roles = await GetRolesFromApi();

            var viewModel = new UserRoleViewModel
            {
                User = user,
                Roles = roles
            };

            return View(viewModel);
        }
        else
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"Erro na API: {errorMessage}");
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Add(UserRoleViewModel viewModel)
    {
        var client = _httpClientFactory.CreateClient();
        HttpResponseMessage response;

        try
        {
            var selectedRole = await GetRoleByIdAsync(viewModel.User.roleId);

            if (selectedRole == null)
            {
                ModelState.AddModelError(string.Empty, "Role selecionada inválida.");
                viewModel.Roles = await GetRolesFromApi();
                return View(viewModel);
            }

            viewModel.User.role = selectedRole;

            var userMonted = new User
            {
                name = viewModel.User.name,
                birthdate = viewModel.User.birthdate,
                email = viewModel.User.email,
                role = new Role
                {
                    id = selectedRole.id,
                    role = selectedRole.role,
                }
            };

            if (viewModel.User.id != 0 && viewModel.User.id != null)
            {
                userMonted.id = viewModel.User.id;

                var jsonUser = JsonConvert.SerializeObject(userMonted, Formatting.Indented);
                var content = new StringContent(jsonUser, Encoding.UTF8, "application/json");

                var apiUrl = $"http://localhost:60702/api/User/{viewModel.User.id}";
                response = await client.PutAsync(apiUrl, content);
            }
            else // Se o ID for 0, cria (POST)
            {
                var jsonUser = JsonConvert.SerializeObject(userMonted, Formatting.Indented);
                var content = new StringContent(jsonUser, Encoding.UTF8, "application/json");

                var apiUrl = "http://localhost:60702/api/User";
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
                viewModel.Roles = await GetRolesFromApi();
                return View(viewModel);
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Erro: {ex.Message}");
            viewModel.Roles = await GetRolesFromApi();
            return View(viewModel);
        }
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var client = _httpClientFactory.CreateClient();
        var apiUrl = $"http://localhost:60702/api/User/{id}";

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

    private async Task<List<RoleDTO>> GetRolesFromApi()
    {
        var client = _httpClientFactory.CreateClient();
        var url = "http://localhost:60702/api/Role";
        var response = await client.GetAsync(url.TrimEnd('&'));
        List<RoleDTO> roles = new List<RoleDTO>(); // Inicialização direta

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            roles = JsonConvert.DeserializeObject<List<RoleDTO>>(json) ?? new List<RoleDTO>(); // Certifique-se de que roles nunca seja nulo
        }

        return roles;
    }

    private async Task<RoleDTO> GetRoleByIdAsync(int roleId)
    {
        using (var client = _httpClientFactory.CreateClient())
        {
            var response = await client.GetAsync($"http://localhost:60702/api/Role/{roleId}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var role = JsonConvert.DeserializeObject<RoleDTO>(json);
                return role;
            }
            else
            {
                return null;
            }
        }
    }

    public User ConvertUserDTOToUser(UserDTO userDTO)
    {
        return new User
        {
            id = userDTO.id,
            name = userDTO.name,
            birthdate = userDTO.birthdate,
            email = userDTO.email,
            role = new Role
            {
                id = userDTO.roleId,
                role = userDTO.roleName
            }
        };
    }
}
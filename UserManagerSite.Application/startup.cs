using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json.Serialization;
using UserManagerSite.Application.Data; // Certifique-se de ajustar o namespace conforme necessário

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // Este método é chamado pelo runtime. Use este método para adicionar serviços ao contêiner.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            });

        // Adicione outros serviços aqui, por exemplo, contexto do Entity Framework
        services.AddDbContext<UserManagerSiteContext>(options =>
            options.UseSqlite(Configuration.GetConnectionString("UserManagerSiteContext") ?? throw new InvalidOperationException("Connection string 'UserManagerSiteContext' not found.")));

        // Outros serviços
        services.AddEndpointsApiExplorer(); // Opcional, se você usar endpoints e não controllers
    }

    // Este método é chamado pelo runtime. Use este método para configurar o pipeline de requisição HTTP.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}

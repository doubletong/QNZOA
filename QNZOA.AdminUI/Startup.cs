using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QNZOA.AdminUI.Data;
using QNZOA.AdminUI.Services;
using Blazored.LocalStorage;
using Blazored.Toast;
using Microsoft.AspNetCore.Components.Authorization;
using QNZOA.Data;
using Microsoft.EntityFrameworkCore;
using Blazored.SessionStorage;
using AutoMapper;

namespace QNZOA.AdminUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SIGOAContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Scoped);
          //  services.AddDbContext<SIGOAContext>(options =>
          //     options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddBlazoredLocalStorage();
            services.AddBlazoredSessionStorage();
            services.AddBlazoredToast();

            services.AddHttpClient();
            services.AddHttpClient("QNZOA",client =>
            {
                client.BaseAddress = new Uri(Configuration["ApiURL"]);
                client.DefaultRequestHeaders.Add("Accept", "application/json");

            });
            services.AddHttpClient<IAccountService, AccountService>(client =>
            {
                client.BaseAddress = new Uri(Configuration["ApiURL"]);
                client.DefaultRequestHeaders.Add("Accept", "application/json");

            });
            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddAutoMapper(typeof(Startup).Assembly);

            services.AddSingleton<WeatherForecastService>();
            //services.AddTransient<IAccountService, AccountService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<AuthenticationStateProvider, QNZAuthenticationStateProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}

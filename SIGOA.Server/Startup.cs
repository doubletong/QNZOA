
using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using SIGOA.Data;
using SIGOA.Infrastructure.Email;
using SIGOA.Model;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;

namespace SIGOA.Server
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        //public Startup(IConfiguration configuration, IHostingEnvironment env)
        //{
        //    var builder = new ConfigurationBuilder()
        //        .SetBasePath(env.ContentRootPath)
        //        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        //    Configuration = builder.Build();
        //}

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
    
            services.AddCors();
            services.AddControllers();
            services.AddOpenApiDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "QNZOA API";
                    document.Info.Description = "A simple ASP.NET Core web API";
                    document.Info.TermsOfService = "None";
                    document.Info.Contact = new NSwag.OpenApiContact
                    {
                        Name = "青鸟",
                        Email = "twotong@gmail.com",
                        Url = "https://www.heiniaozhi.cn"
                    };
                    document.Info.License = new NSwag.OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = "https://example.com/license"
                    };
                };
            }); // add OpenAPI v3 document
                                           //services.AddResponseCompression(options =>
                                           //{
                                           //    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
                                           //    {
                                           //        MediaTypeNames.Application.Octet,
                                           //        //WasmMediaTypeNames.Application.Wasm,
                                           //    });
                                           //});


            services.AddDbContext<SigbugsdbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection")));

            //身份验证服务           
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(cfg =>
                      {
                          cfg.RequireHttpsMetadata = false;
                          cfg.SaveToken = true;
                          cfg.TokenValidationParameters = new TokenValidationParameters()
                          {
                              ValidateIssuerSigningKey = true,
                              ValidIssuer = Configuration["TokenOptions:Issuer"],
                              ValidAudience = Configuration["TokenOptions:Issuer"],
                              IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["TokenOptions:Key"])),
                          };
                      });

            //services.AddAuthorization(options =>
            //{                
            //    options.AddPolicy("Permission", policy => {
            //        //policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
            //        //policy.RequireAuthenticatedUser();
            //        policy.Requirements.Add(new PermissionRequirement("/errors/accessdenied"));
            //        });
            //});

           
            services.AddAutoMapper(typeof(Startup).Assembly);
          

            //services.AddMvc();
            //  .AddJsonOptions(options =>{options.SerializerSettings.ContractResolver = new DefaultContractResolver();});



            services.AddOptions();
            //services.Configure<TokenOptions>(Configuration.GetSection("TokenOptions"));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IEmailService, EmailService>();
            //services.AddScoped<IAuthorizationHandler, PermissionHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseResponseCompression();

            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //    //app.UseDatabaseErrorPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //    app.UseHsts();
            //}

            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(
            //        Path.Combine(Directory.GetCurrentDirectory(), "Uploads")),
            //    RequestPath = "/Uploads",
            //    OnPrepareResponse = ctx =>
            //    {
            //        ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=36000");
            //    }
            //});
            //app.UseCors(
            //     options => options.WithOrigins("http://gzh.anyacos.com", "http://localhost:8000", "http://localhost").AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin().AllowCredentials()
            // );
            //app.UseHangfireServer();
            //app.UseHangfireDashboard();


            ////身份验证服务
            //app.UseAuthentication(); // add the Authentication middleware

            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(name: "default", template: "{controller}/{action}/{id?}");
            //});

            //app.UseEndpoints(endpoints =>
            //{

            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller=Home}/{action=Index}/{id?}")

            //    endpoints.MapRazorPages();
            //});

            //app.UseSwagger();
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SIGTaskManager-API");
            //});

            // Use component registrations and static files from the app project.
            //app.UseServerSideBlazor<App.Startup>();

            app.UseRouting();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });

            app.UseOpenApi(); // serve OpenAPI/Swagger documents
            app.UseSwaggerUi3(); // serve Swagger UI
            app.UseReDoc(); // serve ReDoc UI

        }
    }
}

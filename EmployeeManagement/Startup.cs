using EmployeeManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("EmployeeDBConnection")
                /*,options => options.MigrationsAssembly("EFCoreApp")*/)
            );
            services.AddControllersWithViews().AddXmlSerializerFormatters();
            services.AddMvc(config => {
                var policy = new AuthorizationPolicyBuilder()
                                .RequireAuthenticatedUser()
                                .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            }); 
            services.AddScoped<IEmployeeRepository, SQLEmployeeRepository>();
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("DeleteRolePolicy", policy => policy.RequireClaim("Delete Role", "true"));
                options.AddPolicy("CreateRolePolicy", policy => policy.RequireClaim("Create Role", "true"));
                options.AddPolicy("EditRolePolicy", policy => policy.RequireClaim("Edit Role", "true"));
                options.AddPolicy("AdminRolePolicy", policy => policy.RequireRole("Admin"));
                options.AddPolicy("AllowedCountryPolicy", policy => policy.RequireClaim("Country", "USA", "India", "UK"));
            });
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("EditRolePolicy", policy => policy.RequireAssertion(context =>
            //        context.User.IsInRole("Admin") &&
            //        context.User.HasClaim(claim => claim.Type == "Edit Role" && claim.Value == "true") ||
            //        context.User.IsInRole("Super Admin")
            //    ));
            //});
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("EditRolePolicy", policy =>
            //        policy.RequireAssertion(context => AuthorizeAccess(context)));
            //});
            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = new PathString("/Administration/AccessDenied");
            });
            services.AddAuthentication().AddGoogle(options =>
            {
                options.ClientId = "162032850507-naaqk7vk6ppt9r79qmm2k55pativq14t.apps.googleusercontent.com";
                options.ClientSecret = "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";
                options.AuthorizationEndpoint += "?prompt=consent";
                options.SaveTokens = true;
            });
        }

        private bool AuthorizeAccess(AuthorizationHandlerContext context)
        {
            return context.User.IsInRole("Admin") &&
                    context.User.HasClaim(claim => claim.Type == "Edit Role" && claim.Value == "true") ||
                    context.User.IsInRole("Super Admin");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //Register Syncfusion license
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NTQ2NTU4QDMxMzkyZTMzMmUzMEkxOVkwOWo1SWluUUhHWnJhZlB1UUpseFIwczBScjZuL1FxR0ZTdHYyaHc9");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseExceptionHandler("/Home/Error");
                //// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
                //app.UseStatusCodePagesWithRedirects("/Error/{0}");
                app.UseExceptionHandler("/Error");
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

using System;
using System.Linq;
using System.Reflection;
using IndecisionEngine.Models;
using IndecisionEngine.Services;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IndecisionEngine
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(connectionString))
                .AddDbContext<StoryDbContext>(options =>
                    options.UseSqlServer(connectionString));

            // services.AddCaching();
            services.AddSession();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireClaim("Role", "Admin"));
            });
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                // options.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            services.AddMvc(options =>
            {
                options.SslPort = 44357;
            });

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.Configure<AuthMessageSenderOptions>(options => Configuration.Bind(options));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IActionDescriptorCollectionProvider actionDescriptor)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
                AuditMvc(app, actionDescriptor);
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            
            app.UseStaticFiles();
            app.UseSession();

            app.UseIdentity();

            // To configure external authentication please see http://go.microsoft.com/fwlink/?LinkID=532715

            // Add and configure the options for authentication middleware to the request pipeline.
            // You can add options for middleware as shown below.
            // For more information see http://go.microsoft.com/fwlink/?LinkID=532715
            /*
            app.UseFacebookAuthentication(new FacebookOptions()
            {
                AppId = Configuration["Authentication:Facebook:AppId"],
                AppSecret = Configuration["Authentication:Facebook:AppSecret"],
            });
            app.UseGoogleAuthentication(new GoogleOptions()
            {
                ClientId = Configuration["Authentication:Google:ClientId"],
                ClientSecret = Configuration["Authentication:Google:ClientSecret"],
            });*/
            //app.UseMicrosoftAccountAuthentication(options =>
            //{
            //    options.ClientId = Configuration["Authentication:MicrosoftAccount:ClientId"];
            //    options.ClientSecret = Configuration["Authentication:MicrosoftAccount:ClientSecret"];
            //});
            app.UseTwitterAuthentication(new TwitterOptions()
            {
                ConsumerKey = Configuration["Authentication:Twitter:ConsumerKey"],
                ConsumerSecret = Configuration["Authentication:Twitter:ConsumerSecret"],
                RetrieveUserDetails = true,
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        // http://stackoverflow.com/a/30969888/2588374
        private void AuditMvc(IApplicationBuilder app, IActionDescriptorCollectionProvider actionDescriptor)
        {
            app.Use(async (context, next) =>
            {
                if (!context.Request.Path.StartsWithSegments("/audit-mvc"))
                {
                    await next();
                    return;
                }

                // Official way:
                await context.Response.WriteAsync($"Action Descriptors:");
                foreach (ControllerActionDescriptor descriptor in actionDescriptor.ActionDescriptors.Items)
                {
                    await context.Response.WriteAsync($"{descriptor.ControllerName}.{descriptor.ActionName}: ");
                    foreach (var filterDescriptor in descriptor.FilterDescriptors)
                    {
                        if (filterDescriptor.Filter is AuthorizeFilter)
                        {
                            var authorizeFilter = filterDescriptor.Filter as AuthorizeFilter;
                            foreach (var requirement in authorizeFilter.Policy.Requirements)
                            {
                                if (requirement is DenyAnonymousAuthorizationRequirement)
                                {
                                    await context.Response.WriteAsync($"DenyAnonymous");
                                }
                                else if (requirement is ClaimsAuthorizationRequirement)
                                {
                                    var claimsRequirement = requirement as ClaimsAuthorizationRequirement;
                                    await context.Response.WriteAsync($"Claims Requirement: {claimsRequirement.ClaimType}: {string.Join(",", claimsRequirement.AllowedValues)}");
                                }
                                else
                                {
                                    await context.Response.WriteAsync($" {requirement}");
                                }
                            }
                        }
                    }
                    await context.Response.WriteAsync($"\r\n");
                }

                await context.Response.WriteAsync($"\r\n\r\n");

                // Common way:
                var asm = typeof(Startup).GetTypeInfo().Assembly;

                var controllerlist = asm.GetTypes()
                        .Where(type => typeof(Controller).IsAssignableFrom(type))
                        .Select(x => new
                        {
                            Controller = x,
                            Attributes = x.GetTypeInfo().CustomAttributes
                                .OrderBy(a => a.AttributeType.Name)
                                .Select(a =>
                                    a.AttributeType.Name.Replace("Attribute",
                                    $"({String.Join(",", a.ConstructorArguments.Select(arg => arg.Value))})"))
                        })
                        .OrderBy(x => x.Controller.Name).ToList();

                foreach (var entry in controllerlist)
                {
                    await context.Response.WriteAsync($"{entry.Controller.Name}: {String.Join(",", entry.Attributes)}\r\n");

                    var actionlist = entry.Controller
                            .GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public)
                            .Select(x => new {
                                Action = x.Name,
                                Attributes = x.CustomAttributes.Where(a =>
                                    !string.Equals(a.AttributeType.Name, "AsyncStateMachineAttribute")
                                        && !string.Equals(a.AttributeType.Name, "DebuggerStepThroughAttribute"))
                                    .OrderBy(a => a.AttributeType.Name)
                                    .Select(a => a.AttributeType.Name.Replace("Attribute",
                                        $"({String.Join(",", a.ConstructorArguments.Select(arg => arg.Value))})"))
                            })
                            .OrderBy(x => x.Action).ToList();

                    foreach (var action in actionlist)
                    {
                        await context.Response.WriteAsync($"- {action.Action}: {String.Join(",", action.Attributes)}\r\n");
                    }
                }
            });
        }
    }
}

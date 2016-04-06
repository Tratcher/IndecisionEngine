using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using IndecisionEngine.Models;
using IndecisionEngine.Services;
using Microsoft.AspNet.Authentication.Twitter;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.WebEncoders;
using Newtonsoft.Json.Linq;

namespace IndecisionEngine
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
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

        // TODO: Temp for Twitter
        public HttpClient Backchannel { get; set; } = new HttpClient();

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]))
                .AddDbContext<StoryDbContext>(options =>
                    options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]));

            services.AddCaching();
            services.AddSession();

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                // options.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            services.AddMvc();

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.Configure<AuthMessageSenderOptions>(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseRuntimeInfoPage("/runtime");
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

                // For more details on creating database during deployment see http://go.microsoft.com/fwlink/?LinkID=615859
                try
                {
                    using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                        .CreateScope())
                    {
                        serviceScope.ServiceProvider.GetService<ApplicationDbContext>()
                             .Database.Migrate();
                    }
                }
                catch { }
            }

            app.UseIISPlatformHandler(options => options.AuthenticationDescriptions.Clear());

            app.UseStaticFiles();
            app.UseSession();

            app.UseIdentity();

            // To configure external authentication please see http://go.microsoft.com/fwlink/?LinkID=532715

            // Add and configure the options for authentication middleware to the request pipeline.
            // You can add options for middleware as shown below.
            // For more information see http://go.microsoft.com/fwlink/?LinkID=532715
            app.UseFacebookAuthentication(options =>
            {
                options.AppId = Configuration["Authentication:Facebook:AppId"];
                options.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
                // TODO: remove e-mail workaround. https://github.com/aspnet/Security/issues/620#issuecomment-165464501
                options.UserInformationEndpoint = "https://graph.facebook.com/me?fields=email,name";
                options.Scope.Add("email");
            });
            app.UseGoogleAuthentication(options =>
            {
                options.ClientId = Configuration["Authentication:Google:ClientId"];
                options.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
            });
            //app.UseMicrosoftAccountAuthentication(options =>
            //{
            //    options.ClientId = Configuration["Authentication:MicrosoftAccount:ClientId"];
            //    options.ClientSecret = Configuration["Authentication:MicrosoftAccount:ClientSecret"];
            //});
            app.UseTwitterAuthentication(options =>
            {
                options.ConsumerKey = Configuration["Authentication:Twitter:ConsumerKey"];
                options.ConsumerSecret = Configuration["Authentication:Twitter:ConsumerSecret"];
                // TODO: https://github.com/aspnet/Security/issues/765
                options.Events = new TwitterEvents()
                {
                    OnCreatingTicket = async context =>
                    {
                        var nonce = Guid.NewGuid().ToString("N");

                        var authorizationParts = new SortedDictionary<string, string>
                        {
                            { "oauth_consumer_key", context.Options.ConsumerKey },
                            { "oauth_nonce", nonce },
                            { "oauth_signature_method", "HMAC-SHA1" },
                            { "oauth_timestamp", GenerateTimeStamp() },
                            { "oauth_token", context.AccessToken },
                            { "oauth_version", "1.0" }
                        };

                        var parameterBuilder = new StringBuilder();
                        foreach (var authorizationKey in authorizationParts)
                        {
                            parameterBuilder.AppendFormat("{0}={1}&", UrlEncoder.Default.UrlEncode(authorizationKey.Key), UrlEncoder.Default.UrlEncode(authorizationKey.Value));
                        }
                        parameterBuilder.Length--;
                        var parameterString = parameterBuilder.ToString();

                        var resource_url = "https://api.twitter.com/1.1/account/verify_credentials.json";
                        var resource_query = "include_email=true";
                        var canonicalizedRequestBuilder = new StringBuilder();
                        canonicalizedRequestBuilder.Append(HttpMethod.Get.Method);
                        canonicalizedRequestBuilder.Append("&");
                        canonicalizedRequestBuilder.Append(UrlEncoder.Default.UrlEncode(resource_url));
                        canonicalizedRequestBuilder.Append("&");
                        canonicalizedRequestBuilder.Append(UrlEncoder.Default.UrlEncode(resource_query));
                        canonicalizedRequestBuilder.Append("%26");
                        canonicalizedRequestBuilder.Append(UrlEncoder.Default.UrlEncode(parameterString));

                        var signature = ComputeSignature(context.Options.ConsumerSecret, context.AccessTokenSecret, canonicalizedRequestBuilder.ToString());
                        authorizationParts.Add("oauth_signature", signature);

                        var authorizationHeaderBuilder = new StringBuilder();
                        authorizationHeaderBuilder.Append("OAuth ");
                        foreach (var authorizationPart in authorizationParts)
                        {
                            authorizationHeaderBuilder.AppendFormat(
                                "{0}=\"{1}\", ", authorizationPart.Key, UrlEncoder.Default.UrlEncode(authorizationPart.Value));
                        }
                        authorizationHeaderBuilder.Length = authorizationHeaderBuilder.Length - 2;

                        var request = new HttpRequestMessage(HttpMethod.Get, resource_url + "?include_email=true");
                        request.Headers.Add("Authorization", authorizationHeaderBuilder.ToString());

                        var response = await Backchannel.SendAsync(request, context.HttpContext.RequestAborted);
                        response.EnsureSuccessStatusCode();
                        string responseText = await response.Content.ReadAsStringAsync();

                        var result = JObject.Parse(responseText);

                        var email = result.Value<string>("email");
                        var identity = (ClaimsIdentity)context.Principal.Identity;
                        if (!string.IsNullOrEmpty(email))
                        {
                            identity.AddClaim(new Claim(ClaimTypes.Email, email, ClaimValueTypes.String, "Twitter"));
                        }
                    },
                };
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private static string GenerateTimeStamp()
        {
            var secondsSinceUnixEpocStart = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64(secondsSinceUnixEpocStart.TotalSeconds).ToString(CultureInfo.InvariantCulture);
        }

        private string ComputeSignature(string consumerSecret, string tokenSecret, string signatureData)
        {
            using (var algorithm = new HMACSHA1())
            {
                algorithm.Key = Encoding.ASCII.GetBytes(
                    string.Format(CultureInfo.InvariantCulture,
                        "{0}&{1}",
                        UrlEncoder.Default.UrlEncode(consumerSecret),
                        string.IsNullOrEmpty(tokenSecret) ? string.Empty : UrlEncoder.Default.UrlEncode(tokenSecret)));
                var hash = algorithm.ComputeHash(Encoding.ASCII.GetBytes(signatureData));
                return Convert.ToBase64String(hash);
            }
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}

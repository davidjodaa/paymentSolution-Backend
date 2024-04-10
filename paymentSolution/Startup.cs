using Serilog;
using Serilog.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using System.Text.Json;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Logging;
using paymentSolution.Implementation;

namespace PartyPal
{
    public class Startup
    {
        private readonly IConfigurationRoot configRoot;
        public Startup(IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration().CreateLogger();
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {


            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    options.CheckConsentNeeded = context => false;
            //    options.MinimumSameSitePolicy = SameSiteMode.Strict;
            //    options.HttpOnly = HttpOnlyPolicy.None;
            //    options.Secure = CookieSecurePolicy.Always;
            //});
            services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigin", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });
            services.AddMemoryCache();
            services.AddScoped<IRepository, Repository>();
            services.AddScoped<IDatabaseCalls, DatabaseCalls>();
            services.AddScoped<IApiImplementation, ApiImplementation>();
            //services.AddMvc(
            //    options =>
            //    {
            //        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            //    });

            services.AddControllers();
                //.addjsonoptions(options =>
                //{
                //    options.jsonserializeroptions.propertynamingpolicy = jsonnamingpolicy.camelcase;
                //    options.jsonserializeroptions.ignorenullvalues = true;
                //    add other customizations as needed
                // });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PaymentSolution", Version = "v1" });
            });

            services.AddAntiforgery(options =>
            {
                options.HeaderName = "__RequestVerificationToken";
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });

            //services.AddSession(options =>
            //{
            //    options.IdleTimeout = TimeSpan.FromMinutes(15);
            //    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GenericAPI v1"));

            app.UseRouting();
            app.UseAuthorization();
            app.UseCors("AllowOrigin");

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Frame-Options", "DENY");
                context.Response.Headers.Add("Content-Security-Policy", "frame-ancestors 'self'");
                await next();
            });
            //app.UseHttpsRedirection();
            //app.UseDefaultFiles();
            //app.UseStaticFiles();
            //app.UseCookiePolicy();
            //loggerFactory.AddFile("Logs/mylog-{Date}.txt");
            ////app.UseAuthentication();
            //app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
       
        
    }
}

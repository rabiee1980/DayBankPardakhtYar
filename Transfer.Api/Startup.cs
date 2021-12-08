using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Transfer.Core.Extenstions;
using Transfer.Core.Model.Base;
using Newtonsoft.Json;
using Transfer.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Transfer.Data.Base;
using Transfer.Services.Base;
using Transfer.RemoteServices.Base;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Transfer.Api
{
    public class Startup
    {
        private readonly ILogger _logger;
        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            _logger = logger;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //var xz = SecurityHelpers.DecryptString(Configuration["ConnectionString:DB"], "NarvinPay");
            //services.AddDbContext<NarvinPay_DBContext>(options => options.UseSqlServer(xz));
            services.AddDbContext<TSS_DBContext>(options => options.UseSqlServer(Configuration["ConnectionString:DB"]));
            services.AddControllers();

            services.AddMvc(option => option.EnableEndpointRouting = false)
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.TokenSecret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });


            //services.Configure<ApiBehaviorOptions>(options =>
            //{
            //    options.InvalidModelStateResponseFactory = ctx => new ValidationProblemDetailsResult();
            //});

            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("BankPardakht", new Info { Title = "BankPardakht", Version = "v1" });
            //    c.AddSecurityDefinition("Bearer", new ApiKeyScheme { In = "header", Description = "Please enter JWT with Bearer into field", Name = "Authorization", Type = "apiKey" });
            //    c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
            //    {
            //        { "Bearer", Enumerable.Empty<string>() },
            //    });
            //    //c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            //});

            services.AddHttpContextAccessor();
            services.AddScoped<IRemoteServiceWrapper, RemoteServiceWrapper>();
            services.AddScoped<IServiceWrapper, ServiceWrapper>();
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.ConfigureExceptionHandler(logger);


            app.UseRouting();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            //app.UseSwagger(c =>
            //{
            //    c.PreSerializeFilters.Add((swaggerDoc, httpReq) => swaggerDoc.Host = httpReq.Host.Value);
            //});
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/NarvinPay/swagger.json", "NarvinPay V1");
            //});

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

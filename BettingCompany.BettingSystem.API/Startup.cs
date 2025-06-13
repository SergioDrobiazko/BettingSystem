using BettingCompany.BettingSystem.API;
using BettingCompany.BettingSystem.Application;
using BettingCompany.BettingSystem.Application.Contract;
using BettingCompany.BettingSystem.Domain;
using BettingCompany.BettingSystem.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using System;
using System.Text;

namespace BettingCompany.BettingSystem
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
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.IncludeErrorDetails = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "TestIssuer",
                    ValidAudience = "TestAudience",
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes("mysupersecretkey_for_dev_only_12345"))
                };
            });

            services.AddSingleton<IBetHandlingService, BetHandlingService>();
            services.AddSingleton<IBetAgregator, BetAgregator>();
            services.AddSingleton<IWorkersDirector, WorkersDirector>(x => new WorkersDirector(100, new WorkersFactory()));
            services.AddSingleton<IPersistancePolicy, PersistancePolicy>();
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            services.AddSingleton<IBetRepository, BetRepository>();

            services.AddScoped<IBetSummaryService, BetSummaryService>();

            services.AddScoped<IShutdownSystemService, ShutdownSystemService>();

            services.Configure<MongoSettings>(
                Configuration.GetSection("MongoDb"));

            services.AddSingleton<IMongoClient>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<MongoSettings>>().Value;
                return new MongoClient(settings.ConnectionString);
            });

            services.AddSingleton(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<MongoSettings>>().Value;
                var client = sp.GetRequiredService<IMongoClient>();
                return client.GetDatabase(settings.DatabaseName);
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BettingCompany.BettingSystem", Version = "v1" });

                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Description = "Enter JWT token in format: {your token}",

                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecurityScheme, Array.Empty<string>() }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BettingCompany.BettingSystem v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

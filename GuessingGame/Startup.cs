using FluentValidation;
using FluentValidation.AspNetCore;
using GuessingGame.Data;
using GuessingGame.Domain;
using GuessingGame.Factories;
using GuessingGame.Models;
using GuessingGame.Services;
using GuessingGame.Validators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace GuessingGame
{
    public class Startup
    {
        private SqliteConnection _connection;
        
        private DbContextOptions<GGDbContext> _options;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllersWithViews()
                .AddFluentValidation();

            services
                .AddAuthentication()
                    .AddCookie(AuthenticationDefaults.AuthenticationScheme,
                    options =>
                    {
                        options.LoginPath = "/Home/Index/";
                        options.AccessDeniedPath = "/Home/Error/";
                        options.Cookie.HttpOnly = true;
                        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                    });

            // we need this field to ensure the SQLite database will be available until app shutdown
            _connection =
                new SqliteConnection("Data Source=GGDSInMemory;Mode=Memory;Cache=Shared");

            _connection.Open();
            
            _options = new DbContextOptionsBuilder<GGDbContext>()
                .UseSqlite(_connection)
                .Options;

            using GGDbContext ggDbContext = new(_options);

            ggDbContext.Database.EnsureCreated();

            services
                .AddHttpContextAccessor()
                .AddDbContext<GGDbContext>(
                    contextOptions => contextOptions.UseSqlite(_connection))
                .AddTransient<IValidator<LoginModel>, LoginValidator>()
                .AddTransient<IValidator<GuessModel>, GuessValidator>()
                .AddTransient<IValidator<GameModel>, GameValidator>()
                .AddScoped(typeof(IRepository<>), typeof(EFRepository<>))
                .AddScoped<IRepository<Player>, EFRepository<Player>>()
                .AddScoped<IRepository<Game>, EFRepository<Game>>()
                .AddScoped<IRepository<Guess>, EFRepository<Guess>>()
                .AddScoped<IAuthenticationService, CookieAuthenticationService>()
                .AddScoped<IPlayerService, PlayerService>()
                .AddScoped<IGameService, GameService>()
                .AddScoped<ILeadersModelFactory, LeadersModelFactory>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app, 
            IWebHostEnvironment env, 
            IHostApplicationLifetime applicationLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();

                app.Use(async (context, next) =>
                {
                    await next();
                    
                    // some sample code here to hadle 404 with custom pages, etc.
                    if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
                    {
                        //Re-execute the request so the user gets the error page
                        context.Response.Redirect("/Home/Error");
                        
                        //await next();
                    }
                });
            }
            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Login}/{id?}");
            });

            applicationLifetime.ApplicationStopping.Register(OnStopping);
        }

        private void OnStopping()
        {
            _connection.Close();

            _connection.Dispose();
        }
    }
}

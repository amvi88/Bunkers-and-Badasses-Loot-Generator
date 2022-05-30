using Business.Factories;
using Business.Models.Config;
using Microsoft.AspNetCore.Identity;

namespace Api
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
            services.AddControllers()
            .AddJsonOptions(opts =>
            {
                var enumConverter = new System.Text.Json.Serialization.JsonStringEnumConverter();
                opts.JsonSerializerOptions.Converters.Add(enumConverter);
            });
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddControllers();

            services.Configure<GuildConfigurationOptions>(Configuration.GetSection("GuildConfiguration"));
            services.Configure<WeaponCustomizationOptions>(Configuration.GetSection("WeaponCustomization"));
            services.Configure<WeaponArchetypesOptions>(Configuration.GetSection("WeaponArchetypes"));
            services.AddTransient<IItemFactory<Business.Models.Grenade>, GrenadeFactory>();
            services.AddTransient<IItemFactory<Business.Models.Shield>, ShieldFactory>();
            services.AddTransient<IGunFactory, GunFactory>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!env.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            else
            {
                app.UseSwagger();
                app.UseSwaggerUI();
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
using Hydra.Admin.DataAccess;
using Hydra.Admin.EventBus;
using Hydra.Admin.IServices;
using Hydra.Admin.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;

namespace Hydra.Admin.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(env.ContentRootPath)
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
               .AddEnvironmentVariables();
            Configuration = builder.Build();
        }
        //This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            TranContainer.Build();
            TranContainer.Start();
            //跨域设置
            services.AddCors(options =>
            {
                options.AddPolicy("hydra.admin", builder =>
                {
                    builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();//指定处理cookie
                });
            });
            //(DI)注入使用的类
            services.AddScoped(typeof(IAdminService), typeof(AdminService));
            services.AddScoped(typeof(IMenuService), typeof(MenuService));
            services.AddScoped(typeof(IRoleService), typeof(RoleService));
            services.AddScoped(typeof(IConfigService), typeof(ConfigService));
            services.AddScoped(typeof(ILogsService), typeof(LogsService));
            services.AddScoped(typeof(IAnalysisDashBoardService), typeof(AnalysisDashBoardService));
            services.AddScoped(typeof(IAnalysisRemainService), typeof(AnalysisRemainService));
            services.AddScoped(typeof(IAnalysisGameProfitService), typeof(AnalysisGameProfitService));
            services.AddScoped(typeof(IplayerGoldService), typeof(playerGoldService));
            services.AddScoped(typeof(IplayerBetsService), typeof(playerBetsService));
            services.AddScoped(typeof(IplayerOnlineService), typeof(playerOnlineService));
            //AutoMapper 注册
            AutoMapperConfiguration.Register();
            DbConfig.MasterDB = Configuration["AppSettings:MasterDB"];
            DbConfig.GameAPI = Configuration["AppSettings:GameAPI"];
            services.AddMvc(cfg =>
            {
                cfg.Filters.Add(new AuthorizationFilter());
            });
        }
        //This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            env.EnvironmentName = "env";
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            loggerFactory.AddConsole();
            loggerFactory.ConfigureNLog("Configs/nlog.config");
            loggerFactory.AddNLog();
            var options = new DefaultFilesOptions();
            options.DefaultFileNames.Clear();
            options.DefaultFileNames.Add("index.html");
            app.UseDefaultFiles(options);
            app.UseStaticFiles();
            app.UseMvc();
            app.AddNLogWeb();
        }
    }
}

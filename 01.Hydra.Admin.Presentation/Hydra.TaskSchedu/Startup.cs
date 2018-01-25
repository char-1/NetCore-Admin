using Hangfire;
using Hangfire.MySql;
using Hydra.Admin.DataAccess;
using Hydra.Admin.Jobs;
using Hydra.Admin.Utility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;

namespace Hydra.Admin.UI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(env.ContentRootPath)
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .AddEnvironmentVariables();
            Configuration = builder.Build();
        }
        public void ConfigureServices(IServiceCollection services)
        {
            DbConfig.MasterDB = Configuration["AppSettings:MasterDB"];
            DbConfig.GameAPI = Configuration["AppSettings:GameAPI"];
            services.AddHangfire(x => x.UseStorage(new MySqlStorage(DbConfig.MasterDB, new MySqlStorageOptions
            {
                QueuePollInterval = TimeSpan.FromSeconds(15),
                JobExpirationCheckInterval = TimeSpan.FromHours(1),
                CountersAggregateInterval = TimeSpan.FromMinutes(5),
                PrepareSchemaIfNecessary = true,
                DashboardJobListLimit = 50000,
                TransactionTimeout = TimeSpan.FromMinutes(1),
            })));
        }
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            loggerFactory.ConfigureNLog("Configs/nlog.config");
            loggerFactory.AddNLog();
            app.UseHangfireDashboard("/task-schedu", new DashboardOptions()
            {
                Authorization = new[] { new CustomAuthorizeFilter() }
            });
            app.UseHangfireServer();

            #region 添加 Hangfire 调度任务
            RecurringJob.AddOrUpdate(JobKeys.AnalyOnlinePlayer, () => AnalyOnlinePlayerJob.Execued(), "*/1 * * * *", TimeZoneInfo.Local);
            #endregion
        }
    }
}

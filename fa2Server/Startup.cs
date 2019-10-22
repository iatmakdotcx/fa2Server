using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using fa2Server.Controllers;
using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;
using Quartz.Impl;

namespace fa2Server
{
    public class Startup
    {
        public static ILoggerRepository Repository { get; set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Repository = LogManager.CreateRepository(Configuration["Logging:Log4Net:Name"]);
            //指定配置文件，如果这里你遇到问题，应该是使用了InProcess模式，请查看.csproj,并删之
            XmlConfigurator.Configure(Repository, new FileInfo("log4net.config"));
            CreateJobAsync();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            MakC.Data.DbContext.Init(Configuration["DbCfg:ConnectionString"], Configuration["DbCfg:DbType"]);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseMiddleware<dataHashFilter>();

            //app.UseHttpsRedirection();
            app.UseMvc();
        }
        private async static void CreateJobAsync()
        {
            var props = new NameValueCollection
            {
                { "quartz.serializer.type", "binary" }
            };
            var factory = new StdSchedulerFactory(props);
            var sched = await factory.GetScheduler();
            await sched.Start();
            var job = JobBuilder.Create<MyJob>()
                .WithIdentity("myJob", "group1")
                .Build();
            var trigger = TriggerBuilder.Create()
                .WithIdentity("myTrigger", "group1")
                .StartNow()
                .WithCronSchedule("0 1 0 * * ? *")
            .Build();
            await sched.ScheduleJob(job, trigger);
        }
    }
    public class MyJob : IJob
    {
        private ILog log = LogManager.GetLogger(Startup.Repository.Name, "MyJob");
        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() =>
            {
                log.Info("new day job");
                GameServerController.NewDay();

                var cud = DateTime.Now;
                if (cud.DayOfWeek == DayOfWeek.Monday || cud.DayOfWeek == DayOfWeek.Thursday)
                {
                    //星期1，4报名
                    GameServerController.ctl_mj_0(1);
                }
                else if (cud.DayOfWeek == DayOfWeek.Tuesday || cud.DayOfWeek == DayOfWeek.Friday)
                {
                    //星期2，5开战
                    GameServerController.ctl_mj_0(2);
                }
                else if (cud.DayOfWeek == DayOfWeek.Wednesday || cud.DayOfWeek == DayOfWeek.Saturday)
                {
                    //星期3，6结算
                    GameServerController.ctl_mj_0(3);
                }
                else
                {
                    //周末关闭
                    GameServerController.ctl_mj_0(0);
                }



            });
        }
    }
}

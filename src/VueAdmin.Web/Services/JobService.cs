using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using VueAdmin.Data;
using VueAdmin.Helper;
using VueAdmin.Repository;
using VueAdmin.Web.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Serilog;
using System.Collections.Generic;
using Nito.AsyncEx;
using VueAdmin.Helper.SM4;

namespace VueAdmin.Web.Services
{
    /// <summary>
    /// 物流发货服务
    /// </summary>
    public class JobService : BackgroundService
    {
        private IMapper _mapper;
        private readonly IConfiguration _config;


        private readonly AsyncLock _mutex = new AsyncLock();

        public JobService(
            IMapper mapper,
            IConfiguration config
            )
        {
            _mapper = mapper;
            _config = config;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //异步锁            
                using (await _mutex.LockAsync())
                {
                    var times = Convert.ToInt32(_config["BackgroundService:CallbackTime"]); //回调的时间间隔(秒)
                    await Task.Delay(TimeSpan.FromSeconds(times), stoppingToken);
                    Console.WriteLine("回调服务 > " + DateTime.Now);

                }
            }
        }


    }
}

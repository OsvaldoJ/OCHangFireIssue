using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrchardCore.Users.Services;

namespace OCHangFire.Jobs
{
    public class MyBackgroundJob
    {
        // The job instance can't be initialized due to the DJ
        // 
        //System.InvalidOperationException
        //    Unable to resolve service for type 'OrchardCore.Users.Services.IUserService' while attempting to activate 'OCHangFire.Jobs.MyBackgroundJob'.
        //
        //System.InvalidOperationException: Unable to resolve service for type 'OrchardCore.Users.Services.IUserService' while attempting to activate 'OCHangFire.Jobs.MyBackgroundJob'.
        //at Microsoft.Extensions.DependencyInjection.ActivatorUtilities.ConstructorMatcher.CreateInstance(IServiceProvider provider)
        //at Microsoft.Extensions.DependencyInjection.ActivatorUtilities.CreateInstance(IServiceProvider provider, Type instanceType, Object[] parameters)
        //at Microsoft.Extensions.DependencyInjection.ActivatorUtilities.GetServiceOrCreateInstance(IServiceProvider provider, Type type)
        //at Hangfire.AspNetCore.AspNetCoreJobActivatorScope.Resolve(Type type)
        //at Hangfire.Server.CoreBackgroundJobPerformer.Perform(PerformContext context)
        //at Hangfire.Server.BackgroundJobPerformer.<>c__DisplayClass9_0.<PerformJobWithFilters>b__0()
        //at Hangfire.Server.BackgroundJobPerformer.InvokePerformFilter(IServerFilter filter, PerformingContext preContext, Func`1 continuation)
        //at Hangfire.Server.BackgroundJobPerformer.<>c__DisplayClass9_1.<PerformJobWithFilters>b__2()
        //at Hangfire.Server.BackgroundJobPerformer.PerformJobWithFilters(PerformContext context, IEnumerable`1 filters)
        //at Hangfire.Server.BackgroundJobPerformer.Perform(PerformContext context)
        //at Hangfire.Server.Worker.PerformJob(BackgroundProcessContext context, IStorageConnection connection, String jobId)

        private readonly IUserService _userService;

        public MyBackgroundJob(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<DateTimeOffset> ExecuteAsync(DateTimeOffset request)
        {
            await _userService.GetUserAsync("admin");
            return request;
        }
    }
}

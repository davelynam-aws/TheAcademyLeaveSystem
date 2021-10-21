using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using The_Academy_Leave_System.Areas.Identity.Data;

[assembly: HostingStartup(typeof(The_Academy_Leave_System.Areas.Identity.IdentityHostingStartup))]
namespace The_Academy_Leave_System.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<TALSIdentityContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("TALSIdentityContextConnection")));

                services.AddDefaultIdentity<TALSIdentity>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<TALSIdentityContext>();
            });
        }
    }
}

using Core.Utilities.MailHelper;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DependencyResolvers
{
    public static class ServiceRegistration
    {
        public static void Run(this IServiceCollection service)
        {
            service.AddScoped<AppDbContext>();

          
        }
    }
}

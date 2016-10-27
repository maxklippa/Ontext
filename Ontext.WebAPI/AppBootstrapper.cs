using System.Data.Entity;
using Ontext.DAL.Context;

namespace Ontext.WebAPI
{
    public class AppBootstrapper
    {
        public static void Init()
        {
            // DB context init
            Database.SetInitializer(new MercuryDbContextInitializer());
            
            // Automapper
            AutoMapperConfiguration.Configure();
            AutoMapper.Mapper.AssertConfigurationIsValid();
        }
    }
}
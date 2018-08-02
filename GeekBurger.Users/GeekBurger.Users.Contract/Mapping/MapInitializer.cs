using AutoMapper;
using System.Linq;
using System.Reflection;

namespace GeekBurger.Users.Contract.Mapping
{
    public class MapInitializer
    {
        public static void Init()
        {
            var all = Assembly
                          .GetEntryAssembly()
                          .GetReferencedAssemblies()
                          .Select(Assembly.Load)
                          .SelectMany(x => x.DefinedTypes)
                          .Where(type => typeof(Profile).GetTypeInfo().IsAssignableFrom(type.AsType()))
                          .Select(x => x.AsType());

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfiles(all); // Initialise each Profile classe
            });
        }
    }
}

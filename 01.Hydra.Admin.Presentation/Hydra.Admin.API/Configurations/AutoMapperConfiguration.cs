using AutoMapper;
using Hydra.Admin.Models.Model;
using Hydra.Admin.Models.View;
namespace Hydra.Admin.API
{
    public class AutoMapperConfiguration
    {
        /// <summary>
        /// AutoMapper 注册
        /// </summary>
        public static void Register()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Admins, AdminLoginView>();
                cfg.CreateMap<Config, ConfigView>();
            });
        }
    }
}

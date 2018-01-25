using Hydra.Admin.Models.Model;
using Hydra.Admin.Models.Query;
using Hydra.Admin.Models.View;
using Hydra.Admin.Utility.iViewControl;
using System.Collections.Generic;

namespace Hydra.Admin.IServices
{
    public interface IAdminService : IBaseService<Admins>
    {
        /// <summary>
        /// 管理员登录
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        AdminLoginView Login(AdminLoginQuery query);
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        bool ModifyPass(AdminChangePassQuery query);
        /// <summary>
        /// 获取权限菜单
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        List<PermissionView> GetAdminPermission(string adminId);
        /// <summary>
        /// 获取管理员列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IViewTable<Admins> GetAdminGrid(BaseQuery query);
    }
}

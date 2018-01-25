using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Admin.Models.Query
{
    public class SetRolePermissionQuery
    {
        public string roleId { get; set; }
        public string permissionNodes { get; set; }
    }
}

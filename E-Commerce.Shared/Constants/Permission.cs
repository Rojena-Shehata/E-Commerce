using E_Commerce.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Shared.Constants
{
    public static partial class Permission
    {
        public const string PermissionType = "Permission";
        public static List<string> GeneratePermissionList(string module)
        {
            return new List<string>()
            {
                $"{PermissionType}.{module}.View",
                $"{PermissionType}.{module}.Create",
                $"{PermissionType}.{module}.Edit",
                $"{PermissionType}.{module}.Delete"
            };
        }


        public static List<string> GenerateAllPermissions()
        {
            var allPermissions= new List<string>();
            var allModules = Enum.GetNames(typeof(Modules));
            if(allModules is not null)
            {
                foreach(var module in allModules) 
                    allPermissions.AddRange(GeneratePermissionList(module));

            }
            return allPermissions;
        }
  
       
    }
}

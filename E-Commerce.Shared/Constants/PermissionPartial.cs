using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Shared.Constants
{
    public static partial  class Permission
    {
        public static class Roles
        {
            public const string View = $"{PermissionType}.Roles.View";
            public const string Create = $"{PermissionType}.Roles.Create";
            public const string Edit = $"{PermissionType}.Roles.Edit";
            public const string Delete = $"{PermissionType}.Roles.Delete";
        }
        public static class Users
        {
            public const string View = $"{PermissionType}.Users.View";
            public const string Create = $"{PermissionType}.Users.Create";
            public const string Edit = $"{PermissionType}.Users.Edit";
            public const string Delete = $"{PermissionType}.Users.Delete";
        }
        public static class Products
        {
            public const string View = $"{PermissionType}.Products.View";
            public const string Create = $"{PermissionType}.Products.Create";
            public const string Edit = $"{PermissionType}.Products.Edit";
            public const string Delete = $"{PermissionType}.Products.Delete";
        }
        public static class Brands
        {
            public const string View = $"{PermissionType}.Brands.View";
            public const string Create = $"{PermissionType}.Brands.Create";
            public const string Edit = $"{PermissionType}.Brands.Edit";
            public const string Delete = $"{PermissionType}.Brands.Delete";
        }
        public static class Types
        {
            public const string View = $"{PermissionType}.Types.View";
            public const string Create = $"{PermissionType}.Types.Create";
            public const string Edit = $"{PermissionType}.Types.Edit";
            public const string Delete = $"{PermissionType}.Types.Delete";
        }
        public static class Orders
        {
            public const string View = $"{PermissionType}.Orders.View";
            public const string Create = $"{PermissionType}.Orders.Create";
            public const string Edit = $"{PermissionType}.Orders.Edit";
            public const string Delete = $"{PermissionType}.Orders.Delete";
        }
    }
}

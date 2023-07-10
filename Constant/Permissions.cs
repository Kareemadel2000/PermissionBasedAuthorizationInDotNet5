using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PermissionBasedAuthorizationInDotNet5.Constant
{
    public static class Permissions
    {
        public static List<string> GeneratePermissionsList(string model)
        {
            return new List<string>()
            {
                $"Permissions.{model}.View",
                $"Permissions.{model}.Create",
                $"Permissions.{model}.Edit",
                $"Permissions.{model}.Delete",
            };
        }

        public static List<string> GenerateAllPermissions()
        {
            var allPermission = new List<string>();
            var models = Enum.GetValues(typeof(Models));
            foreach (var model in models)
                allPermission.AddRange(GeneratePermissionsList(model.ToString()));

            return allPermission;
        }
    }
}

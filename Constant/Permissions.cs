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
    }
}

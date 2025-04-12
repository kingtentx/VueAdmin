namespace VueAdmin.Api.Permissions
{
    public static class AuthorizeCode
    {
        public static class Role
        {
            public const string Default = "role";
            public const string Add = Default + ":add";
            public const string Edit = Default + ":edit";
            public const string Delete = Default + ":delete";
            public const string Authorize = Default + ":authorize";//授权菜单权限
        }
        public static class User
        {
            public const string Default = "user";
            public const string Add = Default + ":add";
            public const string Edit = Default + ":edit";
            public const string Delete = Default + ":delete";
            public const string Authorize = Default + ":authorize";//授权角色权限
        }

        public static class Menu
        {
            public const string Default = "menu";
            public const string Add = Default + ":add";
            public const string Edit = Default + ":edit";
            public const string Delete = Default + ":delete";           
        }

        public static class Department
        {
            public const string Default = "department";
            public const string Add = Default + ":add";
            public const string Edit = Default + ":edit";
            public const string Delete = Default + ":delete";
        }       

    }

   
}

namespace Framework.Infrastructure
{
    public static class Roles
    {
        public const string Admin = "1";
        public const string SystemUser = "2";

        public static string GetRoleBy(long id)
        {
            switch (id)
            {
                case 1:
                    return "مدیر سیستم";
                case 2:
                    return "گاربر سیستم";
                default:
                    return "";
            }
        }
    }
}

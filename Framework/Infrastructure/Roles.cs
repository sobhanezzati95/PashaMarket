namespace Framework.Infrastructure;
public static class Roles
{
    public const string Admin = "1";
    public const string SystemUser = "2";
    public static string GetRoleBy(long id)
    {
        return id switch
        {
            1 => "مدیر سیستم",
            2 => "گاربر سیستم",
            _ => "",
        };
    }
}
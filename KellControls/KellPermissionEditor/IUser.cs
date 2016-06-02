using System;
namespace KellControls
{
    public interface IUser
    {
        DateTime CreateTime { get; set; }
        string DepartIDs { get; set; }
        string Description { get; set; }
        System.Collections.Generic.List<IPermission> GetAllPermissions { get; }
        int ID { get; set; }
        string LoginName { get; set; }
        DateTime LoginTime { get; set; }
        bool Online { get; set; }
        string Password { get; set; }
        IRole Role { get; set; }
        string ToString();
        string UserName { get; set; }
    }
}

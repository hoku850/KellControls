using System;
namespace KellControls
{
    public interface IRole
    {
        string Description { get; set; }
        int ID { get; set; }
        string Name { get; set; }
        IPermissionCollection Permissions { get; set; }
        string ToString();
    }
}

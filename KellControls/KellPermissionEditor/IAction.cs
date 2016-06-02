using System;
namespace KellControls
{
    public interface IAction
    {
        int ID { get; set; }
        IModule Module { get; }
        System.Collections.Generic.Dictionary<IPermissionType, bool> Permissions { get; set; }
        string ToString();
        System.Collections.Generic.List<IPermissionType> ValidPermissions { get; }
    }
}

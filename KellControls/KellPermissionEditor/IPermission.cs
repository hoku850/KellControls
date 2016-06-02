using System;
namespace KellControls
{
    public interface IPermission
    {
        IAction Action { get; set; }
        string PermissionExtensionName { get; set; }
        string Description { get; set; }
        int ID { get; set; }
        string Name { get; set; }
        string ToString();
        bool Save(params string[] filename);
        bool Load(params string[] filename);
    }
}

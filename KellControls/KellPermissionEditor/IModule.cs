using System;
namespace KellControls
{
    public interface IModule
    {
        string Description { get; set; }
        int ID { get; set; }
        string Name { get; set; }
    }
}

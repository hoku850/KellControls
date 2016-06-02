using System;
namespace KellControls
{
    public interface IPermissionCollection
    {
        void Add(IPermission item);
        int Count { get; }
        System.Collections.IEnumerator GetEnumerator();
        void Remove(IPermission item);
        void RemoveAt(int index);
        IPermission this[int index] { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KellControls;

namespace Test
{
    [Serializable]
    class Module : IModule
    {
        int id;

        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        string description;

        public string Description
        {
            get { return description; }
            set { description = value; }
        }
    }
}

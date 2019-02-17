using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class ModuleSettings
    {
        public string Endpoint { get; set; }

        public string Name { get; set; }

        public string RootPath { get { return AppDomain.CurrentDomain.BaseDirectory; } }
    }
}

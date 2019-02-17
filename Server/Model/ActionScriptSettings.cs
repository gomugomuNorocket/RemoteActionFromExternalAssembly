using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model
{

    public class Action
    {
        public string Method { get; set; }

        public List<string> Params { get; set; } = new List<string>();
    }

    public class ActionScriptSettings
    {
        public string Path { get; set; }

        public string Namespace { get; set; }

        public int DelayBeforeAction { get; set; }

        public List<Action> Actions { get; set; }
    }
}

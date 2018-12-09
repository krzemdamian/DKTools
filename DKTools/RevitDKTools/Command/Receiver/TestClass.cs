using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace RevitDKTools.Command.Receiver
{
    public class TestClass : DynamicCommandBase
    {
        public TestClass()
        {
            _scriptPath = "test_class";
        }
    }
}

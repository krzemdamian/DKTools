using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace RevitDKTools.Command.Receiver
{
    static class DynamicClassEmiter
    {
        static void Create()
        {
            //create assembly
            AssemblyName an = new AssemblyName();
            an.Name = "DynamicAssembly";
            AppDomain ad = AppDomain.CurrentDomain;
            AssemblyBuilder ab = ad.DefineDynamicAssembly(an,
                AssemblyBuilderAccess.Save);

            //create module
            ModuleBuilder mb = ab.DefineDynamicModule(an.Name, an.Name + ".dll");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace RevitDKTools.Command.Receiver
{
    class DynamicCommandClassEmiter
    {
        readonly AppDomain _appDomain;
        readonly AssemblyName _asseblyName;
        AssemblyBuilder _assemblyBuilder;
        readonly ModuleBuilder _moduleBuilder;

        public DynamicCommandClassEmiter(string dynamicAssemblyName)
        {
            _asseblyName = new AssemblyName
            {
                Name = dynamicAssemblyName
            };
            _appDomain = AppDomain.CurrentDomain;
            _assemblyBuilder = _appDomain.DefineDynamicAssembly(_asseblyName,
                AssemblyBuilderAccess.Run);

            _moduleBuilder = _assemblyBuilder.DefineDynamicModule(_asseblyName.Name);
        }

        public Type BuildCommandType(string commandTypeName, string scriptPath)
        {
            TypeBuilder typeBuilder = _moduleBuilder.DefineType(
                commandTypeName, TypeAttributes.Public | TypeAttributes.Class, typeof(DynamicCommandBase));
            ConstructorBuilder ctorBuilder = typeBuilder.DefineConstructor(
                MethodAttributes.Public|MethodAttributes.HideBySig|MethodAttributes.SpecialName|MethodAttributes.RTSpecialName,
                CallingConventions.Standard, new Type[0]);
            
            ConstructorInfo objCtor = typeof(DynamicCommandBase).GetConstructor(Type.EmptyTypes);

            FieldInfo scriptPathField = typeBuilder.BaseType.GetField("_scriptPath");

            ILGenerator ilg = ctorBuilder.GetILGenerator();

            ilg.Emit(OpCodes.Ldarg_0);
            ilg.Emit(OpCodes.Call, objCtor);
            ilg.Emit(OpCodes.Nop);
            ilg.Emit(OpCodes.Nop);
            ilg.Emit(OpCodes.Ldarg_0);
            ilg.Emit(OpCodes.Ldstr, scriptPath);
            ilg.Emit(OpCodes.Stfld, scriptPathField);
            ilg.Emit(OpCodes.Ret);

            return typeBuilder.CreateType();
        }

    }
}

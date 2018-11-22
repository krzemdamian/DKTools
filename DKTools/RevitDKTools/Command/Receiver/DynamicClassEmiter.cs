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
                MethodAttributes.Public, CallingConventions.Standard, new Type[0]);  //this might be a problem, use null instead of Type.EmptyTypes
            
            //used to get obj default constructor
            Type objType = Type.GetType("System.Object");
            ConstructorInfo objCtor = objType.GetConstructor(new Type[0]);
            //ConstructorInfo objCtor = typeof(DynamicCommandBase).GetConstructor(Type.EmptyTypes);

            //FieldInfo scriptPathField = typeBuilder.GetField("_scriptPath", BindingFlags.NonPublic | BindingFlags.Instance);
            //FieldInfo scriptPathField = typeBuilder.GetRuntimeField("_scriptPath");
            FieldInfo scriptPathField = typeof(DynamicCommandBase).GetField("_scriptPath",BindingFlags.FlattenHierarchy);
            
            ILGenerator ilg = ctorBuilder.GetILGenerator();

            ilg.Emit(OpCodes.Ldarg_0);
            ilg.Emit(OpCodes.Call, objCtor);
            ilg.Emit(OpCodes.Ldarg_0);
            ilg.Emit(OpCodes.Ldstr, scriptPath);
            //ilg.Emit(OpCodes.Ldarg_0);
            ilg.Emit(OpCodes.Stfld, scriptPathField);
            //ilg.Emit(OpCodes.Stelem_Ref, scriptPathField); 
            ilg.Emit(OpCodes.Ret);
            
            return typeBuilder.CreateType();
        }

    }
}

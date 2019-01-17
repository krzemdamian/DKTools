using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using System;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Resources;

namespace RevitDKTools.Commands.Generate
{
    class ClassEmitter<T> : IClassEmitter where T : IExternalCommand
    {
        private AssemblyBuilder _assemblyBuilder;
        private IEmitterSetting _emitterSetting;

        readonly string _location;
        readonly AppDomain _appDomain;
        readonly AssemblyName _asseblyName;
        readonly ModuleBuilder _moduleBuilder;

        public string AssemblyLocation
        {
            get
            {
                return _location + _asseblyName.Name + ".dll";
            }
        }

        public ClassEmitter(IEmitterSetting emitterSetting)
        {
            _emitterSetting = emitterSetting;
            if (!string.IsNullOrEmpty(Assembly.GetExecutingAssembly().Location))
            {
                ResourceManager resourceManager = new ResourceManager(
                    "RevitDKTools.Properties.Resources",
                    Assembly.GetExecutingAssembly());

                _location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
                    resourceManager.GetString("PROXY_DLL_LOCATION");
            }
            else
            {
                _location = Path.GetTempPath();
            }

            _asseblyName = new AssemblyName
            {
                Name = _emitterSetting.DynamicAssemblName
            };
            _appDomain = AppDomain.CurrentDomain;
            _assemblyBuilder = _appDomain.DefineDynamicAssembly(_asseblyName,
                AssemblyBuilderAccess.Save,_location);

            _moduleBuilder = _assemblyBuilder.DefineDynamicModule(_asseblyName.Name, _asseblyName.Name + ".dll");
        }

        public Type BuildCommandType(string commandTypeName, string scriptPath)
        {
            TypeBuilder typeBuilder = _moduleBuilder.DefineType(
                commandTypeName, TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.BeforeFieldInit, 
                typeof(T), new Type[] { typeof(IExternalCommand) });
            
            ConstructorBuilder ctorBuilder = typeBuilder.DefineConstructor(
                MethodAttributes.Public|MethodAttributes.HideBySig|MethodAttributes.SpecialName|MethodAttributes.RTSpecialName,
                CallingConventions.Standard, new Type[0]);

            ConstructorInfo cinfo = typeof(TransactionAttribute).GetConstructor(new Type[] { typeof(TransactionMode) });
            byte[] b = new byte[] { 01, 00, 01, 00, 00, 00, 00, 00 };

            typeBuilder.SetCustomAttribute(cinfo,b);
            ConstructorInfo objCtor = typeof(T).GetConstructor(Type.EmptyTypes);

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

        public void SaveAssembly()
        {
            try
            {
                _assemblyBuilder.Save(_asseblyName.Name + ".dll");

            }
            catch { }
        }
    }
}

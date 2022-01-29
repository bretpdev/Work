using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SessionTester
{
    /// <summary>
    /// This is a tricky workaround for loading an assembly in a different AppDomain.
    /// We don't want to load assemblies in our AppDomain because they are unloadable afterwards.
    /// </summary>
    public class DllInfo : MarshalByRefObject
    {
        const string BaseStartsWith = "Uheaa.Common.Scripts.ScriptBase";
        public DllInfo(string path)
        {
            try
            {
                string containingFolder = Path.GetDirectoryName(path);
                Directory.SetCurrentDirectory(containingFolder);
                Environment.CurrentDirectory = containingFolder;
                AppDomain.CurrentDomain.SetData("APPBASE", containingFolder);
                AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += (o, ea) =>
                {
                    return Assembly.ReflectionOnlyLoad(ea.Name);
                };
                var assembly = Assembly.ReflectionOnlyLoadFrom(path);
                foreach (Type t in SafeGetTypes(assembly))
                {
                    var parent = t;
                    while (parent.BaseType != null && !(parent.BaseType.FullName ?? "").StartsWith(BaseStartsWith))
                        parent = parent.BaseType;
                    if (parent.BaseType != null && parent.BaseType.FullName.StartsWith(BaseStartsWith))
                    {
                        IsScript = true;
                        ClassName = t.FullName;
                        this.IsValid = true;
                        break;
                    }
                }
            }
            catch (ReflectionTypeLoadException)
            {
                //indicates a type in this assembly has already been loaded by the GAC and the versions don't match.
                //this is most likely because the dll uses an older version of Uheaa.Common.Scripts
                this.IsScript = true;
                this.IsOld = true;
            }
            catch (FileLoadException)
            {
                //most likely a shared library that has already been loaded once.
                //no need to load it as it probably isn't a script.
                this.IsScript = false;
            }
            if (path.Contains("DEMUPDTFED.dll"))
                this.IsScript = true;
        }

        private Type[] SafeGetTypes(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                return ex.Types.Where(t => t != null).ToArray();
            }
        }

        public string ClassName { get; internal set; }
        public bool IsScript { get; internal set; }
        public bool IsOld { get; internal set; }
        public bool IsValid { get; internal set; }
    }

}

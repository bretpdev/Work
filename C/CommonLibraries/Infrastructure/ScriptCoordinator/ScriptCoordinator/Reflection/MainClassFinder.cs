using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ScriptCoordinator
{
    public class MainClassFinder : MarshalByRefObject
    {
        public static DllInfo FindMainClass(string scriptDllPath, string className = null)
        {
            var domain = GenerateDomain();
            object[] args = new object[] { scriptDllPath, className };
            var assembly = typeof(MainClassFinder).Assembly.FullName;
            MainClassFinder mcf = (MainClassFinder)domain.CreateInstanceAndUnwrap(assembly, typeof(MainClassFinder).FullName, false, BindingFlags.Default, null, args, null, null);
            var info = mcf.Info;
            AppDomain.Unload(domain);
            return info;
        }
        private static AppDomain GenerateDomain()
        {
            AppDomainSetup setup = new AppDomainSetup();
            AppDomain domain = AppDomain.CreateDomain(Guid.NewGuid().ToString(), null, setup);
            return domain;
        }
        const string CommonBaseStartsWith = "Uheaa.Common.Scripts.ScriptBase";
        const string QBaseStartsWith = "Q.ScriptCommonBase";
        public Assembly Assembly { get; private set; }
        public DllInfo Info { get; private set; }

        public MainClassFinder(string scriptDllPath, string className)
        {
            Info = new DllInfo();
            Info.Path = scriptDllPath;
            string containingFolder = Path.GetDirectoryName(scriptDllPath);
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += (o, ea) =>
            {
                var dependantFile = Path.Combine(containingFolder, ea.Name.Substring(0, ea.Name.IndexOf(',')) + ".dll");
                if (File.Exists(dependantFile))
                    return Assembly.ReflectionOnlyLoadFrom(dependantFile);
                return Assembly.ReflectionOnlyLoad(ea.Name);
            };
            //AppDomain.CurrentDomain.AssemblyResolve += (o, ea) =>
            //{
            //    //sometimes need to resolve the assembly we're loading.
            //    if (assembly != null)
            //        if (assembly.FullName == ea.Name)
            //            return Assembly.LoadFrom(scriptDllPath);
            //    //sometimes need to resolve ScriptSync if run from VBA.
            //    if (ea.Name.StartsWith("Q.")) //they are looking for Q.*, usually Q.resources
            //        return null; //apparently giving them nothing enough times makes them give up
            //    return Assembly.Load(ea.Name);
            //};
            try
            {
                Assembly = Assembly.ReflectionOnlyLoadFrom(scriptDllPath);
                if (FindMainClass(Assembly, CommonBaseStartsWith, className))
                    Info.UsesCommon = true;
                else if (FindMainClass(Assembly, QBaseStartsWith, className))
                    Info.UsesQ = true;
            }
            catch (Exception ex)
            {
                if (ex is FileNotFoundException || ex is FileLoadException)
                {
                    Info.UsesCommon = false;
                    Info.UsesQ = false;
                    Info.MainClassName = null;
                }
                else
                    throw;
            }
        }

        private bool FindMainClass(Assembly assembly, string startsWith, string className)
        {
            Func<Type, bool> match = new Func<Type, bool>(t => t != null && t.FullName != null && t.FullName.StartsWith(startsWith));
            Func<Type, bool> goodNamespace = new Func<Type, bool>(t => !string.IsNullOrEmpty(t.Namespace) && !t.Namespace.StartsWith("Q.") && !t.Namespace.StartsWith("Uheaa.Common."));
            List<Type> matches = new List<Type>();
            try
            {
                foreach (Type t in assembly.GetTypes())
                {
                    if (t.IsAbstract)
                        continue;
                    var parent = t;
                    while (parent.BaseType != null && !match(parent.BaseType))
                        parent = parent.BaseType;
                    if (goodNamespace(t) && match(parent.BaseType))
                        matches.Add(t);
                }
            }
            catch (ReflectionTypeLoadException ex) //meaning these scripts don't currently build.
            {
                return false;
            }
            //remove all non-top-level scripts
            if (string.IsNullOrEmpty(className))
                matches = matches.Where(m => !AnyoneInherits(m, assembly.GetTypes())).ToList();
            Type final = null;
            if (matches.Count == 0)
                return false;
            if (matches.Count == 1)
                final = matches.Single();
            else
            {
                if (className != null)
                    final = matches.SingleOrDefault(o => o.FullName.ToLower() == className.ToLower());
                else
                    //script has multiple classes inheriting from scriptbase, find the one with the shortest namespace
                    final = matches.OrderByDescending(o => InheritanceCount(o)).First();
            }
            Info.MainClassName = final.FullName;
            return true;
        }

        private int InheritanceCount(Type t)
        {
            int count = 0;
            Type parent = t;
            while (parent != null)
            {
                parent = parent.BaseType;
                count++;
            }
            return count;
        }

        private bool AnyoneInherits(Type t, IEnumerable<Type> anyone)
        {
            foreach (Type type in anyone)
                if (!type.Equals(t))
                    if (!type.IsInterface)
                        if (t.IsAssignableFrom(type))
                            return true;
            return false;
        }
    }
}
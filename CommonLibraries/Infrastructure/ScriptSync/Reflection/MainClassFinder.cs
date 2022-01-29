using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Uheaa
{
    public class MainClassFinder : MarshalByRefObject
    {
        public static string FindMainClassName(Script script)
        {
            return FindMainClass(script).MainClassName;
        }
        public static ReflectionResults FindMainClass(Script script)
        {
            AppDomainSetup setup = new AppDomainSetup();
            setup.ApplicationBase = @"C:\Enterprise Program Files\ScriptSync";
            AppDomain domain = AppDomain.CreateDomain(Guid.NewGuid().ToString(), null, setup);
            script.Area.SyncDependencies();
            object[] args = new object[] { script.File.LocalFullPath, script.Area.DependencyLocalRoot };
            MainClassFinder mcf = (MainClassFinder)domain.CreateInstanceAndUnwrap(typeof(MainClassFinder).Assembly.FullName, typeof(MainClassFinder).FullName, false, BindingFlags.Default, null, args, null, null);
            ReflectionResults results = mcf.Results;
            AppDomain.Unload(domain);
            return results;
        }
        const string CommonBaseStartsWith = "Uheaa.Common.Scripts.ScriptBase";
        const string QBaseStartsWith = "Q.ScriptCommonBase";
        public MainClassFinder(string path, string dependencyLocation)
        {
            Results = new ReflectionResults();
            Assembly assembly = null;
            string containingFolder = Path.GetDirectoryName(path);
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += (o, ea) =>
            {
                var dependantFile = Path.Combine(dependencyLocation, ea.Name.Substring(0, ea.Name.IndexOf(',')) + ".dll");
                if (File.Exists(dependantFile))
                    return Assembly.ReflectionOnlyLoadFrom(dependantFile);
                return Assembly.ReflectionOnlyLoad(ea.Name);
            };
            AppDomain.CurrentDomain.AssemblyResolve += (o, ea) =>
            {
                //sometimes need to resolve the assembly we're loading.
                if (assembly != null)
                    if (assembly.FullName == ea.Name)
                        return Assembly.LoadFrom(path);
                //sometimes need to resolve ScriptSync if run from VBA.
                if (ea.Name.StartsWith("Q.")) //they are looking for Q.*, usually Q.resources
                    return null; //apparently giving them nothing enough times makes them give up
                return Assembly.Load(ea.Name);
            };
            try
            {
                assembly = Assembly.ReflectionOnlyLoadFrom(path);
                this.Results.Assembly = assembly;
                if (FindMainClass(assembly, CommonBaseStartsWith))
                    Results.UsesCommon = true;
                else if (FindMainClass(assembly, QBaseStartsWith))
                    Results.UsesQ = true;
            }
            catch (Exception ex)
            {
                if (ex is FileNotFoundException || ex is FileLoadException)
                {
                    Results.UsesCommon = false;
                    Results.UsesQ = false;
                    Results.MainClassName = null;
                }
                else
                    throw;
            }
        }

        private bool FindMainClass(Assembly assembly, string startsWith)
        {
            Func<Type, bool> match = new Func<Type, bool>(t => t != null && t.FullName.StartsWith(startsWith));
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
            catch (ReflectionTypeLoadException) //meaning these scripts don't currently build.
            {
                return false;
            }
            //remove all non-top-level scripts
            matches = matches.Where(m => !AnyoneInherits(m, assembly.GetTypes())).ToList();
            Type final = null;
            if (matches.Count == 0)
                return false;
            if (matches.Count == 1)
                final = matches.Single();
            else
            {
                //script has multiple classes inheriting from scriptbase, find the one with the shortest namespace
                final = matches.OrderByDescending(o => InheritanceCount(o)).First();
            }
            Results.MainClassName = final.FullName;
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
                    if (type.IsAssignableFrom(t))
                        return true;
            return false;
        }

        public ReflectionResults Results { get; set; }
    }
}

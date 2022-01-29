using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uheaa
{
    public interface IInstantiationWrapper<out T>
    {
        T ScriptResult { get; }
        ReflectionResults ReflectionInfo { get; set; }
        Script Script { get; set; }
    }
    public class InstantiationWrapper<T> : IInstantiationWrapper<T>
    {
        public T ScriptResult { get; set; }
        public ReflectionResults ReflectionInfo { get; set; }
        public Script Script { get; set; }
        public InstantiationWrapper(T scriptResult, ReflectionResults reflectionInfo, Script script)
        {
            this.Script = script;
            ScriptResult = scriptResult;
            ReflectionInfo = reflectionInfo;
        }
    }
}

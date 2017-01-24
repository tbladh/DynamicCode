using System;
using System.Linq;
using System.Reflection;

namespace DynamicCode.Extensions
{
    public static class AssemblyExtensions
    {
        
        public static Type FindType(this Assembly assembly, string name)
        {
            return assembly.GetTypes().SingleOrDefault(p => p.Name == name);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using DynamicCode.Extensions;

namespace DynamicCode
{
    public class DynamicAssembly: DynamicObject
    {
        private readonly Assembly _assembly;
        private string _path;
        private readonly Exception _buildException;

        public DynamicAssembly(string code)
        {
            var builder = new AssemblyFactory();
            try
            {
                _assembly = builder.BuildAndLoadAssembly(code);
            }
            catch (Exception ex)
            {
                _buildException = ex;
            }

        }

        public DynamicAssembly(Assembly assembly, string path = null)
        {
            _assembly = assembly;
            _path = path;
        }

        //public override bool TryInvoke(InvokeBinder binder, object[] args, out object result)
        //{
        //    var shortName = GetLastSegment(_path);
        //    var type = _assembly.FindType(shortName) ?? _assembly.GetType(_path.Substring(0, _path.Length - 1));
        //    result = Activator.CreateInstance(type, args);
        //    return result != null;
        //}

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            var path = _path;
            //_path = null;
            if (_buildException != null) throw _buildException;
            result = null;
            var shortName = GetLastSegment(path);
            var type = _assembly.FindType(shortName) ?? _assembly.GetType(path.Substring(0, path.Length - 1));
            if (type == null) return false;
            if (binder.Name == "New")
            {
                result = Activator.CreateInstance(type, args);
                return result != null;
            }

            MethodInfo method;
            try
            {
                method = type.GetMethod(binder.Name, BindingFlags.Public | BindingFlags.Static); // TODO: Ambiguous match when overloaded.
            }
            catch (Exception)
            {
                var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static);
                var key = GetMethodKey(type.Name, binder.Name, args);
                method = methods.FirstOrDefault(p => GetMethodKey(p) == key);
            }
            if (method == null) return false;

            result = method.Invoke(null, args);
            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (_buildException != null) throw _buildException;
            _path += binder.Name + ".";
            result = new DynamicAssembly(_assembly, _path);
            return true;
        }

        private static string GetLastSegment(string path)
        {
            if (string.IsNullOrEmpty(path)) return string.Empty;
            var parts = path.Split(new [] { "." },StringSplitOptions.RemoveEmptyEntries);
            return parts.Last();
        }

        private static string GetMethodKey(string declaringTypeName, string methodName, object[] parameters)
        {
            var sb = new StringBuilder();
            sb.Append(declaringTypeName);
            sb.Append(methodName);
            foreach (var parameter in parameters)
            {
                sb.Append(parameter.GetType().FullName);
            }
            return sb.ToString();
        }

        private static string GetMethodKey(MethodInfo methodInfo)
        {
            if (methodInfo.IsGenericMethod) return null;
            var sb = new StringBuilder();
            sb.Append(methodInfo.DeclaringType?.Name);
            sb.Append(methodInfo.Name);
            sb.Append(GetParametersKey(methodInfo.GetParameters()));
            return sb.ToString();
        }

        private static string GetParametersKey(IEnumerable<ParameterInfo> parameters)
        {
            var sb = new StringBuilder();
            foreach (var parameter in parameters)
            {
                sb.Append(parameter.ParameterType.FullName);
            }
            return sb.ToString();
        }

    }
}

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using DynamicCode.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace DynamicCode
{
    public class AssemblyFactory
    {

        private static readonly Dictionary<ulong, Assembly> Assemblies = new Dictionary<ulong, Assembly>();
        public List<BuildMessage> Messages { get; set; }

        public Assembly BuildAndLoadAssembly(string code)
        {
            Messages = new List<BuildMessage>();

            var assemblyKey = code.Crc64();

            if (Assemblies.ContainsKey(assemblyKey))
            {
                return Assemblies[assemblyKey];
            }

            var syntaxTree = CSharpSyntaxTree.ParseText(code);
            var assemblies = Assembly.GetCallingAssembly().GetReferencedAssemblies().Select(p => p);
            var references = assemblies.Select(item => Assembly.ReflectionOnlyLoad(item.FullName))
                .Select(asm => MetadataReference.CreateFromFile(asm.Location));
            var compilation = CSharpCompilation.Create(
                assemblyKey.ToString("X8"), new[] { syntaxTree }, references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
            Assembly assembly;
            using (var ms = new MemoryStream())
            {
                var result = compilation.Emit(ms);
                if (result.Success)
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    assembly = Assembly.Load(ms.ToArray());
                }
                else
                {
                    Messages = result.Diagnostics.ToBuildMessages().ToList();
                    var errorListing = result.Diagnostics.ToErrorListing();
                    throw new DeferredBuildException($"Errors in source code:\n{errorListing}", Messages);
                }
            }

            Assemblies.Add(assemblyKey, assembly);

            return assembly;
        }

    }
}

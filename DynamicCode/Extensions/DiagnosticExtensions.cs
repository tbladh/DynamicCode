using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;

namespace DynamicCode.Extensions
{
    public static class DiagnosticExtensions
    {
        public static IEnumerable<BuildMessage> ToBuildMessages(this IEnumerable<Diagnostic> diagnostics)
        {
            foreach (var diagnostic in diagnostics)
            {
                var startLine = diagnostic.Location.GetLineSpan().StartLinePosition;
                var message = new BuildMessage
                {
                    Id = diagnostic.Id,
                    Message = diagnostic.GetMessage(),
                    Line = startLine.Line,
                    Character = startLine.Character
                };
                yield return message;
            }
        }

        public static string ToErrorListing(this IEnumerable<Diagnostic> diagnostics)
        {
            var failures = diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error);
            var sb = new StringBuilder();
            foreach (var diagnostic in failures)
            {
                sb.AppendLine($"{diagnostic.Id}: {diagnostic.GetMessage()}");
            }
            return sb.ToString();
        }

    }
}

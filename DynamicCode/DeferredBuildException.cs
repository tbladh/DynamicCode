using System;
using System.Collections.Generic;

namespace DynamicCode
{
    public class DeferredBuildException: Exception
    {

        public List<BuildMessage> BuildMessages { get; }

        public DeferredBuildException()
        {
        }

        public DeferredBuildException(string message) : base(message)
        {
        }

        public DeferredBuildException(string message, Exception inner = null): base(message, inner)
        {
        }

        public DeferredBuildException(List<BuildMessage> buildMessages) : this(null, null, buildMessages)
        {
        }

        public DeferredBuildException(string message, List<BuildMessage> buildMessages = null) : this(message, null, buildMessages)
        {
        }

        public DeferredBuildException(string message, Exception inner = null, List<BuildMessage> buildMessages = null) : base(message, inner)
        {
            if (buildMessages == null) return;
            BuildMessages = new List<BuildMessage>();
            BuildMessages.AddRange(buildMessages);
        }

    }
}

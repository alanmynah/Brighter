using System;

namespace HelloWorldWithLogging
{
    [AttributeUsage(AttributeTargets.Property)]
    sealed class ObfuscateAttribute : Attribute
    {
        public string ObfuscationMessage { get; }

        public ObfuscateAttribute(string obfuscationMessage = "***Redacted***")
        {
            ObfuscationMessage = obfuscationMessage;
        }
    }
}

#region Licence
/* The MIT License (MIT)
Copyright © 2015 Ian Cooper <ian_hammond_cooper@yahoo.co.uk>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the “Software”), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE. */

#endregion

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using Paramore.Brighter;
using Paramore.Brighter.Logging.Attributes;
using Serilog;

namespace HelloWorldWithLogging
{
    internal class GreetingCommandHandler : RequestHandler<GreetingCommand>
    {
        [RequestLogging(step: 1, timing: HandlerTiming.Before)]
        public override GreetingCommand Handle(GreetingCommand command)
        {
            ObfuscateCommand(command);
            
            Console.WriteLine($"Hello {command.Name}");
            Console.WriteLine($"This message is not in the logs: '{command.SensitiveInfo}'");

            return base.Handle(command);
        }

        private void ObfuscateCommand<T>(T command)
        {
            var properties = command.GetType().GetProperties();
            dynamic commandProperties = new ExpandoObject();
            var shallowCopy = (IDictionary<PropertyInfo, object>)commandProperties;
            
            foreach (var property in properties)
            {
                shallowCopy.Add(property, property.GetValue(command));
                ObfuscateAttribute obfuscateAttribute = (ObfuscateAttribute) Attribute.GetCustomAttribute(property, typeof(ObfuscateAttribute), true);
                
                if (obfuscateAttribute != null)
                { 
                    Log.Logger.Debug("Obfuscating {0} from {1}", property, command.GetType().ToString());
                    property.SetValue(command, obfuscateAttribute.ObfuscationMessage, null);
                }
            }
        }
        
    }
}

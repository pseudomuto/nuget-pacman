using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuGet.PacMan
{
    internal static class Guard
    {
        public static void AgainstNullArgument<TArg>(string argumentName, TArg value)
            where TArg : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        public static void AgainstNullArgument(string argumentName, string value)            
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(argumentName);
            }
        }
    }
}

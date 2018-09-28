using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tutorial.Utils
{
    public class ErrorMessages
    {
        public const string required = "{0} is required";
        public const string stringLength = "{0} length must be equal or lower than {1}";
        public const string dateRegex = "{0} doesn't match the expression";
        public const string range = "{0} must be between {1} and {2}";
    }
}

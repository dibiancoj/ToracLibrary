using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.ToracAttributes
{

    /// <summary>
    /// Just lets me know that this method is invoked at runtime and we should not remove it even though it looks like its not being used
    /// </summary>
    public class InvokedDynamicallyAtRuntimeAttribute : Attribute
    {
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Examples.ExtensionsImplements
{
    [Scriptable] public interface IIncidentAction
    {
        [Scriptable] void Started(Incident incident);
    }
}

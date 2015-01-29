//------------------------------------------------------------------------------
// <copyright from='2013' to='2014' company='Polar Engineering and Consulting'>
//    Copyright (c) Polar Engineering and Consulting. All Rights Reserved.
//
//    This file contains confidential material.
//
// </copyright>
//------------------------------------------------------------------------------

namespace Examples.Extensions
{
    // used by application, not used by WWB.NET scripts
    public interface IHost
    {
        Incident TheIncident { get; }
        void Log(string text);
    }
}

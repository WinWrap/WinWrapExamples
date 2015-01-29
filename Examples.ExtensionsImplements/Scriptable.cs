using System;

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
    // http://www.winwrap.com/web2/basic/#!/ref/NET-WinWrap.Basic.BasicNoUIObj.AddScriptableObjectModel.html
    internal class ScriptableAttribute : Attribute
    {
        public override object TypeId { get { return new Guid("542F6A10-6097-445A-B09E-A248863C2873"); } }
    }
}

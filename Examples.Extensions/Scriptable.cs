﻿using System;

namespace Examples.Extensions
{
    // http://www.winwrap.com/web/basic/reference/?p=WinWrap.Basic.BasicNoUIObj.AddScriptableReference.html
    internal class ScriptableAttribute : Attribute
    {
        public override object TypeId { get { return new Guid("542F6A10-6097-445A-B09E-A248863C2873"); } }
    }
}

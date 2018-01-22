#region License
// 
// Copyright (c) 2013, Bzway team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
/*
Bzway is a content management system based on ASP.NET MVC framework. Copyright 2009 Yardi Technology Limited.

This program is free software: you can redistribute it and/or modify it under the terms of the
GNU General Public License version 3 as published by the Free Software Foundation.

This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with this program.
If not, see http://www.Bzway.com/gpl3/.
*/
using System;
using System.Configuration;
using System.Collections.Generic;

namespace OpenData.Framework.Common.Routing
{

    public class RouteChildElement : ConfigurationElement
    {
        private Dictionary<string, string> attributes = new Dictionary<string, string>();


        public Dictionary<string, string> Attributes
        {
            get { return this.attributes; }
        }


        protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
        {
            attributes.Add(name, value);
            return true;
        }
    }
}

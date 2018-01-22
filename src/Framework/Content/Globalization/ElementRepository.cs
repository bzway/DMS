#region License
// 
// Copyright (c) 2013, Bzway team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using OpenData.Globalization.Repository;

namespace OpenData.Globalization
{
    /// <summary>
    /// 
    /// </summary>
    public static class ElementRepository
    {
        /// <summary>
        /// 
        /// </summary>
        public static IElementRepository DefaultRepository = new CacheElementRepository(new XmlElementRepository());
    }
}

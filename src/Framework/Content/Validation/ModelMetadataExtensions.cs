#region License
// 
// Copyright (c) 2013, Bzway team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion

using OpenData.Common.AppEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace OpenData.Framework.Common
{   
    public static class ModelMetadataExtensions
    {
        public static ISelectListDataSource GetDataSource(this ModelMetadata modelMetadata)
        {
            ISelectListDataSource dataSource = null;
            if (modelMetadata is BzwayModelMetadata)
            {
                var dataSourceAttribute = ((BzwayModelMetadata)modelMetadata).DataSourceAttribute;
                if (dataSourceAttribute != null)
                {
                    dataSource = (ISelectListDataSource)TypeActivator.CreateInstance(dataSourceAttribute.DataSourceType);
                }
                if (dataSource == null)
                {
                    dataSource = ((BzwayModelMetadata)modelMetadata).DataSource;
                }
            }
            else
            {
            }
            if (dataSource == null)
            {
                return new EmptySelectListDataSource();
            }
            else
            {
                return dataSource;
            }
        }
        public static Type GetDataSourceType(this ModelMetadata modelMetadata)
        {
            Type dataSourceType = null;
            if (modelMetadata is BzwayModelMetadata)
            {
                var dataSourceAttribute = ((BzwayModelMetadata)modelMetadata).DataSourceAttribute;
                dataSourceType = dataSourceAttribute.DataSourceType;
            }
            else
            {
            }
            if (dataSourceType == null)
            {
                dataSourceType = typeof(EmptySelectListDataSource);
            }

            return dataSourceType;
        }
    }
}

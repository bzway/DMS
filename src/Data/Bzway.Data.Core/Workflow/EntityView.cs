
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Dynamic;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//namespace Bzway.Data.Core
//{
//    public abstract class EntityView
//    {
//        public virtual T GetEntity<T>(string uuid = null) where T : EntityBase, new()
//        {
//            T entity = new T();
//            foreach (PropertyInfo info in this.GetType().GetProperties())
//            {
//                if (info.CanRead)
//                {
//                    object value = null;
//                    try
//                    {
//                        value = info.GetValue(this, null);
//                    }
//                    catch { }
//                    entity.Add(info.Name, value);
//                }
//            }
//            if (!string.IsNullOrEmpty(uuid))
//            {
//                entity.UUID = uuid;
//            }
//            return entity;
//        }
//        public virtual T GetUpdatingEntity<T>(string uuid = null) where T : EntityBase, new()
//        {
//            T entity = new T();
//            foreach (PropertyInfo info in this.GetType().GetProperties())
//            {
//                if (info.CanRead)
//                {
//                    object value = null;
//                    try
//                    {
//                        value = info.GetValue(this, null);
//                    }
//                    catch { }
//                    if (value != null)
//                    {
//                        entity.Add(info.Name, value);
//                    }
//                }
//            }
//            if (!string.IsNullOrEmpty(uuid))
//            {
//                entity.UUID = uuid;
//            }
//            return entity;
//        }
//    }
//}
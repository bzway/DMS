using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace Bzway.Data.Core
{
    public partial class DynamicEntity : IDictionary<string, object>
    {

        IDictionary<string, object> dictionary;
        public DynamicEntity() : this(new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase))
        {
        }
        public DynamicEntity(IDictionary<string, object> dictionary)
        {
            this.dictionary = dictionary;
        }
        public void Add(string key, object value)
        {
            this.dictionary.Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return this.dictionary.ContainsKey(key);
        }

        public ICollection<string> Keys
        {
            get { return this.dictionary.Keys; }
        }

        public bool Remove(string key)
        {
            return this.dictionary.Remove(key);
        }

        public bool TryGetValue(string key, out object value)
        {
            return this.dictionary.TryGetValue(key, out value);
        }

        public ICollection<object> Values
        {
            get { return this.dictionary.Values; }
        }

        public object this[string key]
        {
            get
            {
                return this.dictionary[key];
            }
            set
            {
                this.dictionary[key] = value;
            }
        }

        public void Add(KeyValuePair<string, object> item)
        {
            this.dictionary.Add(item);
        }

        public void Clear()
        {
            this.dictionary.Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return this.dictionary.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            this.dictionary.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return this.dictionary.Count; }
        }

        public bool IsReadOnly
        {
            get { return this.dictionary.IsReadOnly; }
        }

        public object Status { get; set; }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return this.dictionary.Remove(item);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return this.dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.dictionary.GetEnumerator();
        }

    }

    public partial class DynamicEntity : DynamicObject
    {

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var name = binder.Name;
            return this.dictionary.TryGetValue(name, out result);
        }
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            try
            {
                var name = binder.Name;
                this.dictionary[name] = value;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return dictionary.Keys;
        }
    }

    public partial class DynamicEntity : IEntity
    {
        public List<IRelatedEntity> Categories
        {
            get
            {
                if (this.ContainsKey("Categories"))
                {
                    return (List<IRelatedEntity>)this["Categories"];
                }
                return new List<IRelatedEntity>();
            }
            set
            {
                this["Categories"] = value;
            }
        }

        public List<IRelatedEntity> Children
        {
            get
            {
                if (this.ContainsKey("Children"))
                {
                    return (List<IRelatedEntity>)this["Children"];
                }
                return new List<IRelatedEntity>();
            }
            set
            {
                this["Children"] = value;
            }
        }

        public string CreatedBy
        {
            get
            {
                if (this.ContainsKey("CreatedBy"))
                {
                    return (string)this["CreatedBy"];
                }
                return string.Empty;
            }
            set
            {
                this["CreatedBy"] = value;
            }
        }

        public DateTime CreatedOn
        {
            get
            {
                if (this.ContainsKey("CreatedOn"))
                {
                    return (DateTime)this["CreatedOn"];
                }
                return DateTime.MinValue;
            }
            set
            {
                this["CreatedOn"] = value;
            }
        }

        public string Id
        {
            get
            {
                if (this.ContainsKey("Id"))
                {
                    return (string)this["Id"];
                }
                return string.Empty;
            }
            set
            {
                this["Id"] = value;
            }
        }

        public string UpdatedBy
        {
            get
            {
                if (this.ContainsKey("UpdatedBy"))
                {
                    return (string)this["UpdatedBy"];
                }
                return string.Empty;
            }
            set
            {
                this["UpdatedBy"] = value;
            }
        }

        public DateTime UpdatedOn
        {
            get
            {
                if (this.ContainsKey("UpdatedOn"))
                {
                    return (DateTime)this["UpdatedOn"];
                }
                return DateTime.MinValue;
            }
            set
            {
                this["UpdatedOn"] = value;
            }
        }
    }
}
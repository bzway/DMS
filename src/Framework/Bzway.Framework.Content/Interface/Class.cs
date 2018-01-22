using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bzway.Framework.Content
{
    public enum FieldType : int
    {
        Number,
        String,
        Float,
        DateTime,
        Part,
    }

    internal class Helper
    {
        public static Dictionary<string, Type> dict = new Dictionary<string, Type>();
        static Helper()
        {
            dict.Add("Field", typeof(Field));
        }

    }
    public interface IContentPart
    {
        string Name { get; }
        object Data { get; set; }
        string View();
        object Settings { get; set; }
        string Validate();
    }
    public class Field : IContentPart
    {
        public string ContentId { get; set; }
        public string Name { get; set; }

        public FieldType Type { get; set; }
        public string Regex { get; set; }
        public int MaxLenth { get; set; }
        public int MinLenth { get; set; }
        public bool IsValidated { get; set; }

        public string ErrorMessage { get; set; }
        public string HtmlTemplate { get; set; }
        public object Data { get; set; }

        public object Settings
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string View()
        {
            throw new NotImplementedException();
        }

        public string Validate()
        {
            throw new NotImplementedException();
        }
    }

    public interface IType
    {
        string Name { get; }
        string Description { get; }
        List<IContentPart> Fileds { get; }

        IType Parant { get; }
        List<IType> Children { get; }
    }

    public class Part : IContentPart
    {
        private readonly List<Field> fields;
        public object Data
        {
            get
            {
                Dictionary<string, object> data = new Dictionary<string, object>();
                foreach (var item in this.fields)
                {
                    data.Add(item.Name, item.Data);
                }
                return data;
            }
            set
            {
                var data = (Dictionary<string, object>)value;
                if (data != null)
                {
                    foreach (var item in data.Keys)
                    {
                        var field = this.fields.FirstOrDefault(m => m.Name == item);
                        if (field != null)
                        {
                            field.Data = data[item];
                        }
                    }
                }
            }
        }

        public string Name
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public object Settings
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public Part()
        {
            this.fields = new List<Field>();
        }

        public string View()
        {
            StringBuilder html = new StringBuilder();
            foreach (var item in this.fields)
            {
                html.Append(item.View());
            }
            return html.ToString();
        }

        public string Validate()
        {
            StringBuilder error = new StringBuilder();
            foreach (var item in this.fields)
            {
                error.Append(item.Validate());
            }
            return error.ToString();
        }
    }

    public class ContentType : IType
    {
        public ContentType(string Name, string Description)
        {
            this.Name = Name;
            this.Description = Description;
        }
        public string Description { get; private set; }
        public string Name { get; private set; }
        public List<IContentPart> Fileds { get; private set; }
        public IType Parant { get; set; }

        public List<IType> Children { get; set; }
    }
    public interface IContentItem
    {
        IType ContentType { get; }
        object this[string name] { get; }
        IQueryable<IContentItem> Other(string TypeName);
    }
    public class Content : IContentItem, IContentPart
    {
        public Content(string TypeName)
        {
            this.ContentType = new ContentType(TypeName, "Store Member Profiles");

        }
        public IType ContentType { get; set; }

        public object Data
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string Name { get; }

        public object Settings
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public object this[string index] { get { return ""; } }

        public IQueryable<IContentItem> Other(string TypeName)
        {
            throw new NotImplementedException();
        }

        public string View()
        {
            return string.Empty;
        }

        public string Validate()
        {
            throw new NotImplementedException();
        }
    }

    public interface IModule
    {
        string Name { get; set; }
        string Description { get; set; }
        string Version { get; set; }
        void Install();
        void UnInstall();
    }

}

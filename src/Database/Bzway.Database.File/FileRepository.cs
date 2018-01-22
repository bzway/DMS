using System;
using System.Linq;
using Bzway.Database.Core;
using Microsoft.Extensions.FileProviders;
using Bzway.Common.Share;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;
using System.IO;

namespace Bzway.Database.File
{
    internal class FileRepository<T> : IRepository<T> where T : EntityBase, new()
    {
        readonly IFileProvider fileProvider;
        readonly Type type;
        readonly IFileInfo fileInfo;
        readonly IPrincipal user;
        public FileRepository(IFileProvider fileProvider, IPrincipal user)
        {
            this.user = user;
            this.fileProvider = fileProvider;
            this.type = typeof(T);
            var cache = AppEngine.GetService<ICacheManager>();
            this.fileInfo = cache.Get<IFileInfo>("FileInfo." + type.Name, () =>
            {
                string path = $"/{type.Name}.json";
                var fileInfo = this.fileProvider.GetFileInfo(path);
                if (!fileInfo.Exists)
                {
                    using (var stream = System.IO.File.Create(fileInfo.PhysicalPath)) { }
                    return this.fileProvider.GetFileInfo(path);
                }
                return fileInfo;
            });
        }
        public IQueryable<T> Query()
        {
            var data = System.IO.File.ReadAllText(this.fileInfo.PhysicalPath);
            var list = JsonConvert.DeserializeObject<List<T>>(data);
            if (list == null)
            {
                return new List<T>().AsQueryable();
            }
            return list.AsQueryable();
        }
        private void Save(List<T> list)
        {
            var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(list));
            System.IO.File.WriteAllBytes(this.fileInfo.PhysicalPath, data);
        }

        public void Delete(T newData)
        {
            var list = this.Query().ToList();
            var item = list.Where(m => m.Id == newData.Id).FirstOrDefault();
            if (item != null)
            {
                list.Remove(item);
            }
            var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(list));
            using (var stream = this.fileInfo.CreateReadStream())
            {
                stream.Write(data, 0, data.Length);
            }
        }

        public void Delete(string uuid)
        {
            var list = this.Query().ToList();
            var item = list.Where(m => m.Id == uuid).FirstOrDefault();
            if (item != null)
            {
                list.Remove(item);
            }
            var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(list));
            using (var stream = this.fileInfo.CreateReadStream())
            {
                stream.Write(data, 0, data.Length);
            }
        }

        public bool Execute(ICommand<T> command)
        {
            if (command is IUpdateCommand<T>)
            {
                return ExecuteUpdate((IUpdateCommand<T>)command);
            }
            return true;
        }
        private bool ExecuteUpdate(IUpdateCommand<T> command)
        {
            var query = (WhereExpression<T>)command.Where;

            Func<T, bool> predicate;
            if (query == null)
            {
                predicate = new Func<T, bool>(m => { return true; });
            }
            else
            {
                predicate = this.GetFilter(query.Next);
            }
            var list = this.Query().ToList();

            foreach (var item in list.Where(predicate))
            {
                foreach (var update in command.Update.Updates)
                {
                    item.TrySetValue(update.Key, update.Value);
                }
            }
            Save(list);
            return true;
        }

        public IWhere<T> Filter()
        {
            throw new NotImplementedException();
        }

        public void Insert(T newData)
        {
            if (string.IsNullOrEmpty(newData.Id))
            {
                newData.Id = Guid.NewGuid().ToString("N");
            }
            newData.CreatedBy = newData.UpdatedBy = this.user.Identity.Name;
            newData.CreatedOn = newData.UpdatedOn = DateTime.UtcNow;
            var list = this.Query().ToList();
            list.Add(newData);
            Save(list);

        }
        public void Update(IUpdate<T> update, IWhere<T> where)
        {
            throw new NotImplementedException();
        }

        public void Update(T newData, string uuid = "")
        {
            if (string.IsNullOrEmpty(uuid))
            {
                uuid = newData.Id;
            }
            else
            {
                newData.Id = uuid;
                newData.UpdatedBy = this.user.Identity.Name;
                newData.UpdatedOn = DateTime.UtcNow;
            }
            var list = this.Query().ToList();
            var item = list.FirstOrDefault(m => m.Id == uuid);
            if (item == null)
            {
                return;
            }
            list.Remove(item);
            list.Add(newData);
            var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(list));
            using (var stream = this.fileInfo.CreateReadStream())
            {
                stream.Write(data, 0, data.Length);
            }
        }

        private Func<T, bool> GetFilter<T>(IWhere<T> where)
        {
            Func<T, bool> filter = null;


            if (where is WhereExpression<T>)
            {
                filter = GetAndFilter((WhereExpression<T>)where);
            }
            else
            {
                filter = GetOrFilter((OrExpression<T>)where);
            }

            if (filter == null)
            {
                filter = new Func<T, bool>(m => { return true; });
            }
            return filter;
        }
        private Func<T, bool> GetAndFilter<T>(WhereExpression<T> where)
        {
            Func<T, bool> filter = null;
            switch (where.CompareType)
            {
                case CompareType.Equal:
                    filter = new Func<T, bool>((target) =>
                    {
                        var result = object.Equals(target.TryGetValue(where.FieldName), where.Value);
                        return result;
                    });
                    break;
                case CompareType.NotEqual:
                    filter = new Func<T, bool>((target) =>
                    {
                        var result = !object.Equals(target.TryGetValue(where.FieldName), where.Value);
                        return result;
                    });
                    break;
                case CompareType.Like:
                    if (where.Value != null)
                    {
                        filter = new Func<T, bool>((target) => { return target.TryGetValue(where.FieldName).ToString().Contains(where.Value.ToString()); });
                    }
                    else
                    {
                        filter = new Func<T, bool>((target) => { return true; });
                    }
                    break;
                case CompareType.GreaterThan:
                    filter = new Func<T, bool>((target) =>
                    {
                        var result = object.Equals(target.TryGetValue(where.FieldName), where.Value);
                        return false;
                    });
                    break;

                case CompareType.GreaterThanOrEqual:
                    filter = new Func<T, bool>((target) =>
                    {
                        var result = object.Equals(target.TryGetValue(where.FieldName), where.Value);
                        return false;
                    });
                    break;
                case CompareType.LessThan:
                    filter = new Func<T, bool>((target) =>
                    {
                        var result = object.Equals(target.TryGetValue(where.FieldName), where.Value);
                        return false;
                    });
                    break;

                case CompareType.LessThanOrEqual:
                    filter = new Func<T, bool>((target) =>
                    {
                        var result = object.Equals(target.TryGetValue(where.FieldName), where.Value);
                        return false;
                    });
                    break;
                case CompareType.Startwith:
                    if (where.Value != null)
                    {
                        filter = new Func<T, bool>((target) => { return (target.TryGetValue(where.FieldName).ToString().StartsWith(where.Value.ToString())); });

                    }
                    else
                    {
                        filter = new Func<T, bool>((target) => { return true; });
                    }

                    break;
                case CompareType.EndWith:
                    if (where.Value != null)
                    {
                        filter = new Func<T, bool>((target) => { return (target.TryGetValue(where.FieldName).ToString().EndsWith(where.Value.ToString())); });

                    }
                    else
                    {
                        filter = new Func<T, bool>((target) => { return true; });
                    }

                    break;
                case CompareType.Contains:
                    if (where.Value != null)
                    {
                        filter = new Func<T, bool>((target) =>
                        {
                            if (target.TryGetValue(where.FieldName) == null)
                            {
                                return true;
                            }
                            return where.Value.ToString().Contains(target.TryGetValue(where.FieldName).ToString());
                        });

                    }
                    else
                    {
                        filter = new Func<T, bool>((target) => { return true; });
                    }

                    break;
                case CompareType.NoLike:
                    if (where.Value != null)
                    {
                        filter = new Func<T, bool>((target) => { return !(target.TryGetValue(where.FieldName).ToString().Contains(where.Value.ToString())); });
                    }
                    else
                    {
                        filter = new Func<T, bool>((target) => { return true; });
                    }
                    break;
                default:
                    filter = new Func<T, bool>((target) => { return true; });
                    break;
            }
            if (where.Next == null)
            {
                return filter;
            }
            if (where.Next is OrExpression<T>)
            {
                return new Func<T, bool>((target) => { return filter(target) | GetOrFilter((OrExpression<T>)where.Next)(target); });
            }
            else
            {
                return new Func<T, bool>((target) => { return filter(target) & GetAndFilter((WhereExpression<T>)where.Next)(target); });
            }
        }
        private Func<T, bool> GetOrFilter<T>(OrExpression<T> or)
        {
            Func<T, bool> filterLeft;
            if (or.Left is WhereExpression<T>)
            {
                filterLeft = GetAndFilter((WhereExpression<T>)or.Left);
            }
            else
            {
                filterLeft = GetOrFilter((OrExpression<T>)or.Left);
            }
            Func<T, bool> filterRight;
            if (or.Right is WhereExpression<T>)
            {
                filterRight = GetAndFilter((WhereExpression<T>)or.Right);
            }
            else
            {
                filterRight = GetOrFilter((OrExpression<T>)or.Right);
            }
            return new Func<T, bool>((target) => { return filterLeft(target) | filterRight(target); });

        }

    }
}
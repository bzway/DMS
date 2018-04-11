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
            var cache = CacheManager.Default.MemCacheProvider;
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
            var data = System.IO.File.ReadAllText(this.fileInfo.PhysicalPath, Encoding.UTF8);
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
            Save(list);
        }

        public void Delete(string uuid)
        {
            var list = this.Query().ToList();
            var item = list.Where(m => m.Id == uuid).FirstOrDefault();
            if (item != null)
            {
                list.Remove(item);
            }
            Save(list);
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
            IUpdateCommand<T> command = new UpdateCommand() { Update = update, Where = where };
            ExecuteUpdate(command);
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
            Save(list);
        }

        private Func<E, bool> GetFilter<E>(IWhere<E> where)
        {
            Func<E, bool> filter = null;
            if (where is WhereExpression<E>)
            {
                filter = GetAndFilter((WhereExpression<E>)where);
            }
            else
            {
                filter = GetOrFilter((OrExpression<E>)where);
            }

            if (filter == null)
            {
                filter = new Func<E, bool>(m => { return true; });
            }
            return filter;
        }
        private Func<E, bool> GetAndFilter<E>(WhereExpression<E> where)
        {
            Func<E, bool> filter = null;
            switch (where.CompareType)
            {
                case CompareType.Equal:
                    filter = new Func<E, bool>((target) =>
                    {
                        var result = object.Equals(target.TryGetValue(where.FieldName), where.Value);
                        return result;
                    });
                    break;
                case CompareType.NotEqual:
                    filter = new Func<E, bool>((target) =>
                    {
                        var result = !object.Equals(target.TryGetValue(where.FieldName), where.Value);
                        return result;
                    });
                    break;
                case CompareType.Like:
                    if (where.Value != null)
                    {
                        filter = new Func<E, bool>((target) => { return target.TryGetValue(where.FieldName).ToString().Contains(where.Value.ToString()); });
                    }
                    else
                    {
                        filter = new Func<E, bool>((target) => { return true; });
                    }
                    break;
                case CompareType.GreaterThan:
                    filter = new Func<E, bool>((target) =>
                    {
                        var result = object.Equals(target.TryGetValue(where.FieldName), where.Value);
                        return false;
                    });
                    break;

                case CompareType.GreaterThanOrEqual:
                    filter = new Func<E, bool>((target) =>
                    {
                        var result = object.Equals(target.TryGetValue(where.FieldName), where.Value);
                        return false;
                    });
                    break;
                case CompareType.LessThan:
                    filter = new Func<E, bool>((target) =>
                    {
                        var result = object.Equals(target.TryGetValue(where.FieldName), where.Value);
                        return false;
                    });
                    break;

                case CompareType.LessThanOrEqual:
                    filter = new Func<E, bool>((target) =>
                    {
                        var result = object.Equals(target.TryGetValue(where.FieldName), where.Value);
                        return false;
                    });
                    break;
                case CompareType.Startwith:
                    if (where.Value != null)
                    {
                        filter = new Func<E, bool>((target) => { return (target.TryGetValue(where.FieldName).ToString().StartsWith(where.Value.ToString())); });

                    }
                    else
                    {
                        filter = new Func<E, bool>((target) => { return true; });
                    }

                    break;
                case CompareType.EndWith:
                    if (where.Value != null)
                    {
                        filter = new Func<E, bool>((target) => { return (target.TryGetValue(where.FieldName).ToString().EndsWith(where.Value.ToString())); });

                    }
                    else
                    {
                        filter = new Func<E, bool>((target) => { return true; });
                    }

                    break;
                case CompareType.Contains:
                    if (where.Value != null)
                    {
                        filter = new Func<E, bool>((target) =>
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
                        filter = new Func<E, bool>((target) => { return true; });
                    }

                    break;
                case CompareType.NoLike:
                    if (where.Value != null)
                    {
                        filter = new Func<E, bool>((target) => { return !(target.TryGetValue(where.FieldName).ToString().Contains(where.Value.ToString())); });
                    }
                    else
                    {
                        filter = new Func<E, bool>((target) => { return true; });
                    }
                    break;
                default:
                    filter = new Func<E, bool>((target) => { return true; });
                    break;
            }
            if (where.Next == null)
            {
                return filter;
            }
            if (where.Next is OrExpression<E>)
            {
                return new Func<E, bool>((target) => { return filter(target) | GetOrFilter((OrExpression<E>)where.Next)(target); });
            }
            else
            {
                return new Func<E, bool>((target) => { return filter(target) & GetAndFilter((WhereExpression<E>)where.Next)(target); });
            }
        }
        private Func<E, bool> GetOrFilter<E>(OrExpression<E> or)
        {
            Func<E, bool> filterLeft;
            if (or.Left is WhereExpression<E>)
            {
                filterLeft = GetAndFilter((WhereExpression<E>)or.Left);
            }
            else
            {
                filterLeft = GetOrFilter((OrExpression<E>)or.Left);
            }
            Func<E, bool> filterRight;
            if (or.Right is WhereExpression<E>)
            {
                filterRight = GetAndFilter((WhereExpression<E>)or.Right);
            }
            else
            {
                filterRight = GetOrFilter((OrExpression<E>)or.Right);
            }
            return new Func<E, bool>((target) => { return filterLeft(target) | filterRight(target); });

        }
        class UpdateCommand : IUpdateCommand<T>
        {
            public IUpdate<T> Update { get; set; }

            public IWhere<T> Where { get; set; }
        }
    }
}
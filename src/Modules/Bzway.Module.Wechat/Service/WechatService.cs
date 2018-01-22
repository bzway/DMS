using System;
using System.Collections.Generic;
using System.Linq;
using Bzway.Module.Wechat.Entity;
using Bzway.Data.Core;
using Microsoft.Extensions.Logging;
using Bzway.Framework.Application;
using Bzway.Framework.Application.Entity;
using Bzway.Module.Wechat.Interface;
using Bzway.Common.Collections;
using System.Security.Principal;

namespace Bzway.Module.Wechat.Service
{
    public class WechatService : BaseService<WechatService>, IWechatService
    {
        #region ctor
        private readonly IWechatApiService wechatApiService;
        public WechatService(ILoggerFactory loggerFactory, ITenant tenant, IPrincipal user, IWechatApiService wechatApiService) : base(loggerFactory, tenant, user)
        {
            this.wechatApiService = wechatApiService;
        }
        #endregion
        #region MyRegion
        public void SyncMaterial()
        {
            int pageSize = 20;
            int index = 0;
            string type = "news";
            while (index < 100000)
            {
                var result = this.wechatApiService.GetMaterialList(type, pageSize * index, pageSize);
                if (result.item_count <= 0)
                {
                    break;
                }
                foreach (var item in result.item)
                {
                    var existedItem = this.db.Entity<WechatMaterial>().Query().Where(m => m.MediaId, item.media_id, CompareType.Equal).First();
                    if (existedItem != null)
                    {
                        foreach (var newsItem in this.db.Entity<WechatNewsMaterial>().Query().Where(m => m.MaterialID, existedItem.Id, CompareType.Equal).ToList())
                        {
                            this.db.Entity<WechatNewsMaterial>().Delete(newsItem);
                        };
                        this.db.Entity<WechatMaterial>().Delete(existedItem);
                    }
                    var materialId = Guid.NewGuid().ToString("N");
                    this.db.Entity<WechatMaterial>().Insert(new WechatMaterial()
                    {
                        Id = materialId,
                        CreatedBy = "system",
                        CreatedOn = DateTime.UtcNow,
                        UpdatedBy = "system",
                        UpdatedOn = DateTime.UtcNow,
                        Description = item.media_id,
                        IsReleased = true,
                        LastUpdateTime = DateTime.UtcNow,
                        MediaId = item.media_id,
                        Name = item.name,
                        OfficialAccount = "",
                        Type = WechatMaterialType.news,

                        Url = item.url,
                    });
                    foreach (var detail in item.content.news_item)
                    {
                        this.db.Entity<WechatNewsMaterial>().Insert(new Entity.WechatNewsMaterial()
                        {
                            Author = detail.author,
                            Content = detail.content,
                            ContentSourceUrl = detail.content_source_url,
                            CreatedBy = "system",
                            CreatedOn = DateTime.UtcNow,
                            Digest = detail.digest,
                            IsReleased = true,
                            LastUpdateTime = DateTime.UtcNow,
                            MaterialID = materialId,
                            MediaId = item.media_id,
                            ThumbMediaId = detail.thumb_media_id,
                            OfficialAccount = string.Empty,
                            ShowCoverPicture = detail.show_cover_pic > 0,
                            SortBy = 0,
                            Title = detail.title,
                            Url = detail.url,
                            UpdatedBy = "system",
                            UpdatedOn = DateTime.UtcNow,
                        });
                    }
                }
                if (pageSize * (++index) > result.total_count)
                {
                    break;
                }
            }
        }
        #endregion
        public IEnumerable<WechatKeyword> GetWechatResponse(string Keyword = "", SearchType SearchType = SearchType.None, string wechatId = "")
        {
            try
            {
                var today = DateTime.Now;
                var yesterday = today.AddDays(-1);
                var query = this.db.Entity<WechatKeyword>().Query()
                    .Where(m => (!m.FromTime.HasValue || m.FromTime.Value <= today))
                    .Where(m => (!m.EndTime.HasValue || m.EndTime.Value > yesterday));
                switch (SearchType)
                {
                    case SearchType.Equal:
                        query = query.Where(m => m.Keyword == Keyword);
                        break;
                    case SearchType.StartWith:
                        query = query.Where(m => m.Keyword.StartsWith(Keyword));
                        break;
                    case SearchType.EndWith:
                        query = query.Where(m => m.Keyword.EndsWith(Keyword));
                        break;
                    case SearchType.Contain:
                        query = query.Where(m => m.Keyword.Contains(Keyword));
                        break;

                    case SearchType.Include:
                        query = query.Where(m => Keyword.Contains(m.Keyword));
                        break;
                    default:
                        query = query.Where(m =>
                            (m.SearchType == SearchType.Equal && m.Keyword == Keyword) ||
                            (m.SearchType == SearchType.StartWith && m.Keyword.StartsWith(Keyword) ||
                            (m.SearchType == SearchType.EndWith && m.Keyword.EndsWith(Keyword)) ||
                            (m.SearchType == SearchType.Contain && m.Keyword.Contains(Keyword)) ||
                            (m.SearchType == SearchType.Include && Keyword.Contains(m.Keyword))));

                        break;
                }
                return query.ToList();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return new List<WechatKeyword>();

            }
        }

        public IQueryable<WechatUserGroup> GetAllTags()
        {
            throw new NotImplementedException();
        }

        public IQueryable<WechatUser> GetUsers()
        {
            throw new NotImplementedException();
        }

        public IPagedList<WechatMaterial> GetWechatMaterils(string keyword = "", WechatMaterialType type = WechatMaterialType.All, int index = 0, int pageSize = 10)
        {
            var query = this.db.Entity<WechatMaterial>().Query();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(m => m.Name, keyword, CompareType.Contains);
            }
            if (type != WechatMaterialType.All)
            {
                query = query.Where(m => m.Type, type, CompareType.Equal);
            }
            return query.ToPageList(index, pageSize);
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Bzway.Common.Collections
{
    /// <summary>
    /// 
    /// </summary>
    public class PagedList<T> : IPagedList<T>, IEnumerable<T>
    {
        private List<T> list;
        public PagedList(IList<T> items, int pageIndex, int pageSize)
        {
            this.PageSize = pageSize;
            this.TotalItemCount = items.Count;
            this.PageCount = (int)Math.Ceiling(TotalItemCount / (double)PageSize);
            this.PageNumber = pageIndex;
            list = items.Skip(pageIndex - 1).Take(pageSize).ToList();
        }

        public PagedList(IEnumerable<T> items, int pageIndex, int pageSize, int totalItemCount)
        {
            list = items.ToList();
            this.PageSize = pageSize;
            this.TotalItemCount = totalItemCount;
            this.PageCount = (int)Math.Ceiling(totalItemCount / (double)pageSize);
            this.PageNumber = pageIndex;
        }

        public int Count
        {
            get;
            set;
        }

        public IPagedList GetMetaData()
        {
            return this;
        }

        public T this[int index]
        {
            get { return this.list[index]; }
        }

        public int FirstItemOnPage
        {
            get { return this.PageNumber * this.PageSize + 1; }
        }

        public bool HasNextPage
        {
            get { return this.PageCount > this.PageNumber; }
        }

        public bool HasPreviousPage
        {
            get { return PageNumber > 1; }
        }

        public bool IsFirstPage
        {
            get { return PageNumber == 1; }
        }

        public bool IsLastPage
        {
            get { return PageNumber == PageCount; }
        }

        public int LastItemOnPage
        {
            get { return this.PageNumber * (this.PageSize + 1) - 1; }
        }

        public int PageCount
        {
            get;
            set;
        }

        public int PageNumber
        {
            get;
            set;
        }

        public int PageSize
        {
            get;
            set;
        }

        public int TotalItemCount
        {
            get;
            set;
        }


        public IEnumerator<T> GetEnumerator()
        {

            return this.list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.list.GetEnumerator();
        }
    }
}
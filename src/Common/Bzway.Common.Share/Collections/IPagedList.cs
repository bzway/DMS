using System.Collections.Generic;

namespace Bzway.Common.Collections
{
    public interface IPagedList<T> : IPagedList
    {
        T this[int index] { get; }
    }

    public interface IPagedList
    {


        int Count { get; set; }
        int FirstItemOnPage { get; }
        bool HasNextPage { get; }
        bool HasPreviousPage { get; }
        bool IsFirstPage { get; }
        bool IsLastPage { get; }
        int LastItemOnPage { get; }
        int PageCount { get; set; }
        int PageNumber { get; set; }
        int PageSize { get; set; }
        int TotalItemCount { get; set; }
        IPagedList GetMetaData();
    }
}
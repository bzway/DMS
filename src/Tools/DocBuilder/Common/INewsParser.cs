using System;

namespace Bzway.Common.Share
{
    public interface INewsParser
    {
        string Id { get; }
        string Title { get; }
        string Author { get; }
        string Content { get; }
        string Digest { get; }
        string ThumbMedia { get; }
        string Url { get; }
    }
}
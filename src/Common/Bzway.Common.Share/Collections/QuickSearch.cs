using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Bzway.Common.Share.Collections
{
    public class QuickSearch
    {
        /// <summary>
        /// 静态快速查询
        /// </summary>
        /// <typeparam name="T">override GetHashCode()唯一</typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public static IQuickSearch<T> BuildQuickSearch<T>(T[] array)
        {
            return new MyQuickSearch<T>(array);
        }

        //public static IQuickSearch<T> BuildQuickSearch<T>(string dataFile)
        //{
        //    FileStream fileStream = new FileStream(dataFile, FileMode.Open, FileAccess.Read, FileShare.Read);
        //    BinaryFormatter b = new BinaryFormatter();
        //    var array = b.Deserialize(fileStream) as Dictionary<int, T>;
        //    fileStream.Close();
        //    return new MyQuickSearch<T>(array);
        //}
        private class MyQuickSearch<T> : IQuickSearch<T>
        {
            readonly Dictionary<int, T> array;
            public MyQuickSearch(T[] array)
            {
                this.array = new Dictionary<int, T>(array.Length);
                foreach (var item in array)
                {
                    this.array.Add(item.GetHashCode(), item);
                }
            }
            public MyQuickSearch(Dictionary<int, T> array)
            {
                this.array = array;
            }
            public T Search(T word)
            {
                if (this.array.TryGetValue(word.GetHashCode(), out T value))
                {
                    return value;
                }
                return default(T);
            }
        }
        /// <summary>
        /// 静态快速查询
        /// </summary>
        /// <typeparam name="T">override ToString()唯一</typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public static IQuickSearch<T> BuildHashSearch<T>(T[] array)
        {
            return new MyDictionarySearch<T>(array);
        }
        private class MyDictionarySearch<T> : IQuickSearch<T>
        {
            readonly Dictionary<string, T> array;
            public MyDictionarySearch(T[] array)
            {
                this.array = new Dictionary<string, T>(array.Length);

                foreach (var item in array)
                {
                    this.array.Add(item.ToString(), item);
                }
            }
            public T Search(T word)
            {
                T value;
                if (this.array.TryGetValue(word.ToString(), out value))
                {
                    return value;
                }
                return default(T);
            }
        }
        /// <summary>
        /// 动态快速查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public static IQuickSearch<T> BuildBinarySearch<T>(T[] array) where T : IComparable<T>
        {
            return new MyBinarySearch<T>(array);
        }
        private class MyBinarySearch<T> : IQuickSearch<T>
        {
            readonly T[] array;
            public MyBinarySearch(T[] array)
            {
                this.array = array;
            }
            public T Search(T word)
            {
                var i = Array.BinarySearch<T>(array, word);
                if (i > 0)
                {
                    return this.array[i];
                }
                return default(T);
            }
        }

    }
    public interface IQuickSearch<T>
    {
        T Search(T word);
    }
}
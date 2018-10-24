using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;

namespace Bzway.Framework.DistributedFileSystemClient
{
    public class FileMetaData
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string CreationTime { get; set; }
        public string Attributes { get; set; }
        public string LastWriteTime { get; set; }

        public bool IsFile { get; set; }

        public void Create(string path)
        {
            var dir = new DirectoryInfo(path);
            if (!dir.Exists)
            {
                dir.Create();
            }
        }

        public void CreateFile(string path, List<IWebFilePost> files, bool replace = true)
        {
            var dir = new DirectoryInfo(path);
            if (!dir.Exists)
            {
                dir.Create();
            }
            foreach (var file in files)
            {
                var fileName = System.IO.Path.Combine(dir.FullName, file.FileName);
                if (File.Exists(fileName) && replace == false)
                {
                    continue;
                }
                file.SaveAs(fileName);
            }
        }
        public void Delete(string path, bool recursive)
        {
            Directory.Delete(path, recursive);
        }
        public List<FileMetaData> Get(string path)
        {
            List<FileMetaData> list = new List<FileMetaData>();
            if (string.IsNullOrEmpty(path))
            {
                foreach (var item in Environment.GetLogicalDrives())
                {
                    var root = new DirectoryInfo(item);

                    try
                    {
                        list.Add(new FileMetaData
                        {
                            Name = root.Name,
                            Path = root.FullName.Replace("\\", "/"),
                            Attributes = root.Attributes.ToString(),
                            CreationTime = root.CreationTime.ToString(),
                            LastWriteTime = root.LastWriteTime.ToString(),
                            IsFile = false,

                        });
                    }
                    catch { }
                }
                return list;
            }

            System.IO.FileInfo fileInfo = new System.IO.FileInfo(path);
            if (fileInfo.Exists)
            {
                list.Add(new FileMetaData()
                {
                    Name = fileInfo.Name,
                    CreationTime = fileInfo.CreationTime.ToString(),
                    LastWriteTime = fileInfo.LastWriteTime.ToString(),
                    Attributes = fileInfo.FullName,
                    IsFile = true,
                    Path = fileInfo.FullName.Replace("\\", "/"),
                });
                return list;
            }

            var dir = new DirectoryInfo(path);


            try
            {
                foreach (DirectoryInfo item in dir.GetDirectories())
                {
                    list.Add(new FileMetaData()
                    {
                        Attributes = item.Attributes.ToString(),
                        Name = item.Name,
                        Path = item.FullName.Replace("\\", "/"),
                        CreationTime = item.CreationTime.ToString(),
                        LastWriteTime = item.LastWriteTime.ToString(),
                        IsFile = false,
                    });
                }
            }
            catch
            { }


            try
            {

                foreach (System.IO.FileInfo item in dir.GetFiles())
                {
                    list.Add(new FileMetaData()
                    {
                        Name = item.Name,
                        Path = item.FullName.Replace("\\", "/"),
                        CreationTime = item.CreationTime.ToString(),
                        LastWriteTime = item.LastWriteTime.ToString(),
                        Attributes = item.Attributes.ToString(),
                        IsFile = true,
                    });
                }
            }
            catch
            { }
            return list;
        }
    }
}
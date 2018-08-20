using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;

namespace Bzway.Framework.StaticFile
{
    public class FileBrowser
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
        public List<FileBrowser> Get(string path)
        {
            List<FileBrowser> list = new List<FileBrowser>();
            if (string.IsNullOrEmpty(path))
            {
                foreach (var item in Environment.GetLogicalDrives())
                {
                    var root = new DirectoryInfo(item);

                    list.Add(new FileBrowser

                    {
                        Name = root.Name,
                        Path = root.FullName.Replace("\\", "/"),
                        Attributes = root.Attributes.ToString(),
                        CreationTime = root.CreationTime.ToString(),
                        LastWriteTime = root.LastWriteTime.ToString(),
                        IsFile = false,

                    });
                }
                return list;
            }

            FileInfo fileInfo = new FileInfo(path);
            if (fileInfo.Exists)
            {
                list.Add(new FileBrowser()
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
                    list.Add(new FileBrowser()
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

                foreach (FileInfo item in dir.GetFiles())
                {
                    list.Add(new FileBrowser()
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
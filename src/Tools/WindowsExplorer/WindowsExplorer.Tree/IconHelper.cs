using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Runtime.InteropServices;
using System.IO;

namespace ExplorerTreeView
{
    public class SystemIcon
    {
        /// <summary>         
        /// ����shell32�ļ���SHGetFileInfo API����         
        /// </summary>         
        /// <param name="pszPath">ָ�����ļ���,���Ϊ""�򷵻��ļ��е�</param>         
        /// <param name="dwFileAttributes">�ļ�����</param>         
        /// <param name="sfi">���ػ�õ��ļ���Ϣ,��һ����¼����</param>         
        /// <param name="cbFileInfo">�ļ���������</param>        
        /// <param name="uFlags">�ļ���Ϣ��ʶ</param>         
        /// <returns>-1ʧ��</returns>         
        [DllImport("shell32", EntryPoint = "SHGetFileInfo", ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SHGetFileInfo(string pszPath, FileAttribute dwFileAttributes, ref SHFileInfo sfi, uint cbFileInfo, SHFileInfoFlags uFlags);

        /// <summary>   
        /// ����ϵͳ���õ�ͼ��   
        /// </summary>   
        /// <param name="lpszFile">�ļ���,ָ����exe�ļ���dll�ļ�����icon</param>   
        /// <param name="nIconIndex">�ļ���ͼ���еĵڼ���,ָ��icon���������Ϊ0���ָ�����ļ��������1��icon</param>   
        /// <param name="phiconLarge">���صĴ�ͼ���ָ��,��ͼ�������Ϊnull��Ϊû�д�ͼ��</param>   
        /// <param name="phiconSmall">���ص�Сͼ���ָ��,Сͼ�������Ϊnull��Ϊû��Сͼ��</param>   
        /// <param name="nIcons">ico����,�Ҽ���ͼ��</param>   
        /// <returns></returns
        [DllImport("shell32.dll")]
        public static extern uint ExtractIconEx(string lpszFile, int nIconIndex, int[] phiconLarge, int[] phiconSmall, uint nIcons);
        [DllImport("User32.dll", EntryPoint = "DestroyIcon")]
        public static extern int DestroyIcon(IntPtr hIcon);
        /// <summary>         
        /// �ļ���Ϣ��ʶö����,����ö�ٶ���ֵǰʡ��SHGFIͶ�꣬����Icon ��������ӦΪSHGFI_ICON         
        /// </summary>         
        [Flags]
        private enum SHFileInfoFlags : uint
        {
            /// <summary>             
            /// �����е���ͼ�����ļ�ͼ�꣬�ñ�ʶ�����Iconͬʱʹ��             
            /// </summary>             
            AddOveylays = 0x20,         // SHGFI_AddOverlays = 0x000000020
                                        /// <summary>             
                                        /// ֻ��ȡ�ɲ���FileAttributeָ�����ļ���Ϣ��������д��SHFileInfo�ṹ��dwAttributes���ԣ������ָ���ñ�ʶ����ͬʱ��ȡ�����ļ���Ϣ���ñ�־���ܺ�Icon��ʶͬʱʹ��             
                                        /// </summary>             
            Attr_Specified = 0x20000,   //  SHGFI_SpecifiedAttributes = 0x000020000
                                        /// <summary>             
                                        /// ����ȡ���ļ����Ը��Ƶ�SHFileInfo�ṹ��dwAttributes������             
                                        /// </summary>             
            Attributes = 0x800,     // SHGFI_Attributes = 0x000000800
                                    /// <summary>             
                                    /// ��ȡ�ļ�����ʾ���ƣ����ļ����ƣ������临�Ƶ�SHFileInfo�ṹ��dwAttributes������             
                                    /// </summary>             
            DisplayName = 0x200,    // SHGFI_DisplayName = 0x000000200
                                    /// <summary>            
                                    /// ����ļ��ǿ�ִ���ļ�������������Ϣ��������Ϣ��Ϊ����ֵ����              
                                    /// </summary>             
            ExeType = 0x2000,       // SHGFI_EXEType = 0x000002000
                                    /// <summary>             
                                    /// ���ͼ�����������ͼ�������ص�SHFileInfo�ṹ��hIcon�����У��������ص�iIcon������             
                                    /// </summary>             
            Icon = 0x100,           // SHGFI_Icon = 0x000000100
            /// <summary>             
            /// ��������ͼ����ļ��������ļ�����ͼ������ͼ�������ţ��Żص�SHFileInfo�ṹ��             
            /// </summary>             
            IconLocation = 0x1000,  // SHGFI_IconLocation = 0x000001000
                                    /// <summary>             
                                    /// ��ô�ͼ�꣬�ñ�ʶ�����Icon��ʶͬʱʹ��             
                                    /// </summary>             
            LargeIcon = 0x0,        // SHGFI_LargeIcon = 0x000000000
                                    /// <summary>             
                                    /// ��ȡ���Ӹ����ļ�ͼ�꣬�ñ�ʶ�����Icon��ʶͬʱʹ�á�             
                                    /// </summary>             
            LinkOverlay = 0x8000,   // SHGFI_LinkOverlay = 0x000008000
                                    /// <summary>             
                                    /// ��ȡ�ļ���ʱ��ͼ�꣬�ñ�ʶ�����Icon��SysIconIndexͬʱʹ��             
                                    /// </summary>             
            OpenIcon = 0x2,         //  SHGFI_OpenIcon = 0x000000002
                                    /// <summary>             
                                    /// ��ȡ���Ӹ����ļ�ͼ���������ñ�ʶ�����Icon��ʶͬʱʹ�á�             
                                    /// </summary>             
            OverlayIndex = 0x40,    // SHGFI_OverlayIndex = 0x000000040
                                    /// <summary>             
                                    /// ָʾ�����·����һ��ITEMIDLIST�ṹ���ļ���ַ������һ��·������             
                                    /// </summary>             
            Pidl = 0x8,             // SHGFI_PIDL = 0x000000008
                                    /// <summary>             
                                    /// ��ȡϵͳ�ĸ�����ʾͼ�꣬�ñ�ʶ�����Icon��ʶͬʱʹ�á�             
                                    /// </summary>             
            Selected = 0x10000,     // SHGFI_SelectedState = 0x000010000
                                    /// <summary>             
                                    /// ��ȡ Shell-sized icon ���ñ�־�����Icon��ʶͬʱʹ�á�             
                                    /// </summary>             
            ShellIconSize = 0x4,    // SHGFI_ShellIconSize = 0x000000004
                                    /// <summary>             
                                    /// ���Сͼ�꣬�ñ�ʶ�����Icon��SysIconIndexͬʱʹ�á�             
                                    /// </summary>             
            SmallIcon = 0x1,       // SHGFI_SmallIcon = 0x000000001
                                   /// <summary>             
                                   /// ��ȡϵͳͼ���б�ͼ������������ϵͳͼ���б���             
                                   /// </summary>             
            SysIconIndex = 0x4000,  // SHGFI_SysIconIndex = 0x000004000
                                    /// <summary>             
                                    /// ����ļ����ͣ������ַ�����д��SHFileInfo�ṹ��szTypeName������             
                                    /// </summary>             
            TypeName = 0x400,       // SHGFI_TypeName = 0x000000400
                                    /// <summary>             
                                    /// ָʾ�����pszPathָ����·�������ڣ�SHGetFileInfo�����䲻����ͼȥ�����ļ���ָʾ�������ļ�������ص���Ϣ���ñ�ʶ���ܺ�Attributes��ExeType��Pidlͬʱʹ��             
                                    /// </summary>             
            UseFileAttributes = 0x10    // SHGFI_UserFileAttributes = 0x000000010,
        }

        /// <summary>         
        /// �ļ�����ö��         
        /// </summary>         
        [Flags]
        private enum FileAttribute
        {
            ReadOnly = 0x00000001,
            Hidden = 0x00000002,
            System = 0x00000004,
            Directory = 0x00000010,     //·����Ϣ             
            Archive = 0x00000020,
            Device = 0x00000040,
            Normal = 0x00000080,        //��ͨ�ļ���Ϣ             
            Temporary = 0x00000100,
            Sparse_File = 0x00000200,
            Reparse_Point = 0x00000400,
            Compressed = 0x00000800,
            Offline = 0x00001000,
            Not_Content_Indexed = 0x00002000,
            Encrypted = 0x00004000
        }

        /// <summary>        
        /// ���巵�ص��ļ���Ϣ�ṹ         
        /// </summary>         
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct SHFileInfo
        {
            /// <summary>             
            /// �ļ���ͼ����             
            /// </summary>             
            public IntPtr hIcon;
            /// <summary>             
            /// ͼ���ϵͳ������             
            /// </summary>             
            public IntPtr iIcon;
            /// <summary>             
            /// �ļ�������ֵ,��FileAttributeָ�������ԡ�             
            /// </summary>             
            public uint dwAttributes;
            /// <summary>            
            /// �ļ�����ʾ��,�����64λϵͳ����������Ҫ�ƶ�SizeConst=258����260���ܹ���ʾ������ TypeName             
            /// </summary>             
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            /// <summary>             
            /// �ļ���������             
            /// </summary>             
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }

        /// <summary>         
        /// ����ȡ�ļ�ͼ��ʧ�ܵ�Ĭ��ͼ��������         
        /// </summary>         
        public static readonly long ErrorFileIndex = -2;
        /// <summary>         
        /// ����ȡ�ļ���ͼ��ʧ�ܵ�Ĭ��ͼ��������         
        /// </summary>         
        public static readonly long ErrorFolderIndex = -4;
        /// <summary>         
        /// ����ȡ����������ͼ��ʧ�ܵ�Ĭ��ͼ��������         
        /// </summary>         
        public static readonly long ErrorDriverIndex = -8;
        /// <summary>         
        /// ����ȡ��ִ���ļ�ͼ��ʧ�ܵ�Ĭ��ͼ��������         
        /// </summary>         
        public static readonly long ErrorApplicationIndex = -16;

        /// <summary>
        /// ��ȡ�ļ����͵Ĺ���ͼ��
        /// </summary>
        /// <param name="fileName">�ļ����͵���չ�����ļ��ľ���·��</param>
        /// <param name="isSmallIcon">�Ƿ񷵻�Сͼ��</param>
        /// <returns>����һ��Icon���͵��ļ�ͼ�����</returns>
        public static Bitmap GetFileIcon(string fileName, bool isSmallIcon)
        {
            long imageIndex;
            return GetFileIcon(fileName, isSmallIcon, out imageIndex);
        }

        /// <summary>
        /// ��ȡϵͳ�ļ�ͼ��
        /// </summary>
        /// <param name="fileName">�ļ����͵���չ�����ļ��ľ���·��,�����һ��exe��ִ���ļ������ṩ�������ļ���������·����Ϣ��</param>
        /// <param name="isSmallIcon">�Ƿ񷵻�Сͼ��</param>
        /// <param name="imageIndex">����뷵��ͼ���Ӧ��ϵͳͼ��������</param>
        /// <returns>����һ��Icon���͵��ļ�ͼ�����</returns>
        public static Bitmap GetFileIcon(string fileName, bool isSmallIcon, out long imageIndex)
        {
            imageIndex = ErrorFileIndex;
            if (String.IsNullOrEmpty(fileName))
                return null;

            SHFileInfo shfi = new SHFileInfo();
            SHFileInfoFlags uFlags = SHFileInfoFlags.Icon | SHFileInfoFlags.ShellIconSize;
            if (isSmallIcon)
                uFlags |= SHFileInfoFlags.SmallIcon;
            else
                uFlags |= SHFileInfoFlags.LargeIcon;
            FileInfo fi = new FileInfo(fileName);
            if (fi.Name.ToUpper().EndsWith(".EXE"))
                uFlags |= SHFileInfoFlags.ExeType;
            else
                uFlags |= SHFileInfoFlags.UseFileAttributes;

            int iTotal = (int)SHGetFileInfo(fileName, FileAttribute.Normal, ref shfi, (uint)Marshal.SizeOf(shfi), uFlags);
            //��int iTotal = (int)SHGetFileInfo(fileName, 0, ref shfi, (uint)Marshal.SizeOf(shfi), uFlags);
            Icon icon = null;
            if (iTotal > 0)
            {
                icon = Icon.FromHandle(shfi.hIcon).Clone() as Icon;
                imageIndex = shfi.iIcon.ToInt64();
            }
            DestroyIcon(shfi.hIcon); //�ͷ���Դ
            return icon.ToBitmap();
        }

        /// <summary>  
        /// ��ȡϵͳ�ļ���Ĭ��ͼ��
        /// </summary>  
        /// <param name="isSmallIcon">�Ƿ񷵻�Сͼ��</param>
        /// <returns>����һ��Icon���͵��ļ���ͼ�����</returns>
        public static Bitmap GetFolderIcon(bool isSmallIcon)
        {
            long imageIndex;
            return GetFolderIcon(isSmallIcon, out imageIndex);
        }

        /// <summary>  
        /// ��ȡϵͳ�ļ���Ĭ��ͼ��
        /// </summary>  
        /// <param name="isSmallIcon">�Ƿ񷵻�Сͼ��</param>
        /// <param name="imageIndex">����뷵��ͼ���Ӧ��ϵͳͼ��������</param>
        /// <returns>����һ��Icon���͵��ļ���ͼ�����</returns>
        public static Bitmap GetFolderIcon(bool isSmallIcon, out long imageIndex)
        {
            return GetFolderIcon(Environment.SystemDirectory, isSmallIcon, out imageIndex);
        }

        /// <summary>  
        /// ��ȡϵͳ�ļ���Ĭ��ͼ��
        /// </summary>  
        /// <param name="folderName">�ļ�������,������ȡ�Զ����ļ���ͼ�꣬��ָ���������ļ������ƣ��� F:\test)</param>
        /// <param name="isSmallIcon">�Ƿ񷵻�Сͼ��</param>
        /// <param name="imageIndex">����뷵��ͼ���Ӧ��ϵͳͼ��������</param>
        /// <returns>����һ��Icon���͵��ļ���ͼ�����</returns>
        public static Bitmap GetFolderIcon(string folderName, bool isSmallIcon, out long imageIndex)
        {
            imageIndex = ErrorFolderIndex;
            if (String.IsNullOrEmpty(folderName))
                return null;

            SHFileInfo shfi = new SHFileInfo();
            SHFileInfoFlags uFlags = SHFileInfoFlags.Icon | SHFileInfoFlags.ShellIconSize | SHFileInfoFlags.UseFileAttributes;
            if (isSmallIcon)
                uFlags |= SHFileInfoFlags.SmallIcon;
            else
                uFlags |= SHFileInfoFlags.LargeIcon;

            int iTotal = (int)SHGetFileInfo(folderName, FileAttribute.Directory, ref shfi, (uint)Marshal.SizeOf(shfi), uFlags);
            //��int iTotal = (int)SHGetFileInfo("", 0, ref shfi, (uint)Marshal.SizeOf(shfi), SHFileInfoFlags.Icon | SHFileInfoFlags.SmallIcon);
            Icon icon = null;
            if (iTotal > 0)
            {
                icon = Icon.FromHandle(shfi.hIcon).Clone() as Icon;
                imageIndex = shfi.iIcon.ToInt64();
            }
            DestroyIcon(shfi.hIcon); //�ͷ���Դ
            return icon.ToBitmap();
        }

        /// <summary>         
        /// ��ȡ����������ͼ��
        /// </summary>         
        /// <param name="driverMark">��Ч�Ĵ��̱�ţ���C��D��I�ȵȣ������ִ�Сд</param>         
        /// <param name="isSmallIcon">��ʶ�ǻ�ȡСͼ�껹�ǻ�ȡ��ͼ��</param>         
        /// <param name="imageIndex">����뷵��ͼ���Ӧ��ϵͳͼ��������</param>         
        /// <returns>����һ��Icon���͵Ĵ���������ͼ�����</returns>         
        public static Bitmap GetDriverIcon(char driverMark, bool isSmallIcon)
        {
            long imageIndex;
            return GetDriverIcon(driverMark, isSmallIcon, out imageIndex);
        }

        /// <summary>         
        /// ��ȡ����������ͼ��
        /// </summary>         
        /// <param name="driverMark">��Ч�Ĵ��̱�ţ���C��D��I�ȵȣ������ִ�Сд</param>         
        /// <param name="isSmallIcon">��ʶ�ǻ�ȡСͼ�껹�ǻ�ȡ��ͼ��</param>         
        /// <param name="imageIndex">����뷵��ͼ���Ӧ��ϵͳͼ��������</param>         
        /// <returns>����һ��Icon���͵Ĵ���������ͼ�����</returns>         
        public static Bitmap GetDriverIcon(char driverMark, bool isSmallIcon, out long imageIndex)
        {
            imageIndex = ErrorDriverIndex;
            //����Ч�̷������ط�װ�Ĵ���ͼ��             
            if (driverMark < 'a' && driverMark > 'z' && driverMark < 'A' && driverMark > 'Z')
            {
                return null;
            }
            string driverName = driverMark.ToString().ToUpper() + ":\\";

            SHFileInfo shfi = new SHFileInfo();
            SHFileInfoFlags uFlags = SHFileInfoFlags.Icon | SHFileInfoFlags.ShellIconSize | SHFileInfoFlags.UseFileAttributes;
            if (isSmallIcon)
                uFlags |= SHFileInfoFlags.SmallIcon;
            else
                uFlags |= SHFileInfoFlags.LargeIcon;
            int iTotal = (int)SHGetFileInfo(driverName, FileAttribute.Normal, ref shfi, (uint)Marshal.SizeOf(shfi), uFlags);
            //int iTotal = (int)SHGetFileInfo(driverName, 0, ref shfi, (uint)Marshal.SizeOf(shfi), uFlags);
            Icon icon = null;
            if (iTotal > 0)
            {
                icon = Icon.FromHandle(shfi.hIcon).Clone() as Icon;
                imageIndex = shfi.iIcon.ToInt64();
            }
            DestroyIcon(shfi.hIcon); //�ͷ���Դ
            return icon.ToBitmap();
        }
    }

    public class IconReader
    {



        /// <summary>
        /// Returns an icon for a given file - indicated by the name parameter.
        /// </summary>
        /// <param name="name">Pathname for file.</param>
        /// <param name="size">Large or small</param>
        /// <param name="linkOverlay">Whether to include the link icon</param>
        /// <returns>System.Drawing.Icon</returns>
        public static Bitmap GetFileIcon(string name, ShellAPI.SHGFI flags = ShellAPI.SHGFI.SHGFI_ICON | ShellAPI.SHGFI.SHGFI_USEFILEATTRIBUTES | ShellAPI.SHGFI.SHGFI_SMALLICON)
        {
            Shell32.SHFILEINFO shfi = new Shell32.SHFILEINFO();
            Shell32.SHGetFileInfo(name, Shell32.FILE_ATTRIBUTE_NORMAL, ref shfi, (uint)Marshal.SizeOf(shfi),(uint) flags);
            Icon icon = (Icon)Icon.FromHandle(shfi.hIcon).Clone();

            User32.DestroyIcon(shfi.hIcon);     // Cleanup
            return icon.ToBitmap();
        }
        /// <summary>
        /// Used to access system folder icons.
        /// </summary>
        /// <param name="size">Specify large or small icons.</param>
        /// <param name="folderType">Specify open or closed FolderType.</param>
        /// <returns>System.Drawing.Icon</returns>
        public static Bitmap GetFolderIcon(ShellAPI.SHGFI flags = ShellAPI.SHGFI.SHGFI_ICON | ShellAPI.SHGFI.SHGFI_SMALLICON | ShellAPI.SHGFI.SHGFI_DISPLAYNAME | ShellAPI.SHGFI.SHGFI_ADDOVERLAYS | ShellAPI.SHGFI.SHGFI_SYSICONINDEX)
        {
            // Get the folder icon
            Shell32.SHFILEINFO shfi = new Shell32.SHFILEINFO();
            Shell32.SHGetFileInfo(null, Shell32.FILE_ATTRIBUTE_DIRECTORY, ref shfi, (uint)Marshal.SizeOf(shfi), (uint)flags);
            // Now clone the icon, so that it can be successfully stored in an ImageList
            Icon icon = (Icon)Icon.FromHandle(shfi.hIcon).Clone();

            User32.DestroyIcon(shfi.hIcon);     // Cleanup
            return icon.ToBitmap();
        }
    }

    /// <summary>
    /// Wraps necessary Shell32.dll structures and functions required to retrieve Icon Handles using SHGetFileInfo. Code
    /// courtesy of MSDN Cold Rooster Consulting case study.
    /// </summary>
    /// 

    // This code has been left largely untouched from that in the CRC example. The main changes have been moving
    // the icon reading code over to the IconReader type.
    public class Shell32
    {

        public const int MAX_PATH = 256;
        [StructLayout(LayoutKind.Sequential)]
        public struct SHITEMID
        {
            public ushort cb;
            [MarshalAs(UnmanagedType.LPArray)]
            public byte[] abID;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ITEMIDLIST
        {
            public SHITEMID mkid;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BROWSEINFO
        {
            public IntPtr hwndOwner;
            public IntPtr pidlRoot;
            public IntPtr pszDisplayName;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpszTitle;
            public uint ulFlags;
            public IntPtr lpfn;
            public int lParam;
            public IntPtr iImage;
        }

        // Browsing for directory.
        public const uint BIF_RETURNONLYFSDIRS = 0x0001;
        public const uint BIF_DONTGOBELOWDOMAIN = 0x0002;
        public const uint BIF_STATUSTEXT = 0x0004;
        public const uint BIF_RETURNFSANCESTORS = 0x0008;
        public const uint BIF_EDITBOX = 0x0010;
        public const uint BIF_VALIDATE = 0x0020;
        public const uint BIF_NEWDIALOGSTYLE = 0x0040;
        public const uint BIF_USENEWUI = (BIF_NEWDIALOGSTYLE | BIF_EDITBOX);
        public const uint BIF_BROWSEINCLUDEURLS = 0x0080;
        public const uint BIF_BROWSEFORCOMPUTER = 0x1000;
        public const uint BIF_BROWSEFORPRINTER = 0x2000;
        public const uint BIF_BROWSEINCLUDEFILES = 0x4000;
        public const uint BIF_SHAREABLE = 0x8000;

        [StructLayout(LayoutKind.Sequential)]
        public struct SHFILEINFO
        {
            public const int NAMESIZE = 80;
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = NAMESIZE)]
            public string szTypeName;
        };

    
        public const uint FILE_ATTRIBUTE_DIRECTORY = 0x00000010;
        public const uint FILE_ATTRIBUTE_NORMAL = 0x00000080;

        [DllImport("Shell32.dll")]
        public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, uint uFlags);
    }

    /// <summary>
    /// Wraps necessary functions imported from User32.dll. Code courtesy of MSDN Cold Rooster Consulting example.
    /// </summary>
    public class User32
    {
        /// <summary>
        /// Provides access to function required to delete handle. This method is used internally
        /// and is not required to be called separately.
        /// </summary>
        /// <param name="hIcon">Pointer to icon handle.</param>
        /// <returns>N/A</returns>
        [DllImport("User32.dll")]
        public static extern int DestroyIcon(IntPtr hIcon);
    }
}


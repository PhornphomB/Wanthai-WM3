using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prototype.Providers
{
    public static class Zip
    {
        public static bool ZipDirectory(string _directoryTarget, string _directoryPutZip)
        {
            return ZipDirectory(_directoryTarget, _directoryPutZip, string.Empty);
        }
        public static bool ZipDirectory(string _directoryTarget, string _directoryPutZip, string _dirNameInArchive)
        {
            var chk = false;
            try
            {
                using (var zipFile = new Ionic.Zip.ZipFile())
                {
                    if (_dirNameInArchive != null && _dirNameInArchive != string.Empty)
                        zipFile.AddDirectory(_directoryTarget, _dirNameInArchive);
                    else
                        zipFile.AddDirectory(_directoryTarget);

                    zipFile.Save(_directoryPutZip);
                    chk = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return chk;
        }

        public static bool ZipFile(string _directoryTarget, string _directoryPutZip)
        {
            return ZipFile(_directoryTarget, _directoryPutZip, string.Empty);
        }
        public static bool ZipFile(string _directoryTarget, string _directoryPutZip, string _dirNameInArchive)
        {
            var chk = false;
            try
            {
                using (var zipFile = new Ionic.Zip.ZipFile())
                {
                    if (_dirNameInArchive != null && _dirNameInArchive != string.Empty)
                        zipFile.AddFile(_directoryTarget, _dirNameInArchive);
                    else
                        zipFile.AddFile(_directoryTarget);

                    zipFile.Save(_directoryPutZip);
                    chk = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return chk;
        }

        public static bool ZipEntity(ZipTemp _zipTemp, string _directoryPutZip)
        {
            return ZipEntity(new[] { _zipTemp }, _directoryPutZip);
        }
        public static bool ZipEntity(ZipTemp[] _zipTemp, string _directoryPutZip)
        {
            var chk = false;
            try
            {
                using (var zipFile = new Ionic.Zip.ZipFile())
                {
                    foreach (var item in _zipTemp)
                    {
                        switch (item.TargetType)
                        {
                            case ZipTargetType.File:

                                if (item.FolderInArchive != null && item.FolderInArchive != string.Empty)
                                    zipFile.AddFile(item.DirectoryTarget, item.FolderInArchive);
                                else
                                    zipFile.AddFile(item.DirectoryTarget);
                                break;

                            case ZipTargetType.Folder:

                                if (item.FolderInArchive != null && item.FolderInArchive != string.Empty)
                                    zipFile.AddDirectory(item.DirectoryTarget, item.FolderInArchive);
                                else
                                    zipFile.AddDirectory(item.DirectoryTarget);
                                break;
                        }

                    }

                    zipFile.Save(_directoryPutZip);
                    chk = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return chk;
        }

        public static bool UnZipFile(string _directoryZipFile, string _directoryExtractFile)
        {
            var chk = false;
            try
            {
                using (var zip = Ionic.Zip.ZipFile.Read(_directoryZipFile))
                {
                    zip.ExtractAll(_directoryExtractFile, Ionic.Zip.ExtractExistingFileAction.OverwriteSilently);
                    chk = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return chk;
        }
    }

    public class ZipTemp
    {
        public string DirectoryTarget { get; set; }
        public string FolderInArchive { get; set; }
        public ZipTargetType TargetType { get; set; }
    }

    public enum ZipTargetType
    {
        File,
        Folder
    }
}

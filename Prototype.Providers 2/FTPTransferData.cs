using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace Prototype.Providers
{
    public class FTPTransferData : IDisposable
    {
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }


        private string[] GetFileList(string _ftpServerIP, string _remoteDir, string _ftpUserID, string _ftpPassword)
        {
            StringBuilder result = new StringBuilder();
            WebResponse response = null;
            StreamReader reader = null;
            try
            {
                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + _ftpServerIP + "/" + _remoteDir + "/"));
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(_ftpUserID, _ftpPassword);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                reqFTP.Proxy = null;
                reqFTP.KeepAlive = true;
                reqFTP.UsePassive = true;
                reqFTP.Timeout = 30000;

                response = reqFTP.GetResponse();
                reader = new StreamReader(response.GetResponseStream());
                string line = reader.ReadLine();

                int countRead = 0;
                while (line != null && countRead < 10)
                {
                    countRead++;

                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }

                if (countRead > 0)
                {
                    // to remove the trailing '\n'
                    result.Remove(result.ToString().LastIndexOf('\n'), 1);
                }

                return result.ToString().Split('\n');
            }
            catch (WebException ex)
            {
                String status = ((FtpWebResponse)ex.Response).StatusDescription;

                if (status == null || status == string.Empty)
                {
                    throw new Exception("GetList File : " + ex.Message);
                }
                else
                {
                    throw new Exception("GetList File : " + status);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GetList File : " + ex.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                }

                if (response != null)
                    response.Close();
            }
        }

        public void DownloadAllFile(string _FolderClientPath, string _ftpServerIP, string _remoteDir, string _FTPUser, string _FTPPass)
        {
            string[] files = GetFileList(_ftpServerIP, _remoteDir, _FTPUser, _FTPPass);
            foreach (string file in files.Where(str => !string.IsNullOrEmpty(str)))
            {
                var downloadPath = _ftpServerIP + "/" + _remoteDir + "/" + file;
                var localPath = _FolderClientPath + @"\" + file.Replace(".dat".ToLower(), ".tmp");

                DownloadFile(localPath, "ftp://" + downloadPath, _FTPUser, _FTPPass);

                DeleteFile("ftp://" + downloadPath, _FTPUser, _FTPPass);
            }
        }

        public void DownloadFile(string _FileClientPath, string _DownloadPath, string _FTPUser, string _FTPPass)
        {
            try
            {
                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(_DownloadPath));
                reqFTP.Credentials = new NetworkCredential(_FTPUser, _FTPPass);
                reqFTP.KeepAlive = true;
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Proxy = null;
                reqFTP.UsePassive = true;
                reqFTP.Timeout = 30000;

                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream responseStream = response.GetResponseStream();
                FileStream writeStream = new FileStream(_FileClientPath, FileMode.Create);
                int Length = 2048;
                Byte[] buffer = new Byte[Length];
                int bytesRead = responseStream.Read(buffer, 0, Length);
                while (bytesRead > 0)
                {
                    writeStream.Write(buffer, 0, bytesRead);
                    bytesRead = responseStream.Read(buffer, 0, Length);
                }

                writeStream.Close();
                writeStream.Dispose();

                response.Close();
            }
            catch (WebException ex)
            {
                String status = ((FtpWebResponse)ex.Response).StatusDescription;

                if (status == null || status == string.Empty)
                {
                    throw new Exception("Download File : " + ex.Message);
                }
                else
                {
                    throw new Exception("Download File : " + status);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Download File : " + ex.Message);
            }
        }

        public void UploadFile(string _FileClientPath, string _UploadPath, string _FTPUser, string _FTPPass)
        {
            try
            {
                System.IO.FileInfo _FileInfo = new System.IO.FileInfo(_FileClientPath);

                // Create FtpWebRequest object from the Uri provided
                System.Net.FtpWebRequest _FtpWebRequest = (System.Net.FtpWebRequest)System.Net.FtpWebRequest.Create(new Uri(_UploadPath));
                _FtpWebRequest.Credentials = new System.Net.NetworkCredential(_FTPUser, _FTPPass);
                _FtpWebRequest.KeepAlive = true;
                _FtpWebRequest.UsePassive = true;
                _FtpWebRequest.Timeout = 30000;
                _FtpWebRequest.Method = System.Net.WebRequestMethods.Ftp.UploadFile;
                _FtpWebRequest.UseBinary = true;
                _FtpWebRequest.ContentLength = _FileInfo.Length;

                // The buffer size is set to 2kb
                int buffLength = 2048;
                byte[] buff = new byte[buffLength];

                // Opens a file stream (System.IO.FileStream) to read the file to be uploaded
                System.IO.FileStream _FileStream = _FileInfo.OpenRead();


                // Stream to which the file to be upload is written
                System.IO.Stream _Stream = _FtpWebRequest.GetRequestStream();

                // Read from the file stream 2kb at a time
                int contentLen = _FileStream.Read(buff, 0, buffLength);

                // Till Stream content ends
                while (contentLen != 0)
                {
                    // Write Content from the file stream to the FTP Upload Stream
                    _Stream.Write(buff, 0, contentLen);
                    contentLen = _FileStream.Read(buff, 0, buffLength);
                }

                // Close the file stream and the Request Stream
                _Stream.Close();
                _Stream.Dispose();
                _FileStream.Close();
                _FileStream.Dispose();
            }
            catch (WebException ex)
            {
                String status = ((FtpWebResponse)ex.Response).StatusDescription;

                if (status == null || status == string.Empty)
                {
                    throw new Exception("Upload File : " + ex.Message);
                }
                else
                {
                    throw new Exception("Upload File : " + status);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Upload File : " + ex.Message);
            }
        }

        public void RenameFile(string _CurrentFilename, string _NewFilename, string _FTPUser, string _FTPPass)
        {
            FtpWebRequest reqFTP = null;
            Stream ftpStream = null;
            try
            {

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(_CurrentFilename));
                reqFTP.RenameTo = _NewFilename;
                reqFTP.Credentials = new NetworkCredential(_FTPUser, _FTPPass);
                reqFTP.Method = WebRequestMethods.Ftp.Rename;
                reqFTP.KeepAlive = true;
                reqFTP.UseBinary = true;
                reqFTP.Proxy = null;
                reqFTP.UsePassive = true;
                reqFTP.Timeout = 30000;

                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                ftpStream = response.GetResponseStream();

                ftpStream.Close();
                ftpStream.Dispose();

                response.Close();
            }
            catch (WebException ex)
            {
                String status = ((FtpWebResponse)ex.Response).StatusDescription;

                if (status == null || status == string.Empty)
                {
                    throw new Exception("Rename File : " + ex.Message);
                }
                else
                {
                    throw new Exception("Rename File : " + status);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Rename File : " + ex.Message);
            }
        }

        public void DeleteFile(string _DeletePath, string _FTPUser, string _FTPPass)
        {
            try
            {
                // Get the object used to communicate with the server.
                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(_DeletePath));
                reqFTP.Credentials = new NetworkCredential(_FTPUser, _FTPPass);
                reqFTP.KeepAlive = true;
                reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;
                reqFTP.UseBinary = true;
                reqFTP.Proxy = null;
                reqFTP.UsePassive = true;
                reqFTP.Timeout = 30000;

                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                response.Close();
            }
            catch (WebException ex)
            {
                String status = ((FtpWebResponse)ex.Response).StatusDescription;

                if (status == null || status == string.Empty)
                {
                    throw new Exception("Delete File : " + ex.Message);
                }
                else
                {
                    throw new Exception("Delete File : " + status);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Delete File : " + ex.Message);
            }
        }
    }
}

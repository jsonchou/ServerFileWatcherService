using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;
using System.Web.Mail;
using JsonUtil;

namespace BbsFileWatcher
{
    class My
    {
        //日志文件
        private static string logfile = @"D:\Web\bbs.52wuhan.com\upload\data\sysdata.log";
        //退出程序命令
        private const String Exit = "exit";
        //监视的文件夹
        private const String folder = @"D:\Web\bbs.52wuhan.com\upload\data\sysdata";
        private const String cloneFolder = @"D:\Web\bbs.52wuhan.com\upload\data\_cloneSysdata\sysdata";
        private static FileSystemWatcher _fsw;

        //邮件配置信息

        private static string senderServerIp = "smtp.163.com";
        private static string toMailAddress = "527426267@qq.com";
        private static string fromMailAddress = "onlyone_329@163.com";
        private static string subjectInfo = "我擦，我爱武汉网站又被人攻击了";
        private static string bodyInfo = "bbs.52wuhan.com网站又被人攻击了";
        private static string mailUsername = "onlyone_329@163.com";
        private static string mailPassword = "amwewihcv128";
        private static string mailPort = "25";
        private static string attachPath = "";

        public My(string[] args)
        {
            _fsw = new FileSystemWatcher();
            //建立检测文件夹
            if (Directory.Exists(folder))
            {
                //Directory.Delete(folder, true);
                //Directory.CreateDirectory(folder);
            }

            _fsw.Path = folder;
            _fsw.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            _fsw.Filter = "*.php";

            _fsw.Changed += new FileSystemEventHandler(OnChanged);
            _fsw.Created += new FileSystemEventHandler(OnChanged);
            _fsw.Deleted += new FileSystemEventHandler(OnChanged);
            _fsw.Renamed += new RenamedEventHandler(OnRenamed);

            _fsw.EnableRaisingEvents = true;
        }

        //处理变化时间，变化可以包含创建、修改和删除
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            var info = "文件被修改: " + e.FullPath + " " + e.ChangeType + "\r\n";
            Operation(info);
        }

        //处理重命名事件
        private void OnRenamed(object source, RenamedEventArgs e)
        {
            var info = "文件: " + e.OldFullPath + " 重命名为： " + e.FullPath + "\r\n";
            Operation(info);
        }

        //创建日志
        private static void CreateLog()
        {
            using (StreamWriter SW = File.CreateText(logfile))
            {
                StringBuilder sb2 = new StringBuilder();
                sb2.Append("日志文件创建时间: " + DateTime.Now + "\r\n");
                sb2.Append("日志文件路径: " + logfile + "\r\n");
                sb2.Append("=================================================\r\n");
                SW.WriteLine(sb2.ToString());
                SW.Close();
            }
        }

        //写日志
        private static void WriteLog(string Log)
        {

            if (!File.Exists(logfile))
            {
                CreateLog();
            }

            using (StreamWriter SW = File.AppendText(logfile))
            {
                SW.WriteLine(Log);
                SW.Close();
            }
        }

        //拷贝覆盖文件夹
        private static void CoverFolder()
        {
            string[] fileList = Directory.GetFileSystemEntries(cloneFolder);
            for (int i = 0; i < fileList.Length; i++)
            {
                var item = fileList[i];
                try
                {
                    File.Copy(item, item.Replace("\\_cloneSysdata\\", "\\"), true);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
            }
        }

        private static void SendEmail()
        {
            Email email = new Email(senderServerIp, toMailAddress, fromMailAddress, subjectInfo, bodyInfo, mailUsername, mailPassword, mailPort, false, false);
            email.AddAttachments(attachPath);
            email.Send();
        }

        //操作
        private void Operation(string info)
        {
            _fsw.EnableRaisingEvents = false;
            WriteLog(info);
            CoverFolder();
            SendEmail();
            _fsw.EnableRaisingEvents = true;
        }



    }
}

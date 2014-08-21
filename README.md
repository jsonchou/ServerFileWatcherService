ServerFileWatcherService
========================

服务器端文件监控


注册服务
========================

将当前项目工程版本的.net framework 文件夹下的installUtil.exe 复制到工程文件夹：e:/project/watcherService/bin/release(debug)/下，执行

cd e:/project/watcherService/bin/release(debug)/
installUtil BbsFileWatcher.exe

卸载服务
=======================
cd e:/project/watcherService/bin/release(debug)/
installUtil BbsFileWatcher -u

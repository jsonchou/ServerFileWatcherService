using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration.Install;
using System.ServiceProcess;
using System.ComponentModel;

namespace BbsFileWatcher
{
    [RunInstaller(true)]
    public class BbsWatcherInstall : Installer
    {
        private ServiceInstaller serviceInstaller;
        private ServiceProcessInstaller processInstaller;

        public BbsWatcherInstall()
        {
            processInstaller = new ServiceProcessInstaller();
            serviceInstaller = new ServiceInstaller();

            processInstaller.Account = ServiceAccount.LocalSystem;
            serviceInstaller.StartType = ServiceStartMode.Automatic;
            serviceInstaller.ServiceName = "BbsWatcher";

            Installers.Add(serviceInstaller);
            Installers.Add(processInstaller);
        }
    }
}

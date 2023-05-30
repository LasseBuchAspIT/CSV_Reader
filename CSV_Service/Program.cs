using System.Globalization;
using System.Runtime.InteropServices;
using CliWrap;

namespace CSV_Service
{
    static class ServiceInstaller
    {
        private const int STANDARD_RIGHTS_REQUIRED = 0xF0000;
        private const int SERVICE_WIN32_OWN_PROCESS = 0x10;



        [StructLayout(LayoutKind.Sequential)]
        private class SERVICE_STATUS
        {
            public int dwServiceType = 0;
            public ServiceState dwCurrentState = 0;
            public int dwControlsAccepted = 0;
            public int dwWin32ExitCode = 0;
            public int dwServiceSpecificExitCode = 0;
            public int dwCheckPoint = 0;
            public int dwWaitHint = 0;
        }



        [DllImport("advapi32.dll", EntryPoint = "OpenSCManagerW", ExactSpelling = true, CharSet = CharSet.Unicode, SetLastError = true)]
        private extern static IntPtr OpenSCManager(string machineName, string databaseName, ScmAccessRights dwDesiredAccess);



        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private extern static IntPtr OpenService(IntPtr hSCManager, string lpServiceName, ServiceAccessRights dwDesiredAccess);



        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private extern static IntPtr CreateService(IntPtr hSCManager, string lpServiceName, string lpDisplayName, ServiceAccessRights dwDesiredAccess, int dwServiceType, ServiceBootFlag dwStartType, ServiceError dwErrorControl, string lpBinaryPathName, string lpLoadOrderGroup, IntPtr lpdwTagId, string lpDependencies, string lp, string lpPassword);
  



        [DllImport("advapi32.dll", SetLastError = true)]
        private extern static bool CloseServiceHandle(IntPtr hSCObject);



        [DllImport("advapi32.dll")]
        private extern static int QueryServiceStatus(IntPtr hService, SERVICE_STATUS lpServiceStatus);



        [DllImport("advapi32.dll", SetLastError = true)]
        private extern static bool DeleteService(IntPtr hService);



        [DllImport("advapi32.dll")]
        private extern static int ControlService(IntPtr hService, ServiceControl dwControl, SERVICE_STATUS lpServiceStatus);


        [DllImport("advapi32.dll", SetLastError = true)]
        private extern static int StartService(IntPtr hService, int dwNumServiceArgs, int lpServiceArgVectors);



        public static void Uninstall(string serviceName)
        {
            IntPtr scm = OpenSCManager(ScmAccessRights.AllAccess);
            try
            {
                IntPtr service = OpenService(scm, serviceName, ServiceAccessRights.AllAccess);
                if (service == IntPtr.Zero)
                    throw new ApplicationException("Service not installed.");



                try
                {
                    StopService(service);
                    if (!DeleteService(service))
                        throw new ApplicationException("Could not delete service " + Marshal.GetLastWin32Error());
                }
                finally
                {
                    CloseServiceHandle(service);
                }
            }
            finally
            {
                CloseServiceHandle(scm);
            }
        }



        public static bool ServiceIsInstalled(string serviceName)
        {
            IntPtr scm = OpenSCManager(ScmAccessRights.Connect);



            try
            {
                IntPtr service = OpenService(scm, serviceName, ServiceAccessRights.QueryStatus);
                if (service == IntPtr.Zero)
                    return false;
                CloseServiceHandle(service);
                return true;
            }
            finally
            {
                CloseServiceHandle(scm);
            }
        }



        public static void Install(string serviceName, string displayName, string fileName)
        {
            IntPtr scm = OpenSCManager(ScmAccessRights.AllAccess);



            try
            {
                IntPtr service = OpenService(scm, serviceName, ServiceAccessRights.AllAccess);
                if (service == IntPtr.Zero)
                    service = CreateService(scm, serviceName, displayName, ServiceAccessRights.AllAccess, SERVICE_WIN32_OWN_PROCESS, ServiceBootFlag.AutoStart, ServiceError.Normal, fileName, null, IntPtr.Zero, null, null, null);
                if (service == IntPtr.Zero)
                    throw new ApplicationException("Failed to install service.");
            }
            finally
            {
                CloseServiceHandle(scm);
            }
        }



        public static void InstallAndStart(string serviceName, string displayName, string fileName)
        {
            IntPtr scm = OpenSCManager(ScmAccessRights.AllAccess);



            try
            {
                IntPtr service = OpenService(scm, serviceName, ServiceAccessRights.AllAccess);
                if (service == IntPtr.Zero)
                    service = CreateService(scm, serviceName, displayName, ServiceAccessRights.AllAccess, SERVICE_WIN32_OWN_PROCESS, ServiceBootFlag.AutoStart, ServiceError.Normal, fileName, null, IntPtr.Zero, null, null, null);
                if (service == IntPtr.Zero)
                    throw new ApplicationException("Failed to install service.");



                try
                {
                    StartService(service);
                }
                finally
                {
                    CloseServiceHandle(service);
                }
            }
            finally
            {
                CloseServiceHandle(scm);
            }
        }



        public static void StartService(string serviceName)
        {
            IntPtr scm = OpenSCManager(ScmAccessRights.Connect);



            try
            {
                IntPtr service = OpenService(scm, serviceName, ServiceAccessRights.QueryStatus | ServiceAccessRights.Start);
                if (service == IntPtr.Zero)
                    throw new ApplicationException("Could not open service.");



                try
                {
                    StartService(service);
                }
                finally
                {
                    CloseServiceHandle(service);
                }
            }
            finally
            {
                CloseServiceHandle(scm);
            }
        }



        public static void StopService(string serviceName)
        {
            IntPtr scm = OpenSCManager(ScmAccessRights.Connect);



            try
            {
                IntPtr service = OpenService(scm, serviceName, ServiceAccessRights.QueryStatus | ServiceAccessRights.Stop);
                if (service == IntPtr.Zero)
                    throw new ApplicationException("Could not open service.");



                try
                {
                    StopService(service);
                }
                finally
                {
                    CloseServiceHandle(service);
                }
            }
            finally
            {
                CloseServiceHandle(scm);
            }
        }



        private static void StartService(IntPtr service)
        {
            SERVICE_STATUS status = new SERVICE_STATUS();
            StartService(service, 0, 0);
            var changedStatus = WaitForServiceStatus(service, ServiceState.StartPending, ServiceState.Running);
            if (!changedStatus)
                throw new ApplicationException("Unable to start service");
        }



        private static void StopService(IntPtr service)
        {
            SERVICE_STATUS status = new SERVICE_STATUS();
            ControlService(service, ServiceControl.Stop, status);
            var changedStatus = WaitForServiceStatus(service, ServiceState.StopPending, ServiceState.Stopped);
            if (!changedStatus)
                throw new ApplicationException("Unable to stop service");
        }



        public static ServiceState GetServiceStatus(string serviceName)
        {
            IntPtr scm = OpenSCManager(ScmAccessRights.Connect);



            try
            {
                IntPtr service = OpenService(scm, serviceName, ServiceAccessRights.QueryStatus);
                if (service == IntPtr.Zero)
                    return ServiceState.NotFound;



                try
                {
                    return GetServiceStatus(service);
                }
                finally
                {
                    CloseServiceHandle(service);
                }
            }
            finally
            {
                CloseServiceHandle(scm);
            }
        }



        private static ServiceState GetServiceStatus(IntPtr service)
        {
            SERVICE_STATUS status = new SERVICE_STATUS();
            if (QueryServiceStatus(service, status) == 0)
                throw new ApplicationException("Failed to query service status.");
            return status.dwCurrentState;
        }



        private static bool WaitForServiceStatus(IntPtr service, ServiceState waitStatus, ServiceState desiredStatus)
        {
            SERVICE_STATUS status = new SERVICE_STATUS();
            QueryServiceStatus(service, status);
            if (status.dwCurrentState == desiredStatus)
                return true;
            int dwStartTickCount = Environment.TickCount;
            int dwOldCheckPoint = status.dwCheckPoint;



            while (status.dwCurrentState == waitStatus)
            {
                int dwWaitTime = status.dwWaitHint / (int)10;



                if (dwWaitTime < 1000)
                    dwWaitTime = 1000;
                else if (dwWaitTime > 10000)
                    dwWaitTime = 10000;



                Thread.Sleep(dwWaitTime);
                if (QueryServiceStatus(service, status) == 0)
                    break;



                if (status.dwCheckPoint > dwOldCheckPoint)
                {
                    dwStartTickCount = Environment.TickCount;
                    dwOldCheckPoint = status.dwCheckPoint;
                }
                else if (Environment.TickCount - dwStartTickCount > status.dwWaitHint)
                    break;
            }



            return (status.dwCurrentState == desiredStatus);
        }



        private static IntPtr OpenSCManager(ScmAccessRights rights)
        {
            IntPtr scm = OpenSCManager(null, null, rights);
            if (scm == IntPtr.Zero)
                throw new ApplicationException("Could not connect to service control manager.");
            return scm;
        }
    }



    public enum ServiceState
    {
        Unknown = -1,
        NotFound = 0,
        Stopped = 1,
        StartPending = 2,
        StopPending = 3,
        Running = 4,
        ContinuePending = 5,
        PausePending = 6,
        Paused = 7
    }



    [Flags]
    public enum ScmAccessRights
    {
        Connect = 0x1,
        CreateService = 0x2,
        EnumerateService = 0x4,
        Lock = 0x8,
        QueryLockStatus = 0x10,
        ModifyBootConfig = 0x20,
        StandardRightsRequired = 0xF0000,
        AllAccess = (ScmAccessRights.StandardRightsRequired | ScmAccessRights.Connect | ScmAccessRights.CreateService | ScmAccessRights.EnumerateService | ScmAccessRights.Lock | ScmAccessRights.QueryLockStatus | ScmAccessRights.ModifyBootConfig)
    }



    [Flags]
    public enum ServiceAccessRights
    {
        QueryConfig = 0x1,
        ChangeConfig = 0x2,
        QueryStatus = 0x4,
        EnumerateDependants = 0x8,
        Start = 0x10,
        Stop = 0x20,
        PauseContinue = 0x40,
        Interrogate = 0x80,
        UserDefinedControl = 0x100,
        Delete = 0x10000,
        StandardRightsRequired = 0xF0000,
        AllAccess = (ServiceAccessRights.StandardRightsRequired | ServiceAccessRights.QueryConfig | ServiceAccessRights.ChangeConfig | ServiceAccessRights.QueryStatus | ServiceAccessRights.EnumerateDependants | ServiceAccessRights.Start | Stop | ServiceAccessRights.PauseContinue | ServiceAccessRights.Interrogate | ServiceAccessRights.UserDefinedControl)
    }



    public enum ServiceBootFlag
    {
        Start = 0x0,
        SystemStart = 0x1,
        AutoStart = 0x2,
        DemandStart = 0x3,
        Disabled = 0x4
    }



    public enum ServiceControl
    {
        Stop = 0x1,
        Pause = 0x2,
        Continue = 0x3,
        Interrogate = 0x4,
        Shutdown = 0x5,
        ParamChange = 0x6,
        NetBindAdd = 0x7,
        NetBindRemove = 0x8,
        NetBindEnable = 0x9,
        NetBindDisable = 0xA
    }



    public enum ServiceError
    {
        Ignore = 0x0,
        Normal = 0x1,
        Severe = 0x2,
        Critical = 0x3
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            if(args.Length != 0 &&  System.Environment.UserInteractive)
            {
                string arg = args[0].ToLower();

                if (arg.Contains("/i"))
                {
                    ServiceInstaller.Install("Csv_reader", "Csv_Reader", "Csv_Reader");
                    Console.WriteLine("Installed Service");
                }
                else if(arg.Contains("/u"))
                {
                    ServiceInstaller.Uninstall("Csv_reader");
                    Console.WriteLine("Uninstalled Service");

                }
                return;
            }
            IHost host = Host.CreateDefaultBuilder(args)
                        .ConfigureServices(services =>
                        {
                            services.AddHostedService<Worker>();
                        })
                        .Build();
            CultureInfo.CurrentCulture = new("da-DK");
            Thread.CurrentThread.CurrentCulture = new("da-DK");
            host.Run();

        }
    }
}
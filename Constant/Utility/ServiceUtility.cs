using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.ServiceProcess;
using System.Text;

namespace Constant.Utility
{
    public static class ServiceUtility
    {
        internal static readonly ILogger Logger = LoggerManager.Instance.GetLogger();


        // Service
        public static bool HasService(string serviceName)
        {
            return ServiceController.GetServices().Any(s => s.ServiceName == serviceName);
        }

        /// <summary>
        /// Infinitely wait for service restart
        /// </summary>
        /// <param name="serviceName"></param>
        public static void RestartService(string serviceName)
        {
            try
            {
                using (ServiceController sc = new ServiceController(serviceName))
                {
                    sc.Restart();
                }
            }
            catch (System.Exception ex)
            {
                Logger.Error(ex);
            }
        }

        /// <summary>
        /// Waits for the service to restart or for the specific time-out to expire
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="timeout"></param>
        public static void RestartService(string serviceName, TimeSpan timeout)
        {
            try
            {
                using (ServiceController sc = new ServiceController(serviceName))
                {
                    sc.Restart(timeout);
                }
            }
            catch (System.Exception ex)
            {
                Logger.Error(ex);
            }
        }

        public static void Restart(this ServiceController sc)
        {
            if (!(sc.Status == ServiceControllerStatus.Stopped || sc.Status == ServiceControllerStatus.StopPending))
            {
                try
                {
                    sc.Stop();
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
            }

            sc.WaitForStatus(ServiceControllerStatus.Stopped);

            sc.Start();
        }

        public static void Restart(this ServiceController sc, TimeSpan timeout)
        {
            sc.Stop(timeout);

            sc.Start();
        }

        public static void Stop(this ServiceController sc, TimeSpan timeout)
        {
            if (!(sc.Status == ServiceControllerStatus.Stopped || sc.Status == ServiceControllerStatus.StopPending))
            {
                try
                {
                    sc.Stop();
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
            }

            try
            {
                sc.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
            }
            catch (System.ServiceProcess.TimeoutException ex)
            {
                sc.Kill();

                Logger.Info(ex);
            }
        }

        public static void Kill(this ServiceController sc)
        {
            KillService(sc.ServiceName);
        }

        public static void KillService(string serviceName)
        {
            try
            {
                var info = GetServiceInfo(serviceName);

                var proc = Process.GetProcessById((int)info.ProcessId);

                proc.Kill();
            }
            catch (Exception ex)
            {
                Logger.Info(ex);
            }
        }

        public static string GetPath(this ServiceController sc)
        {
            var info = GetServiceInfo(sc.ServiceName);

            var path = ProcessUtility.GetPath(info.ProcessId);

            return path;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceName"></param>
        /// <exception cref="System.InvalidOperationException"></exception>
        /// <returns></returns>
        public static ServiceInfo GetServiceInfo(string serviceName)
        {
            var wqlObjectQuery = new WqlObjectQuery(string.Format("SELECT DisplayName, Name, PathName, ProcessId FROM Win32_Service WHERE Name = '{0}'", serviceName));
            using (var managementObjectSearcher = new ManagementObjectSearcher(wqlObjectQuery))
            using (var result = managementObjectSearcher.Get())
            {
                var item = result.OfType<ManagementObject>().First();
                var info = new ServiceInfo()
                {
                    DisplayName = (string)item["DisplayName"],
                    Name = (string)item["Name"],
                    PathName = (string)item["PathName"],
                    ProcessId = (uint)item["ProcessId"],
                };

                return info;
            }
        }
    }

    public class ServiceInfo
    {
        public string DisplayName { get; set; }

        public string Name { get; set; }

        public string PathName { get; set; }

        public uint ProcessId { get; set; }
    }
}

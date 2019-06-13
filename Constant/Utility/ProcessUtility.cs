using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;

namespace Constant.Utility
{
    public static class ProcessUtility
    {
        internal static readonly ILogger Logger = LoggerManager.Instance.GetLogger();


        // Process
        public static string GetPath(this Process process)
        {
            var path = GetPath((uint)process.Id);

            return path;
        }

        public static string GetPath(uint processId)
        {
            string wmiQueryString = string.Format("SELECT ProcessId, ExecutablePath FROM Win32_Process where ProcessId = {0}", processId);
            using (var searcher = new ManagementObjectSearcher(wmiQueryString))
            using (var results = searcher.Get())
            {
                var obj = results.OfType<ManagementObject>().First();

                return (string)obj["ExecutablePath"];
            }
        }

        [DllImport("iphlpapi.dll", SetLastError = true)]
        static extern uint GetExtendedTcpTable(IntPtr pTcpTable, ref int dwOutBufLen, bool sort, int ipVersion, TcpTable tblClass, int reserved);

        public static TcpEntry[] GetAllTcpConnections()
        {
            const int AF_INET = 2; // IP_v4
            int buffSize = 0;

            // how much memory do we need?
            uint ret = GetExtendedTcpTable(IntPtr.Zero, ref buffSize, true, AF_INET, TcpTable.OwnerPidAll, 0);
            if (ret != 0 && ret != 122) // 122 insufficient buffer size
            {
                throw new Exception("bad ret on check " + ret);
            }

            TcpEntry[] tcpEntries;

            IntPtr buffTable = Marshal.AllocHGlobal(buffSize);

            try
            {
                ret = GetExtendedTcpTable(buffTable, ref buffSize, true, AF_INET, TcpTable.OwnerPidAll, 0);
                if (ret != 0)
                {
                    throw new Exception("bad ret " + ret);
                }

                // get the number of entries in the table
                var tcpTable = (TcpDataTable)Marshal.PtrToStructure(buffTable, typeof(TcpDataTable));
                IntPtr rowPtr = (IntPtr)((long)buffTable + Marshal.SizeOf(tcpTable.dwNumEntries));
                tcpEntries = new TcpEntry[tcpTable.dwNumEntries];

                for (int i = 0; i < tcpTable.dwNumEntries; i++)
                {
                    TcpEntry tcpRow = (TcpEntry)Marshal.PtrToStructure(rowPtr, typeof(TcpEntry));
                    tcpEntries[i] = tcpRow;

                    // next entry
                    rowPtr = (IntPtr)((long)rowPtr + Marshal.SizeOf(tcpRow));
                }
            }
            finally
            {
                // Free the Memory
                Marshal.FreeHGlobal(buffTable);
            }
            return tcpEntries;
        }

        public static ushort GetListeningPort(this Process process)
        {
            var tcpDataTable = GetAllTcpConnections();

            foreach (var tcpEntry in tcpDataTable)
            {
                if (tcpEntry.owningPid == process.Id)
                {
                    return tcpEntry.LocalPort;
                }
            }

            throw new Exception(string.Format("The TCP port of the process ({0}) is not found.", process.ProcessName));
        }

        public static void Kill(string processName)
        {
            try
            {
                var process = Process.GetProcessesByName(processName).FirstOrDefault();
                if (process != null)
                {
                    process.Kill();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        public static bool HasProcess(string processName)
        {
            return Process.GetProcessesByName(processName).Any();
        }
    }


    public enum TcpTable : int
    {
        BasicListener,
        BasicConnections,
        BasicAll,
        OwnerPidListener,
        OwnerPidConnections,
        OwnerPidAll,
        OwnerModuleListener,
        OwnerModuleConnections,
        OwnerModuleAll
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TcpEntry
    {
        public uint state;
        public uint localAddr;
        public byte localPort1;
        public byte localPort2;
        public byte localPort3;
        public byte localPort4;
        public uint remoteAddr;
        public byte remotePort1;
        public byte remotePort2;
        public byte remotePort3;
        public byte remotePort4;
        public int owningPid;

        public ushort LocalPort
        {
            get
            {
                return BitConverter.ToUInt16(new[] { localPort2, localPort1 }, 0);
            }
        }

        public ushort RemotePort
        {
            get
            {
                return BitConverter.ToUInt16(new[] { remotePort2, remotePort1 }, 0);
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    struct TcpDataTable
    {
        public uint dwNumEntries;
        TcpEntry table;
    }
}

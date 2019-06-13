using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Constant;

namespace Interface
{
    public abstract class DataManagerBase : IDataManager
    {
        protected ServerCredential SISCredential;

        protected IServer Server;


        protected DataManagerBase(IServer server, ServerCredential sisCredential)
        {
            Server = server;
            SISCredential = sisCredential;
        }


        public ManagerReadyState ReadyStatus { get; set; }

        public virtual string Status
        {
            get { return null; }
        }


        public virtual void Initialize() { }

        public abstract void Load();

        public abstract void Load(string xml);

        public abstract void Save();

        public abstract void Save(string xml);


        // Events
        public event EventHandler OnLoadComplete;
        protected void RaiseOnLoadComplete(EventArgs e)
        {
            var handler = OnLoadComplete;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event EventHandler OnSaveComplete;
        protected void RaiseOnSaveComplete(EventArgs e)
        {
            var handler = OnSaveComplete;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }

    public abstract class GenericDeviceManager<T> : DataManagerBase where T : IGenericDevice
    {
        // Field
        protected readonly List<T> DeviceList = new List<T>();


        // Constructor
        protected GenericDeviceManager(IServer server, ServerCredential sisCredential)
            : base(server, sisCredential)
        {

        }


        // Property
        public IEnumerable<T> Devices { get { return DeviceList; } }


        // Methods
        public void AddDevice(T device)
        {
            DeviceList.Add(device);

            OnDevicesChanged(EventArgs.Empty);
        }

        public void AddDevices(IEnumerable<T> devices)
        {
            DeviceList.AddRange(devices);
        }

        public void RemoveDevice(T device)
        {
            DeviceList.Remove(device);

            OnDevicesChanged(EventArgs.Empty);
        }

        public virtual ushort NewId()
        {
            ushort id = 1;
            var devices = DeviceList.Where(d => d.ReadyState != ReadyState.Delete && d.Id != 0).Select(d => d.Id).OrderBy(d => d);
            foreach (var deviceId in devices)
            {
                if (id != deviceId)
                {
                    return id;
                }
                id++;
            }

            return id;
        }


        // Event
        public event EventHandler DevicesChanged;
        protected void OnDevicesChanged(EventArgs e)
        {
            var handler = DevicesChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }

    public interface IGenericDevice : INotifyPropertyChanged
    {
        UInt16 Id { get; set; }
        String Name { get; set; }
        ReadyState ReadyState { get; set; }
    }
}
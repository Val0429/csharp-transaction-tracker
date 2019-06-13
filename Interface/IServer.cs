using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Constant;

namespace Interface
{
    public interface IServer
    {
        event EventHandler<EventArgs<String>> OnLoadComplete;
        event EventHandler<EventArgs<String>> OnLoadFailure;
        event EventHandler<EventArgs<String>> OnSaveComplete;
        event EventHandler<EventArgs<String>> OnSaveFailure;

        ReadyState ReadyState { get; set; }
        NVRStatus NVRStatus { get; set; }

        UInt16 Id { get; set; }
        String Name { get; set; }
        String Manufacture { get; set; }
        String Driver { get; set; }
        Form Form { get; set; }

        IUtility Utility { get; }

        Boolean IsPatrolInclude { get; set; }

        ServerCredential Credential { get; set; }
        UInt16 ServerPort { get; set; } //for Hikvision
        UInt16 ServerStatusCheckInterval { get; set; } //for iSAP Failover Server
        List<IDevice> FailoverDeviceList { get; set; }//for iSAP Failover Server device tree
        Boolean ValidateCredential();
        Boolean ValidateCredentialWithMessage();
        Dictionary<UInt16, IDevice> ReadNVRDeviceWithoutSaving();
        List<IDevice> ReadDeviceList();

        ILicenseManager License { get; }
        IServerManager Server { get; }
        IConfigureManager Configure { get; }
        IUserManager User { get; }
        IDeviceManager Device { get; }
        IIOModelManager IOModel { get; set; }
        String LoginProgress { get; set; }

        void Login();
        void Logout();
        void Save();
        void Initialize();

        void DeviceModify(IDevice device);
        void GroupModify(IDeviceGroup group);

        void WriteOperationLog(String msg);
    }
}

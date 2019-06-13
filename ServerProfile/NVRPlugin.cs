using Interface;

namespace ServerProfile
{
    //public partial class NVR
    //{
    //    public virtual INVRManager Nvrs { get; protected set; }
    //    public virtual IRegisterManager Register { get; protected set; }
    //    public virtual IPosExceptionManager PosException { get; protected set; }

    //    public virtual void NVRModify(INVR nvr)
    //    {
    //        //if (nvr.ReadyState == ReadyState.Delete)
    //        //{
    //        //    if (nvr.Utility != null)
    //        //    {
    //        //        nvr.Utility.StopEventReceive();
    //        //        nvr.Utility.StopAudioTransfer();
    //        //    }

    //        //    foreach (var obj in Device.Groups)
    //        //    {
    //        //        if (!obj.Value.Items.Any(device => (device.Server == nvr))) continue;

    //        //        var list = obj.Value.Items.FindAll(device => device.Server == nvr);
    //        //        foreach (var device in list)
    //        //        {
    //        //            obj.Value.Items.Remove(device);
    //        //        }
    //        //        if (OnGroupModify != null)
    //        //            OnGroupModify(this, new EventArgs<IDeviceGroup>(obj.Value));
    //        //    }
    //        //}

    //        //if (OnNVRModify != null)
    //        //	OnNVRModify(this, new EventArgs<INVR>(nvr));
    //    }
    //}

    //public partial class CMS
    //{
    //    public INVRManager Nvrs { get; protected set; }
    //    public IRegisterManager Register { get; protected set; }
    //    public IPosExceptionManager PosException { get; protected set; }
    //}

    //public partial class VAS
    //{
    //    public INVRManager Nvrs { get; protected set; }
    //    public IRegisterManager Register { get; protected set; }
    //    public IPosExceptionManager PosException { get; protected set; }
    //}
}

using System;
using Interface;
using NVR = App_NetworkVideoRecorder;

namespace App_ConfigurationToOrder
{
    public partial class ConfigurationToOrder : NVR.NetworkVideoRecorder
    {
        public override void ExportVideoWithInfo(IDevice[] usingDevices, DateTime start, DateTime end, String xmlInfo)
        {
            if (_exportVideoForm == null)
                _exportVideoForm = new ExportVideoForm { App = this, ExportInformation = xmlInfo };

            _exportVideoForm.ExportVideo(_nvr, usingDevices, start, end);
        }
    }
}

using System;
using Interface;
using NVR = App_NetworkVideoRecorder;

namespace App_ConfigurationToOrder
{
    public partial class ConfigurationToOrder : NVR.NetworkVideoRecorder
    {
        public ConfigurationToOrder()
        {
            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            Version += " P:" +  version.Major + "." + version.Minor.ToString().PadLeft(2, '0') + "." + version.Build.ToString().PadLeft(2, '0');
        }


        public override void ExportVideoWithInfo(IDevice[] usingDevices, DateTime start, DateTime end, String xmlInfo)
        {
            if (_exportVideoForm == null)
                _exportVideoForm = new ExportVideoForm { App = this, ExportInformation = xmlInfo };
            else
                _exportVideoForm.ExportInformation = xmlInfo;

            _exportVideoForm.ExportVideo(Nvr, usingDevices, start, end);
        }
    }
}

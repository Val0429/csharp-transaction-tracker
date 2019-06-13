using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Constant;
using DeviceCab;
using DeviceConstant;

namespace ServerProfile
{
    public partial class DeviceManager
    {
        private const String DeviceModelFile = @"data/{PRODUCTLIST}";

        public Dictionary<String, List<CameraModel>> Manufacture { get; private set; }

        private void LoadDeviceCapability()
        {
            Manufacture.Clear();
            foreach (var deviceManufacture in Server.Server.DeviceManufacture)
            {
                XmlDocument xmlDoc;
                try
                {
                    xmlDoc = Xml.LoadXmlFromFile(DeviceModelFile.Replace("{PRODUCTLIST}", deviceManufacture.Value.File));
                }
                catch(Exception)
                {
                    //xml maybe not found
                    continue;
                }

                if (xmlDoc == null) continue;
                
                var devices = xmlDoc.GetElementsByTagName("Device");
                if (devices.Count == 0) continue;
                var brand = deviceManufacture.Key;//.ToUpper(_enus);

                List<CameraModel> list;
                if (!Manufacture.ContainsKey(brand))
                {
                    list = new List<CameraModel>();
                    Manufacture.Add(brand, list);
                }
                else
                {
                    list = Manufacture[brand];
                }

                ParseCameraModel.Parse(brand, devices, list);
            }

            SortManufacture();
            SortDevice();
        }

        private void SortManufacture()
        {
            //Sort Manufacture by A-Z
            if (Manufacture.Count <= 1) return;

            var manufacture = new Dictionary<String, List<CameraModel>>(Manufacture);
            Manufacture.Clear();

            var keys = new List<String>(manufacture.Keys.ToList());
            keys.Sort();

            foreach (var key in keys)
            {
                //remove no model manufacture (maybe xml not found or parse error)
                if (manufacture[key].Count == 0) continue;

                Manufacture.Add(key, manufacture[key]);
            }

            manufacture.Clear();
            keys.Clear();
        }

        private void SortDevice()
        {
            //Sort DeviceCapability by A-Z
            if (Manufacture.Count <= 1) return;

            foreach (var obj in Manufacture)
            {
                obj.Value.Sort((x, y) => String.Compare(x.Model, y.Model));
            }
        }
    }
}
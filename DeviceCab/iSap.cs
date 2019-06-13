using System;
using System.Collections.Generic;
using System.Xml;
using Constant;
using DeviceConstant;

namespace DeviceCab
{
    public partial class ParseCameraModel
    {
        public static void ParseiSap(XmlElement node, List<CameraModel> list)
        {
            var cameraModel = new BasicCameraModel();

            var manufacture = Xml.GetFirstElementValueByTagName(node, "Manufacture");
            if (!ParseStandardCameraModel(cameraModel, node, list)) return;

            var model = Xml.GetFirstElementValueByTagName(node, "Model");

            foreach (var mode in list)
            {
                if (String.Equals(mode.Model, model)) return;
            }
            list.Add(cameraModel);

            cameraModel.Manufacture = manufacture;
            cameraModel.Model = cameraModel.Alias = model;

            cameraModel.Type = Xml.GetFirstElementValueByTagName(node, "Type");
            cameraModel.ConnectionProtocol = Array.ConvertAll(
                Xml.GetFirstElementValueByTagName(node, "ConnectionProtocol").
                    Split(','), ConnectionProtocols.ToIndex);
        }
    }
}

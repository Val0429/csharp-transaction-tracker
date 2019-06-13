using System;
using System.Collections.Generic;
using System.Xml;
using Constant;
using DeviceConstant;

namespace DeviceCab
{
    public partial class ParseCameraModel
    {
        public static void ParseKedacom(XmlElement node, List<CameraModel> list)
        {
            var cameraModel = new BasicCameraModel();
            if (!ParseStandardCameraModel(cameraModel, node, list)) return;

            var manufacture = Xml.GetFirstElementValueByTagName(node, "Manufacture");

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

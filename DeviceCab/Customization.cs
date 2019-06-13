using System.Collections.Generic;
using System.Xml;
using DeviceConstant;

namespace DeviceCab
{
    public partial class ParseCameraModel
    {
        public static void ParseCustomization(XmlElement node, List<CameraModel> list)
        {
            var cameraModel = new BasicCameraModel();
            ParseStandardCameraModel(cameraModel, node, list);

            cameraModel.CameraMode.Clear();
            cameraModel.CameraMode.Add(CameraMode.Single);
            cameraModel.CameraMode.Add(CameraMode.Dual);
            cameraModel.CameraMode.Add(CameraMode.Triple);
            cameraModel.CameraMode.Add(CameraMode.Multi);

            //var manufacture = Xml.GetFirstElementValueByTagName(node, "Manufacture");

            //var model = Xml.GetFirstElementValueByTagName(node, "Model");

            //foreach (var mode in list)
            //{
            //    if (String.Equals(mode.Model, model)) return;
            //}
            //list.Add(cameraModel);

            //cameraModel.Manufacture = manufacture;
            //cameraModel.Model = cameraModel.Alias = model;

            //cameraModel.Type = Xml.GetFirstElementValueByTagName(node, "Type");
            //cameraModel.ConnectionProtocol = Array.ConvertAll(
            //    Xml.GetFirstElementValueByTagName(node, "ConnectionProtocol").
            //        Split(','), ConnectionProtocols.ToIndex);

        }
    }
}

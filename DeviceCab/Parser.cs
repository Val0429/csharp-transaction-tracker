using System;
using System.Collections.Generic;
using System.Xml;
using DeviceConstant;

namespace DeviceCab
{
    public partial class ParseCameraModel
    {
        public static void Parse(String brand, XmlNodeList devices, List<CameraModel> list)
        {
            switch (brand)
            {
                case "ETROVISION":
                case "IPSurveillance":
                case "XTS":
                    foreach (XmlElement node in devices)
                        ParseEtroVision(node, list);
                    break;

                case "Messoa":
                    foreach (XmlElement node in devices)
                        ParseMessoa(node, list);
                    break;

                case "ZAVIO":
                    foreach (XmlElement node in devices)
                        ParseZAVIO(node, list);
                    break;

                case "Dahua":
                    foreach (XmlElement node in devices)
                        ParseDahua(node, list);
                    break;

                case "GoodWill":
                    foreach (XmlElement node in devices)
                        ParseGoodWill(node, list);
                    break;

                case "FINE":
                    foreach (XmlElement node in devices)
                        ParseFINE(node, list);
                    break;

                case "MOBOTIX":
                    foreach (XmlElement node in devices)
                        ParseMOBOTIX(node, list);
                    break;

                case "GeoVision":
                    foreach (XmlElement node in devices)
                        ParseGeoVision(node, list);
                    break;

                case "Surveon":
                    foreach (XmlElement node in devices)
                        ParseSurveon(node, list);
                    break;

                case "Certis":
                    foreach (XmlElement node in devices)
                        ParseCertis(node, list);
                    break;

                case "EverFocus":
                    foreach (XmlElement node in devices)
                        ParseEverFocus(node, list);
                    break;

                case "A-MTK":
                    foreach (XmlElement node in devices)
                        ParseAMTK(node, list);
                    break;

                case "Axis":
                    foreach (XmlElement node in devices)
                        ParseAxis(node, list);
                    break;

                case "Brickcom":
                    foreach (XmlElement node in devices)
                        ParseBrickcom(node, list);
                    break;

                case "VIGZUL":
                    foreach (XmlElement node in devices)
                        ParseVIGZUL(node, list);
                    break;

                case "ACTi":
                    foreach (XmlElement node in devices)
                        ParseACTi(node, list);
                    break;

                case "iSapSolution":
                    foreach (XmlElement node in devices)
                        ParseiSap(node, list);
                    break;

                case "YUAN":
                    foreach (XmlElement node in devices)
                        ParseYuan(node, list);
                    break;

                case "Stretch":
                    foreach (XmlElement node in devices)
                        ParseStrech(node, list);
                    break;

                case "BOSCH":
                    foreach (XmlElement node in devices)
                        ParseBOSCH(node, list);
                    break;

                case "NEXCOM":
                    foreach (XmlElement node in devices)
                        ParseNexcom(node, list);
                    break;

                case "DLink":
                    foreach (XmlElement node in devices)
                        ParseDLink(node, list);
                    break;

                case "Panasonic":
                    foreach (XmlElement node in devices)
                        ParsePanasonic(node, list);
                    break;

                case "ZeroOne":
                    foreach (XmlElement node in devices)
                        ParseZeroOne(node, list);
                    break;

                case "VIVOTEK":
                    foreach (XmlElement node in devices)
                        ParseVIVOTEK(node, list);
                    break;

                case "ArecontVision":
                    foreach (XmlElement node in devices)
                        ParseArecontVision(node, list);
                    break;

                case "MegaSys":
                    foreach (XmlElement node in devices)
                        ParseMegaSys(node, list);
                    break;

                case "Avigilon":
                    foreach (XmlElement node in devices)
                        ParseAvigilon(node, list);
                    break;

                case "DivioTec":
                    foreach (XmlElement node in devices)
                        ParseDivioTec(node, list);
                    break;

                case "SIEMENS":
                    foreach (XmlElement node in devices)
                        ParseSIEMENS(node, list);
                    break;

                case "SAMSUNG":
                    foreach (XmlElement node in devices)
                        ParseSAMSUNG(node, list);
                    break;

                case "inskytec":
                    foreach (XmlElement node in devices)
                        ParseInskytec(node, list);
                    break;

                case "HIKVISION":
                    foreach (XmlElement node in devices)
                        ParseHIKVISION(node, list);
                    break;

                case "PULSE":
                    foreach (XmlElement node in devices)
                        ParsePULSE(node, list);
                    break;

                case "ONVIF":
                    foreach (XmlElement node in devices)
                        ParseONVIF(node, list);
                    break;

                case "Kedacom":
                    foreach (XmlElement node in devices)
                        ParseKedacom(node, list);
                    break;

                case "Customization":
                    foreach (XmlElement node in devices)
                        ParseCustomization(node, list);
                    break;

                default:
                    try
                    {
                        foreach (XmlElement node in devices)
                            ParseStandard(node, list);
                    }
                    catch (Exception)
                    {
                    }
                    break;
            }
        }
    }
}

using System;
using System.Collections.Generic;

namespace DeviceConstant
{
    public enum AspectRatio : ushort
    {
        NonSpecific = 0,
        Ratio43 = 1,
        Ratio169 = 2,
        RatioAuto = 3,
        Ratio43_1300K = 4,
        Ratio43_3000K = 5,
        Ratio169_1300K = 6,
        Ratio169_2000K = 7,
        Ratio43_2000K = 8,

        Ratio169_2000K_DoublePanorama_Ceiling = 9,
        Ratio169_1000K_DoublePanorama_Ceiling = 10,
        Ratio43_1300K_QuadPTZ_Ceiling = 11,
        Ratio43_1300K_SinglePTZ_Ceiling = 12,
        Ratio_DoublePanoramaDoublePTZ_Ceiling = 13,
        Ratio_DoublePanoramaSinglePTZ_Ceiling = 14,
        Ratio43_VGA_QuadStreams_Ceiling = 15,
        Ratio43_3000K_Fisheye_Ceiling = 16,
        Ratio43_1300K_Fisheye_Ceiling = 17,
        Ratio_2000K_DoublePanoramaFisheye_Ceiling = 18,
        Ratio_1000K_DoublePanoramaFisheye_Ceiling = 19,
        Ratio_FisheyeQuadPTZ_Ceiling = 20,
        Ratio_2000K_PanoramaFisheye_Ceiling = 21,
        Ratio_1000K_PanoramaFisheye_Ceiling = 22,
        Ratio169_2000K_Panorama_Wall = 23,
        Ratio169_1000K_Panorama_Wall = 24,
        Ratio43_1300K_QuadPTZ_Wall = 25,
        Ratio43_1300K_SinglePTZ_Wall = 26,
        Ratio_PanoramaQuadPTZ_Wall = 27,
        Ratio_PanoramaSinglePTZ_Wall = 28,
        Ratio43_3000K_Fisheye_Wall = 29,
        Ratio43_1300K_Fisheye_Wall = 30,
        Ratio_2000K_DoublePanoramaFisheye_Wall = 31,
        Ratio_1000K_DoublePanoramaFisheye_Wall = 32,
        Ratio_FisheyeQuadPTZ_Wall = 33,
        Ratio_2000K_PanoramaFisheye_Wall = 34,
        Ratio_1000K_PanoramaFisheye_Wall = 35,

        Ratio43_VGA = 36,
        Ratio43_800x600 = 37
    }

    public static class AspectRatios
    {
        public static AspectRatio ToIndex(String value)
        {
            foreach (var keyValuePair in List)
            {
                if (String.Equals(value, keyValuePair.Value))
                    return keyValuePair.Key;
            }

            return 0;
        }

        public static String ToString(AspectRatio compression)
        {
            foreach (var keyValuePair in List)
            {
                if (compression == keyValuePair.Key)
                    return keyValuePair.Value;
            }

            return "";
        }

        public static readonly Dictionary<AspectRatio, String> List = new Dictionary<AspectRatio, String>
                                                             {
                                                                 { AspectRatio.NonSpecific, ""},
                                                                 { AspectRatio.Ratio43, "4:3"},
                                                                 { AspectRatio.Ratio169, "16:9"},
                                                                 { AspectRatio.RatioAuto, "Auto"},
                                                                 { AspectRatio.Ratio43_1300K, "1.3M-4:3"},
                                                                 { AspectRatio.Ratio43_3000K, "3M-4:3"},
                                                                 { AspectRatio.Ratio169_1300K, "1.3M-16:9"},
                                                                 { AspectRatio.Ratio169_2000K, "2M-16:9"},
                                                                 { AspectRatio.Ratio43_2000K, "2M-4:3"},

                                                                 { AspectRatio.Ratio169_2000K_DoublePanorama_Ceiling, "Ceiling-2M-Double Panorama-16:9"},
                                                                 { AspectRatio.Ratio169_1000K_DoublePanorama_Ceiling, "Ceiling-1M-Double Panorama-16:9"},
                                                                 { AspectRatio.Ratio43_1300K_QuadPTZ_Ceiling, "Ceiling-1.3M-Quad PTZ-4:3"},
                                                                 { AspectRatio.Ratio43_1300K_SinglePTZ_Ceiling, "Ceiling-1.3M-Single PTZ-4:3"},
                                                                 { AspectRatio.Ratio_DoublePanoramaDoublePTZ_Ceiling, "Ceiling-Double Panorama + Double PTZ"},
                                                                 { AspectRatio.Ratio_DoublePanoramaSinglePTZ_Ceiling, "Ceiling-Double Panorama + Single PTZ"},
                                                                 { AspectRatio.Ratio43_VGA_QuadStreams_Ceiling, "Ceiling-VGA-Quad streams-4:3"},
                                                                 { AspectRatio.Ratio43_3000K_Fisheye_Ceiling, "Ceiling-3M-Fisheye-4:3"},
                                                                 { AspectRatio.Ratio43_1300K_Fisheye_Ceiling, "Ceiling-1.3M-Fisheye-4:3"},
                                                                 { AspectRatio.Ratio_2000K_DoublePanoramaFisheye_Ceiling, "Ceiling-2M-Double Panorama + Fisheye"},
                                                                 { AspectRatio.Ratio_1000K_DoublePanoramaFisheye_Ceiling, "Ceiling-1M-Double Panorama + Fisheye"},
                                                                 { AspectRatio.Ratio_FisheyeQuadPTZ_Ceiling, "Ceiling-Fisheye + Quad PTZ"},
                                                                 { AspectRatio.Ratio_2000K_PanoramaFisheye_Ceiling, "Ceiling-2M-Panorama + Fisheye"},
                                                                 { AspectRatio.Ratio_1000K_PanoramaFisheye_Ceiling, "Ceiling-1M-Panorama + Fisheye"},
                                                                 { AspectRatio.Ratio169_2000K_Panorama_Wall, "Wall-2M-Panorama-16:9"},
                                                                 { AspectRatio.Ratio169_1000K_Panorama_Wall, "Wall-1M-Panorama-16:9"},
                                                                 { AspectRatio.Ratio43_1300K_QuadPTZ_Wall, "Wall-1.3M-Quad PTZ-4:3"},
                                                                 { AspectRatio.Ratio43_1300K_SinglePTZ_Wall, "Wall-1.3M-Single PTZ-4:3"},
                                                                 { AspectRatio.Ratio_PanoramaQuadPTZ_Wall, "Wall-Panorama + Quad PTZ"},
                                                                 { AspectRatio.Ratio_PanoramaSinglePTZ_Wall, "Wall-Panorama + Single PTZ"},
                                                                 { AspectRatio.Ratio43_3000K_Fisheye_Wall, "Wall-3M-Fisheye-4:3"},
                                                                 { AspectRatio.Ratio43_1300K_Fisheye_Wall, "Wall-1.3M-Fisheye-4:3"},
                                                                 { AspectRatio.Ratio_2000K_DoublePanoramaFisheye_Wall, "Wall-2M-Double Panorama + Fisheye"},
                                                                 { AspectRatio.Ratio_1000K_DoublePanoramaFisheye_Wall, "Wall-1M-Double Panorama + Fisheye"},
                                                                 { AspectRatio.Ratio_FisheyeQuadPTZ_Wall, "Wall-Fisheye + Quad PTZ"},
                                                                 { AspectRatio.Ratio_2000K_PanoramaFisheye_Wall, "Wall-2M-Panorama + Fisheye"},
                                                                 { AspectRatio.Ratio_1000K_PanoramaFisheye_Wall, "Wall-1M-Panorama + Fisheye"},
                                                                 { AspectRatio.Ratio43_VGA, "4:3-VGA"},
                                                                 { AspectRatio.Ratio43_800x600, "4:3-800x600"},
                                                             };
    }
}
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Constant.Utility
{
    public class ConvertUtility
    {
        public static bool ToBoolean(string boolString, bool defaultValue = false)
        {
            bool result;

            return bool.TryParse(boolString, out result) ? result : defaultValue;
        }

        public static double ToDouble(string number, double defaultValue = 0)
        {
            double result;

            return double.TryParse(number, out result) ? result : defaultValue;
        }

        public static float ToSingle(string number, float defaultValue = 0)
        {
            float result;

            return float.TryParse(number, out result) ? result : defaultValue;
        }

        public static byte ToByte(string number, NumberStyles numberStyles, byte defaultValue = 0)
        {
            byte result;

            return byte.TryParse(number, numberStyles, CultureInfo.InvariantCulture, out result) ? result : defaultValue;
        }

        public static UInt16 ToUInt16(string number, ushort defaultValue = 0)
        {
            ushort result;

            return ushort.TryParse(number, out result) ? result : defaultValue;
        }

        public static Int16 ToInt16(string number, short defaultValue = 0)
        {
            short result;

            return short.TryParse(number, out result) ? result : defaultValue;
        }

        public static int ToInt32(string number, int defaultValue = 0)
        {
            int result;

            return int.TryParse(number, out result) ? result : defaultValue;
        }

        /// <summary>
        /// Returns a 16-bit signed integer converted by endianness from two bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="startIndex"></param>
        /// <param name="isBigEndian"></param>
        /// <returns>A 16-bit signed integer formed by two bytes beginning at startIndex.</returns>
        public static short ToInt16(byte[] data, int startIndex, bool isBigEndian)
        {
            byte[] buffer = GetEndianOrderBytes(data, startIndex, 2, isBigEndian);

            return BitConverter.ToInt16(buffer, 0);
        }

        /// <summary>
        /// Returns a 16-bit unsigned integer converted from two bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="startIndex"></param>
        /// <param name="isBigEndian"></param>
        /// <returns></returns>
        public static ushort ToUInt16(byte[] data, int startIndex, bool isBigEndian)
        {
            byte[] buffer = GetEndianOrderBytes(data, startIndex, 2, isBigEndian);

            return BitConverter.ToUInt16(buffer, 0);
        }

        public static int ToInt32(byte[] data, int startIndex, bool isBigEndian)
        {
            byte[] buffer = GetEndianOrderBytes(data, startIndex, 4, isBigEndian);

            return BitConverter.ToInt32(buffer, 0);
        }

        /// <summary>
        /// Returns a 32-bit unsigned integer converted from four bytes at a specified
        /// position in a byte array.
        /// </summary>
        /// <param name="data">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <param name="isBigEndian">Specified endianness</param>
        /// <returns>A 32-bit unsigned integer formed by four bytes beginning at startIndex.</returns>
        public static uint ToUInt32(byte[] data, int startIndex, bool isBigEndian)
        {
            byte[] buffer = GetEndianOrderBytes(data, startIndex, 4, isBigEndian);

            return BitConverter.ToUInt32(buffer, 0);
        }

        public static long ToInt64(string number, long defaultValue = 0)
        {
            long result;

            return long.TryParse(number, out result) ? result : defaultValue;
        }

        /// <summary>
        /// Returns a 64-bit signed integer converted from eight bytes at a specified
        /// position in a byte array.
        /// </summary>
        /// <param name="data">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <param name="isBigEndian">Specified endianness</param>
        /// <returns>A 64-bit signed integer formed by eight bytes beginning at startIndex.</returns>
        public static long ToInt64(byte[] data, int startIndex, bool isBigEndian)
        {
            byte[] buffer = GetEndianOrderBytes(data, startIndex, 8, isBigEndian);

            return BitConverter.ToInt64(buffer, 0);
        }

        public static ulong ToUInt64(string number, ulong defaultValue = 0)
        {
            ulong result;

            return ulong.TryParse(number, out result) ? result : defaultValue;
        }
        /// <summary>
        /// Returns a 64-bit unsigned integer converted from eight bytes at a specified
        /// position in a byte array.
        /// </summary>
        /// <param name="data">An array of bytes.</param>
        /// <param name="startIndex">The starting position within value.</param>
        /// <param name="isBigEndian">Specified endianness</param>
        /// <returns>A 64-bit unsigned integer formed by four bytes beginning at startIndex.</returns>
        public static ulong ToUInt64(byte[] data, int startIndex, bool isBigEndian)
        {
            byte[] buffer = GetEndianOrderBytes(data, startIndex, 8, isBigEndian);

            return BitConverter.ToUInt64(buffer, 0);
        }

        private static byte[] GetEndianOrderBytes(byte[] data, int startIndex, int length, bool isBigEndian)
        {
            byte[] buffer = new byte[length];
            Array.Copy(data, startIndex, buffer, 0, length);

            if ((isBigEndian && BitConverter.IsLittleEndian) // data big, current little
            || (!isBigEndian && !BitConverter.IsLittleEndian))//data little, current big
            {
                Array.Reverse(buffer);
            }

            return buffer;
        }
    }
}

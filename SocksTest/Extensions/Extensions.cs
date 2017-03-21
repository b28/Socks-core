using System;

namespace SocksTest.Extensions
{
    public static class Extensions
    {
        public static byte[] ToArray(this ushort source)
        {
            return new byte[] { Convert.ToByte(source & 0xFF), Convert.ToByte((source >> 8) & 0xFF) };
        }

        public static ushort SwapBytes(this ushort source)
        {
            var temp = BitConverter.GetBytes(source);
            Array.Reverse(temp);
            return BitConverter.ToUInt16(temp, 0);
        }

        public static int GetSizeType(this ushort souuce)
        {
            return souuce.ToArray().Length;
        }

        public static byte[] SwapUBytes(this ushort source)
        {
            return source.SwapBytes().ToArray();
        }


    }

}
using System;

namespace SocksCore.Utils
{
    public static class NumbersExtensions
    {
        public static ushort SwapBytes(this ushort source)
        {
            var temp = BitConverter.GetBytes(source);
            Array.Reverse(temp);
            return BitConverter.ToUInt16(temp, 0);
        }
    }
}
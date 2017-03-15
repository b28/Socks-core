using System;
using System.Runtime.InteropServices;

namespace SocksCore.Utils
{
    public static class MarshalHelper
    {
        public static byte[] ToByteArray<T>(this ValueType obj) where T : struct
        {
            int len = Marshal.SizeOf(obj);

            IntPtr ptr = Marshal.AllocHGlobal(len);
            Marshal.StructureToPtr(obj, ptr, true);

            byte[] arr = new byte[len];
            Marshal.Copy(ptr, arr, 0, len);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }


        public static byte[] ToByteArray(this ValueType obj)
        {
            int len = Marshal.SizeOf(obj);

            IntPtr ptr = Marshal.AllocHGlobal(len);
            Marshal.StructureToPtr(obj, ptr, true);

            byte[] arr = new byte[len];
            Marshal.Copy(ptr, arr, 0, len);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }

        public static T ToStructure<T>(this byte[] arr) where T : struct
        {
            var str = default(T);
            int size = Marshal.SizeOf(str);
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.Copy(arr, 0, ptr, size);
            str = (T)Marshal.PtrToStructure(ptr, str.GetType());
            Marshal.FreeHGlobal(ptr);
            return str;
        }

        //public static T ByteArrayToStructure<T>(byte[] bytes) where T : struct
        //{
        //    GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
        //    T stuff = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(),
        //        typeof(T));
        //    handle.Free();
        //    return stuff;
        //}
        //public static byte[] StructureToByteArray<T>(T obj) where T : struct
        //{
        //    var len = Marshal.SizeOf(obj);
        //    var arr = new byte[len];
        //    var ptr = Marshal.AllocHGlobal(len);
        //    Marshal.StructureToPtr(obj, ptr, true);
        //    Marshal.Copy(ptr, arr, 0, len);
        //    Marshal.FreeHGlobal(ptr);
        //    return arr;
        //}


    }
}
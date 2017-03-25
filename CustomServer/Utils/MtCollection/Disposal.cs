using System;

namespace CustomServer.Utils.MtCollection
{
    public static class Disposal
    {
        public static void Dispose(object o)
        {
            IDisposable disposable = o as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }
    }
}

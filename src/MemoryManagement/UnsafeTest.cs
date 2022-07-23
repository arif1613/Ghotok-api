using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

//[DllImport(...)]

namespace MemoryManagement
{
    internal class UnsafeTest
    {

    private static extern unsafe int ExportedMethod(byte* pbData, int cbData);

    public unsafe int ManagedWrapper(Span<byte> data)
    {
        fixed (byte* pbData = &MemoryMarshal.GetReference(data))
        {
            int retVal = ExportedMethod(pbData, data.Length);

            /* error checking retVal goes here */

            return retVal;
        }
    }
}
}

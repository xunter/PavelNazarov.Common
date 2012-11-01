using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PavelNazarov.Common
{
    public static class BytesUtil
    {
        public static string BytesToString(byte[] bytes)
        {
            string bytesStr = String.Concat(bytes.Select(b => b.ToString("x2")));
            return bytesStr;
        }
    }
}

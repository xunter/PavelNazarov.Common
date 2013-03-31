using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;

namespace PavelNazarov.Common.Security
{
    /// <summary>
    /// Additional useful extension methods to the System.SecureString class
    /// </summary>
    public static class SecureStringExtensions
    {
        /// <summary>
        /// Reads a string from the memory
        /// </summary>
        /// <param name="secureString">an instance of the System.SecureString class.</param>
        /// <returns>an instance of the System.String class.</returns>
        /// <exception cref="System.ArgumentNullException" />
        public static string GetString(this SecureString secureString)
        {
            if (secureString == null) {
                throw new ArgumentNullException("secureString");
            }
            IntPtr ptr = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(secureString);
            string sDecrypString = System.Runtime.InteropServices.Marshal.PtrToStringUni(ptr);
            return sDecrypString;
        }
    }
}

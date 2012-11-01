using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace PavelNazarov.Common.Web.MVC
{
    public sealed class RawHtmlString : IHtmlString
    {
        private readonly string _html;

        public RawHtmlString(string html)
        {
            _html = html;
        }

        public string ToHtmlString()
        {
            return _html;
        }
    }
}

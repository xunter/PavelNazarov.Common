using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;
using PavelNazarov.Common.Web.MVC;

namespace PavelNazarov.Common.Web
{
    public class JavaScriptRedirectResult : JavaScriptResult
    {
        private readonly string _path;

        public JavaScriptRedirectResult(string path)
        {
            _path = path;
            Script = String.Format("window.location.href=\"{0}\";", VirtualPathUtility.ToAbsolute(_path));
        }

        public RedirectResult AsRedirectResult()
        {
            return new FakeRedirectResult(this);
        }        
    }
}

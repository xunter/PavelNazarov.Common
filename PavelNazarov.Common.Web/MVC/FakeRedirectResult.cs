using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Runtime.Serialization;
using System.Collections.Specialized;

namespace PavelNazarov.Common.Web.MVC
{
    public class FakeRedirectResult : RedirectResult
    {
        public readonly ActionResult _original;

        public FakeRedirectResult(ActionResult original)
            : base("fake")
        {
            _original = original;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            _original.ExecuteResult(context);
        }
    }
}

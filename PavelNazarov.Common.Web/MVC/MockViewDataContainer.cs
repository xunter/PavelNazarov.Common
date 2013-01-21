using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace PavelNazarov.Common.Web.MVC
{
    /// <summary>
    /// It is an implementation of the IViewDataContainer interface as mock
    /// </summary>
    public class MockViewDataContainer : IViewDataContainer
    {
        public static readonly ViewDataDictionary EmptyViewData = new ViewDataDictionary();

        public ViewDataDictionary ViewData { get; set; }

        public MockViewDataContainer()
        {
            ViewData = EmptyViewData;
        }
    }
}

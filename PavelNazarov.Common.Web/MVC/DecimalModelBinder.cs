using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Globalization;

namespace PavelNazarov.Common.Web.MVC
{
    public sealed class DecimalModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);            
            if (valueProviderResult == null)
            {
                return base.BindModel(controllerContext, bindingContext);
            }
            else
            {
                var attemptedValue = valueProviderResult.AttemptedValue;
                if (String.IsNullOrEmpty(attemptedValue))
                {
                    return null;
                }
                else if (attemptedValue == "null")
                {
                    return null;
                }
                else
                {
                    try
                    {
                        return Convert.ToDecimal(attemptedValue);
                    }
                    catch (FormatException)
                    {
                        try
                        {
                            return Convert.ToDecimal(attemptedValue, CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            return null;
                        }
                    }
                }
            }
        }
    }
}

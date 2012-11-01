using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Collections;

namespace PavelNazarov.Common.Web.MVC
{
    public static class ControllerExtensions
    {
        class ControllerWrapper : Controller
        {
            private readonly Controller _controller;

            public ControllerWrapper(Controller controller)
            {
                _controller = controller;
            }

            private ViewResult InvokeViewShared(string viewType, object model = null, string viewName = null)
            {
                Stack parameters = new Stack();
                if (model == null)
                {
                    if (viewName == null)
                    {
                    }
                    else 
                    {
                        parameters.Push(viewName);
                    }
                }
                else
                {
                    if (viewName == null)
                    {
                        parameters.Push(model);
                    }
                    else
                    {
                        parameters.Push(viewName);
                        parameters.Push(model);
                    } 
                }
                return (ViewResult)_controller.GetType().GetMethod(viewType, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic, null, parameters.Cast<Object>().Select(p => p.GetType()).ToArray(), null).Invoke(_controller, parameters.ToArray());
            }

            public ViewResult ViewShared()
            {
                return InvokeViewShared("View");
            }

            public ViewResult ViewShared(object model)
            {
                return InvokeViewShared("View", model: model);
            }

            public ViewResult ViewShared(string viewName)
            {
                return InvokeViewShared("View", viewName: viewName);
            }

            public ViewResult ViewShared(object model, string viewName)
            {
                return InvokeViewShared("View", model: model, viewName: viewName);
            }

            public ViewResult PartialViewShared()
            {
                return InvokeViewShared("PartialView");
            }

            public ViewResult PartialViewShared(object model)
            {
                return InvokeViewShared("PartialView", model: model);
            }

            public ViewResult PartialViewShared(string viewName)
            {
                return InvokeViewShared("PartialView", viewName: viewName);
            }

            public ViewResult PartialViewShared(object model, string viewName)
            {
                return InvokeViewShared("PartialView", model: model, viewName: viewName);
            }

            public RedirectResult RedirectShared(string url)
            {
                return (RedirectResult)_controller.GetType().GetMethod("Redirect", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance, null, new[] { typeof(string) }, null).Invoke(_controller, new object[] { url });
            }
        }

        public static ViewResultBase UniversalView(this Controller controllerInstance, object model = null, string viewName = null)
        {
            var controllerWrapper = new ControllerWrapper(controllerInstance);
            var Request = controllerInstance.ControllerContext.HttpContext.Request;
            if (Request.IsAjaxRequest())
            {
                if (model == null)
                {
                    if (viewName == null)
                    {
                        return controllerWrapper.PartialViewShared();
                    }
                    else
                    {
                        return controllerWrapper.PartialViewShared(viewName);
                    }
                }
                else
                {
                    if (viewName == null)
                    {
                        return controllerWrapper.PartialViewShared(model);
                    }
                    else
                    {
                        return controllerWrapper.PartialViewShared(model, viewName);
                    }
                }
            }
            else
            {
                if (model == null)
                {
                    if (viewName == null)
                    {
                        return controllerWrapper.ViewShared();
                    }
                    else
                    {
                        return controllerWrapper.ViewShared(viewName);
                    }
                }
                else
                {
                    if (viewName == null)
                    {
                        return controllerWrapper.ViewShared(model);
                    }
                    else
                    {
                        return controllerWrapper.ViewShared(model, viewName);
                    }
                }
            }
        }

        public static JavaScriptRedirectResult JavaScriptRedirect(this Controller controller, string path)
        {
            return new JavaScriptRedirectResult(path);
        }

        public static RedirectResult UniversalRedirect(this Controller controller, string url)
        {
            if (controller.ControllerContext.HttpContext.Request.IsAjaxRequest())
            {
                return new JavaScriptRedirectResult(url).AsRedirectResult();
            }
            else
            {
                return new ControllerWrapper(controller).RedirectShared(url);
            }
        }
    }
}

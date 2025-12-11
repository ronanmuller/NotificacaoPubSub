using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Linq;

namespace NotificacaoPubSub.Api.Extensions
{
    public class ApiExplorerGroupPerVersionConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            if (controller == null)
                throw new ArgumentNullException(nameof(controller));

            var controllerNamespace = controller.ControllerType.Namespace; // e.g. "Controllers.v1"
            controller.ApiExplorer.GroupName = controllerNamespace.Split('.').Last();
        }
    }
}

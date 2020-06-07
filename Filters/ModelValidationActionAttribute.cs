using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snehix.Core.API.Filters
{
    public class ModelValidationActionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            // Do something before the action executes.
            if (actionContext.ModelState.IsValid == false)
                throw new ApplicationException();
            

        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            // Do something after the action executes.            
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Snehix.Core.API.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snehix.Core.API.Filters
{
    public class CustomExceptionAttribute : ExceptionFilterAttribute
    {      
        public override void OnException(ExceptionContext context)
        {
            var request = context.HttpContext.Request;
           if(context.ModelState?.IsValid==false)
            {
                var errorDetail = GetErrorListFromModelState(context.ModelState);
                GenericResponse<string> res = new GenericResponse<string>()
                {
                    Result="Model Validation Failed.",
                    ErrorMessage=errorDetail,
                    IsSuccess=false,
                    Message="Failure.",
                    ResponseCode=500
                };                
                context.Result= new ObjectResult(res)
                {
                    StatusCode = 500,
                };
            }
           else
            {
                var errorDetail = new List<string>();
                errorDetail.Add(context.Exception.Message);
                if(context.Exception.InnerException!=null)
                    errorDetail.Add(context.Exception.InnerException.Message);
                GenericResponse<string> res = new GenericResponse<string>()
                {
                    Result = "Server error.",
                    ErrorMessage = errorDetail,
                    IsSuccess = false,
                    Message = "Failure.",
                    ResponseCode = 500
                };
                context.Result = new ObjectResult(res)
                {
                    StatusCode = 500,
                };
            }
        }

        List<string> GetErrorListFromModelState(ModelStateDictionary modelState)
        {
            var query = from state in modelState.Values
                        from error in state.Errors
                        select error.ErrorMessage;

            var errorList = query.ToList();
            return errorList;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snehix.Core.API.DTO
{
    public class GenericResponse<T>
    {
        public string Message { get; set; }
        public List<string> ErrorMessage { get; set; }
        public T Result { get; set; }
        public bool IsSuccess { get; set; }
        public int ResponseCode { get; set; }
    }
}

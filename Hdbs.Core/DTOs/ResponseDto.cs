using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Core.DTOs
{
    public class ResponseDto<T>
    {
        public T? Data { get; set; }
        public ErrorDto? Error { get; set; }
    }
}

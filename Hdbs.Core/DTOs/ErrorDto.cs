using Hdbs.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Core.DTOs
{
    public class ErrorDto
    {
        public CustomErrorCode ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }
    }
}

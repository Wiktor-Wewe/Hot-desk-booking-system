using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hdbs.Transfer.Employees.Queries
{
    public class GetImportExcelEmployeeQuery : IRequest<MemoryStream>
    {
    }
}

using ShopManagment.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagment.Services.Interface
{
    public interface IReportInterface
    {
        List<Report> GenarateReport(string type);
    }
}

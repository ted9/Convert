using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Convert.Model;

namespace Convert.Service.Interface
{
    public interface IChequeWritingService
    {
        ChequeModel Translate(ChequeModel amount);
    }
}

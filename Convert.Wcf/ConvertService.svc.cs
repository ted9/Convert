using Convert.Infrastruture.Logging;
using Convert.Model;
using Convert.Service.Interface;
using Convert.Wcf.DataContract;
using Convert.Wcf.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

namespace Convert.Wcf
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceContract]
    public class ConvertService : IConvertService
    {
        protected IChequeWritingService ChequeService { get; private set; }
        protected ILogger Logger { get; private set; }
        public ConvertService(ILogger logger, IChequeWritingService service)
        {
            this.ChequeService = service;
            this.Logger = logger;
        }
        [OperationContract]
        public ChequeModel Translate(ChequeDataModel model)
        {
            try
            {
                var result = ChequeService.Translate(new ChequeModel()
                {
                    UserName = model.UserName,
                    Amount = model.Amount
                });

                return result;
            }
            catch (Exception ex)
            {
               Logger.LogException(ex);
               return null;
            }
        }
    }
}

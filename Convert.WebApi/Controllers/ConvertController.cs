using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Convert.Infrastruture.Logging;
using Convert.Model;
using Convert.Service.Interface;
using Convert.WebApi.Controllers.Base;
using System.Web.Http.Results;
using Convert.WebApi.Models;

namespace Convert.WebApi.Controllers
{
    [RoutePrefix("api/convert")]
    public class ConvertController : BaseController
    {
        protected IChequeWritingService ChequeService { get; private set; }
        protected ILogger Logger { get; private set; }
        protected bool IsDebugging { get; private set; }
        public ConvertController(ILogger logger, IChequeWritingService service) : base(logger)
        {
            this.ChequeService = service;
            this.Logger = logger;
            this.IsDebugging = (ConfigurationManager.AppSettings["Debugging"].ToLower() == "true");
        }

        [HttpGet]
        [Route("Translate")]
        public IHttpActionResult GetConvertedChequeModel([FromUri] ChequeDataModel model)
        {
            try
            {
                var result = ChequeService.Translate(new ChequeModel()
                {
                    UserName = model.UserName,
                    Amount = model.Amount
                });
                if (IsDebugging)
                {
                    Logger.LogMessage(string.Format("[{0}] To [{1}]", model.Amount,
                        result.AmountString));
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return BadRequest();
            }
        }
    }
}



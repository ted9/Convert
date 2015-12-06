using Convert.Infrastruture.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Convert.WebApi.Controllers.Base
{
    public class BaseController : ApiController
    {
        protected ILogger Logger { get; private set; }
        public BaseController(ILogger logger)
        {
            this.Logger = logger;
        }

        protected void HandleException(Exception ex)
        {
            Logger.LogException(ex);
        }
    }
}

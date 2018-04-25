using System;
using System.ServiceModel;
using System.Net.Http;
using System.Net;
using System.Web.Http;
using System.Data.Entity.Infrastructure;

namespace Wollo.Base.Api
{
    public class BaseApiController : ApiController, IBaseApiController
    {
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        protected void CleanChannel(IClientChannel clientChannel)
        {
            if (clientChannel != null)
            {
                if (clientChannel.State != CommunicationState.Closed)
                {
                    clientChannel.Abort();
                    clientChannel.Close();
                }

                clientChannel.Dispose();
            }
        }

        protected HttpResponseMessage CreateHttpResponse(HttpRequestMessage request, Func<HttpResponseMessage> function)
        {
            HttpResponseMessage response = null;

            try
            {
                response = function.Invoke();
            }
            catch (DbUpdateException error)
            {
                response = request.CreateResponse(HttpStatusCode.BadRequest, error.InnerException.Message);
            }
            catch (Exception error)
            {
                //response = request.CreateResponse(HttpStatusCode.InternalServerError, error.Message);

                String innerMessage = (error.InnerException != null)
                      ? error.InnerException.Message
                      : error.Message;
                response = request.CreateResponse(HttpStatusCode.InternalServerError, innerMessage);
            }

            return response;
        }
    }
}

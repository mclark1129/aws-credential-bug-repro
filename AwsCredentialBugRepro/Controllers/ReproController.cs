using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.AspNetCore.Mvc;

namespace AwsCredentialBugRepro.Controllers
{
    [Route("repro")]
    public class ReproController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            var config = new AmazonSQSConfig()
            {
                RegionEndpoint = RegionEndpoint.USEast1
            };

            // In IIS Express, sqsClient is initialized with the correct BasicAwsCredentials specified
            // in my local credential file
            // When attempting to use IIS, the client is initialized with DefaultInstanceProfileAWSCredentials.
            // This results in a null reference exception when attempting to perform any operations against SQS.
            var sqsClient = new AmazonSQSClient(config);
            var req = new ListQueuesRequest();
            var rsp = sqsClient.ListQueues(req);

            return Ok(rsp.QueueUrls);
        }
    }
}

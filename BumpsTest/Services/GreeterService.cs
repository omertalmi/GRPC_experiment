using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.Core;
using BumpsTest;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;

namespace BumpsTest.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;

        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }
        
        public override Task<CalculateResponse> CalculateSum(CalculateRequest request, ServerCallContext context)
        {
            return Task.FromResult(new CalculateResponse
            {
                Result = request.X + request.Y
            });
        }

        public override async Task<GetBumpsResponse> GetBumps(GetBumpsRequest request, IServerStreamWriter<GetBumpsResponse> responseStream,
            ServerCallContext context)
        {
            string s = String.Format("DIES: [{0}], bumps per die: [{1}]", request.NumberOfDies, request.BumpsInDie);
            Console.WriteLine(s);
            var limit = 3000;
            for (var i = 0; i < request.NumberOfDies; i++)
            {
                var total_sent = 0;
                int multi_id = 0;
                while (total_sent < request.BumpsInDie)
                {
                    List<Bump> bumpsList = new List<Bump>();
                    for (var j = 0; j < Math.Min(limit, request.BumpsInDie-total_sent); j++)
                    {
                        Bump b = new Bump();
                        b.Id = j + multi_id * limit;
                        b.DieXCord= 243;
                        b.DieYCord = 242;
                        b.WaferXCord = 14;
                        b.WaferYCord = 15;
                        b.Type = 3;
                        b.Height = (float) 15.2231;
                        b.Cop = (float) 0.00232;
                        b.DieId = i;
                        b.WaferId = Guid.NewGuid().ToString();
                        bumpsList.Add(b);
                    }
                    multi_id = multi_id + 1;
                    total_sent = total_sent + limit;
                    var reply = new GetBumpsResponse();
                    reply.Bump.AddRange(bumpsList);
                    reply.DieId = i;
                    await responseStream.WriteAsync(reply);
                }
            }
            var lastResponse = new GetBumpsResponse();
            lastResponse.DieId = -1;
            Console.WriteLine("FINISH STREAMING");
            return lastResponse;
        }
    }
}
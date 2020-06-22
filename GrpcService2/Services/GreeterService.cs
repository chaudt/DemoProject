using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;

namespace GrpcService2
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override async Task SayHello(HelloRequest request, IServerStreamWriter<HelloReply> responseStream, ServerCallContext context)
        {
            await responseStream.WriteAsync(new HelloReply { Message = $"Service 2 response data: {request.Name}" });
            // gọi ngược lại Service1 để lấy dữ liệu rồi thực thi
            Console.WriteLine("Service2: receive data from service 1");
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new GrpcService1.Student.StudentClient(channel);
            using (var call = client.BrowseStudent(new GrpcService1.BrowseStudentRequest  {Count = 100 }))
            {
                while (await call.ResponseStream.MoveNext())
                {
                    await new StudentExecute().Execute(call.ResponseStream.Current);
                }
            }

            
        }
    }
    public class StudentExecute
    {
        public async Task Execute(GrpcService1.BrowseStudentReply model)
        {
            
            Console.WriteLine($"{model.StudentId} - {model.StudentName} - {model.Address}");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;

namespace GrpcService1.Services
{
    public class StudentService : Student.StudentBase
    {
        public StudentService()
        {

        }
        public override async Task BrowseStudent(BrowseStudentRequest request, IServerStreamWriter<BrowseStudentReply> responseStream, ServerCallContext context)
        {
            for (int i = 0; i < request.Count; i++)
            {
                await responseStream.WriteAsync(new BrowseStudentReply
                {
                    StudentId = (i + 1),
                    Address = $"Address {(i + 1)}",
                    StudentName = $"Student Name {(i + 1)}"
                });
            }
        }
    }
}

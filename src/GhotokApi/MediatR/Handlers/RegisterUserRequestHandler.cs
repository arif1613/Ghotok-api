using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Ghotok.Data.DataModels;
using GhotokApi.Repo;
using GhotokApi.Utils.Authentication;
using MediatR;

namespace GhotokApi.MediatR.Handlers
{
    public class RegisterUserRequestHandler : IRequestHandler<RegisterUserRequest, string>
    {
        private readonly ILoginFlow _loginFlow;


        public RegisterUserRequestHandler(IUnitOfWork unitOfWork, ILoginFlow loginFlow)
        {
            _loginFlow = loginFlow;
        }

        public async Task<string> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    await _loginFlow.RegisterUserAsync(request.UserToRegister);
                    return "Done";
                }
                catch (Exception e)
                {
                    return e.Message;
                }
               
            });
        }
    }

    public class RegisterUserRequest : IRequest<string>
    {
        public AppUser UserToRegister{ get; set; }
    }
}

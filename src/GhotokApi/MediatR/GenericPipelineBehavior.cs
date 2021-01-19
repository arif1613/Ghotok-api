using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GhotokApi.Repo;
using GhotokApi.Utils.Authentication;
using MediatR;

namespace GhotokApi.MediatR
{
    public class GenericPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly ILoginFlow _loginFlow;


        public GenericPipelineBehavior(UnitOfWork unitOfWork, ILoginFlow loginFlow)
        {
            _unitOfWork = unitOfWork;
            _loginFlow = loginFlow;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var response = await next();
            return response;
        }
    }
}

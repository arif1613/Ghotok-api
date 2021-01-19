using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Ghotok.Data.DataModels;
using GhotokApi.Repo;
using MediatR;

namespace GhotokApi.MediatR.Handlers
{
    public class ComitDatabaseRequestHandler : IRequestHandler<ComitDatabaseRequest>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ComitDatabaseRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(ComitDatabaseRequest request, CancellationToken cancellationToken)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    _unitOfWork.Commit();
                    return Unit.Value;

                }
                catch (Exception e)
                {
                    throw e;
                }
               
            });
        }
    }

    public class ComitDatabaseRequest : IRequest
    {
    }
}

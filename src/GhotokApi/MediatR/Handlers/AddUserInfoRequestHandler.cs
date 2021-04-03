using System;
using System.Threading;
using System.Threading.Tasks;
using Ghotok.Data.DataModels;
using Ghotok.Data.Repo;
using Ghotok.Data.UnitOfWork;
using GhotokApi.MediatR.NotificationHandlers;
using MediatR;

namespace GhotokApi.MediatR.Handlers
{
    public class AddUserInfoRequestHandler : IRequestHandler<AddUserInfoRequest, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;


        public AddUserInfoRequestHandler(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<string> Handle(AddUserInfoRequest request, CancellationToken cancellationToken)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    _unitOfWork.UserRepository.Insert(request.UserToAdd);
                    await _mediator.Publish(new ComitDatabaseNotification(), cancellationToken);
                    return "Done";
                }
                catch (Exception e)
                {
                    return e.Message;
                }
               
            });
        }
    }

    public class AddUserInfoRequest : IRequest<string>
    {
        public User UserToAdd{ get; set; }
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;
using Ghotok.Data.DataModels;
using Ghotok.Data.Repo;
using Ghotok.Data.UnitOfWork;
using MediatR;

namespace GhotokApi.MediatR.Handlers
{
    public class UpdateUserInfoRequestHandler:IRequestHandler<UpdateUserInfoRequest, string>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserInfoRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(UpdateUserInfoRequest request, CancellationToken cancellationToken)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    _unitOfWork.UserRepository.Update(request.UserTobeUpdated);
                    return "Done";
                }
                catch (Exception e)
                {
                    return e.Message;
                }
               
            });
        }
    }

    public class UpdateUserInfoRequest : IRequest<string>
    {
        public User UserTobeUpdated { get; set; }
    }
}

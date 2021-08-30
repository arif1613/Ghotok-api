using System;
using System.Threading;
using System.Threading.Tasks;
using Ghotok.Data.DataModels;
using MediatR;
using QQuery.UnitOfWork;

namespace GhotokApi.MediatR.Handlers
{
    public class AddAppUserRequestHandler : IRequestHandler<AddAppUserRequest, string>
    {
        private readonly IQqService<AppUser> _unitOfWork;

        public AddAppUserRequestHandler(IQqService<AppUser> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(AddAppUserRequest request, CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
            {
                try
                {
                    _unitOfWork.QqRepository.Insert(request.AppUserToAdd);
                    return "Done";
                }
                catch (Exception e)
                {
                    return e.Message;
                }
               
            });
        }

       
    }

    public class AddAppUserRequest : IRequest<string>
    {
        public AppUser AppUserToAdd{ get; set; }
    }


}

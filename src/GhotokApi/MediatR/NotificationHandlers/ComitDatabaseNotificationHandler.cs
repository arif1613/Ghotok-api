using System;
using System.Threading;
using System.Threading.Tasks;
using Ghotok.Data.DataModels;
using MediatR;
using QQuery.UnitOfWork;

namespace GhotokApi.MediatR.NotificationHandlers
{
    public class ComitDatabaseNotificationHandler : INotificationHandler<ComitDatabaseNotification>
    {
        private readonly IQqService<AppUser> _unitOfWork;

        public ComitDatabaseNotificationHandler(IQqService<AppUser> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(ComitDatabaseNotification request, CancellationToken cancellationToken)
        {

             await Task.Run(() =>
            {
                try
                {
                    _unitOfWork.Commit();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }, cancellationToken);


        }
    }

    public class ComitDatabaseNotification : INotification
    {
    }


}

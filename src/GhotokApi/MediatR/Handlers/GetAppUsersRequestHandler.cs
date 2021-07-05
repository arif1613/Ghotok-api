using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ghotok.Data.DataModels;
using Ghotok.Data.UnitOfWork;
using GhotokApi.Common;
using GhotokApi.Models.RequestModels;
using GhotokApi.Utils.FilterBuilder;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhotokApi.MediatR.Handlers
{
    public class GetAppUsersRequestHandler : IRequestHandler<GetAppUsersRequest, List<AppUser>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFilterBuilder _filterBuilder;

        public GetAppUsersRequestHandler(IUnitOfWork unitOfWork, IFilterBuilder filterBuilder)
        {
            _unitOfWork = unitOfWork;
            _filterBuilder = filterBuilder;
        }

        public async Task<List<AppUser>> Handle(GetAppUsersRequest request, CancellationToken cancellationToken)
        {
            IEnumerable<AppUser> appusers;

            try
            {

                if (request.model.HasInclude)
                {
                    appusers = _unitOfWork.AppUseRepository.Get(
                        _filterBuilder.GetAppUserFilter(request.model.Filters),
                        null,
                        include: s => s
                            .Include(a => a.User)
                            .Include(a => a.User.BasicInfo)
                            .Include(a => a.User.EducationInfo).ThenInclude(b => b.Educations)
                            .Include(a => a.User.EducationInfo).ThenInclude(b => b.CurrentJob)
                            .Include(a => a.User.FamilyInfo).ThenInclude(d => d.FamilyMembers));

                    var enumerable = appusers as AppUser[] ?? appusers.ToArray();
                    

                }
                else
                {
                    appusers = await Task.Run(() => _unitOfWork.AppUseRepository.Get(
                        _filterBuilder.GetAppUserFilter(request.model.Filters), null, null));

                    var enumerable = appusers as AppUser[] ?? appusers.ToArray();
                    
                }
            }
            catch (Exception e)
            {
                return null;
            }

            List<AppUser> list = new List<AppUser>();
            foreach (var appuser in appusers) list.Add(appuser);
            return list;

        }


    }

    public class GetAppUsersRequest : IRequest<List<AppUser>>
    {
        public AppUserInfosRequestModel model { get; set; }
    }


}

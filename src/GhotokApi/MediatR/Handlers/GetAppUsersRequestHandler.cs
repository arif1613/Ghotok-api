using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ghotok.Data.DataModels;
using GhotokApi.Models.RequestModels;
using GhotokApi.Utils.FilterBuilder;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QQuery.Helper;
using QQuery.UnitOfWork;

namespace GhotokApi.MediatR.Handlers
{
    public class GetAppUsersRequestHandler : IRequestHandler<GetAppUsersRequest, List<AppUser>>
    {
        private readonly IQqService<AppUser> _unitOfWork;
        private readonly IFilterBuilder _filterBuilder;

        public GetAppUsersRequestHandler(IQqService<AppUser> unitOfWork, IFilterBuilder filterBuilder)
        {
            _unitOfWork = unitOfWork;
            _filterBuilder = filterBuilder;
        }

        public async Task<List<AppUser>> Handle(GetAppUsersRequest request, CancellationToken cancellationToken)
        {
            IEnumerable<AppUser> appUsers = null;
            if (request.model.HasInclude && request.model.HasOrderBy)
            {
                appUsers = await Task.Run(() => _unitOfWork.QqRepository.Get(
                   _filterBuilder.GetAppUserFilter(request.model.Filters),
                    orderBy: source => source.OrderBy(r => r.User.BasicInfo.Name),
                    include: s => s
                        .Include(r => r.User)
                        .ThenInclude(a => a.BasicInfo)
                        .Include(r => r.User)
                        .ThenInclude(b => b.EducationInfo).ThenInclude(b => b.Educations)
                        .Include(r => r.User)
                        .ThenInclude(b => b.EducationInfo).ThenInclude(b => b.CurrentJob)
                        .Include(r => r.User)
                        .ThenInclude(c => c.FamilyInfo).ThenInclude(c => c.FamilyMembers),
                    request.model.StartIndex, request.model.ChunkSize));
                return appUsers.ToList();

            }

            if (request.model.HasInclude)
            {
                appUsers = await Task.Run(() => _unitOfWork.QqRepository.Get(
                    _filterBuilder.GetAppUserFilter(request.model.Filters),
                    null,
                    include: s => s
                        .Include(a => a.User.BasicInfo)
                        .Include(a => a.User.EducationInfo).ThenInclude(b => b.Educations)
                        .Include(a => a.User.EducationInfo).ThenInclude(b => b.CurrentJob)
                        .Include(a => a.User.FamilyInfo).ThenInclude(d => d.FamilyMembers),
                     request.model.StartIndex, request.model.ChunkSize));
                return appUsers.ToList();


            }

            if (request.model.HasOrderBy)
            {
                appUsers = await Task.Run(() => _unitOfWork.QqRepository.Get(
                    _filterBuilder.GetAppUserFilter(request.model.Filters),
                    orderBy: source => source.OrderBy(r => r.User.BasicInfo.Name),
                    null, request.model.StartIndex, request.model.ChunkSize));
                return appUsers.ToList();

            }

            appUsers = await Task.Run(() => _unitOfWork.QqRepository.Get(
                null,
                null, null, request.model.StartIndex, request.model.ChunkSize));
            return appUsers.ToList();

        }


    }

    public class GetAppUsersRequest : IRequest<List<AppUser>>
    {
        public AppUserInfosRequestModel model  { get; set; }
    }


}

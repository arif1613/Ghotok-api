using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ghotok.Data.DataModels;
using GhotokApi.Models.RequestModels;
using GhotokApi.Utils.FilterBuilder;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QQuery.UnitOfWork;

namespace GhotokApi.MediatR.Handlers
{
    public class GetUserRequestHandler : IRequestHandler<GetUserRequest, User>
    {
        private readonly IQqService<User> _unitOfWork;
        private readonly IFilterBuilder _filterBuilder;

        public GetUserRequestHandler(IQqService<User> unitOfWork, IFilterBuilder filterBuilder)
        {
            _unitOfWork = unitOfWork;
            _filterBuilder = filterBuilder;
        }

        public async Task<User> Handle(GetUserRequest request, CancellationToken cancellationToken)
        {
            User user = null;

            try
            {
                IEnumerable<User> users;

                if (request.model.HasInclude)
                {
                    users = _unitOfWork.QqRepository.Get(
                        _filterBuilder.GetUserFilter(request.model.Filters),
                        null,
                        include: s => s
                            .Include(a => a.BasicInfo)
                            .Include(a => a.EducationInfo).ThenInclude(b => b.Educations)
                            .Include(a => a.EducationInfo).ThenInclude(b => b.CurrentJob)
                            .Include(a => a.FamilyInfo).ThenInclude(d => d.FamilyMembers));

                    var enumerable = users as User[] ?? users.ToArray();
                    if (enumerable.Any())
                    {
                        user = enumerable.FirstOrDefault();
                    }

                }
                else
                {
                    users = await Task.Run(() => _unitOfWork.QqRepository.Get(
                        _filterBuilder.GetUserFilter(request.model.Filters), null, null));

                    var enumerable = users as User[] ?? users.ToArray();
                    if (enumerable.Any())
                    {
                        user = enumerable.FirstOrDefault();
                    }
                }
            }
            catch (Exception e)
            {
                return null;
            }

            return user;

        }


    }

    public class GetUserRequest : IRequest<User>
    {
        public UserInfoRequestModel model { get; set; }
    }


}

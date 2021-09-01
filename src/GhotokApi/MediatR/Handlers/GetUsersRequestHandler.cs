﻿using System.Collections.Generic;
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
    public class GetUsersRequestHandler : IRequestHandler<GetUsersRequest, List<User>>
    {
        private readonly IQqService<User> _unitOfWork;
        private readonly IFilterBuilder _filterBuilder;

        public GetUsersRequestHandler(IQqService<User> unitOfWork, IFilterBuilder filterBuilder)
        {
            _unitOfWork = unitOfWork;
            _filterBuilder = filterBuilder;
        }

        public async Task<List<User>> Handle(GetUsersRequest request, CancellationToken cancellationToken)
        {
            IEnumerable<User> users;
            if (request.model.HasInclude && request.model.HasOrderBy)
            {
                users = await Task.Run(() => _unitOfWork.QqRepository.Get(
                    _filterBuilder.GetUserFilter(request.model.Filters),
                    orderBy: source => source.OrderByDescending(r => r.ValidFrom),
                    include: s => s
                        .Include(a => a.BasicInfo)
                        .Include(a => a.EducationInfo).ThenInclude(b => b.Educations)
                        .Include(a => a.EducationInfo).ThenInclude(b => b.CurrentJob)
                        .Include(a => a.FamilyInfo).ThenInclude(d => d.FamilyMembers)));
                return users.ToList();
            }

            if (request.model.HasInclude)
            {
                users = await Task.Run(() => _unitOfWork.QqRepository.Get(
                    _filterBuilder.GetUserFilter(request.model.Filters),
                    null, include: s => s
                        .Include(a => a.BasicInfo)
                        .Include(a => a.EducationInfo).ThenInclude(b => b.Educations)
                        .Include(a => a.EducationInfo).ThenInclude(b => b.CurrentJob)
                        .Include(a => a.FamilyInfo).ThenInclude(d => d.FamilyMembers),
                     request.model.StartIndex, request.model.ChunkSize));
                return users.ToList();


            }

            if (request.model.HasOrderBy)
            {
                users = await Task.Run(() => _unitOfWork.QqRepository.Get(
                    _filterBuilder.GetUserFilter(request.model.Filters),
                    orderBy: source => source.OrderBy(r => r.BasicInfo.Name),
                    null, request.model.StartIndex, request.model.ChunkSize));
                return users.ToList();

            }


            users = await Task.Run(() => _unitOfWork.QqRepository.Get(
                _filterBuilder.GetUserFilter(request.model.Filters),
                null, null, request.model.StartIndex, request.model.ChunkSize));
            return users.ToList();

        }


    }

    public class GetUsersRequest : IRequest<List<User>>
    {
        public UserInfosRequestModel model  { get; set; }
    }


}

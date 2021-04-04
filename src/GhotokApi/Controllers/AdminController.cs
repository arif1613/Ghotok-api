using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ghotok.Data.DataModels;
using Ghotok.Data.UnitOfWork;
using GhotokApi.Models.NotificationModels;
using GhotokApi.Models.SharedModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GhotokApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;


        public AdminController(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("gettokenvalue")]
        public IActionResult GetRouteValue()
        {
            var dict = new Dictionary<string, string>();
            HttpContext.User.Claims.ToList().ForEach(item => dict.Add(item.Type, item.Value));
            return Ok(dict);
        }

        [HttpPost]
        [Route("bulkinsertgroom")]

        public async Task<IActionResult> BulkInsertGroomData([FromBody] int totalrecord)
        {
            var appusers = new List<AppUser>();
            var users = new List<User>();


            for (int i = 0; i < totalrecord; i++)
            {

                var role = AppRole.User.ToString();
                var validtill = GetValidTill(role);

                var appuser = new AppUser
                {
                    Id = Guid.NewGuid(),
                    CountryCode = $"+{i}",
                    MobileNumber = $"{i}{i}{i}",
                    LoggedInDevices = 1,
                    IsVarified = true,
                    LookingForBride = true,
                    UserRole = role,
                    Password = "123456",
                    Email = $"{i}@ghotok.com",
                    RegisterByMobileNumber = false,
                    LanguageChoice = Language.English,
                    ValidFrom = DateTime.UtcNow,
                    ValidTill = validtill
                };
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    IsPublished = true,
                    LookingForBride = true,
                    MobileNumber = $"{i}{i}{i}",
                    Email = $"{i}@ghotok.com",
                    RegisterByMobileNumber = false,
                    LanguageChoice = Language.English,
                    PictureName = Guid.NewGuid().ToString(),
                    IsPictureUploaded = false,
                    ValidFrom = DateTime.UtcNow,
                    ValidTill = validtill,
                    CountryCode = $"+{i}",
                    BasicInfo = new BasicInfo
                    {
                        Id = Guid.NewGuid(),
                        ContactNumber = $"{i}{i}",
                        Dob = DateTime.UtcNow - TimeSpan.FromDays(365 * 18),
                        Height_cm = 150,
                        MaritalStatus = "Single",
                        MaternalHomeDistrict = "TestMatHomeDistrict",
                        PaternalHomeDistrict = $"TestpatHomeDistrict{i}",
                        Name = $"Recent testname{i}",
                        Religion = "Islam",
                        ReligionCast = "Sunni",
                        ProfileCreatingFor = $"Brother{i}",
                        PresentAddress = "Present Address Present Address Present Address Present Address Present Address Present Address Present Address Present Address "
                    },
                    EducationInfo = new EducationInfo
                    {
                        Id = Guid.NewGuid(),

                        CurrentJob = new CurrentProfession
                        {
                            Id = Guid.NewGuid(),
                            //Id = userid,
                            JobDesignation = $"Manager{i}",
                            OfficeName = $"testofficename{i}",
                            SalaryRange = "10000-2000 per month(local currency)"
                        },
                        Educations = new List<Education>
                        {
                            new Education
                            {
                                Id = Guid.NewGuid(),
                                Degree = $"testdegree{i}",
                                InstituteName = $"testschool{i}",
                                PassingYear = $"199{i}",
                                Result = $"testresult{i}"
                            },
                            new Education
                            {
                                Id = Guid.NewGuid(),
                                Degree = $"testdegree{i}",
                                InstituteName = $"testschool{i}",
                                PassingYear = $"199{i}",
                                Result = $"testresult{i}"
                            }
                        }
                    },
                    FamilyInfo = new FamilyInfo
                    {
                        Id = Guid.NewGuid(),

                        FamilyMembers = new List<FamilyMember>
                        {

                            new FamilyMember
                            {
                                Id = Guid.NewGuid(),

                                FamilyMemberName = $"testfamilymemebername{i}",
                                FamilyMemberOccupation = $"business{i}",
                                Relationship = $"brother{i}"
                            },
                            new FamilyMember
                            {
                                Id = Guid.NewGuid(),
                                FamilyMemberName = $"testfamilymemebername{i}",
                                FamilyMemberOccupation = $"business{i}",
                                Relationship = $"mother{i}"
                            }

                        }
                    }

                };

                appuser.User = user;
                _unitOfWork.AppUseRepository.Insert(appuser);
                //_unitOfWork.UserRepository.Insert(user);


                appusers.Add(appuser);
                //users.Add(user);

            }




            try
            {
                await _mediator.Publish(new ComitDatabaseNotification());

            }
            catch (Exception e)
            {
                throw e;
            }
            return Ok($"{totalrecord} groom data added");
        }


        [HttpPost]
        [Route("bulkinsertbride")]

        public async Task<IActionResult> BulkInsertBrideData([FromBody] int totalrecord)
        {
            var appusers = new List<AppUser>();
            var users = new List<User>();


            for (int i = 0; i < totalrecord; i++)
            {

                var role = AppRole.User.ToString();
                var validtill = GetValidTill(role);

                var appuser = new AppUser
                {
                    Id = Guid.NewGuid(),
                    CountryCode = $"+{i}",
                    MobileNumber = $"{i}{i}{i}{i}{i}{i}{i}{i}{i}{i}",
                    LoggedInDevices = 1,
                    IsVarified = true,
                    LookingForBride = false,
                    Password = "123456",
                    UserRole = role,
                    Email = $"{i}@ghotok.com",
                    RegisterByMobileNumber = false,
                    LanguageChoice = Language.English,
                    ValidFrom = DateTime.UtcNow,
                    ValidTill = validtill,
                    };
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    IsPublished = true,
                    LookingForBride = false,
                    MobileNumber = $"{i}{i}{i}",
                    Email = $"{i}@ghotok.com",
                    RegisterByMobileNumber = false,
                    LanguageChoice = Language.English,
                    PictureName = Guid.NewGuid().ToString(),
                    IsPictureUploaded = false,
                    ValidFrom = DateTime.UtcNow,
                    ValidTill = validtill,
                    CountryCode = $"+{i}",
                    BasicInfo = new BasicInfo
                    {
                        Id = Guid.NewGuid(),
                        ContactNumber = $"{i}{i}",
                        Dob = DateTime.UtcNow - TimeSpan.FromDays(365 * 18),
                        Height_cm = 150,
                        MaritalStatus = "Single",
                        MaternalHomeDistrict = "TestMatHomeDistrict",
                        PaternalHomeDistrict = $"TestpatHomeDistrict{i}",
                        Name = $"recent testname{i} bride",
                        Religion = "Islam",
                        ReligionCast = "Sunni",
                        ProfileCreatingFor = $"Brother{i}",
                        PresentAddress = "Present Address Present Address Present Address Present Address Present Address Present Address Present Address Present Address "
                    },
                    EducationInfo = new EducationInfo
                    {
                        Id = Guid.NewGuid(),
                        CurrentJob = new CurrentProfession
                        {
                            Id = Guid.NewGuid(),
                            JobDesignation = $"Manager{i}",
                            OfficeName = $"testofficename{i}",
                            SalaryRange = "10000-2000 per month(local currency)"
                        },
                        Educations = new List<Education>
                        {
                            new Education
                            {
                                Id = Guid.NewGuid(),
                                Degree = $"testdegree{i}",
                                InstituteName = $"testschool{i}",
                                PassingYear = $"199{i}",
                                Result = $"testresult{i}"
                            },
                            new Education
                            {
                                Id = Guid.NewGuid(),
                                Degree = $"testdegree{i}",
                                InstituteName = $"testschool{i}",
                                PassingYear = $"199{i}",
                                Result = $"testresult{i}"
                            }
                        }
                    },
                    FamilyInfo = new FamilyInfo
                    {
                        Id = Guid.NewGuid(),
                        FamilyMembers = new List<FamilyMember>
                        {
                            new FamilyMember
                            {
                                Id = Guid.NewGuid(),
                                FamilyMemberName = $"testfamilymemebername{i}",
                                FamilyMemberOccupation = $"business{i}",
                                Relationship = $"brother{i}"
                            },
                            new FamilyMember
                            {
                                Id = Guid.NewGuid(),
                                FamilyMemberName = $"testfamilymemebername{i}",
                                FamilyMemberOccupation = $"business{i}",
                                Relationship = $"mother{i}"
                            }

                        }
                    }

                };

                appuser.User = user;
                _unitOfWork.AppUseRepository.Insert(appuser);
                //_unitOfWork.UserRepository.Insert(user);


                appusers.Add(appuser);
                //users.Add(user);

            }




            try
            {
                await _mediator.Publish(new ComitDatabaseNotification(),default(CancellationToken));


            }
            catch (Exception e)
            {
                throw e;
            }
            return Ok($"{totalrecord} bride data added");
        }


        private DateTime GetValidTill(string role)
        {
            if (role == AppRole.PremiumUser.ToString())
            {
                return DateTime.UtcNow + TimeSpan.FromDays(90);
            }
            return DateTime.UtcNow + TimeSpan.FromDays(3650);
        }

    }
}

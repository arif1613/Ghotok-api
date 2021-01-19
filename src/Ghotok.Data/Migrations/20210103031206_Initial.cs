using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ghotok.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BasicInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProfileCreatingFor = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    MaritalStatus = table.Column<string>(nullable: false),
                    Dob = table.Column<DateTime>(nullable: false),
                    ContactNumber = table.Column<string>(nullable: false),
                    Height_cm = table.Column<int>(nullable: false),
                    PresentAddress = table.Column<string>(nullable: false),
                    Religion = table.Column<string>(nullable: false),
                    ReligionCast = table.Column<string>(nullable: false),
                    email = table.Column<string>(nullable: true),
                    PermanentAddress = table.Column<string>(nullable: true),
                    PaternalHomeDistrict = table.Column<string>(nullable: true),
                    MaternalHomeDistrict = table.Column<string>(nullable: true),
                    PropertyInfo = table.Column<string>(nullable: true),
                    OtherBasicInfo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasicInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CurrentProfession",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    JobDesignation = table.Column<string>(nullable: true),
                    OfficeName = table.Column<string>(nullable: true),
                    SalaryRange = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrentProfession", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FamilyInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilyInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EducationInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CurrentJobId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EducationInfo_CurrentProfession_CurrentJobId",
                        column: x => x.CurrentJobId,
                        principalTable: "CurrentProfession",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FamilyMember",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FamilyMemberName = table.Column<string>(nullable: true),
                    FamilyMemberOccupation = table.Column<string>(nullable: true),
                    Relationship = table.Column<string>(nullable: true),
                    FamilyInfoId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilyMember", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FamilyMember_FamilyInfo_FamilyInfoId",
                        column: x => x.FamilyInfoId,
                        principalTable: "FamilyInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Education",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Degree = table.Column<string>(nullable: true),
                    InstituteName = table.Column<string>(nullable: true),
                    PassingYear = table.Column<string>(nullable: true),
                    Result = table.Column<string>(nullable: true),
                    EducationInfoId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Education", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Education_EducationInfo_EducationInfoId",
                        column: x => x.EducationInfoId,
                        principalTable: "EducationInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CountryCode = table.Column<string>(nullable: true),
                    MobileNumber = table.Column<string>(nullable: false),
                    LookingForBride = table.Column<bool>(nullable: false),
                    RegisterByMobileNumber = table.Column<bool>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    LanguageChoice = table.Column<int>(nullable: false),
                    ValidFrom = table.Column<DateTime>(nullable: false),
                    ValidTill = table.Column<DateTime>(nullable: false),
                    PictureName = table.Column<string>(nullable: true),
                    IsPictureUploaded = table.Column<bool>(nullable: false),
                    IsPublished = table.Column<bool>(nullable: false),
                    BasicInfoId = table.Column<Guid>(nullable: true),
                    FamilyInfoId = table.Column<Guid>(nullable: true),
                    EducationInfoId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_BasicInfo_BasicInfoId",
                        column: x => x.BasicInfoId,
                        principalTable: "BasicInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_EducationInfo_EducationInfoId",
                        column: x => x.EducationInfoId,
                        principalTable: "EducationInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_FamilyInfo_FamilyInfoId",
                        column: x => x.FamilyInfoId,
                        principalTable: "FamilyInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CountryCode = table.Column<string>(nullable: true),
                    MobileNumber = table.Column<string>(nullable: false),
                    LookingForBride = table.Column<bool>(nullable: false),
                    RegisterByMobileNumber = table.Column<bool>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    LanguageChoice = table.Column<int>(nullable: false),
                    ValidFrom = table.Column<DateTime>(nullable: false),
                    ValidTill = table.Column<DateTime>(nullable: false),
                    UserRole = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: false),
                    IsVarified = table.Column<bool>(nullable: false),
                    IsLoggedin = table.Column<bool>(nullable: false),
                    LoggedInDevices = table.Column<int>(nullable: false),
                    UserId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_UserId",
                table: "AppUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Education_EducationInfoId",
                table: "Education",
                column: "EducationInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_EducationInfo_CurrentJobId",
                table: "EducationInfo",
                column: "CurrentJobId");

            migrationBuilder.CreateIndex(
                name: "IX_FamilyMember_FamilyInfoId",
                table: "FamilyMember",
                column: "FamilyInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_BasicInfoId",
                table: "Users",
                column: "BasicInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_EducationInfoId",
                table: "Users",
                column: "EducationInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_FamilyInfoId",
                table: "Users",
                column: "FamilyInfoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUsers");

            migrationBuilder.DropTable(
                name: "Education");

            migrationBuilder.DropTable(
                name: "FamilyMember");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "BasicInfo");

            migrationBuilder.DropTable(
                name: "EducationInfo");

            migrationBuilder.DropTable(
                name: "FamilyInfo");

            migrationBuilder.DropTable(
                name: "CurrentProfession");
        }
    }
}

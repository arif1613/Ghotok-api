USE [GhotokApiDb]

IF OBJECT_ID('GetData') IS NOT NULL
DROP PROCEDURE GetData
GO
CREATE PROCEDURE GetData @type nvarchar(30),@skip bigint,@take bigint,@lookingforbride bit,@isvarified bit,@ispublished bit AS
BEGIN


IF @type='AppUser'
	BEGIN
		--CREATE OR REPLACE VIEW [TempGetAppUserDataView] AS
		SELECT * FROM AppUsers au 
		where au.LookingForBride=@lookingforbride AND au.IsVarified=@isvarified
		ORDER BY au.Id
		OFFSET @skip ROWS
		FETCH NEXT @take ROWS ONLY;

		
	END

IF @type='User'
	BEGIN
		--CREATE OR REPLACE VIEW [TempGetUserDataView] AS
		--SELECT * FROM Users ORDER BY Id
		--OFFSET @skip ROWS
		--FETCH NEXT @take ROWS ONLY;

			SELECT * FROM Users u
			
			inner join BasicInfo b on b.Id=u.BasicInfoId
			inner join EducationInfo eduinfo on eduinfo.Id=u.EducationInfoId
			inner join FamilyInfo famiinfo on famiinfo.Id=u.FamilyInfoId
			inner join dbo.Address ad on ad.Id=b.PresentAddressId
			inner join CurrentProfession cp on cp.Id=eduinfo.CurrentJobId
			join Education edu on edu.EducationInfoId=eduinfo.Id
			join FamilyMember famimem on famimem.FamilyInfoId=famiinfo.Id
			where u.LookingForBride=@lookingforbride AND u.IsPublished=@ispublished AND u.Id>=@skip AND u.Id<=@take
			ORDER BY u.Id
	END

END
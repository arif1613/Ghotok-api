CREATE OR ALTER VIEW dbo.UserShortInfo
AS
SELECT  a.Id,
		a.LookingForBride,
		a.IsPictureUploaded,
		a.PictureName,
		b.Name,
		b.email,
		b.ContactNumber,
		b.Dob,
		b.MaritalStatus
		FROM dbo.Users a
		INNER JOIN dbo.BasicInfo b on a.BasicInfoId = b.Id
		INNER JOIN dbo.EducationInfo e on a.EducationInfoId = e.Id


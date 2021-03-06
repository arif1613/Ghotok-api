﻿--GO

--CREATE VIEW [dbo].[REMAllocation]
--AS
--WITH allocations AS(
--	SELECT
		 
--		c.[ClientNumber],
--		a.[AllocationID],
--		a.[CompanyID],
--		a.[PeriodID],
--		a.[ProductGUID],
--		c.[CompanyGUID],
--		c.[ClientName],
--		c.[OrgNumber],
--		c.[Closingmonth],
--		c.[ExclusionID],
--		c.[BusinessUnitID],
--		c.[Segment],
--		c.IsExcluded,
--		p.[StartDate],
--		p.[EndDate],
--		p.[DcDeliveryDate],
--		r.DCStatusCode,
--		r.DCStatusName,
--		ath.[AllocationTeamMemberHoursID],
--		ath.TimeSpanStartMin,
--		ath.TimeSpanEndMin
--		,COUNT(DCStatusCode) OVER (PARTITION BY r.DcStatusCode ORDER BY r.DcStatusCode)  as Requests
--		,CASE	
--			WHEN DCStatusName IN ('BOOKED','ASSIGNED','ACCEPTED','HOLD') THEN 'UPCOMING'
--			WHEN DCStatusName IN ('SENT','INREVIEW','REVIEWED','ARCHIVED') AND 
--				CAST((SELECT COUNT(*) FROM dbo.Team t WHERE a.AllocationID = t.AllocationID AND t.RoleID = 3) as BIT) = 0
--				THEN 'INPROGRESS'
--			WHEN DCStatusName IN ('SENT','ASSIGNED','INREVIEW','REVIEWED','ARCHIVED') 
--				AND CAST((SELECT COUNT(*) FROM dbo.Team t WHERE a.AllocationID = t.AllocationID AND t.RoleID = 3) as BIT) = 1 
--				THEN 'DONE'
--			WHEN DCStatusName LIKE 'CANCELED' OR DCStatusName LIKE 'CANCELLED' THEN 'NOTTRACKED'
--			ELSE 'UPCOMING'
--		END AS RequestStatusCode
--	FROM dbo.Allocation a
--	INNER JOIN dbo.Company c on c.CompanyID = a.CompanyID
--	INNER JOIN dbo.[Period] p on p.PeriodID = a.PeriodID
--	LEFT JOIN dbo.Request r on r.CompanyID = c.CompanyID AND r.DCDeliveryDate = p.DcDeliveryDate
--	LEFT JOIN dbo.AllocationTeamMemberHours ath on ath.AllocationTeamMemberHoursID = a.AllocationTeamMemberHoursID
	
--)
--SELECT  [ClientNumber],
--		[AllocationID],
--		[CompanyID],
--		[PeriodID],
--		[ProductGUID],
--		[CompanyGUID],
--		[ClientName],
--		[OrgNumber],
--		[Closingmonth],
--		[ExclusionID],
--		[BusinessUnitID],
--		[Segment],
--		IsExcluded,
--		[StartDate],
--		[EndDate],
--		[DcDeliveryDate],
--		DCStatusCode,
--		DCStatusName,
--		CASE WHEN DCStatusCode IS NULL THEN 0 ELSE Requests END as Requests,
--		a.RequestStatusCode,
--		a.[AllocationTeamMemberHoursID],
--		a.TimeSpanStartMin,
--		a.TimeSpanEndMin
--		FROM allocations a
--GO
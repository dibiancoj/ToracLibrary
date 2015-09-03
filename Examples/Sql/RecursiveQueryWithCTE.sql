USE [Sage]
GO
/****** Object:  StoredProcedure [dbo].[ClaimTreeTeamDelete]    Script Date: 1/7/2013 2:36:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[ClaimTreeTeamDelete]
		@TreeId INT
AS
BEGIN
	SET NOCOUNT ON;

	--reason we put this in a Sp is because sql server doesn't allow cascade deletes on the same table.

	;WITH Nodes (TreeId, ParentTreeId, LevelId) 
	AS (
	    SELECT  T.TreeId, T.ParentTreeId, 0 AS LevelId
	    FROM    Ref_ClaimTree AS T
	    WHERE   T.TreeId = @TreeId

	    UNION ALL

		SELECT  TT.TreeId, TT.ParentTreeId, TT.LevelId + 1
		FROM    Ref_ClaimTree AS TT
        INNER JOIN Nodes N ON N.TreeId = TT.ParentTreeId
	)


	--TOP 100 PERCENT mean all the records. Without it you'll get The ORDER BY clause is invalid in views, inline functions, derived tables, subqueries, and common table expressions, unless TOP or FOR XML is also specified. error. 

	DELETE
	FROM    Ref_ClaimTree
	WHERE  LevelId <> 1 AND TreeId IN (
										SELECT TOP 100 PERCENT  N.TreeId
										FROM    Nodes N
										ORDER BY N.LevelId DESC)

END
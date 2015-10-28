/* 
So we want to sort the table by the hierachy. 
ie:
	Node1
		Node1A
			Node1AA
		Node2A
	Node2
		Node2A

		Etc.

so we use the  N.CTESortOrder + CONVERT(NVARCHAR(4000), SEE.SequenceNumber) + N'/' in the items below.
which basically says SortCriteria1 + SortCriteria2...i can keep going
Then the bottom sorts by ORDER BY CONVERT(HIERARCHYID, N.CTESortOrder)
*/

;WITH Nodes (Id, ParentTreeId, LevelId, ElementType, ElementTypeId, PageNumber, Seq, SubElementIdentityId, RootId, CTESortOrder) 
	AS (
			SELECT SE.Id, SE.ParentId, 0 AS LevelId, ET.Description, SE.ElementTypeId, SE.PageNumber, SE.SequenceNumber, SE.IdentityId, SE.Id, N'/' + CONVERT(NVARCHAR(4000),SE.SequenceNumber) + N'/'
			FROM SurveyElement AS SE
			INNER JOIN @ElementType AS ET
			ON SE.ElementTypeId = ET.ElementTypeId
			WHERE SE.SurveyId = @SurveyId AND SE.ParentId IS NULL --AND SE.Id = 2

			UNION ALL 

			SELECT SEE.Id, SEE.ParentId, N.LevelId + 1, ET.Description, SEE.ElementTypeId, SEE.PageNumber, SEE.SequenceNumber, SEE.IdentityId, N.RootId, N.CTESortOrder + CONVERT(NVARCHAR(4000), SEE.SequenceNumber) + N'/'
			FROM SurveyElement AS SEE
			INNER JOIN @ElementType AS ET
			ON SEE.ElementTypeId = ET.ElementTypeId
			INNER JOIN Nodes N ON 
			N.Id = SEE.ParentId
			WHERE SEE.SurveyId = @SurveyId
	    )

SELECT 
	SPACE(LevelId * 4) + N.ElementType AS CategoryName, 
	N.*, 

FROM Nodes AS N
WHERE N.PageNumber = 2

ORDER BY CONVERT(HIERARCHYID, N.CTESortOrder)
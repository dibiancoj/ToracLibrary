--All test's I've run were actually slower and used more resources with cross apply rather then something like 
--QueryXMLColumnDataType.sql

--Full example
--Returns 21 and 11 in 2 rows of data

	SELECT x.value('(FilterId)[1]','int')
	FROM Ref_SavedSearch AS S
	CROSS APPLY S.SearchCriteria.nodes('QuickSearchCriteria/SelectedCheckBoxIds/QuickBox') AS xDoc(x)
	WHERE S.SectionId = @SectionId AND S.SearchTypeId = 0

--<QuickSearchCriteria>
--  <QuickSearchText>exc</QuickSearchText>
--  <QuickSearchType>contains</QuickSearchType>
--  <SelectedCheckBoxIds>
--    <QuickBox>
--      <Id>ClaimRefBox</Id>
--      <FilterId>21</FilterId>
--    </QuickBox>
--    <QuickBox>
--      <Id>HandlerBox</Id>
--      <FilterId>11</FilterId>
--    </QuickBox>
--  </SelectedCheckBoxIds>
--</QuickSearchCriteria>

-----************************************************************************************
--another example
    SELECT
		B.FileID,
		xDoc.Data.value('(DocumentType)[1]', 'VARCHAR(50)') AS [Document Type],
		xDoc.Data.value('(AccountNumber)[1]', 'VARCHAR(50)') AS [Member #],
		xDoc.Data.value('(DocumentCategory)[1]', 'VARCHAR(50)') AS [a],
		xDoc.Data.value('(ReviewGroup)[1]', 'VARCHAR(50)') AS [b]
 
	FROM dbo.BranchMemberScanningUploadItems AS B
	CROSS APPLY B.ItemToUpload.nodes('UploadItemMembersInfo') AS xDoc(Data)
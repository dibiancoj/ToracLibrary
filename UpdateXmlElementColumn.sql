--this is to update a element text value

UPDATE Ref_SavedSearch
SET SearchCriteria.modify(
'
    replace value of (/ArrayOfCommonSearchCriteria/CommonSearchCriteria/SectionId/text())[1] 
    with ("1")
')
WHERE SavedSearchId = 9953;


data structure is like 

<ArrayOfCommonSearchCriteria>
  <CommonSearchCriteria>
    <SectionId>1</SectionId>
    <FilterId>210</FilterId>
    <FilterText>Closed File Trending Period</FilterText>
    <ValueDescription>[Month]  Last Closed Month (May 2012)</ValueDescription>
    <ValueIds>
      <ListOfStrings>
        <string>LastClosedMonth</string>
      </ListOfStrings>
      <Radioselection>Months</Radioselection>
    </ValueIds>
    <PartialViewId>19</PartialViewId>
    <IsDistinctPeriodFilter>false</IsDistinctPeriodFilter>
    <GetsInvalidatedByHistoryFilter>false</GetsInvalidatedByHistoryFilter>
  </CommonSearchCriteria>
</ArrayOfCommonSearchCriteria>

--or something like this
UPDATE Ref_SavedSearch 
SET 
   SearchCriteria.modify('replace value of (//string/text())[1] with "DateTimeLastMonth"')
WHERE
   SavedSearchId = 61683
   and SearchCriteria.value('(//string/text())[1]', 'varchar(1000)') = 'DateTimeLastMonth' 
SELECT 
	B.FileID,
	C.CatalogDescription,
	B.ItemToUpload.value('(UploadItemMembersInfo/FilePath)[1]', 'VARCHAR(255)') AS [File Path] ,
	B.IsUploaded,
	B.UploadInProgress,
	B.EntryDate,
	dbo.TrackingStatusDescription(B.IsUploaded,B.UploadInProgress,TS.[Order]);

--another example

--you can also query up to the node (either use //thisItem or full path root/item/thisItem then since int is the value we are trying to read, we just use '.'
--value type of this item is an integer
SELECT T1.Loc1.value('.','int') AS ListIds
FROM @SavedSearchInDbXml.nodes('//int') AS T1(Loc1)
WHERE T1.Loc1.value('.','int') = 1;

--another example for xml, is to use the exists which will produce a boolean if the node exists
SELECT @ElementExists = xDoc.x.exist('.') 
FROM @FilterIds.nodes('ArrayOfCommonSearchCriteria') AS xDoc(x);
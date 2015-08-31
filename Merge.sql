DECLARE @Id INT = 21;
DECLARE @Txt VARCHAR(30) = 'test 1';

------------------------------------
MERGE z_To_SSIS AS TDestination
USING (SELECT @Id AS Id) AS TSource --select the primary key on the source table
ON TDestination.Id = TSource.Id --now join the table we want to either insert or update with the id of the item passed in

WHEN MATCHED THEN --if the item is found, then we run an insert
   UPDATE SET Description = @Txt, CreatedDate = GETDATE()

WHEN NOT MATCHED THEN --if the item is not found, then we run an insert
   INSERT (Description, CreatedDate) VALUES (@Txt, GETDATE());

--WHEN NOT MATCHED BY SOURCE AND @Id = 2
--   THEN DELETE;



--other examples

MERGE BookInventory bi
USING BookOrder bo
ON bi.TitleID = bo.TitleID
WHEN MATCHED AND
  bi.Quantity + bo.Quantity = 0 THEN
  DELETE
WHEN MATCHED THEN
  UPDATE
  SET bi.Quantity = bi.Quantity + bo.Quantity
WHEN NOT MATCHED BY TARGET THEN
  INSERT (TitleID, Title, Quantity)
  VALUES (bo.TitleID, bo.Title,bo.Quantity);
 
SELECT * FROM BookInventory;
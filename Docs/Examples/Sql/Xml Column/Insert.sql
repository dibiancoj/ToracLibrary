BEGIN TRY
   BEGIN TRANSACTION    -- Start the transaction


INSERT INTO dbo.MAIL_CAMPAIGN (AccountNumber,Suffix,ProductName,ProductOpenDate,BaseOpenDate,EmailAddress,SendDate,StatusID)
	SELECT Data.AccountNumber,Data.Suffix,Data.ProductName,Data.ProductOpenDate,Data.BaseAccountOpenDate,Data.EmailAddress,Data.SendDate,Data.StatusID
	FROM
		(SELECT
			X.Data.value('AccountNumber[1]', 'int') AS AccountNumber,
			X.Data.value('Suffix[1]', 'tinyint') AS Suffix,
			X.Data.value('ProductName[1]', 'varchar(50)') AS ProductName,
			X.Data.value('BaseOpenDate[1]', 'datetime') AS ProductOpenDate,
			X.Data.value('BaseOpenDate[1]', 'datetime') AS BaseAccountOpenDate,
			X.Data.value('EmailAddress[1]', 'varchar(100)') AS EmailAddress,
			X.Data.value('SendDate[1]', 'datetime') AS SendDate,
			X.Data.value('StatusID[1]', 'int') AS StatusID

		FROM
			@xDoc.nodes('Navigators/QueueInfo') AS X(Data)) AS Data;


COMMIT

END TRY
BEGIN CATCH
  -- Whoops, there was an error
  IF @@TRANCOUNT > 0
     ROLLBACK

  -- Raise an error with the details of the exception
  DECLARE @ErrMsg nvarchar(4000), @ErrSeverity int
  SELECT @ErrMsg = ERROR_MESSAGE(),
         @ErrSeverity = ERROR_SEVERITY()

  RAISERROR(@ErrMsg, @ErrSeverity, 1)
END CATCH
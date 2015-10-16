BEGIN TRY
   BEGIN TRANSACTION    -- Start the transaction

	--declare the send date
	DECLARE @DistributionSendDate DATETIME;

	--set the send date (this way we don't keep having to call getdate for each update)
	SET @DistributionSendDate = GETDATE();

	--just going to update the status and the distribution send date for history purposes
	UPDATE dbo.MAIL_CAMPAIGN
		SET
			StatusID = X.Data.value('StatusID[1]','int'),
			DistributionSendDate = @DistributionSendDate
			
		FROM
			@xDoc.nodes('Navigators/QueueHistoryInfo') AS X(Data)

		WHERE
			AccountNumber = X.Data.value('AccountNumber[1]','int')
			AND Suffix = X.Data.value('Suffix[1]','tinyint');

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


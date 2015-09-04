CREATE FUNCTION [dbo].[fnHasDocumentRecord]
(
	@BitMaskFlag INT,
	@DocumentStatus INT
)
RETURNS BIT
AS
BEGIN
	
	--Checks to see if you have a bit mask (used for document status in the loans table)

	--Return Value
	DECLARE @ReturnValue BIT;

		--Check to see if the status passed in contains the bit mask you are checking for which is also passed in so you can reuse this function
		IF (@DocumentStatus & @BitMaskFlag = @BitMaskFlag)
			SET @ReturnValue = 'true';
		ELSE
			SET @ReturnValue = 'false';

		--Return the results
		RETURN @ReturnValue;

END
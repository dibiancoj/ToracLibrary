﻿BEGIN TRY
		BEGIN TRANSACTION
	
		--logic to run in a transaction
		

		COMMIT TRANSACTION
	END TRY
	
	BEGIN CATCH
		ROLLBACK TRANSACTION
	END CATCH  
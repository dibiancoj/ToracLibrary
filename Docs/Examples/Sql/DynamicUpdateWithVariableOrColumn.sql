--if you need to update from column value, do it 1 by 1 setting a variable.

DECLARE @NewValue INT;

@NewValue = 10


 UPDATE thisTable
		   SET DashboardData.modify('replace value of (//SaveSearchId_Center/SaveSearchId/text())[1] with sql:variable("@NewValue")')
		   WHERE RandomId = @Id;

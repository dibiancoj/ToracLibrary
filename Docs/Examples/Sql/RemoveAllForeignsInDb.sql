
--this delete's all the foreign keys in the db. 
--it basically loops through each foreign key and deletes it

--Grab the foreign key constraints and loop through them with a recursive loop
WHILE(EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE = 'FOREIGN KEY'))
BEGIN
	--let's build up the sql so we can execute it and delete this foreign key we are up too
	DECLARE @SqlToExecute NVARCHAR(2000);

	--grab the next guy in line and let's go drop it
	SELECT TOP 1 @SqlToExecute = ('ALTER TABLE ' + TABLE_SCHEMA + '.[' + TABLE_NAME	+ '] DROP CONSTRAINT [' + CONSTRAINT_NAME + ']')
	FROM information_schema.table_constraints
	WHERE CONSTRAINT_TYPE = 'FOREIGN KEY';

	--now go run the drop script
	EXEC (@SqlToExecute);
END
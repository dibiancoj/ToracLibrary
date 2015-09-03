SELECT * 
FROM MEMBER_FILE AS MF
WHERE MF.Account IN (SELECT Data.ID
					 FROM
						(SELECT X.Data.value('@id[1]', 'int') AS ID
						 FROM @xDoc.nodes('Quorum/Account') AS X(Data)) AS Data);
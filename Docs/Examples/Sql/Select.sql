(SELECT
	(SELECT
		MC.AccountNumber,
		MC.Suffix,
		MC.ProductName,
		MC.ProductOpenDate,
		MC.BaseOpenDate,
		MC.SendDate,
		MC.StatusID

	FROM Mail_Campaign AS MC
	WHERE MC.StatusID NOT IN (3,4) --grab every item that hasn't been sent 
	AND MC.SendDate <= @DateToQuery -- and the date is equal or less then now
	FOR XML PATH('QueueInfo'),TYPE))
FOR XML PATH('QueueInfos'),ROOT('Navigators');
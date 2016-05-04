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




--also a select with a where in the xml column
              SELECT  F.PatId, 
                      F.FormId, 
       
                           --select the question 2 answer for privacy notice
                           (CASE 
                                  WHEN F.FormId = 'PrivacyNotice' THEN F.FormData.value('(//FormQuestionAnswer[@QuestionId="2"]/node())[1]', 'varchar(100)') 
                                  ELSE NULL
                           END) AS PrivacyNotice,
       
                           --select the question 1 answer for guarantee of account
                           (CASE 
                                  WHEN F.FormId = 'GuaranteeOfAccount' THEN F.FormData.value('(//FormQuestionAnswer[@QuestionId="1"]/node())[1]', 'varchar(100)') 
                                  ELSE NULL
                           END) AS GuaranteeOfAccount,

                           Cadence.NextAppt AS NextAppt

              FROM FormData AS F
              LEFT OUTER JOIN @FakeCadence AS Cadence
              ON F.PatId = Cadence.PatId
              WHERE F.CompletedDate IS NOT NULL -- anything submitted
              AND F.FormId IN ('PrivacyNotice','GuaranteeOfAccount')) As TempData

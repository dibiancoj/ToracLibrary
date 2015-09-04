UPDATE AC
SET CreateTimeStamp = A.CreateTimeStamp
 
FROM Ref_AuditClaim AS AC
INNER JOIN Ref_Audit AS A
ON AC.AuditId = A.AuditId;
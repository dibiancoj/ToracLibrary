INSERT INTO Results(AuditID,CustomerID,RuleIDViolated)
EXEC @StoredProcedureName @RuleID,@ActionID,@ReportRunDate
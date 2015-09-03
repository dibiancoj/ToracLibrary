

--delete with no where criteria
	UPDATE D
	SET DashboardData.modify('delete WelcomeInfo/SaveSearchId_Center')
	FROM Ref_SavedDashboards AS D
	WHERE D.DashboardData.value('(WelcomeInfo/SaveSearchId_Center/SaveSearchId)[1]', 'INT') NOT IN (SELECT SS.SavedSearchId
																									FROM Ref_SavedSearch AS SS)


--delete with a where criteria (delete the CustomGroupingConfiguration node where the grouping attribute = "TotalMarketCapital")

UPDATE e
SET CustomGroupings.modify('delete //CustomGroupingConfiguration[@Grouping="TotalMarketCapital"]')
FROM Ref_EmployeePreference AS e
where e.CustomGroupings is not null


--<ArrayOfCustomGroupingConfiguration>
--  <CustomGroupingConfiguration Grouping="AccidentPeriod" Visible="false" />
--  <CustomGroupingConfiguration Grouping="AccidentYear" Visible="false" />
--  <CustomGroupingConfiguration Grouping="TransactionAccountingPeriod" Visible="false" />
--  <CustomGroupingConfiguration Grouping="AdvisedPeriod" Visible="false" />
--  <CustomGroupingConfiguration Grouping="AdvisedYear" Visible="false" />
--  <CustomGroupingConfiguration Grouping="Branch" Visible="false" />
--  <CustomGroupingConfiguration Grouping="BrokerName" Visible="false" />
--  <CustomGroupingConfiguration Grouping="BrokerCode" Visible="true" />

--  <CustomGroupingConfiguration Grouping="BrokerUniversalGroup" Visible="true" />
--</ArrayOfCustomGroupingConfiguration>

--take not of the end of the statement "//Columns)[1]...so it inserts it into the columns/parent node
update Ref_SavedSearch
set GroupedGridSettings.modify('insert <GridColumn Name="ThirdGroupName" ColumnOrder="10000" Hidden="true" ChartPick="false" Width="150" /> into (//Columns)[1]')
where SavedSearchId = 5


--if you want to add multiple nodes into the same xDocument.
--syntax: insert (node 1, node2) into (//Column)[1])

update Ref_SavedSearch
set GridSettings.modify('insert (<GridColumn Name="AmericanDepositoryReceiptLevel" ColumnOrder="510" Hidden="true" ChartPick="false" Width="150" />,
								 <GridColumn Name="Revenue" ColumnOrder="5104" Hidden="true" ChartPick="false" Width="150" />,
								 <GridColumn Name="UnderlyingPremium" ColumnOrder="5105" Hidden="true" ChartPick="false" Width="150" />,
								 <GridColumn Name="RevenueBandDisplay" ColumnOrder="5106" Hidden="true" ChartPick="false" Width="150" />,
								 <GridColumn Name="UnderlyingPremiumBandDisplay" ColumnOrder="5107" Hidden="true" ChartPick="false" Width="150" />,
								 <GridColumn Name="USMarketCapitalBandDisplay" ColumnOrder="5108" Hidden="true" ChartPick="false" Width="150" />,
								 <GridColumn Name="TotalMarketCapitalBandDisplay" ColumnOrder="5109" Hidden="true" ChartPick="false" Width="150" />)
								into (//Columns)[1]')
where SectionId = 2 and ReportTypeId is null
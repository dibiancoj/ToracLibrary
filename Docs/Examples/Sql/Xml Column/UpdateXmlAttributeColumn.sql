--this updates an xml attibute

UPDATE Ref_SavedSearch
SET GridSettings.modify(
'
    replace value of (/GridConfig/@SortName)[1] 
    with ("Id")
')
WHERE SavedSearchId = 9953;


--xml data structure


/*
<GridConfig SortName="Id" SortOrder="Asc" RowsPerPage="10" DecimalPlaces="false" IsDefault="false">
  <Columns>
    <GridColumn Name="Id" ColumnOrder="0" Hidden="true" ChartPick="true" Width="150" />
    <GridColumn Name="ClaimRef" ColumnOrder="1" Hidden="false" ChartPick="true" Width="150" />
    <GridColumn Name="Currency" ColumnOrder="2" Hidden="true" ChartPick="true" Width="150" />
    <GridColumn Name="ClosedPeriodDisplay" ColumnOrder="3" Hidden="true" ChartPick="true" Width="150" />
    <GridColumn Name="ClaimTitle" ColumnOrder="4" Hidden="false" ChartPick="true" Width="150" />
    <GridColumn Name="Status" ColumnOrder="5" Hidden="false" ChartPick="true" Width="150" />
    <GridColumn Name="Handler" ColumnOrder="6" Hidden="false" ChartPick="true" Width="150" />
    <GridColumn Name="Accountability" ColumnOrder="7" Hidden="false" ChartPick="true" Width="150" />
    <GridColumn Name="Division" ColumnOrder="8" Hidden="false" ChartPick="true" Width="150" />
    <GridColumn Name="BrokerRegion" ColumnOrder="9" Hidden="false" ChartPick="true" Width="150" />
    <GridColumn Name="LineOfBusiness" ColumnOrder="10" Hidden="false" ChartPick="true" Width="150" />
    <GridColumn Name="Branch" ColumnOrder="11" Hidden="false" ChartPick="true" Width="150" />
    <GridColumn Name="PolicyRef" ColumnOrder="12" Hidden="false" ChartPick="true" Width="150" />
    <GridColumn Name="LossPeriodDisplay" ColumnOrder="13" Hidden="false" ChartPick="true" Width="150" />
    <GridColumn Name="GrossIncurred" ColumnOrder="14" Hidden="false" ChartPick="true" Width="150" />
    <GridColumn Name="GrossPaid" ColumnOrder="15" Hidden="false" ChartPick="true" Width="150" />
    <GridColumn Name="GrossOutstanding" ColumnOrder="16" Hidden="false" ChartPick="true" Width="150" />
    <GridColumn Name="DateOfAdviceDisplay" ColumnOrder="17" Hidden="false" ChartPick="true" Width="150" />
    <GridColumn Name="DateOfLossDisplay" ColumnOrder="18" Hidden="false" ChartPick="true" Width="150" />
    <GridColumn Name="DateOfFirstReserveDisplay" ColumnOrder="19" Hidden="true" ChartPick="true" Width="150" />
    <GridColumn Name="PaidIndemnity" ColumnOrder="20" Hidden="false" ChartPick="true" Width="150" />
    <GridColumn Name="PaidExpense" ColumnOrder="21" Hidden="false" ChartPick="true" Width="150" />
    <GridColumn Name="PaidCoverage" ColumnOrder="22" Hidden="false" ChartPick="true" Width="150" />
    <GridColumn Name="OutstandingIndemnity" ColumnOrder="23" Hidden="false" ChartPick="true" Width="150" />
    <GridColumn Name="OutstandingExpense" ColumnOrder="24" Hidden="false" ChartPick="true" Width="150" />
    <GridColumn Name="OutstandingCoverage" ColumnOrder="25" Hidden="false" ChartPick="true" Width="150" />
    <GridColumn Name="IncurredIndemnity" ColumnOrder="26" Hidden="false" ChartPick="true" Width="150" />
    <GridColumn Name="IncurredExpense" ColumnOrder="27" Hidden="false" ChartPick="true" Width="150" />
    <GridColumn Name="IncurredCoverage" ColumnOrder="28" Hidden="false" ChartPick="true" Width="150" />
    <GridColumn Name="FinalizationDateDisplay" ColumnOrder="29" Hidden="false" ChartPick="true" Width="150" />
    <GridColumn Name="DateOfSuitDisplay" ColumnOrder="30" Hidden="false" ChartPick="true" Width="150" />
    <GridColumn Name="LargeLossReport" ColumnOrder="31" Hidden="false" ChartPick="true" Width="150" />
    <GridColumn Name="Claimant" ColumnOrder="32" Hidden="false" ChartPick="true" Width="150" />
    <GridColumn Name="PolicyInceptionDateDisplay" ColumnOrder="33" Hidden="false" ChartPick="true" Width="150" />
    <GridColumn Name="PolicyExpiryDateDisplay" ColumnOrder="34" Hidden="false" ChartPick="true" Width="150" />
    <GridColumn Name="UnderwritingYear" ColumnOrder="35" Hidden="false" ChartPick="true" Width="150" />
    <GridColumn Name="MinAttPoint" ColumnOrder="36" Hidden="false" ChartPick="true" Width="150" />
    <GridColumn Name="MaxLimit" ColumnOrder="37" Hidden="false" ChartPick="true" Width="150" />
    <GridColumn Name="WatchListFlag" ColumnOrder="38" Hidden="false" ChartPick="true" Width="150" />
    <GridColumn Name="CauseOfLoss" ColumnOrder="39" Hidden="false" ChartPick="true" Width="150" />
    <GridColumn Name="SubrogationPaidMovement" ColumnOrder="40" Hidden="true" ChartPick="true" Width="150" />
    <GridColumn Name="SalvagePaidMovement" ColumnOrder="41" Hidden="true" ChartPick="true" Width="150" />
    <GridColumn Name="PaidExpenseLegal" ColumnOrder="42" Hidden="true" ChartPick="true" Width="150" />
    <GridColumn Name="PaidExpenseNonLegal" ColumnOrder="43" Hidden="true" ChartPick="true" Width="150" />
    <GridColumn Name="InsuredDeductRecoveryMovement" ColumnOrder="44" Hidden="true" ChartPick="true" Width="150" />
    <GridColumn Name="IndemnityRecoveryMovement" ColumnOrder="45" Hidden="true" ChartPick="true" Width="150" />
    <GridColumn Name="ShipownersRecoveryMovement" ColumnOrder="46" Hidden="true" ChartPick="true" Width="150" />
    <GridColumn Name="FeeRecoveryMovement" ColumnOrder="47" Hidden="true" ChartPick="true" Width="150" />
    <GridColumn Name="RecoverableRecoveryMovement" ColumnOrder="48" Hidden="true" ChartPick="true" Width="150" />
    <GridColumn Name="RecoveryConvnRecoveryMovement" ColumnOrder="49" Hidden="true" ChartPick="true" Width="150" />
    <GridColumn Name="TotalRecoveryMovement" ColumnOrder="50" Hidden="false" ChartPick="true" Width="150" />
    <GridColumn Name="IncludeInGrouping" ColumnOrder="51" Hidden="false" ChartPick="true" Width="130" />
  </Columns>
</GridConfig>
*/
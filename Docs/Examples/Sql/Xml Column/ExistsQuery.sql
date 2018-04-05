select *
from Ref_SavedSearch as s
where s.GridSettings.exist('//GridColumn[@Name="Branch"]') = 'True'


//<Event_ID>123</Event_ID>
select *
from CohortDataHistory as cd
where cd.ID = 1168 
and cd.Data.exist('//EVENT_ID[.=123]') = 'True'
order by cd.TriggeredOn desc

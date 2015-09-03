select *
from Ref_SavedSearch as s
where s.GridSettings.exist('//GridColumn[@Name="Branch"]') = 'True'

select x.query('COHORT/row/COHORT_INCLUSION_EVENTS/row/ADDITIONAL_DETAILS/row[NAME="status"]/VALUE')
from CohortData as c
CROSS APPLY C.Data.nodes('//row') AS xDoc(x)
where c.ID in (1238,1239,1240) and x.value('(COHORT/row/DROPPED_DT)[1]','datetime') = ''
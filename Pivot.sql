with cte (accPd, statType,policyid, value)
as 
(
	select 201501,
		   'gwp',
		   1,
		   100
	union all
	select 201502,
		   'gwp',
		   2,
		   200
	union all
	select 201502,
		   'gwp',
		   1,
		   300
		 union all
	select 201501,
		   'gep',
		   1,
		   101
	union all
	select 201502,
		   'gep',
		   5,
		   201
	union all
	select 201502,
		   'gep',
		   10,
		   301
)

--select *
--from cte

select statType, policyid, [201501],[201502]
from cte  as c
pivot
(
	sum(value)
	for accPd in ([201501],[201502])
) as pivotTable
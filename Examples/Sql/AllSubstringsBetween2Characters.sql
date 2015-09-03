--let's grab every string that is between (...)

DECLARE @BrokerText VARCHAR(200) = 'Synapse Services, LLC (Georgia) (SYNA0018)'
 
--temp table to test this
DECLARE @tbl table
(
       BrokerTryToParse varchar(200)
)
 

 --throw in some test data
insert into @tbl(BrokerTryToParse) values(@BrokerText);
insert into @tbl(BrokerTryToParse) values('broker safddsafds (jason001)');
 
 --create the recursive function
;with cte([text], CodeToTryToParse)
as
(
    select
        right(BrokerTryToParse, len(BrokerTryToParse) - charindex(')', BrokerTryToParse, 0)),
        substring(BrokerTryToParse, charindex('(', BrokerTryToParse, 0) + 1, charindex(')', BrokerTryToParse, 0) - charindex('(', BrokerTryToParse, 0) - 1) 
    from @tbl
    where charindex('(', BrokerTryToParse, 0) > 0 and charindex(')', BrokerTryToParse, 0) > 0
    union all
    select
        right([text], len([text]) - charindex(')', [text], 0)),
        substring([text], charindex('(', [text], 0) + 1, charindex(')', [text], 0) - charindex('(', [text], 0) - 1) 
    from cte
    where charindex('(', [text], 0) > 0 and charindex(')', [text], 0) > 0
)
 
 
select CodeToTryToParse
from CTE
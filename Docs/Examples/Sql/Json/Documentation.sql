--json column is just nvarchar(4000) or nvarchar(max)


insert into jsontest(Id, JsonColumn)
values (1, '[{ "Id": 5}, {"Id": 6}]')


insert into jsontest(Id, JsonColumn)
values (2, '{ "Id": -9999, "Txt": "-9999"})')

insert into jsontest(Id, JsonColumn)
values (3, '{ "Id": -9999, "SubObject": { "SubId": 15 } })')

insert into jsontest(Id, JsonColumn)
values (4, '[{ "Id": 5, "Txt": "5Text" }, {"Id": 6, "Txt": "6Text"}]')

--delete JsonTest
--truncate table JsonTest

--normal select
select 
	JSON_VALUE(t.JSONColumn, '$.Id') as Id,
	JSON_VALUE(t.JSONColumn, '$.Txt') as Txt
from JsonTest as t
where JSON_VALUE(t.JSONColumn, '$.Id') = -9999

--select stuff in an array
select 
	JSON_VALUE(t.JSONColumn, '$[0].Id') as Id,
	JSON_VALUE(t.JSONColumn, '$[1].Id') as Id2
from JsonTest as t
where t.Id = 1

--select stuff in sub object
select 
	JSON_VALUE(t.JSONColumn, '$.SubObject.SubId') as Id
from JsonTest as t
where t.Id = 3

--transform json into a relational table
select *
from JsonTest as j
CROSS APPLY OPENJSON (j.JSONColumn) WITH (
											ArrayId nvarchar(100) '$.Id',
											TextValue nvarchar(100) '$.Txt'		
										)  as t
where j.id = 4

--another example of OpenJson
SELECT Number, Customer, Date, Quantity 
 FROM OPENJSON (@JSalestOrderDetails, '$.OrdersArray')
 WITH (
        Number varchar(200), 
        Date datetime,
        Customer varchar(200),
        Quantity int
 ) AS OrdersArray

 --more complex example...json is an array within an array. [ { id: 80, Questions: [{id: 1, text: 'question1'},{id: 2, text: 'question2'}}] 
select  t.Id,
		QuestionList.Text as QuestionText

from dbo.UserItemList as t
 CROSS APPLY  
 
 OPENJSON(t.ItemText)
    WITH
        (
            Id   varchar(200)   '$.Id',  
            Questions  nvarchar(MAX)  AS JSON  
        ) AS parent
CROSS APPLY
    OPENJSON(parent.Questions)
    WITH
        (
            Id   varchar(200)   '$.Id',  
            Text  varchar(300)  '$.Text'
        ) AS QuestionList
where t.Type = 'CheckList' and parent.id = 80 and t.PatId = 'Z12345678'

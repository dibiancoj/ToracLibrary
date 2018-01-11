--you can also do updates, inserts, etc. The good thing with the selects is it doesn't pull down all the data. It just passes through the query.

SELECT * FROM OPENQUERY (MyLinkedServerObject, 'SELECT name FROM joe.titles WHERE name = ''NewTitle''');  


INSERT OPENQUERY (MyLinkedServerObject, 'SELECT name FROM joe.titles')  VALUES ('NewTitle');  


DELETE OPENQUERY (MyLinkedServerObject, 'SELECT name FROM joe.titles WHERE name = ''NewTitle''');  

UPDATE OPENQUERY (MyLinkedServerObject, 'SELECT name FROM joe.titles WHERE id = 101') SET name = 'ADifferentName';  
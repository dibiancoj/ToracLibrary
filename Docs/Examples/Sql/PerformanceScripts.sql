--the xml call [SET STATISTICS XML ON] (if you have show execution plan checked, it won't display. You need to turn that off)
SET STATISTICS IO ON
SET STATISTICS TIME ON
SET STATISTICS XML ON

--run they query here


SET STATISTICS TIME OFF
SET STATISTICS IO OFF
SET STATISTICS XML OFF

------------------------------------------------------------------

--find out-dated stats
SELECT
  id                    AS [Table ID]
, OBJECT_NAME(id)       AS [Table Name]
, name                  AS [Index Name]
, STATS_DATE(id, indid) AS [LastUpdated]
, rowmodctr             AS [Rows Modified]
FROM sys.sysindexes 
WHERE STATS_DATE(id, indid) <= DATEADD(DAY,-1,GETDATE()) 
AND rowmodctr > 10 AND (OBJECTPROPERTY(id,'IsUserTable')) = 1
order by rowmodctr desc, LastUpdated
 
--go fix those problems
--update statistics Ref_SearchLookupData

--------------------------------------------------------------------
--find indexes never used
SELECT
  DB_NAME(i.database_id) as DatabaseName,
  OBJECT_NAME(i.object_id, i.database_id) as ObjectName,
  indexes.name,
  i.last_user_seek,
  i.last_user_scan,
  *
 
FROM sys.dm_db_index_usage_stats AS i
INNER JOIN sys.indexes Indexes
ON Indexes.index_id = i.index_id AND Indexes.object_id = i.object_id
WHERE i.object_id > 100 --and DB_NAME(i.database_id) = 'GuruDevelopment'
--and OBJECT_NAME(i.object_id, i.database_id) = 'Ref_SavedSearch'

--------------------------------------------------------------------

--to update all the tables with all stats in a database
EXEC sp_updatestats;
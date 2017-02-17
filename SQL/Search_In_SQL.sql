--select string using in procedure
select b.name, a.text from syscomments a left join sysobjects b on b.id=a.id
where b.xtype='P' and a.text like '%USP_FOUNDATIONUPDATE%'


--select columns name
select b.name as Table1, a.name as Columns1,a.type  from syscolumns a left join sysobjects b on b.id=a.id
where b.xtype='u' and a.name like '%season%'

 

--select table name
select name from sysobjects where xtype='u' and name like '%habit%'


--select string using in function
select b.name, a.text from syscomments a left join sysobjects b on b.id=a.id
where b.xtype='fn' and a.text like '%Fn_GetIR%'


--select string using in view
select b.name, a.text from syscomments a left join sysobjects b on b.id=a.id
where b.xtype='V' and a.text like '%vw_species%'

--select duplicate records
select ACCOUNTDETAILS_NTId from dbo.SOX_AccountDetails where ACCOUNTDETAILS_NTId in (
	select ACCOUNTDETAILS_NTId  from dbo.SOX_AccountDetails WHERE accountdetails_ntid is NOT NULL 
	group by accountdetails_ntid
	having count(*) > 1
)

-- Shows all user tables and row counts for the current database 
-- Remove is_ms_shipped = 0 check to include system objects 
-- i.index_id < 2 indicates clustered index (1) or hash table (0) 
SELECT o.name,
  ddps.row_count 
FROM sys.indexes AS i
  INNER JOIN sys.objects AS o ON i.OBJECT_ID = o.OBJECT_ID
  INNER JOIN sys.dm_db_partition_stats AS ddps ON i.OBJECT_ID = ddps.OBJECT_ID
  AND i.index_id = ddps.index_id 
WHERE i.index_id < 2  AND o.is_ms_shipped = 0 AND ddps.row_count <> 0 
ORDER BY o.NAME 

--Drop all tables
EXEC sp_MSforeachtable @command1 = "DROP TABLE ?"
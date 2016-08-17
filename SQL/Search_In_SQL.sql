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
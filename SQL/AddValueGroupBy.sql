--add value ; group by id 
select id, [values] = STUFF((SELECT ';'+[value] from #temp as t WHERE id=tb.id FOR xml path('')),1,1,'') 
from dbo.Simpdoc_Production as tb
group by id



--substring 
select  SUBSTRING(username,CHARINDEX('\',username)+1,(LEN(username)-CHARINDEX('\',username))) as userid, *  
from dbo.Export_userlist  




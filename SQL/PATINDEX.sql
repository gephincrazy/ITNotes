select top 100 PATINDEX('%/hotel%',HOIN_URL), SUBSTRING(HOIN_URL,PATINDEX('%/hotel%', HOIN_URL),len(HOIN_URL) - PATINDEX('%/hotel%', HOIN_URL)+1),* from [Spider].[dbo].[HotelInfo]

update [Spider].[dbo].[HotelInfo] set HOIN_URL = SUBSTRING(HOIN_URL,PATINDEX('%/hotel%', HOIN_URL),len(HOIN_URL) - PATINDEX('%/hotel%', HOIN_URL)+1)
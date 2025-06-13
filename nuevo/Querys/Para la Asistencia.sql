SELECT C.[idperplan], CONCAT(TRIM(PH.apepat),' ', TRIM(PH.apemat),' ', TRIM(PH.nombres)) usuario ,[fecha], [hora], [regEntrada], 
[dni], [regSalida], [mensaje], CF.descia cia, CF.ruccia
FROM [Asistencia].[dbo].[control] C
LEFT JOIN [Asistencia].[dbo].[perplan_historia] PH ON
PH.idperplan = C.idperplan
LEFT JOIN [Asistencia].[dbo].[ciafile] CF ON
PH.idcia = CF.idcia
--WHERE C.idperplan = '01547768' AND MONTH(fecha) = 02 AND YEAR(fecha) = 2024
WHERE DAY(fecha) = 15 AND MONTH(fecha) = 02 AND YEAR(fecha) = 2023 AND CF.idcia = 03-- AND CONCAT(TRIM(PH.apepat),' ', TRIM(PH.apemat),' ', TRIM(PH.nombres)) = 'VASQUEZ CASTRO JIMMY'
ORDER BY fecha

--total  44461 
--AGRICOLA  38611 
--SUCRO  5367 
--BIO  483 

SELECT *
FROM [Asistencia].[dbo].[control]
WHERE DAY(fecha) IN (14, 15) AND MONTH(fecha) = 02 AND YEAR(fecha) = 2024 AND idlogin IN ('GMTLPF', 'GMTLR1')

--WHERE MONTH(fecha) = 01 AND YEAR(fecha) = 2024 AND 
--idperplan='01564241'
--316982

UPDATE [Asistencia].[dbo].[control] set regSalida = null WHERE idperplan = '01564241' and fecha = '2024-01-10' and nromov = '15061729'
DELETE FROM [Asistencia].[dbo].[control] WHERE idperplan = '01564241' and fecha = '2024-01-10' and nromov = '15061817'
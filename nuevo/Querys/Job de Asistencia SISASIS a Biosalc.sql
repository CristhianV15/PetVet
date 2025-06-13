--========SCRIPT PARA EL JOB==========
--DELETE FROM [BD_BIOSALC].[BiosChira].[dbo].[TBU_MARCA_ASISTENCIA]
--WHERE fecha BETWEEN CONVERT(DATE,GETDATE()-3) AND CONVERT(DATE,GETDATE(-1)

--INSERT INTO [BD_BIOSALC].[BiosChira].[dbo].[TBU_MARCA_ASISTENCIA] (SOCIEDAD, PLANTA, COD_TRABAJADOR, FECHA, HORA_ENTRADA, HORA_SALIDA)
SELECT CF.codaux AS SOCIEDAD, '1533202' AS PLANTA, C.[idperplan] AS COD_TRABAJADOR, [fecha] AS FECHA, 
CONVERT(TIME, [regEntrada]) AS HORA_ENTRADA, CONVERT(TIME, [regSalida]) AS HORA_SALIDA 
FROM [Asistencia].[dbo].[control] C 
LEFT JOIN [Asistencia].[dbo].[perplan_historia] PH ON
PH.idperplan = C.idperplan
LEFT JOIN [Asistencia].[dbo].[ciafile] CF ON
PH.idcia = CF.idcia
WHERE fecha BETWEEN CONVERT(DATE,GETDATE()-1) AND CONVERT(DATE,GETDATE())
--WHERE DAY(fecha) = 22 AND MONTH(fecha) = 9 AND YEAR(fecha) = 2023

--============================
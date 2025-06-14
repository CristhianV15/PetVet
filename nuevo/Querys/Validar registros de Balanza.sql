SELECT DISTINCT(TALHAO), SETOR, FAZENDA, LOTE, SAFRA, LIB, DATA_ENT, SUM(PL) SUMATORIA FROM [BiosChira].[dbo].[ENT_SAF]
WHERE DATA_ENT BETWEEN '2023-10-16' AND '2023-10-17' AND SETOR <>'FDT'
GROUP BY TALHAO, SETOR, FAZENDA, LOTE, SAFRA, LIB, DATA_ENT
ORDER BY DATA_ENT

---AQUI
SELECT DISTINCT(TALHAO), SETOR, FAZENDA, LOTE, SAFRA, LIB, DIA, SUM(PL) SUMATORIA FROM [BiosChira].[dbo].[ENT_SAF]
WHERE DIA BETWEEN '2024-01-22' AND '2024-01-25' AND SETOR <>'FDT'
GROUP BY TALHAO, SETOR, FAZENDA, LOTE, SAFRA, LIB, DIA
ORDER BY DIA

select * FROM [BiosChira].[dbo].[ENT_SAF]
WHERE YEAR(DIA) = 2024

SELECT DISTINCT(TALHAO), SETOR, FAZENDA, LOTE, TALHAO, SUM(PL) SUMATORIA, LIB, DATA_ENT FROM [BiosChira].[dbo].[ENT_SAF]
WHERE DATA_ENT='2023-10-04' AND SETOR <>'FDT'
GROUP BY TALHAO, SETOR, FAZENDA, LOTE, TALHAO, LIB, DATA_ENT

SELECT DISTINCT(TALHAO), SETOR, FAZENDA, LOTE, TALHAO, SUM(PL) SUMATORIA, LIB, DATA_ENT FROM [BiosChira].[dbo].[ENT_SAF]
WHERE DATA_ENT='2023-10-05' AND SETOR <>'FDT'
GROUP BY TALHAO, SETOR, FAZENDA, LOTE, TALHAO, LIB, DATA_ENT



SELECT TALHAO, DATA_ENT, HORA_ENT, DATA_SAI, HORA_SAI, PL, DIA  FROM [BiosChira].[dbo].[ENT_SAF]
WHERE TALHAO='LB01E06T6' AND YEAR(DIA)='2023'
--ORDER BY DATA_ENT
--WHERE DATA_ENT='2023-10-05' AND HORA_ENT BETWEEN '06:00' AND '24:59' AND TALHAO='LB01E06T6' AND SETOR<>'FDT'
--ORDER BY DATA_ENT
UNION
SELECT DATA_ENT,DIA FROM [BiosChira].[dbo].[ENT_SAF]
WHERE DATA_ENT='2023-10-06' AND HORA_ENT BETWEEN '00:00' AND '05:59' AND TALHAO='LB01E06T6' AND SETOR<>'FDT'
--ORDER BY DATA_ENT


SELECT DATA_ENT, HORA_ENT FROM [BiosChira].[dbo].[ENT_SAF]
WHERE DATA_ENT='2023-10-24'-- AND HORA_ENT BETWEEN '06:00' AND '24:59'
ORDER BY HORA_ENT

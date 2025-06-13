SELECT DISTINCT(nromov), count(idperplan) cuenta, idlogin, fecha
FROM Asistencia.dbo.control 
group by nromov, idlogin, fecha
having count(idperplan) > 1 and MONTH(fecha) = MONTH(GETDATE()) AND YEAR(fecha) = YEAR(GETDATE())
order by fecha

-- Select rows from a Table or View '[TableOrViewName]' in schema '[dbo]'
SELECT * FROM Asistencia.dbo.control
WHERE nromov IN (

)

SELECT max(nromov)
from  Asistencia.dbo.control

--
INSERT INTO CONTROL SELECT nromov, idperplan, tipomov, fecha, hora, regEntrada, flag, dni, regSalida, mensaje, idlogin, teclado FROM Asistencia.dbo.control
WHERE nromov in ('15122427') GROUP BY nromov, idperplan, tipomov, fecha, hora, regEntrada, flag, dni, regSalida, mensaje, idlogin, teclado
--

select * FROM Asistencia.dbo.control
WHERE idperplan = '00388754' and fecha = '2023-10-03' and nromov = '1417848'

--UPDATE Asistencia.dbo.control set nromov = '1476898' WHERE idperplan = '01555617' and fecha = '2023-11-09' and nromov = '1475307'


SELECT * FROM Asistencia.dbo.control WHERE nromov = '15140923'
--UPDATE Asistencia.dbo.control set nromov = '1490055' WHERE idperplan = '00388894' and fecha = '2023-11-17' and nromov = '1489845'
------

SELECT * FROM Asistencia.dbo.control WHERE nromov = '15107186'
UPDATE Asistencia.dbo.control set nromov = '15333411' WHERE idperplan = '00361121' and fecha = '2024-06-28' and nromov = '15333120'


--INSERT INTO Asistencia.dbo.control (nromov, idperplan, tipomov, fecha,hora, regEntrada, flag, dni, regSalida, mensaje, idlogin, teclado) VALUES ()

--="UPDATE Asistencia.dbo.control set nromov = '"&A2&"' WHERE idperplan = '"&C2&"' and fecha = '"&TEXTO(E2;"yyyy-mm-dd")&"' and nromov = '"&B2&"'"

-- WITH CTE AS (
--     SELECT 
--         *,
--         ROW_NUMBER() OVER (PARTITION BY nromov, idperplan, tipomov, fecha, hora, regEntrada, flag, dni, regSalida, mensaje, idLogin, teclado ORDER BY (SELECT NULL)) AS rn
--     FROM Asistencia.dbo.control
--     WHERE nromov IN (
--         
--     )
-- )
-- DELETE FROM CTE WHERE rn = 2;

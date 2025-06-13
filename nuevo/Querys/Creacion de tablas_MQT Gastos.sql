--===== CREACION DE BASE DE DATOS Y TABLAS =====
CREATE DATABASE MQT_Gastos_Chira

USE MQT_Gastos_Chira

CREATE TABLE TBL_Ordenes_Ceco (
    Id VARCHAR(25),
    Soc CHAR(3) NOT NULL,
    Orden_Ceco VARCHAR(20) NOT NULL,
    Cta_Contable VARCHAR(20) NOT NULL,
    Desc_Ceco_Orden VARCHAR(MAX) NOT NULL,
    CeBe VARCHAR(20),
    Tipo_Gasto VARCHAR(MAX) NOT NULL,
    Gerencia VARCHAR(MAX) NOT NULL,
    Jefe_Responsable VARCHAR(MAX) NOT NULL,
    Macro_Fundo VARCHAR(MAX),
    Area VARCHAR(MAX),
    Sistema VARCHAR(MAX) NOT NULL,
    Subsistema VARCHAR(MAX) NOT NULL,
    Id_Agrupador INT NOT NULL,
    Agrupador VARCHAR(MAX) NOT NULL,
    Id_Jefe_Revisor INT NOT NULL,
    Jefe_Revisor VARCHAR(MAX) NOT NULL,
    Estado INT NOT NULL
);

SELECT * FROM TBL_Ordenes_Ceco
DROP TABLE TBL_Ordenes_Ceco
--AGREGAR CTA CONTABLE
--CONCATENADO DE ORDEN Y CTA CONTABLE
CREATE TABLE TBL_Cta_Contables (
    Cta_Contable VARCHAR(20) NOT NULL,
    Desc_Cta_Contable VARCHAR(MAX) NOT NULL,
    Estado INT NOT NULL
);

CREATE TABLE TBL_Materiales (
    N_Material VARCHAR(25) NOT NULL,
    Desc_Material VARCHAR(MAX) NOT NULL,
    UM VARCHAR(10) NOT NULL,
    Status CHAR(5),
    Estado INT NOT NULL
);

CREATE TABLE TBL_Tipo_Cambio (
    Mes CHAR(2),
    Anhio CHAR(4),
    Tipo_Cambio FLOAT
);

CREATE TABLE TBL_Soc (
    Soc CHAR(3),
    Desc_Soc VARCHAR(MAX),
    Estado INT
);

CREATE TABLE TBL_Agrupadores (
    Id_Agrupador INT NOT NULL,
    Cta_Contable VARCHAR(20) NOT NULL,
    Agrupador VARCHAR(MAX) NOT NULL,
    Estado INT NOT NULL
);

CREATE TABLE TBL_Tipo_Man (
    Cta_Contable VARCHAR(20) NOT NULL,
    Desc_Tipo_Man VARCHAR(MAX) NOT NULL,
    Estado INT NOT NULL
);

CREATE TABLE TBL_Jefe_Revisor (
    Id_Agrupador INT NOT NULL,
    Jefe_Revisor VARCHAR(MAX) NOT NULL,
    Estado INT NOT NULL
);
DROP TABLE TBL_Jefe_Revisor
--====
CREATE TABLE TBL_Gastos_Reales (
    --Id VARCHAR(25) NOT NULL,
    Escenario VARCHAR(10),
    Soc CHAR(3) NOT NULL,
    Orden_Ceco VARCHAR(20),
    Orden_PM VARCHAR(10),
    N_Cuenta VARCHAR(10) NOT NULL,
    En_mon_so DECIMAL(20,9) NOT NULL,
    --FeContab VARCHAR(10) NOT NULL,
    FeContab DATE NOT NULL,
    Material VARCHAR(30) NOT NULL,
    Cantidad DECIMAL(20,9) NOT NULL,
    Texto VARCHAR(MAX),
    N_doc TEXT NOT NULL,
    N_doc_ref TEXT NOT NULL,
    Doc_Compras VARCHAR(15),
    Tipo_Arch VARCHAR(11) NOT NULL,
    Ope VARCHAR(8) NOT NULL,
    --Estado INT NOT NULL
);

CREATE TABLE TBL_Gastos_PB_PY (
    Escenario VARCHAR(20),
    Soc CHAR(3) NOT NULL,
    Orden_Ceco VARCHAR(20) NOT NULL,
    N_Cuenta VARCHAR(10) NOT NULL,
    En_mon_so DECIMAL(20,9) NOT NULL,
    FeContab DATE NOT NULL,
    Material VARCHAR(30) NOT NULL,
    Cantidad DECIMAL(20,9) NOT NULL,
    Texto VARCHAR(MAX) NOT NULL,
    UM VARCHAR(10) NOT NULL,
    Mes_PB_PY INT,
    --Estado INT NOT NULL
);

CREATE TABLE Mqt_Version (
    Tipo CHAR(2),
    Anio INT,
    Fecha DATE,
    Versi INT,
    Escenario VARCHAR(30),
    Indicador INT
)

--===QUERYS

ALTER TABLE TBL_Jefe_Revisor
   ADD CONSTRAINT FK_Id_Agrupador FOREIGN KEY (Id_Agrupador)
      REFERENCES TBL_Agrupadores (Id)
      ON DELETE CASCADE
      ON UPDATE CASCADE
;


INSERT INTO TBL_Soc VALUES ('153', 'Agricola del Chira', 1)
INSERT INTO TBL_Soc VALUES ('157', 'Sucroalcolera del Chira', 1)
INSERT INTO TBL_Soc VALUES ('158', 'Bioenergia del Chira', 1)

DELETE FROM TBL_Soc
SELECT * FROM TBL_Soc

SELECT * FROM Mqt_Version
DELETE FROM Mqt_Version

SELECT * FROM TBL_Gastos_Reales
DELETE FROM TBL_Gastos_Reales

SELECT Escenario, Mes_PB_PY FROM TBL_Gastos_PB_PY
ORDER BY Escenario, Mes_PB_PY
DELETE FROM TBL_Gastos_PB_PY

INSERT INTO TBL_Ordenes_Ceco
VALUES (NULL, 153, '456ABC', 12345, 'PRUEBA', 'PRUEBA', 'PRUEBA', 'PRUEBA', 'PRUEBA', 'PRUEBA', 'PRUEBA', 'PRUEBA', 'PRUEBA', '1')

INSERT INTO TBL_Gastos_PB_PY
VALUES (NULL, '153', '153AG09901', '684110006', 8205.49, '01.10.2024', 'SERVICIO', 0, 'PruebasPY', 'und', 4)

UPDATE TBL_Gastos_Reales
SET Escenario = 'Re2024';

UPDATE TBL_Gastos_PB_PY
SET Escenario = 'PY2024 - version 1'
WHERE Escenario IS NULL;

UPDATE TBL_Gastos_PB_PY
SET Mes_PB_PY = 02
WHERE Mes_PB_PY IS NULL


SELECT Escenario FROM TBL_Gastos_PB_PY
GROUP BY Escenario


SELECT Tipo_Arch 
FROM TBL_Gastos_Reales
--WHERE Tipo_Arch = 'GA-A-4-2024'
GROUP BY Tipo_Arch

DELETE
FROM TBL_Gastos_Reales
WHERE Tipo_Arch = 'GO-B-4-2024'

--11952
--4749

UPDATE TBL_Ordenes_Ceco SET Id = CONCAT(Orden_Ceco,Cta_Contable) WHERE Id IS NULL

SELECT * FROM TBL_Ordenes_Ceco
SELECT * FROM TBL_Cta_Contables
SELECT * FROM TBL_Materiales
SELECT * FROM TBL_Agrupadores
SELECT * FROM TBL_Tipo_Man
SELECT * FROM TBL_Jefe_Revisor

DELETE FROM TBL_Ordenes_Ceco
DELETE FROM TBL_Cta_Contables
DELETE FROM TBL_Materiales
DELETE FROM TBL_Agrupadores
DELETE FROM TBL_Tipo_Man
DELETE FROM TBL_Jefe_Revisor


SELECT COUNT(*) AS N_Rows FROM TBL_Materiales

DELETE FROM TBL_Ordenes_Ceco WHERE Estado IN ('1')


SELECT * FROM Mqt_Version

SELECT Tipo, Anio, Fecha, Versi, Escenario, 1 Indicador
FROM (
    SELECT Tipo, Anio, Fecha, Versi, Escenario, Indicador,
           ROW_NUMBER() OVER (PARTITION BY Tipo, YEAR(Fecha), MONTH(Fecha) ORDER BY Versi DESC) Num
    FROM Mqt_Version
) Mqt2_Version
WHERE Num = 1;


--CREATE VIEW MqtVersion_Final AS
WITH UltimaVersion AS (
    SELECT Tipo, Anio, MONTH(Fecha) Mes, Versi, Escenario, 1 Indicador
    FROM (
        SELECT Tipo, Anio, Fecha, Versi, Escenario, Indicador,
               ROW_NUMBER() OVER (PARTITION BY Tipo, YEAR(Fecha), MONTH(Fecha) ORDER BY Versi DESC) AS Num
        FROM Mqt_Version
    ) AS sub
    WHERE Num = 1
)

SELECT * FROM UltimaVersion
UNION ALL
SELECT 'PY' AS Tipo,
       Anio,
       NumMes.Mes Mes,
       0 Versi,
       CONCAT('PY', Anio, ' - versión 0') Escenario,
       1 Indicador
FROM (VALUES (1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11),(12)) AS NumMes(Mes)
CROSS JOIN (SELECT DISTINCT Anio FROM UltimaVersion WHERE Tipo = 'PY') AS Anios
WHERE NOT EXISTS (
    SELECT 1
    FROM UltimaVersion
    WHERE UltimaVersion.Tipo = 'PY'
    AND UltimaVersion.Anio = Anios.Anio
    AND UltimaVersion.Mes = NumMes.Mes
)
ORDER BY Tipo, Anio, Mes;

----
WITH UltimaVersion AS (
    SELECT Tipo, Anio, Fecha, Versi, Escenario, Indicador
    FROM (
        SELECT Tipo, Anio, Fecha, Versi, Escenario, Indicador,
               ROW_NUMBER() OVER (PARTITION BY Tipo, YEAR(Fecha), MONTH(Fecha) ORDER BY Versi DESC) AS rn
        FROM Mqt_Version
    ) AS sub
    WHERE rn = 1
)
SELECT Tipo, Anio, Fecha, Versi, Escenario, Indicador
FROM UltimaVersion
UNION ALL
SELECT 'PY' AS Tipo,
       Anio,
       DATEFROMPARTS(Anio, MonthNumber.Month, 1) AS Fecha,
       0 AS Versi,
       CONCAT('PY', Anio, ' - versión 0') AS Escenario,
       0 AS Indicador
FROM (VALUES (1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11),(12)) AS MonthNumber(Month)
CROSS JOIN (SELECT DISTINCT Anio FROM UltimaVersion WHERE Tipo = 'PY') AS Anios
WHERE NOT EXISTS (
    SELECT 1
    FROM UltimaVersion
    WHERE UltimaVersion.Tipo = 'PY'
    AND UltimaVersion.Anio = Anios.Anio
    AND UltimaVersion.Fecha = DATEFROMPARTS(Anios.Anio, MonthNumber.Month, 1)
)
ORDER BY Tipo, Anio, Fecha;

----



DROP VIEW MqtVersion_Final

INSERT INTO Mqt_Version
VALUES ('PY', 2024, '2024-12-01', 1, 'PY2024 - version 1', 0)
INSERT INTO Mqt_Version
VALUES ('PY', 2024, '2024-12-01', 2, 'PY2024 - version 2', 0)
INSERT INTO Mqt_Version
VALUES ('PY', 2024, '2024-12-01', 3, 'PY2024 - version 3', 0)
INSERT INTO Mqt_Version
VALUES ('PY', 2024, '2024-12-01', 4, 'PY2024 - version 4', 0)

SELECT * FROM  MqtVersion_Final
ORDER BY Tipo, Anio, Fecha;

--// DICICONARIO //--
SELECT 
    COLUMN_NAME AS Columna,
    CASE WHEN DATA_TYPE = 'int' THEN 'Número' ELSE 'Texto' END AS 'Tipo de Dato',
    CASE 
        WHEN COLUMN_NAME = 'Estado' THEN '1 dígito'
        WHEN DATA_TYPE IN ('int', 'smallint', 'tinyint', 'bigint') THEN CAST(COALESCE(NUMERIC_PRECISION, CHARACTER_MAXIMUM_LENGTH) AS VARCHAR) + ' dígitos'
        WHEN DATA_TYPE IN ('varchar', 'char', 'text', 'nvarchar', 'nchar', 'ntext') THEN 
            CASE 
                WHEN CHARACTER_MAXIMUM_LENGTH = -1 THEN 'máxima' ELSE CAST(CHARACTER_MAXIMUM_LENGTH AS VARCHAR) + ' caracteres'
            END
        ELSE CAST(COALESCE(CHARACTER_MAXIMUM_LENGTH, NUMERIC_PRECISION) AS VARCHAR)
    END AS Longitud,
    CASE WHEN IS_NULLABLE = 'YES' THEN 'Sí' ELSE 'No' END AS 'Valores Nulos'
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'TBL_Tipo_Man' AND COLUMN_NAME != 'Id';

--// TABLA PY COMPLETA //--
--CREATE VIEW UltimaVersionConMesesFaltantes AS
WITH UltimaVersion AS (
    SELECT Tipo, Anio, MONTH(Fecha) AS Mes, Versi, Escenario, 1 AS Indicador,
           ROW_NUMBER() OVER (PARTITION BY Tipo, Anio, MONTH(Fecha) ORDER BY Versi DESC) AS Num
    FROM Mqt_Version
),
-- Filtrar la última versión de cada mes
UltimaVersionFiltered AS (
    SELECT Tipo, Anio, Mes, Versi, Escenario, Indicador, Mes AS Mes_PY,
           ROW_NUMBER() OVER (PARTITION BY Tipo, Anio, Mes ORDER BY Versi DESC) AS rn
    FROM UltimaVersion
    WHERE Num = 1
),
-- Agregar filas faltantes para PY
MesesFaltantes AS (
    SELECT 'PY' AS Tipo, Anios.Anio, NumMes.Mes AS Mes,
           COALESCE((
               SELECT TOP 1 Versi
               FROM UltimaVersionFiltered AS uv
               WHERE uv.Tipo = 'PY' AND uv.Anio = Anios.Anio AND uv.Mes < NumMes.Mes
               ORDER BY uv.Mes DESC
           ), 0) AS Versi,
           COALESCE((
               SELECT TOP 1 Escenario
               FROM UltimaVersionFiltered AS uv
               WHERE uv.Tipo = 'PY' AND uv.Anio = Anios.Anio AND uv.Mes < NumMes.Mes
               ORDER BY uv.Mes DESC
           ), CONCAT('PY', Anios.Anio, ' - versión 0')) AS Escenario,
           1 AS Indicador,
           COALESCE((
               SELECT TOP 1 Mes_PY
               FROM UltimaVersionFiltered AS uv
               WHERE uv.Tipo = 'PY' AND uv.Anio = Anios.Anio AND uv.Mes < NumMes.Mes
               ORDER BY uv.Mes DESC
           ), NumMes.Mes - 1) AS Mes_PY
    FROM (VALUES (1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11),(12)) AS NumMes(Mes)
    CROSS JOIN (SELECT DISTINCT Anio FROM UltimaVersionFiltered WHERE Tipo = 'PY') AS Anios
    WHERE NOT EXISTS (
        SELECT 1
        FROM UltimaVersionFiltered AS uv
        WHERE uv.Tipo = 'PY'
        AND uv.Anio = Anios.Anio
        AND uv.Mes = NumMes.Mes
    )
)
-- Combinar la última versión y los meses faltantes
SELECT Tipo, Anio, Mes, Versi, Escenario, Indicador, Mes_PY
FROM UltimaVersionFiltered
WHERE rn = 1
UNION ALL
SELECT Tipo, Anio, Mes, Versi, Escenario, Indicador, Mes_PY
FROM MesesFaltantes
ORDER BY Tipo, Anio, Mes;

--// TABLA REAL COMPLETA //--
--CREATE VIEW Vista_AnioMesEscenario AS
WITH FeContabDates AS (
    SELECT DISTINCT YEAR(FeContab) AS Anio, MONTH(FeContab) AS Mes
    FROM TBL_Gastos_Reales
),
FechaIndicador AS (
    SELECT DISTINCT YEAR(Fecha) AS Anio, MONTH(Fecha) AS Mes, Escenario
    FROM Mqt_Version
    WHERE Indicador = 1 AND SUBSTRING(Escenario, 1, 2) = 'PY'
),
TodosMeses AS (
    SELECT DISTINCT Anio, Mes
    FROM (
        SELECT Anio, Mes
        FROM FeContabDates
        UNION
        SELECT Anio, Mes
        FROM FechaIndicador
        UNION
        SELECT DISTINCT Anio, n AS Mes
        FROM FeContabDates
        CROSS JOIN (VALUES (1), (2), (3), (4), (5), (6), (7), (8), (9), (10), (11), (12)) AS NumMes(n)
    ) AS Combined
),
EscenariosFeContab AS (
    SELECT tm.Anio, tm.Mes, 'RE' + CAST(tm.Anio AS VARCHAR(4)) AS Escenario
    FROM TodosMeses tm
    WHERE tm.Mes <= (
        SELECT MAX(fcd.Mes)
        FROM FeContabDates fcd
        WHERE fcd.Anio = tm.Anio
    )
),
EscenariosFechaIndicador AS (
    SELECT tm.Anio, tm.Mes, fi.Escenario
    FROM TodosMeses tm
    JOIN FechaIndicador fi
    ON tm.Anio = fi.Anio AND tm.Mes >= fi.Mes
),
EscenariosFinales AS (
    SELECT tm.Anio,tm.Mes,
        COALESCE(
            (SELECT TOP 1 efi.Escenario FROM EscenariosFechaIndicador efi WHERE efi.Anio = tm.Anio AND efi.Mes = tm.Mes),
            (SELECT TOP 1 efc.Escenario FROM EscenariosFeContab efc WHERE efc.Anio = tm.Anio AND efc.Mes = tm.Mes)
        ) AS Escenario
    FROM TodosMeses tm
)
SELECT DISTINCT Anio, Mes, Escenario
FROM EscenariosFinales
ORDER BY Anio, Mes;




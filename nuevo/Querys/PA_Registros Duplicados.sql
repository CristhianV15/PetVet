-- CREATE TABLE #Temp_DuplicadosGaritas (
--     nromov INT,
--     idperplan CHAR(8), 
--     idlogin VARCHAR(20),
--     fecha DATE
-- )

CREATE PROCEDURE RegistrosDuplicados_Garitas
   
AS
BEGIN
    DECLARE @fecha DATE;
    DECLARE @nromov INT;
    DECLARE @idperplan CHAR(8);
    DECLARE @nroMax INT;
    DECLARE @nromovActual INT;

    INSERT INTO #Temp_DuplicadosGaritas (nromov, idperplan, idlogin, fecha)
    SELECT DISTINCT(nromov), count(idperplan) cuenta, idlogin, fecha
    FROM Asistencia.dbo.control 
    GROUP BY nromov, idlogin, fecha
    HAVING count(idperplan) > 1 AND MONTH(fecha) = MONTH(GETDATE()) AND YEAR(fecha) = YEAR(GETDATE())
    ORDER BY fecha

    IF @@ROWCOUNT > 0
    BEGIN
        SELECT @nroMax = MAX(nromov) FROM Asistencia.dbo.control

        DECLARE CursorNromov CURSOR FOR
        SELECT nromov
        FROM #Temp_DuplicadosGaritas;

        OPEN CursorNromov;

        FETCH NEXT FROM CursorNromov INTO @nromovActual;

        WHILE @@FETCH_STATUS = 0
        BEGIN
            SELECT idperplan FROM Asistencia.dbo.control
            WHERE nromov = @nromovActual;
            
            
            
            
            FETCH NEXT FROM CursorNromov INTO @nromovActual;
        END


        CLOSE CursorNromov;
        DEALLOCATE CursorNromov;

    END
    ELSE
    BEGIN
        PRINT 'No se encontraron resultados.';
    END
END;


--EXEC RegistrosDuplicados_Garitas

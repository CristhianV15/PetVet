--SERV: BiosalcApp   BD:BiosChira
--- ========================= CLIMA
SELECT              COD_PUESTO, NOM_SECTOR, MES, AÑO, FECHA, HORA_SOL, DIR_VIENTO, PRECIPITACION, TEMP_MIN, TEMP_MAX, TEMP_MEDIA, 
					VELOC_MAX, E_SOLAR, EVAPO_TRANS, Hum_Rel_Min, Hum_Rel_Max, Hum_Rel_Avg, DifTermico
FROM                VW_GERENCIAL_CLIMA
WHERE               COD_PUESTO IN ('03','10') AND YEAR (FECHA)>=2015

--SELECT		C.POSTO Puesto, P.NOME Estacion, C.DIA Dia, MIN(C.TMINIMA) TMin, MAX(C.TMAXIMA) TMax, AVG(C.TMEDIA) TProm, SUM(C.RADIACAO) RadiacíonSolar, SUM(C.ESOLAR) AS EnergiaSolar,
--				AVG(UMIDADE) Humedad, SUM(C.PRECIPITACAO) Precipitacion, SUM(C.EVAPO_TRANS) Evapotranspiracion
--FROM			CLIMA C 
--INNER JOIN	POSTOMET P ON C.Posto=P.CODIGO 
--WHERE		P.Codigo IN ('03','10') AND YEAR(DATA)>=2015
--GROUP BY	C.POSTO, P.NOME, C.DIA

--- ========================= BIOMETRÍA
SELECT			Q.MF+Q.FD+Q.EQ+Q.TR+Q.Safra ID_Eq_Tr,
				Q.MF,
				Q.FD, 
				Q.EQ, 
				Q.TR, 
				Q.Safra, 
				Area_Parcela,
				P.Area Area_Turno,
				P.FCultivo,
				DATEDIFF(DAY,FCultivo,Dia) Edad,
				Dia, 
				TML, 
				Long, 
				(Long-LAG(Long,1,Long) OVER(PARTITION BY Q.MF, Q.FD, Q.EQ, Q.TR, Q.Safra ORDER BY Q.MF, Q.FD, Q.EQ, Q.TR, Q.Safra, Dia))/(DATEDIFF(DAY, LAG(Dia,1,Dia) OVER(PARTITION BY Q.MF, Q.FD, Q.EQ, Q.TR, Q.Safra ORDER BY Q.MF, Q.FD, Q.EQ, Q.TR, Q.Safra, Dia),Dia)+1) Tasa_Crecimiento,
				Diam,
				Long_Nudo,
				Núm_Nudo
FROM			(
				SELECT			S.REGIAO_AGR MF, C.SETOR FD, C.FAZENDA EQ, C.LOTE TR, C.SAFRA Safra,
								C.DATA Dia,
								SUM(C.ANA3_N*T.Area_P)/ SUM(T.Area_P)TML,
								SUM(C.ANA4_N*T.Area_P)/ SUM(T.Area_P) Long,
								SUM(C.ANA5_N*T.Area_P)/ SUM(T.Area_P) Diam,
								SUM(C.ANA6_N*T.Area_P)/ SUM(T.Area_P) Long_Nudo,
								SUM(C.ANA7_N*T.Area_P)/ SUM(T.Area_P) Núm_Nudo,
								SUM(T.Area_P) Area_Parcela
				FROM			CA_ANALISIS_DIG C
				INNER JOIN		SETOR S ON C.SETOR=S.CODIGO
				INNER JOIN		TALHAO T ON C.SETOR+C.FAZENDA+C.LOTE+C.TALHAO=T.SETOR+T.FAZ+T.LOTE+T.TAL
				WHERE			C.CODIGO='CHI60' AND C.SEQUENCIA<>99 AND S.REGIAO_AGR IN ('ML') AND YEAR(DATA)>=2015 AND NOT C.SETOR IN ('HT01') AND NOT C.SETOR+C.FAZENDA+C.LOTE IN ('SJ01EQ02TR06','SJ01EQ01TR01')
				GROUP BY		S.REGIAO_AGR, C.SETOR, C.FAZENDA, C.LOTE, C.SAFRA, C.DATA
				HAVING		SUM(T.Area_P)>0
				) Q
INNER JOIN		CUBE_MAESTRO_PARCELAS P ON Q.FD+Q.EQ+Q.TR+Q.SAFRA=P.FD+P.EQ+P.TR+P.SAFRA

--- ========================= MAESTRO DE PARCELAS
SELECT			MF+FD+EQ+TR+Safra ID_Eq_Tr,
				MF, FD, EQ, TR, Safra, Var Variedad, Est Estado, Area, FCultivo, FAgoste, FCosecha, Broza, Suelo
FROM			CUBE_MAESTRO_PARCELAS
WHERE			MF IN ('ML') AND NOT FD IN ('HT01') AND NOT FD+EQ+TR IN ('SJ01','SJ01EQ01TR01')

--- ========================= PODUCCIONES
SELECT			P.REGIAO_AGR+P.Fundo+P.Equipo+P.Turno+P.Safra ID_Eq_Tr,
				P.REGIAO_AGR MF, 
				P.Fundo FD,
				P.Equipo EQ, 
				P.Turno TR, 
				P.Safra, 
				P.Variedad Variedad,
				P.FecSiembra FCultivo, 
				DATEADD(DAY,SUM(DATEDIFF(DAY, FecSiembra,FecProd)*P.AreaCosecha)/SUM(AreaCosecha),P.FecSiembra) FCosecha,
				SUM(P.Tn) TM, 
				SUM(P.Tn)/SUM(P.Areacosecha) T_Ha,
				SUM(P.Areacosecha) AreaCosechada, 
				P.Corte Estado
FROM			IND_PRODUCCION P
WHERE			P.REGIAO_AGR IN ('ML') AND NOT P.Fundo IN ('HT01') AND YEAR(FecProd)>=2015 AND NOT P.Fundo IN ('HT01') 
				AND NOT P.Fundo+P.Equipo+P.Turno IN ('SJ01EQ02TR06','SJ01EQ01TR01')
GROUP BY		P.REGIAO_AGR, P.Fundo, P.Equipo, P.Turno, P.Parcela, P.Safra, P.Fecsiembra, Area_P,P.Corte,P.Variedad

--- ========================= PRE-COSECHAS
SELECT			S.REGIAO_AGR+P.SETOR+P.FAZENDA+P.LOTE+P.Safra ID_Eq_Tr,
				S.REGIAO_AGR MF, P.SETOR FD, P.FAZENDA EQ, P.LOTE TR, P.Safra, P.Dia,
				AVG(CASE WHEN NOT P.ANA59 IS NULL THEN P.ANA59 END) BRIX, 
				AVG(CASE WHEN NOT P.ANA67 IS NULL THEN P.ANA67 END) FIBRA, 
				AVG(CASE WHEN NOT P.ANA74 IS NULL THEN P.ANA74 END) HUM, 
				AVG(CASE WHEN ISNULL(P.ANA60,0)>0 THEN P.ANA73 ELSE P.ANA93 END) ART,
				AVG(CASE WHEN ISNULL(P.ANA60,0)>0 THEN P.ANA69 ELSE P.ANA95 END) POL,
				AVG(CASE WHEN ISNULL(P.ANA60,0)>0 THEN P.ANA70 ELSE P.ANA3 END) AR,				
				AVG(CASE WHEN ISNULL(P.ANA60,0)>0 THEN P.ANA64 ELSE P.ANA97 END) PUREZA
FROM			CAD_PRECOL_DIG P
INNER JOIN		SETOR S ON P.SETOR=S.CODIGO
WHERE			S.REGIAO_AGR IN ('ML') AND YEAR(P.DIA)>=2015 AND NOT P.SETOR IN ('HT01') AND NOT P.SETOR+P.FAZENDA+P.LOTE IN ('SJ01EQ02TR06','SJ01EQ01TR01','SJ01EQ01TR02','SJ01EQ01TR03')
GROUP BY		S.REGIAO_AGR, P.SETOR, P.FAZENDA, P.LOTE, P.Safra, P.Dia

--- ========================= PÉRDIDA DE COSECHA
SELECT			*
FROM			(
				SELECT			MACROFUNDO MF, FUNDO FD, EQUIPO EQ, TURNO TR, PARCELA PAR, Safra, Fecha, AVG(TM_HA) TM_Ha,
								AVG(Trozada_TM_Ha) Trozada_TM_Ha, AVG(Entera_TM_Ha) Entera_TM_Ha, AVG(Soplada_TM_Ha) Soplada_TM_Ha, AVG(Perd_Cosechadora_TM_Ha) Perd_Cosechadora_TM_Ha, 
								AVG(Trozada_TM_Ha+Entera_TM_Ha+Soplada_TM_Ha) Perd_Total_TM_Ha,
								AVG(Trozada_Porc_TM) Trozada_Porc_TM, AVG(Entera_Porc_TM) Entera_Porc_TM, AVG(Soplada_Porc_TM) Soplada_Porc_TM, AVG(Perd_Cosechadora_Porc_TM) Perd_Cosechadora_Porc_TM,
								AVG(Trozada_Porc_TM+Entera_Porc_TM+Soplada_Porc_TM) Perd_Total_Porc_Ha
				FROM			VW_PERDIDA_COSECHA_BRUNA
				WHERE			MACROFUNDO IN ('HC','LB','LB2','ML','SV')
				GROUP BY		MACROFUNDO, FUNDO, EQUIPO, TURNO, PARCELA, Safra, Fecha
				) Q

--- ========================= FERTILIZACIÓN
SELECT			S.REGIAO_AGR+D.SETOR+D.FAZ+D.LOTE+D.Safra ID_Eq_Tr,
				S.REGIAO_AGR MF, D.SETOR FD, D.FAZ EQ, D.LOTE TR, D.TAL PAR, D.SAFRA Safra, T.Area, E.DATA Fecha,
				SUM(E.QTDE*D.AREA/E.AREA*P.N/100)/AVG(T.Area) N, SUM(E.QTDE*D.AREA/E.AREA*P.P/100)/AVG(T.Area)  P, SUM(E.QTDE*D.AREA/E.AREA*P.K/100)/AVG(T.Area)  K
FROM			OS_APLPROD E
INNER JOIN		OS_APLPROD_DET D ON E.OS=D.OS AND E.NUMERO=D.NUMERO
INNER JOIN		OUTROSPROD P ON E.PRODUTO=P.CODIGO
INNER JOIN		SETOR S ON D.SETOR=S.CODIGO
LEFT JOIN		CUBE_MAESTRO_PARCELAS T ON D.SETOR+D.FAZ+D.LOTE+D.TAL+D.SAFRA=T.FD+T.EQ+T.TR+T.PAR+T.SAFRA
WHERE			YEAR(E.DATA)>=2017 AND ISNULL(P.N,0)+ISNULL(P.P,0)+ISNULL(P.K,0)>0 AND T.AREA>0 AND E.AREA>0
GROUP BY		S.REGIAO_AGR, D.SETOR, D.FAZ, D.LOTE, D.TAL, D.SAFRA, T.Area, E.PRODUTO, P.NOME, E.DATA

--- ========================= COEFICIENTE DE UNIFORMIDAD
SELECT			FD+'-'+EQ+'-'+TR Eq_Tr,
				MF, FD, EQ, TR, YEAR(FCosecha) Anho_Cos, EQ+'-'+TR Equipo_Turno, Safra, PAR, Area, FCultivo, FCosecha, Fecha, Evaluacion, Manguera, 
				Analisis_caudal, 
				Presion_Inicial, Presion_Final,
				Caudal_Prom, Caudal_Max, Caudal_Min, 
				(Caudal_Max-Caudal_Min) Caudal_Rango,
				Registros,
				Caudal_Nominal,
				Coef_Uniformidad/100 Coef_Uniformidad,
				Merriam_Keller,
				Q01, Q02, Q03, Q04, Q05, Q06, Q07, Q08, Q09, Q10, Q11, Q12, Q13, Q14, Q15, Q16
FROM			EXTRACT_CoeficienteUniformidad(0) 
WHERE			MF='ML' AND YEAR(Fecha) BETWEEN 2017 AND 2022 AND NOT FD IN ('HT01') AND NOT FD+EQ+TR IN ('SJ01EQ02TR06','SJ01EQ01TR01')

--- ========================= RIEGO
SELECT			S.REGIAO_AGR+R.SETOR+R.FAZ+R.LOTE+R.Safra ID_Eq_Tr,
				S.REGIAO_AGR MF, R.SETOR FD, R.FAZ EQ, R.LOTE TR, R.Safra, 
				YEAR(P.FCosecha) Anho_Cos,
				R.DATA Dia, YEAR(R.DATA) Anho, MONTH(R.DATA) Mes, P.Area, SUM(R.M3) M3, P.Est, SUM(R.M3)/AVG(P.AREA) M3_Ha
FROM			RI_IRRIG_TAL R
INNER JOIN		SETOR S ON R.SETOR=S.CODIGO
LEFT JOIN		CUBE_MAESTRO_PARCELAS P ON R.SETOR+R.FAZ+R.LOTE+R.TAL+R.SAFRA=P.FD+P.EQ+P.TR+P.PAR+P.SAFRA
WHERE			S.REGIAO_AGR IN ('ML') AND YEAR(R.DATA)>=2015 AND NOT R.SETOR IN ('HT01') AND NOT R.SETOR+R.FAZ+R.LOTE IN ('SJ01EQ02TR06','SJ01EQ01TR01')
GROUP BY		S.REGIAO_AGR, R.SETOR, R.FAZ, R.LOTE, R.Safra, YEAR(P.FCosecha), R.DATA, P.Area, P.Est



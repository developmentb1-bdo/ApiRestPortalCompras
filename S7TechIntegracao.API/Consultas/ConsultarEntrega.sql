SET DATEFORMAT YMD

SELECT
	A0."DocEntry",
	A0."DocNum",
	A0."Serial",
	A0."DocDate",
	A0."DocDueDate",
	A0."TaxDate",
	A0."SeriesStr",
	A0."DocTotal",
	A0."QoP",
	CASE
		WHEN A0."CANCELED" <> 'N' THEN
			'Cancelada'
		WHEN A0."DocStatus" = 'O' THEN
			'Aberta'
		WHEN A0."DocStatus" = 'C' THEN
			'Faturada'
	END AS "DocStatus",
	A0."CANCELED",
	A0."Serial", 
	A0."Chave acesso"
FROM
(
	SELECT
		T0."DocEntry",
		T0."DocNum",
		T0."Serial",
		T0."DocDate",
		T0."DocDueDate",
		T0."TaxDate",
		T0."SeriesStr",
		T0."DocTotal",
		T3."QoP",
		T0."DocStatus",
		T0."CANCELED",
		COALESCE(T1."U_ChaveAcesso", '') AS "Chave acesso",
		COALESCE(T0."UpdateDate", T0."CreateDate") AS "Data",
		LEFT(RIGHT('00' + CAST(COALESCE(T0."UpdateTS", T0."CreateTS") AS NVARCHAR), 6), 4) AS "Horario",
		CAST(LEFT(LEFT(RIGHT('00' + CAST(COALESCE(T0."UpdateTS", T0."CreateTS") AS NVARCHAR), 6), 4), 2) AS INT) AS "HH",
		CAST(RIGHT(LEFT(RIGHT('00' + CAST(COALESCE(T0."UpdateTS", T0."CreateTS") AS NVARCHAR), 6), 4), 2) AS INT) AS "MM"
	FROM ODLN T0
		INNER JOIN DLN12 T3 ON T3."DocEntry" = T0."DocEntry"
		LEFT JOIN "@SKL25NFE" T1 ON T0."DocEntry" = T1."U_DocEntry" AND COALESCE(T1."U_tipoDocumento", '') = 'EM'
	WHERE
		COALESCE(T1."U_inStatus", 0) = 3
		AND COALESCE(T1."U_cdErro", 0) = 100	
) A0
WHERE
	DATEADD(MINUTE, A0.MM, DATEADD(HOUR, A0.HH, A0."Data")) > '{0}'
SET DATEFORMAT YMD

SELECT
	A0."Código Interno nota",
	A0."Código nota", 
	A0."Serial nota",
	A0."Código Interno entrega",
	A0."Código entrega", 
	A0."Código Interno pedido",
	A0."Código pedido", 
	A0."Chave acesso"
FROM
(
	SELECT
		T0."DocEntry" AS "Código Interno nota",
		T0."DocNum" AS "Código nota", 
		T0."Serial" AS "Serial nota",
		T3."DocEntry" AS "Código Interno entrega",
		T3."DocNum" AS "Código entrega", 
		T5."DocEntry" AS "Código Interno pedido",
		T5."DocNum" AS "Código pedido", 
		COALESCE(T6."U_ChaveAcesso", '') AS "Chave acesso",
		COALESCE(T0."UpdateDate", T0."CreateDate") AS "Data",
		LEFT(RIGHT('00' + CAST(COALESCE(T0."UpdateTS", T0."CreateTS") AS NVARCHAR), 6), 4) AS "Horario",
		CAST(LEFT(LEFT(RIGHT('00' + CAST(COALESCE(T0."UpdateTS", T0."CreateTS") AS NVARCHAR), 6), 4), 2) AS INT) AS "HH",
		CAST(RIGHT(LEFT(RIGHT('00' + CAST(COALESCE(T0."UpdateTS", T0."CreateTS") AS NVARCHAR), 6), 4), 2) AS INT) AS "MM"
	FROM OINV T0
		INNER JOIN INV1 T1 ON T1."DocEntry" = T0."DocEntry"
		INNER JOIN DLN1 T2 ON T2."DocEntry" = T1."BaseEntry" AND T2."LineNum" = T1."BaseLine"
		INNER JOIN ODLN T3 ON T3."DocEntry" = T2."DocEntry" AND T3."ObjType" = T1."BaseType"
		INNER JOIN RDR1 T4 ON T4."DocEntry" = T2."BaseEntry" AND T4."LineNum" = T2."BaseLine"
		INNER JOIN ORDR T5 ON T5."DocEntry" = T4."DocEntry" AND T5."ObjType" = T2."BaseType"
		LEFT JOIN "@SKL25NFE" T6 ON T0."DocEntry" = T6."U_DocEntry" AND COALESCE(T6."U_tipoDocumento", '') = 'NS'
	WHERE
		COALESCE(T6."U_inStatus", 0) = 3
		AND COALESCE(T6."U_cdErro", 0) = 100
	GROUP BY T0."DocEntry", T0."DocNum", T0."Serial", T3."DocEntry", T3."DocNum", T5."DocEntry", T5."DocNum", COALESCE(T6."U_ChaveAcesso", ''), COALESCE(T0."UpdateTS", T0."CreateTS"), COALESCE(T0."UpdateDate", T0."CreateDate")
) A0
WHERE
	DATEADD(MINUTE, A0.MM, DATEADD(HOUR, A0.HH, A0."Data")) > '{0}'
SELECT
	T0."DocEntry",
	T0."DocNum",
	T0."ReqName" AS "RequesterName",
	T0."U_S7T_DataSolicitacao",
	T0."DocDate",
	T0."DocDueDate",
	T0."ReqDate" AS "RequriedDate",
	T0."BPLName",
	CASE T1."Status"
		WHEN 'W' THEN
			'Pendente'
		WHEN 'Y' THEN
			'Aprovado' 
		WHEN 'N' THEN
			'Reprovado'
	END
FROM ODRF T0
	INNER JOIN OWDD T1 ON T1."DraftEntry" = T0."DocEntry" AND T1."ObjType" = T0."ObjType"
	INNER JOIN DRF1 T2 ON T2."DocEntry" = T0."DocEntry"
	INNER JOIN "@S7T_APROVOPRQ" T3 ON T3."U_CentroCusto" = T2."OcrCode2"
WHERE
	T0."ObjType" = '1470000113'
	AND T0."CANCELED" = 'N'
	AND COALESCE(T3."U_GestorArea", 0) = {0}
GROUP BY T0."DocEntry", T0."DocNum", T0."ReqName", T0."U_S7T_DataSolicitacao", T0."DocDate", T0."DocDueDate", T0."ReqDate", T0."BPLName", T2."OcrCode2", T1."Status"
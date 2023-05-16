/*SELECT
	T0."DocEntry",
	T0."DocNum",
	T0."Requester",
	T0."ReqName" AS "RequesterName",
	T0."U_S7T_DataSolicitacao",
	T0."DocDate",
	T0."DocDueDate",
	T0."ReqDate" AS "RequriedDate",
	T0."BPLName",
	'Aprovado' AS "StatusAprovacao"
FROM OPOR T0
WHERE
	T0."ReqType" = 171
	AND T0."Requester" = '{0}'
	AND T0."CANCELED" = 'N'
	AND T0."DataSource" = 'O'

UNION*/

SELECT
	T0."DocEntry",
	T0."DocNum",
	T0."Requester",
	T0."ReqName" AS "RequesterName",
	T0."TaxDate" AS "U_S7T_DataSolicitacao",
	T0."DocDate",
	T0."DocDueDate",
	T0."ReqDate" AS "RequriedDate",
	T0."BPLName",
	CASE T1."Status"
		WHEN 'W' THEN
			'Pendente'
		WHEN 'N' THEN
			'Rejeitado'
	END AS "StatusAprovacao"
FROM ODRF T0
	INNER JOIN OWDD T1 ON T1."DraftEntry" = T0."DocEntry" AND T1."ObjType" = T0."ObjType"
	INNER JOIN "@S7T_OWDD" T2 ON T2."U_DraftEntry" = T1."DraftEntry" AND T2."U_ObjType" = T1."ObjType"
	INNER JOIN "@S7T_WDD1" T3 ON T3."Code" = T2."Code"
WHERE
	T0."ObjType" = '22'
	AND T0."CANCELED" = 'N'
	AND T1."Status" <> 'Y'
	AND T3."U_empID" = {0}
	AND T3."U_Status" = 'W'
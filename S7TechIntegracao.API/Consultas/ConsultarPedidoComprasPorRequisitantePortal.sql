SELECT
	T4."DraftEntry" AS "DocEntry",
	T0."DocNum",
	T0."DocEntry" As "DocEntryPedido",
	T1."empID" AS "Requester",
	(T1."firstName" || ' ' || T1."lastName") AS "RequesterName",
	T0."TaxDate" AS "U_S7T_DataSolicitacao",
	T0."DocDate",
	T0."DocDueDate",
	T0."TaxDate",
	T0."BPLName",
	T0."CardCode",
	T0."CardName",
	'Aprovado' AS "StatusAprovacao",
	CASE T0."CANCELED"
	  WHEN 'N' THEN
	   'Aberto'
	   WHEN 'Y' THEN
	    'Cancelado'
	END AS"Cancelled"
FROM OPOR T0
	INNER JOIN OHEM T1 ON T0."OwnerCode" = T1."empID"
	--INNER JOIN "@S7T_OWDD" T3 ON T3."U_DraftEntry" = T0."draftKey" AND T3."U_ObjType" = T0."ObjType"
	INNER JOIN OWDD T4 ON T4."DraftEntry" = T0."draftKey" AND T4."ObjType" = T0."ObjType"
WHERE
 T4."Status" = 'Y'
 AND T0."OwnerCode" = '{0}'

UNION

SELECT
	T0."DocEntry",
	T0."DocEntry",
	0 as "DocEntryPedido",
	T2."empID" AS "Requester",
	(T2."firstName" || ' ' || T2."lastName") AS "RequesterName",
	T0."TaxDate" AS "U_S7T_DataSolicitacao",
	T0."DocDate",
	T0."DocDueDate",
	T0."TaxDate",
	T0."BPLName",
	T0."CardCode",
	T0."CardName",
	CASE T1."Status"
		WHEN 'W' THEN
			'Pendente'
		WHEN 'N' THEN
			'Rejeitado'
		else
			'Error'
	END AS "StatusAprovacao",
	CASE T0."CANCELED"
	  WHEN 'N' THEN
	   'Aberto'
	   WHEN 'Y' THEN
	    'Cancelado'
	END AS"Cancelled"
FROM ODRF T0
	INNER JOIN OWDD T1 ON T1."DraftEntry" = T0."DocEntry" AND T1."ObjType" = T0."ObjType"
	INNER JOIN OHEM T2 ON T0."OwnerCode" = T2."empID"
	INNER JOIN "@S7T_OWDD" T3 ON T3."U_DraftEntry" = T0."DocEntry" AND T3."U_ObjType" = T0."ObjType"
	inner Join "@S7T_WDD1" T4 ON T4."Code" = T3."Code"
WHERE
	T0."ObjType" = '22'
	AND (T1."Status" <> 'Y' OR (T1."Status" = 'Y' and T3."U_Status" = 'Y' and T1."ProcesStat" ='Y')OR(T1."Status" = 'W' and T1."ProcesStat" = 'W' and T3."U_Status" = 'Y'))
	AND T0."OwnerCode" = '{0}'
	
	
ORDER BY "DocDate" DESC
LIMIT {1} offset {2};
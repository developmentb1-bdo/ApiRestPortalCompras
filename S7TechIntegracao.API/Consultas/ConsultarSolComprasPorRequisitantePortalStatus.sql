﻿SELECT
	T1."DraftEntry" AS "DocEntry",
	T0."DocNum",
	T0."DocEntry" AS "DocEntrySolicitacao",
	T0."AtcEntry" AS "AttachmentEntry",	
	'Aprovado' AS "StatusAprovacao",
	COALESCE(T3."U_CentroCusto", '') AS "CentroCusto",
	CASE T0."CANCELED"
	  WHEN 'N' THEN
	   'Aberto'
	   WHEN 'Y' THEN
	    'Cancelado'
	END AS"Cancelled"
FROM OPRQ T0
	INNER JOIN OWDD T1 ON T1."DraftEntry" = T0."draftKey" AND T1."ObjType" = T0."ObjType"
	INNER JOIN PRQ1 T2 ON T2."DocEntry" = T0."DocEntry"
	INNER JOIN "@S7T_APROVOPRQ" T3 ON COALESCE(T3."U_CentroCusto", '') = T2."OcrCode2"
	INNER JOIN "@S7T_APROVPRQ1" T4 ON T4."Code" = T3."Code"
WHERE
	T0."ReqType" = 171
	AND T0."Requester" = '{0}' 	
	AND T0."DocEntry" = '{1}'		
UNION

SELECT
	T0."DocEntry",
	T0."DocEntry",
	0  AS "DocEntrySolicitcao",
	T0."AtcEntry" AS "AttachmentEntry",	
	CASE T1."Status"
		WHEN 'W' THEN
			'Pendente'
		WHEN 'N' THEN
			'Rejeitado'
	END AS "StatusAprovacao",
	COALESCE(T3."U_CentroCusto", '') AS "CentroCusto",
	CASE T0."CANCELED"
	  WHEN 'N' THEN
	   'Aberto'
	   WHEN 'Y' THEN
	    'Cancelado'
	END AS"Cancelled"
FROM ODRF T0
	INNER JOIN OWDD T1 ON T1."DraftEntry" = T0."DocEntry" AND T1."ObjType" = T0."ObjType"
	INNER JOIN DRF1 T2 ON T2."DocEntry" = T0."DocEntry"
	INNER JOIN "@S7T_APROVOPRQ" T3 ON COALESCE(T3."U_CentroCusto", '') = T2."OcrCode2"
	INNER JOIN "@S7T_APROVPRQ1" T4 ON T4."Code" = T3."Code"
WHERE
	T0."ObjType" = '1470000113'	
	AND T1."Status" <> 'Y'
	AND T0."Requester" = '{0}'
	AND T0."DocEntry" = '{1}'
	
	

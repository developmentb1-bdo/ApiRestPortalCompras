SELECT
	T4."DraftEntry" AS "DocEntry",
	T0."DocNum",
	T0."DocEntry" As "DocEntryPedido",
	T0."AtcEntry" as "AttachmentEntry",
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
 AND T0."DocEntry" = '{1}'
UNION

SELECT
	T0."DocEntry",
	T0."DocEntry",
	0 as "DocEntryPedido",	
	T0."AtcEntry" as "AttachmentEntry",
	CASE T1."Status"
		WHEN 'W' THEN
			'Pendente'
		WHEN 'N' THEN
			'Rejeitado'
		WHEN 'Y' THEN
			'Aprovado'
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
	--INNER JOIN "@S7T_OWDD" T3 ON T3."U_DraftEntry" = T0."DocEntry" AND T3."U_ObjType" = T0."ObjType"
WHERE
	T0."ObjType" = '22'
	AND T1."Status" <> 'Y'
	AND T0."OwnerCode" = '{0}'
	AND T0."DocEntry" = '{1}'
	

--SELECT
--	T4."DraftEntry" ,
--	T0."DocEntry" ,
--	T0."DocNum",
--	T0."AtcEntry" as "AttachmentEntry",
--	T1."empID" AS "Requester",
--	(T1."firstName" || ' ' || T1."lastName") AS "RequesterName",
--	T0."TaxDate" AS "U_S7T_DataSolicitacao",
--	T0."DocDate",
--	T0."DocDueDate",
--	T0."TaxDate",
--	T0."BPLName",
--	T0."CardCode",
--	T0."CardName",
--	'Aprovado' AS "StatusAprovacao",
--	CASE T0."CANCELED"
--	  WHEN 'N' THEN
--	   'Aberto'
--	   WHEN 'Y' THEN
--	    'Cancelado'
--	END AS"Cancelled"
--FROM OPOR T0
--	INNER JOIN OHEM T1 ON T0."OwnerCode" = T1."empID"
--	INNER JOIN "@S7T_OWDD" T3 ON T3."U_DraftEntry" = T0."draftKey" AND T3."U_ObjType" = T0."ObjType"
--	INNER JOIN OWDD T4 ON T4."DraftEntry" = T0."draftKey" AND T4."ObjType" = T0."ObjType"
--WHERE
-- T4."Status" = 'Y'
-- AND T0."OwnerCode" = '{0}'
-- AND T0."DocEntry" = '{1}'







DO BEGIN


	CREATE LOCAL temporary table "#varTabCentroCusto" AS
	(
		SELECT DISTINCT
					 T0."U_CentroCusto"
				FROM 
					"@S7T_APROVOPRQ" T0
					INNER JOIN "@S7T_APROVPRQ1" T1
						ON T1."Code" = T0."Code"
				WHERE
					 T1."U_CodApr" = '{0}'
	);

		SELECT
			T2."DraftEntry" AS "DocEntry",
			T0."DocNum",
			T1."empID" AS "Requester",
			(T1."firstName" || ' ' || T1."lastName") AS "RequesterName",
			T0."TaxDate" AS "U_S7T_DataSolicitacao",
			T0."DocDate",
			T0."DocDueDate",
			T0."TaxDate",
			T0."BPLName",
			T0."CardCode",
			T0."CardName",
			CASE T3."Status"
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
		FROM OPOR T0
			INNER JOIN POR1 T4 ON T4."DocEntry" = T0."DocEntry"
			INNER JOIN OHEM T1 ON T1."empID" = '{0}'
			INNER JOIN OWDD T2 ON T2."DraftEntry" = T0."draftKey" AND T2."ObjType" = T0."ObjType"
			INNER JOIN WDD1 T3 ON T3."WddCode" = T2."WddCode"
		WHERE			
		 T0."TaxDate" BETWEEN '{1}' AND '{2}'
		AND T4."OcrCode2" IN (SELECT "U_CentroCusto" from "#varTabCentroCusto")
		--AND T0."CANCELED" = 'N'

		UNION
	
		SELECT
			T0."DocEntry",
			T0."DocEntry",
			T1."empID" AS "Requester",
			(T1."firstName" || ' ' || T1."lastName") AS "RequesterName",
			T0."TaxDate" AS "U_S7T_DataSolicitacao",
			T0."DocDate",
			T0."DocDueDate",
			T0."TaxDate",
			T0."BPLName",
			T0."CardCode",
			T0."CardName",
			CASE T3."Status"
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
			INNER JOIN DRF1 T4 ON T4."DocEntry" = T0."DocEntry"
			INNER JOIN OHEM T1 ON T1."empID" = '{0}'
			INNER JOIN OWDD T2 ON T2."DraftEntry" = T0."DocEntry" AND T2."ObjType" = T0."ObjType"
			INNER JOIN WDD1 T3 ON T3."WddCode" = T2."WddCode"
		WHERE
			T0."ObjType" = '22'		
		AND T0."DocStatus" = 'O'
		AND T0."TaxDate" BETWEEN '{1}' AND '{2}'
		AND T4."OcrCode2" IN (SELECT "U_CentroCusto" from "#varTabCentroCusto")
		--AND T0."CANCELED" = 'N'
		ORDER BY 
			"DocDate" DESC;
			
		drop table "#varTabCentroCusto";

END;
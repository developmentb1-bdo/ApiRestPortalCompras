DO BEGIN

CREATE LOCAL temporary table "#varTabelaPedidosPendentes" AS
(
	SELECT
		T0."DocEntry",
		T0."DocNum",
		T0."CardCode",
		T0."CardName",
		T0."ReqName" AS "RequesterName",
		T0."TaxDate" AS "U_S7T_DataSolicitacao",
		T0."DocDate",
		T0."DocDueDate",
		T0."ReqDate" AS "RequriedDate",
		T0."BPLName",
		T0."ObjType",
		CASE T1."Status"
			WHEN 'W' THEN
				'Pendente'
			WHEN 'Y' THEN
				'Aprovado' 
			WHEN 'N' THEN
				'Reprovado'
		END AS "Status",
		TO_NVARCHAR(T0."DocTotal") AS "DocTotal",
		COALESCE(T3."U_CentroCusto", '') AS "CentroCusto",
		T3."Code",
		T3."LineId"  AS "LineId" 
	FROM ODRF T0
		INNER JOIN OWDD T1 ON T1."DraftEntry" = T0."DocEntry" AND T1."ObjType" = T0."ObjType"
		INNER JOIN "@S7T_OWDD" T2 ON T2."U_DraftEntry" = T1."DraftEntry" AND T2."U_ObjType" = T1."ObjType"
		INNER JOIN "@S7T_WDD1" T3 ON T3."Code" = T2."Code"		
	WHERE
		T0."ObjType" = '22'
		AND T0."CANCELED" = 'N'
		AND T1."Status" in ('W','Y') 		
        AND T3."U_Status" in ('W','Y') 
		AND T3."U_empID" = {0}
		and T3."U_CentroCusto" <> '' 
		and T1."ProcesStat" not in ('P','A','C')		
	GROUP BY 
		T0."DocEntry", 
		T0."DocNum", 
		T0."ReqName", 
		T0."TaxDate", 
		T0."DocDate", 
		T0."DocDueDate", 
		T0."ReqDate", 
		T0."BPLName", 
		T1."Status", 
		T3."U_Status",
		T0."CardCode",
		T0."CardName",
		T0."DocTotal",
		T0."ObjType",
		COALESCE(T3."U_CentroCusto", ''),
		T3."Code",
		T3."LineId"
);

CREATE LOCAL temporary table "#varTabelaLinhasAprovacoes" AS 
(

SELECT 
	X0."Code",
	X0."U_empID",
	COALESCE(X0."U_CentroCusto", '') AS "U_CentroCusto",
	(ROW_NUMBER() OVER (PARTITION BY X0."Code", COALESCE(X0."U_CentroCusto", '') ORDER BY X0."Code", X0."LineId")) AS "LineId",
	X0."LineId" AS "LineIdOrigin",
	X0."U_Status"
FROM "@S7T_WDD1" X0
);

SELECT
	A0.*
FROM "#varTabelaPedidosPendentes" A0
	INNER JOIN "#varTabelaLinhasAprovacoes" A1 ON A0."Code" = A1."Code" AND A1."U_CentroCusto" = A0."CentroCusto" AND A0."LineId" = A1."LineIdOrigin"
WHERE
	(NOT EXISTS(SELECT X0."Code" FROM "#varTabelaLinhasAprovacoes" X0 WHERE X0."Code" = A1."Code" AND X0."LineId" < A1."LineId" AND X0."U_CentroCusto" = A1."U_CentroCusto" and X0."U_Status" = 'W') OR A1."LineId" = 1);

drop table "#varTabelaPedidosPendentes";
drop table "#varTabelaLinhasAprovacoes";

END
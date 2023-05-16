SELECT distinct
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
		T3."Code"
		
	FROM ODRF T0
		INNER JOIN OWDD T1 ON T1."DraftEntry" = T0."DocEntry" AND T1."ObjType" = T0."ObjType"
		INNER JOIN "@S7T_OWDD" T2 ON T2."U_DraftEntry" = T1."DraftEntry" AND T2."U_ObjType" = T1."ObjType"
		INNER JOIN "@S7T_WDD1" T3 ON T3."Code" = T2."Code"
		inner join "@S7T_APROVOVLPO1" t4 ON t4."U_CodApr" = t3."U_empID"
	WHERE
		T0."DocTotal" Between t4."U_AlcadaDe" and t4."U_AlcadaAte"
		and T0."ObjType" = '22'
		AND T0."CANCELED" = 'N'
		AND T1."Status" = 'W'
		AND T3."U_Status" = 'W'
		AND T3."U_empID" = {0}
		AND T3."U_CentroCusto" is null
		
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
		T3."Code"
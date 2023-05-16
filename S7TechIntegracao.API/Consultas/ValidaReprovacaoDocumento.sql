SELECT
	T0."DocEntry",
	T1."DraftEntry",
	T4."U_empID",
	T4."U_CentroCusto",
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
		WHEN 'Y' THEN
			'Aprovado'
	END AS "StatusAprovacao",
	T1."ProcesStat"
FROM OPOR T0
	INNER JOIN OWDD T1 ON T1."DraftEntry" = T0."DocEntry" AND T1."ObjType" = T0."ObjType"
	INNER JOIN WDD1	T2 ON T2."WddCode" = T1."WddCode"
	INNER JOIN "@S7T_OWDD" T3 ON T3."U_DraftEntry" = T0."DocEntry" AND T3."U_ObjType" = T0."ObjType"
	inner Join "@S7T_WDD1" T4 ON T4."Code" = T3."Code"		
WHERE
	T1."DraftEntry" = '{0}'
	and T1."Status" = 'W'
	and T3."U_Status" = 'N'
	and T4."U_Status" = 'N'
	and T1."ProcesStat" <> 'P'
SELECT
	T1."U_empID" AS "CodAprovador",
	T2."firstName" || ' ' || T2."lastName" AS "Aprovador",
	T1."U_CentroCusto" AS "CentroCusto",
	CASE T1."U_Status"
		WHEN 'W' THEN
			'Pendente'
		WHEN 'Y' THEN
			'Aprovado'
		WHEN 'N' THEN
			'Reprovado'
	END AS "Status",
	T1."U_EmailEnv" AS "EmailEnviado",
	T2."email" AS "Email",
	T1."U_Remarks" AS "Remarks",
	Concat(TO_VARCHAR (TO_DATE(T1."U_DataAprovacao"), 'DD/MM/YYYY'), Concat(' ', S7T_CONVERTTOTIME(T1."U_HoraAprovacao"))) AS "DataHora"
FROM "@S7T_OWDD" T0
	INNER JOIN "@S7T_WDD1" T1 ON T1."Code" = T0."Code"
	INNER JOIN OHEM T2 ON T2."empID" = T1."U_empID"
WHERE 
	T0."U_DraftEntry" = {0}
ORDER BY T1."LineId"
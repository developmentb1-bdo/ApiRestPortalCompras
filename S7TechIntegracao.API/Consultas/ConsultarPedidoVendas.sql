SET DATEFORMAT YMD

SELECT
	A0."DocEntry",
	A0."DocNum",
	A0."CardCode",
	A0."CardName",
	A0."DocDate",
	A0."DocDueDate",
	A0."TaxDate",
	A0."SlpCode",
	A0."SlpName",
	A0."Comments",
	A0."U_CodeCRM",
	A0."U_B2B",
	A0."U_TipoPedido",
	A0."TrnspName",
	A0."Carrier",
	A0."TotalExpns",
	A0."ShipToCode",
	A0."PayToCode",
	A0."Confirmed",
	A0."GroupNum",
	A0."PymntGroup",
	A0."PeyMethod",
	A0."Descript"
FROM
(
	SELECT
		T0."DocEntry",
		T0."DocNum",
		T0."CardCode",
		T0."CardName",
		T0."DocDate",
		T0."DocDueDate",
		T0."TaxDate",
		T0."SlpCode",
		T1."SlpName",
		T0."Comments",
		T0."U_CodeCRM",
		T0."U_B2B",
		T0."U_TipoPedido",
		COALESCE(T2."TrnspName", '') AS "TrnspName",
		T3."Carrier",
		T0."TotalExpns",
		T0."ShipToCode",
		T0."PayToCode",
		CASE
			WHEN T0."Confirmed" = 'Y' THEN
				'Autorizado'
			WHEN T0."Confirmed" = 'N' THEN
				'Não autorizado'
		END AS "Confirmed",
		T0."GroupNum",
		COALESCE(T4."PymntGroup", '') AS "PymntGroup",
		T0."PeyMethod",
		COALESCE(T5."Descript", '') AS "Descript",
		COALESCE(T0."UpdateDate", T0."CreateDate") AS "Data",
		LEFT(RIGHT('00' + CAST(COALESCE(T0."UpdateTS", T0."CreateTS") AS NVARCHAR), 6), 4) AS "Horario",
		CAST(LEFT(LEFT(RIGHT('00' + CAST(COALESCE(T0."UpdateTS", T0."CreateTS") AS NVARCHAR), 6), 4), 2) AS INT) AS "HH",
		CAST(RIGHT(LEFT(RIGHT('00' + CAST(COALESCE(T0."UpdateTS", T0."CreateTS") AS NVARCHAR), 6), 4), 2) AS INT) AS "MM"
	FROM ORDR T0
		INNER JOIN OSLP T1 ON T1."SlpCode" = T0."SlpCode"
		LEFT JOIN OSHP T2 ON T0."TrnspCode" = T2."TrnspCode"
		INNER JOIN RDR12 T3 ON T3."DocEntry" = T0."DocEntry"
		LEFT JOIN OCTG T4 ON T0."GroupNum" = T4."GroupNum"
		LEFT JOIN OPYM T5 ON T0."PeyMethod" = T5."PayMethCod"
) A0
WHERE
	DATEADD(MINUTE, A0.MM, DATEADD(HOUR, A0.HH, A0."Data")) > '{0}'
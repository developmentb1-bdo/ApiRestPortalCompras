SELECT 
	A0."DocEntry",
	A0."DocNum",
	A0."CardCode", 
	A0."DocDate",
	A0."DocDueDate",
	A0."TaxDate"
FROM
(
	SELECT
		T0."DocEntry",
		T0."DocNum",
		T0."CardCode", 
		T0."DocDate",
		T0."DocDueDate",
		T0."TaxDate",
		COALESCE(T0."UpdateDate", T0."CreateDate") AS "Data",
		LEFT(RIGHT('00' + CAST(COALESCE(T0."UpdateTS", T0."CreateTS") AS NVARCHAR), 6), 4) AS "Horario",
		CAST(LEFT(LEFT(RIGHT('00' + CAST(COALESCE(T0."UpdateTS", T0."CreateTS") AS NVARCHAR), 6), 4), 2) AS INT) AS "HH",
		CAST(RIGHT(LEFT(RIGHT('00' + CAST(COALESCE(T0."UpdateTS", T0."CreateTS") AS NVARCHAR), 6), 4), 2) AS INT) AS "MM"
	FROM ORCT T0
) A0
WHERE
	DATEADD(MINUTE, A0.MM, DATEADD(HOUR, A0.HH, A0."Data")) > '{0}'
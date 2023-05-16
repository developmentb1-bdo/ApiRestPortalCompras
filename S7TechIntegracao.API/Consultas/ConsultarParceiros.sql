SET DATEFORMAT YMD

SELECT
	A0."CardCode",
	A0."CardName",
	A0."Series",
	A0."CardType",
	A0."CardFName",
	A0."GroupCode",
	A0."Currency",
	A0."Phone1",
	A0."Phone2",
	A0."Cellular",
	A0."E_Mail",
	A0."CmpPrivate",
	A0."Notes",
	A0."Territory",
	A0."GroupNum",
	A0."PymCode",
	DATEADD(MINUTE, A0.MM, DATEADD(HOUR, A0.HH, A0."Data")) AS "Data"
FROM
(
	SELECT
		T0."CardCode",
		T0."CardName",
		T0."Series",
		T0."CardType",
		T0."CardFName",
		COALESCE(T0."GroupCode", -1) AS "GroupCode",
		T0."Currency",
		COALESCE(T0."Phone1", '') AS "Phone1",
		COALESCE(T0."Phone2", '') AS "Phone2",
		COALESCE(T0."Cellular", '') AS "Cellular",
		T0."E_Mail",
		T0."CmpPrivate",
		T0."Notes",
		T0."Territory",
		T0."GroupNum",
		T0."PymCode",
		COALESCE(T0."UpdateDate", T0."CreateDate") AS "Data",
		LEFT(RIGHT('00' + CAST(COALESCE(T0."UpdateTS", T0."CreateTS") AS NVARCHAR), 6), 4) AS "Horario",
		CAST(LEFT(LEFT(RIGHT('00' + CAST(COALESCE(T0."UpdateTS", T0."CreateTS") AS NVARCHAR), 6), 4), 2) AS INT) AS "HH",
		CAST(RIGHT(LEFT(RIGHT('00' + CAST(COALESCE(T0."UpdateTS", T0."CreateTS") AS NVARCHAR), 6), 4), 2) AS INT) AS "MM"
	FROM OCRD T0
) AS A0
WHERE
	DATEADD(MINUTE, A0.MM, DATEADD(HOUR, A0.HH, A0."Data")) > '{0}'

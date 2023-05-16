SET DATEFORMAT YMD

SELECT
	A0.ItemCode,
	A0.ItemName,
	A0."SWW",
	A0."FrgnName",
	A0."ItmsGrpNam",
	A0."FirmName",
	A0."NcmCode",
	A0."SalUnitMsr",
	A0."NumInSale",
	A0."SalPackMsr",
	A0."SalPackUn",
	A0."Sheight1",
	A0."SWidth1",
	A0."SLength1",
	A0."SVolume",
	A0."SWeight1",
	A0."UgpEntry",
	A0."UgpName",
	A0."CodeBars",
	A0."U_LG_SETOR",
	A0."U_LG_SUBGProd",
	A0."U_LG_FamProd",
	A0."U_LG_EspProd",
	A0."SUomCode",
	DATEADD(MINUTE, A0.MM, DATEADD(HOUR, A0.HH, A0."Data")) AS "Data"
FROM
(
	SELECT
		T0."ItemCode",
		T0."ItemName",
		T0."SWW",
		T0."FrgnName",
		T1."ItmsGrpNam",
		COALESCE(T2."FirmName", '') AS "FirmName",
		T3."NcmCode",
		T0."SalUnitMsr",
		T0."NumInSale",
		T0."SalPackMsr",
		T0."SalPackUn",
		T0."Sheight1",
		T0."SWidth1",
		T0."SLength1",
		T0."SVolume",
		T0."SWeight1",
		T0."UgpEntry",
		T4."UgpName",
		T0."CodeBars",
		T0."U_LG_SETOR",
		T0."U_LG_SUBGProd",
		T0."U_LG_FamProd",
		T0."U_LG_EspProd",
		T5."UomCode" AS "SUomCode",
		COALESCE(T0."UpdateDate", T0."CreateDate") AS "Data",
		LEFT(RIGHT('00' + CAST(COALESCE(T0."UpdateTS", T0."CreateTS") AS NVARCHAR), 6), 4) AS "Horario",
		CAST(LEFT(LEFT(RIGHT('00' + CAST(COALESCE(T0."UpdateTS", T0."CreateTS") AS NVARCHAR), 6), 4), 2) AS INT) AS "HH",
		CAST(RIGHT(LEFT(RIGHT('00' + CAST(COALESCE(T0."UpdateTS", T0."CreateTS") AS NVARCHAR), 6), 4), 2) AS INT) AS "MM"
	FROM OITM T0
		INNER JOIN OITB T1 ON T1."ItmsGrpCod" = T0."ItmsGrpCod"
		LEFT JOIN OMRC T2 ON T0."FirmCode" = T2."FirmCode"
		INNER JOIN ONCM T3 ON T0."NCMCode" = T3."AbsEntry"
		LEFT JOIN OUGP T4 ON T0."UgpEntry" = T4."UgpEntry"
		LEFT JOIN OUOM T5 ON T0."SUoMEntry" = T5."UomEntry"
) AS A0
WHERE
	DATEADD(MINUTE, A0.MM, DATEADD(HOUR, A0.HH, A0."Data")) > '{0}'
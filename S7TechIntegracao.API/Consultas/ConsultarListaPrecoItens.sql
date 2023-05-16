SET DATEFORMAT YMD

SELECT
	T0."ListNum",
	T0."ListName",
	COALESCE(T1."Price", 0) AS "Price",
	T1."ItemCode"
FROM OPLN T0
	INNER JOIN ITM1 T1 ON T1."PriceList" = T0."ListNum"
	INNER JOIN OITM T2 ON T2."ItemCode" = T1."ItemCode"
WHERE
	COALESCE(T0."U_ECommerce", '') = 'Y'
	AND COALESCE(T2."U_ECommerce", '') = 'Y'
	AND CONVERT(DATETIME, COALESCE(T0."U_SD_DataUpdate", CONVERT(NVARCHAR, GETDATE(), 112))) >= '{0}'
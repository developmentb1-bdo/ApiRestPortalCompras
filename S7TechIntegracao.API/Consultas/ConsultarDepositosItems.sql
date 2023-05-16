SET DATEFORMAT YMD

SELECT
	T0."ItemCode",
	T0."WhsCode",
	T0."OnHand", 
	T0."IsCommited",
	T0."OnOrder",
	T0."OnHand" - T0."IsCommited" AS "Saldo"
FROM OITW T0	
	INNER JOIN OITM T1 ON T1."ItemCode" = T0."ItemCode"
	INNER JOIN OWHS T2 ON T2."WhsCode" = T0."WhsCode"
WHERE
	COALESCE(T1."U_ECommerce", '') = 'Y'
	AND COALESCE(T2."U_ECommerce", '') = 'Y'
	AND CAST(COALESCE(T0."U_SD_DataUpdate", GETDATE()) AS DATETIME) >= '{0}'
ORDER BY T0."ItemCode", T0."WhsCode"
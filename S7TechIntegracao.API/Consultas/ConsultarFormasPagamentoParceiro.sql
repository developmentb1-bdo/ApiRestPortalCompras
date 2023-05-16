SET DATEFORMAT YMD

SELECT
	T0."CardCode",
	T0."LineNum",
	T0."PymCode"
FROM CRD2 T0
WHERE
	T0."CardCode" = '{0}'

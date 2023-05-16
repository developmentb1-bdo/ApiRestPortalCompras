SET DATEFORMAT YMD

SELECT
	T0."Address",
	T0."AddrType",
	T0."Street",
	T0."StreetNo",
	COALESCE(T0."Building", '') AS "Building",
	COALESCE(T0."ZipCode", '') AS "ZipCode",
	T0."Block",
	T0."City",
	T0."State",
	T0."Country"
FROM CRD1 T0
WHERE
	T0."CardCode" = '{0}'

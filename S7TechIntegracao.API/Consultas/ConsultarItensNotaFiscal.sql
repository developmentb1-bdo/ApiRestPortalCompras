SELECT
	T0."ItemCode",
	T0."Quantity",
	T0."LineTotal",
	CASE
		WHEN T0."LineStatus" = 'O' THEN
			'Aberta'
		WHEN T0."LineStatus" = 'C' THEN
			'Faturada'
	END AS "LineStatus",
	T0."TargetType"
FROM INV1 T0
WHERE
	T0."DocEntry" = {0}

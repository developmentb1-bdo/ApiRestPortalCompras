SET DATEFORMAT YMD

SELECT
	T0.ItemCode,
	T0.Dscription AS "ItemName",
	T0.Quantity,
	T0.Price,
	T0.DelivrdQty,
	T0.OpenCreQty,
	CASE 
		WHEN T0.LineStatus = 'O' THEN
			'Aberto'
		ELSE
			'Fechado'
	END AS "LineStatus",
	CASE 
		WHEN T0.TargetType = 15 THEN
			'Entrega'
		WHEN T0.TargetType = 13 THEN
			'Nota fiscal de saída'
		ELSE
			'-1'
	END AS "TargetType",
	COALESCE(T0."TrgetEntry", -1) AS "TrgetEntry",
	T0."DiscPrcnt",
	T0."WhsCode",
	T0."ShipDate"
FROM RDR1 T0
WHERE
	T0."DocEntry" = {0}
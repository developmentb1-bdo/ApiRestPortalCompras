SELECT
	T0."DocEntry",
	T0."DocNum",
	T0."InvType", 
	T0."SumApplied"
FROM RCT2 T0
WHERE
	T0."DocNum" = {0}
DO BEGIN

DECLARE varContadorAprovado INTEGER;
DECLARE varContadorReprovado INTEGER;

SELECT COUNT('A') INTO varContadorAprovado FROM "@S7T_WDD1" T0 INNER JOIN "@S7T_OWDD" T1 ON T0."Code" = T1."Code" WHERE T1."U_DraftEntry" = {0} AND (T1."U_ObjType" = '22' OR T1."U_ObjType" = '1470000113') AND T0."U_Status" = 'Y';
SELECT COUNT('A') INTO varContadorReprovado FROM "@S7T_WDD1" T0 INNER JOIN "@S7T_OWDD" T1 ON T0."Code" = T1."Code" WHERE T1."U_DraftEntry" = {0} AND (T1."U_ObjType" = '22' OR T1."U_ObjType" = '1470000113') AND T0."U_Status" = 'N';

UPDATE T0
	SET T0."U_Status" = (
							CASE 
								WHEN T0."U_MaxReqr" = varContadorAprovado THEN 
									'Y'
								WHEN T0."U_MaxRejReqr" = varContadorReprovado THEN 
									'N'
								ELSE
									'W'
							END
						)
FROM "@S7T_OWDD" T0 
	INNER JOIN "@S7T_WDD1" T1 ON T0."Code" = T1."Code"
WHERE
	T0."U_DraftEntry" = {0}
	AND (T0."U_ObjType" = '22' 	OR T0."U_ObjType" = '1470000113');

END
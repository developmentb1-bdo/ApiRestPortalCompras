SELECT 
	T0."ID",
	T0."Usage",
	T0."U_S7T_ItemPortal",
	concat(T0."ID",concat(' - ', T0."Usage")) As Usage
FROM OUSG T0
WHERE
	T0."Locked" = 'N'
    AND T0."U_S7T_ItemPortal" = 'Y'



--SELECT
--	T0."ID",
--	T0."Usage"
--FROM OUSG T0
--WHERE
--	T0."Locked" = 'N'
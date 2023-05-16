SELECT 
	T0."GroupNum" AS "GroupNumber", 
	T0."PymntGroup" AS "PaymentTermsGroupName"
FROM 
	OCTG T0 
WHERE 
	T0."U_S7T_ItemPortal" = 'Y'
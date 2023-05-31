﻿SELECT
A."DocEntry",
A."DocTime" as "HourCreateTitle",
A."CardCode",
A."DocNum",
C."TaxIdNum" as "FederalTaxId",
A."CANCELED" as "Cancelled",
TO_VARCHAR (TO_DATE(A."CreateDate"), 'YYYYMMDD')|| ':'|| A."DocTime"  as "CreateDate",
D."Id" as "IdFilialIntBank"
FROM
OINV A
INNER JOIN INV6 B ON B."DocEntry" = A."DocEntry"
INNER JOIN OBPL C ON C."BPLId" = A."BPLId"
INNER JOIN "IntegrationBank"."IV_IB_CompanyLocal" D ON D."Id" = A."BPLId" and "CompanyDb" = '{3}'
WHERE
B."TotalBlck" <> B."InsTotal"
and A."CANCELED" <> 'N'
{0}
{1}
{2}
group by   A."DocEntry", A."CreateDate", A."DocTime", A."CardCode", A."DocNum",C."TaxIdNum",A."CANCELED", D."Id"
order by A."CreateDate", A."DocTime" asc
LIMIT {4} offset {5};

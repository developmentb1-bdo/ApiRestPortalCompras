select "ID" as "codigo","InstallmentID" as "parcela" from "IntegrationBank"."IV_IB_BillOfExchange" A
inner join "OINV" B on B."DocEntry" = A."DocEntry"
where A."DocEntry" = {0}

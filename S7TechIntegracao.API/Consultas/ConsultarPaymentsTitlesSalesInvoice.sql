SELECT 
	 T0."DocNum",
	 T1."DocEntry" ,
	 T2."DocEntry" as nf,
	 T2."DocNum",
	 T0."TrsfrDate" as DataTransferencia,
	 T0."Canceled",
	 T0."DocDate" as DataPagamento,  
	 Replace(T1."SumApplied",',','.') as "SumApplied",
	 T1."InstId" as "InstallmentId",
	 T3."InsTotal",
	 T3."PaidToDate",
	 case when (T0."TrsfrAcct" is null) then
	 T0."CashAcct"
	 else T0."TrsfrAcct"
	 end as "ContaDoRazao",
	T4."AcctName" as NomeDaConta,
	 case when (T0."TrsfrAcct" is null and T5."ContaRenegociacao" = T0."CashAcct") then	 
	 'Y'
	 when (T0."CashAcct" is null and T5."ContaRenegociacao" = T0."TrsfrAcct") then
	 'Y'
	 else 'N'
	 end as "BaixaRenegociacao",
	 T5."ContaRenegociacao"	 
FROM ORCT T0  
left JOIN RCT2 T1 ON T0."DocEntry" = T1."DocNum"
left join OINV T2 on T1."DocEntry" = T2."DocEntry" 
left join INV6 T3 on T3."DocEntry" = T2."DocEntry" and T1."InstId" = T3."InstlmntID"
left join OACT T4 on T4."AcctCode" = T0."TrsfrAcct" or T4."AcctCode" = T0."CashAcct"
left JOIN "IntegrationBank"."IV_IB_CompanyLocal" T6 ON T6."BPLId" = T0."BPLId" and T6."CompanyDb" = '{1}'
left JOIN "IntegrationBank"."IV_IB_RenegociacaoParametro" T5 ON T5."EmpresaId" = T6."Id"
where  T1."InvType"  = 13
AND T2."DocEntry" = '{0}'




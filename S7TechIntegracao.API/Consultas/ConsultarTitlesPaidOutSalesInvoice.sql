﻿select 
case when ((T3."PaidToDate" - T3."InsTotal") = 0 ) then
	 'Y'
	 else
	  'N'	 
	 end as "Baixado"
	 from  INV6 T3
	 where  T3."DocEntry" = {0} 
	 and T3."InstlmntID" = {1}


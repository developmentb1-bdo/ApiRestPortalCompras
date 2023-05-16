<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" exclude-result-prefixes="xsi" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

    <xsl:output omit-xml-declaration="yes"/>
  
	<xsl:decimal-format name="real" decimal-separator="," grouping-separator="."/>
	<xsl:param name="RequesterName"></xsl:param>
	<xsl:param name="RequesterEmail"></xsl:param>
	<xsl:param name="PaymentTermsGroupName"></xsl:param>
	<xsl:param name="ApprovalLink"></xsl:param>
	<xsl:param name="RepprovalLink"></xsl:param>
	
    <xsl:template match="/">
		<html>		
			<style>
        #approval {
        background-color: #ff4e00;
        border: 1px solid #ff4e00;
        color: white;
        padding: 10px 20px;
        }

        #repproval {
        background-color: white;
        border: 1px solid #ff4e00;
        color: #ff4e00;
        padding: 10px 20px;
        margin:0 10px;
        }
        #customers {
        border-collapse: collapse;
        width: 100%;
        }

        #customers td, #customers th {
        border: 1px solid #ddd;
        padding: 8px;
        }

        #customers tr:nth-child(even){background-color: #f2f2f2;}

        #customers tr:hover {background-color: #ddd;}

        #customers th {
        padding-top: 12px;
        padding-bottom: 12px;
        text-align: left;
        background-color: #ff4e00;
        color: white;
        }

        img {
        border: 1px solid #ddd;
        border-radius: 4px;
        padding: 2px;
        width: 150px;
        }
      </style>
			
			<body>
				<form id="form1" method="post" action="#">

          <!--<div class="col-10" >
            <img src="logo.png" alt="logo" style="width:100px" />
					</div>-->

					<span style="padding:1%">
						<strong>Pedido de compras</strong>
					</span>
					
					<xsl:call-template name="header-block"/>
						
					<xsl:call-template name="items-block"/>
										
				    <xsl:call-template name="buttons-block"/>

				</form>
			</body>
		
		</html>
		
    </xsl:template>
	
	<xsl:template name="formatDate">
		<xsl:param name="dateTime" />
		<xsl:variable name="date" select="substring-before($dateTime, 'T')" />
		<xsl:variable name="year" select="substring-before($date, '-')" />
		<xsl:variable name="month" select="substring-before(substring-after($date, '-'), '-')" />
		<xsl:variable name="day" select="substring-after(substring-after($date, '-'), '-')" />
		<xsl:value-of select="concat($day, '/', $month, '/', $year)" />
	</xsl:template>
	
	<xsl:template name="formatTime">
		<xsl:param name="dateTime" />
		<xsl:value-of select="substring-after($dateTime, 'T')" />
	</xsl:template>
		<xsl:template name="formatNumberReal">
		<xsl:param name="number" />
		<xsl:choose>
		  <xsl:when test="$number = 0">
			0,00
		  </xsl:when>
		  <xsl:otherwise>
			<xsl:value-of select="format-number($number, '#.###,00', 'real')"/>
		  </xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="header-block">
	
		<div class="form-group" style="padding:1%">
			<div class="form-row col-12">
				<div class="col-10" >

						<div class="form-group col-lg-12">
							<label>Documento: </label>
							<xsl:value-of select="Documents/DocNum"/>
						</div>

						<div class="form-group col-lg-2">
							<label>Solicitante: </label>
							<xsl:value-of select="$RequesterName" />
						</div>
					
						<div class="form-group col-lg-3">
							<label>Email: </label>
							<a>
								<xsl:attribute name="href">
									<xsl:value-of select="concat('mailto:', $RequesterEmail)" />
							    </xsl:attribute>
								<xsl:value-of select="$RequesterEmail"/>
							</a>
						</div>

						<div class="form-group col-lg-3">
							<label>Data solicitação: </label>
							<xsl:call-template name="formatDate">
								<xsl:with-param name="dateTime" select="Documents/CreationDate" />
							</xsl:call-template>
						</div>

						<div class="form-group col-lg-6" >
							<label>Parceiro: </label>
							<xsl:value-of select="concat(Documents/CardCode, concat(' - ', Documents/CardName))" />
						</div>
						
						<div class="form-group col-lg-2" >
							<label>Data de lançamento: </label>
							<xsl:call-template name="formatDate">
								<xsl:with-param name="dateTime" select="Documents/DocDate" />
							</xsl:call-template>
						</div>
						
						<div class="form-group col-lg-2">
							<label>Válido até: </label>
							<xsl:call-template name="formatDate">
								<xsl:with-param name="dateTime" select="Documents/DocDueDate" />
							</xsl:call-template>
						</div>
						
						<div class="form-group col-lg-4" >
							<label>Filial: </label>
							<xsl:value-of select="concat(Documents/BPL_IDAssignedToInvoice, concat(' - ', Documents/BPLName))" />
						</div>

						<div class="form -group col-lg-12">
							<label>Condição de Pagamento: </label>
							<xsl:value-of select="$PaymentTermsGroupName"/>
						</div>

						<div class="form -group col-lg-12">
							<label>Moeda: </label>
							<xsl:value-of select="Documents/DocCurrency"/>
						</div>

						<div class="form -group col-lg-12">
							<label>Total moeda estrangeira: </label>
							<xsl:call-template name="formatNumberReal">
								<xsl:with-param name="number" select="Documents/DocTotalFc" />
							</xsl:call-template>
						</div>

						<div class="form -group col-lg-12">
							<h3>
								<strong>Total R$: </strong>
								<strong>
									<xsl:call-template name="formatNumberReal">
										<xsl:with-param name="number" select="Documents/DocTotal" />
									</xsl:call-template>
								</strong>
							</h3>
						</div>
						
						<div class="form-row" >
							<label>Observação: </label>
						</div>
						<div class="form-row" >
							<div class="form-group col-12" >
								<xsl:value-of select="Documents/Comments"/>
							</div>
						</div>	
					
				</div>
			</div>
		</div>
	
	</xsl:template>
	
		<xsl:template name="items-block">
		
			<div class="form-group" style="padding:1%">
				<div class="form-row" >
					<table id="customers" style="white-space:nowrap; width:90%">
						<thead>
							<tr>
								<th>Item</th>
								<th>Descrição</th>
								<th>Dt Entrega</th>
								<th>Quant</th>
								<th>Vl Unitário</th>
								<th>Desc%</th>
								<th>Vl Desc.</th>
								<th>Vl Total R$</th>
								<th>Vl Total ME</th>
								<th>Projeto</th>
								<th>Centro custo</th>
								<th>Detalhes</th>
							</tr>
						</thead>
						<tbody>
							<xsl:for-each select="Documents/DocumentLines/DocumentLines">
								<tr>
									<td>
										<xsl:value-of select="ItemCode"/>
									</td>
									<td>
										<xsl:value-of select="ItemDescription"/>
									</td>
									<td>
										<xsl:call-template name="formatDate">
											<xsl:with-param name="dateTime" select="ShipDate" />
										</xsl:call-template>
									</td>
									<td>
										<xsl:value-of select="Quantity"/>
									</td>
									<td>
										<xsl:call-template name="formatNumberReal">
											<xsl:with-param name="number" select="UnitPrice" />
										</xsl:call-template>
									</td>
									<td>
										<xsl:value-of select="DiscountPercent"/>
									</td>
									<td>
										<xsl:call-template name="formatNumberReal">
											<xsl:with-param name="number" select="Price" />
										</xsl:call-template>
									</td>
									<td>
										<xsl:call-template name="formatNumberReal">
											<xsl:with-param name="number" select="LineTotal" />
										</xsl:call-template>
									</td>
									<td>
										<xsl:call-template name="formatNumberReal">
											<xsl:with-param name="number" select="RowTotalFC" />
										</xsl:call-template>
									</td>
									<td>
										<xsl:value-of select="ProjectCode"/>
									</td>
									<td>
										<xsl:value-of select="CostingCode2"/>
									</td>
									<td>
										<xsl:value-of select="FreeText"/>
									</td>
								</tr>
							</xsl:for-each>
						</tbody>
					</table>
				</div>	
			</div>

	</xsl:template>
	
	<xsl:template name="buttons-block">
 		<div style="padding: 1%" >
			<div class="form-group">
				<a id="approval" href="$ApprovalLink" target="_self" >Aprovar</a>
				<a id="repproval" href="$RepprovalLink" target="_self" >Reprovar</a>
			</div>
		</div> 
	</xsl:template>

</xsl:stylesheet>
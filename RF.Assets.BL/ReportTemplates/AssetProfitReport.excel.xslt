<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:helper="http://www.regionfund.ru/fol/ReportXsltHelper/"
	xmlns:o="urn:schemas-microsoft-com:office:office"
	xmlns:x="urn:schemas-microsoft-com:office:excel"
	xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet"
	xmlns="urn:schemas-microsoft-com:office:spreadsheet"
	xmlns:ms="urn:schemas-microsoft-com:xslt"
>

	<xsl:variable name="DateFormat" select="'dd.MM.yyyy'" />
	<xsl:variable name="NumberFormat" select="'0.###\%'" />
	<xsl:output version="1.0" encoding="utf-8" standalone="yes" method="xml" omit-xml-declaration="no" />
	<xsl:decimal-format grouping-separator=" " decimal-separator="," name="rus-money"/>

	<xsl:template match="/Report">
		<xsl:processing-instruction name="mso-application">
			<xsl:text>progid="Excel.Sheet"</xsl:text>
		</xsl:processing-instruction>

		<Workbook xmlns="urn:schemas-microsoft-com:office:spreadsheet"
			 xmlns:o="urn:schemas-microsoft-com:office:office"
			 xmlns:x="urn:schemas-microsoft-com:office:excel"
			 xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet"
			 xmlns:html="http://www.w3.org/TR/REC-html40">
			<DocumentProperties xmlns="urn:schemas-microsoft-com:office:office">
				<Version>11.6360</Version>
			</DocumentProperties>
			<ExcelWorkbook xmlns="urn:schemas-microsoft-com:office:excel">
				<WindowHeight>11400</WindowHeight>
				<WindowWidth>15180</WindowWidth>
				<WindowTopX>570</WindowTopX>
				<WindowTopY>-120</WindowTopY>
				<ProtectStructure>False</ProtectStructure>
				<ProtectWindows>False</ProtectWindows>
			</ExcelWorkbook>
			<Styles>
				<Style ss:ID="Default" ss:Name="Normal">
					<Alignment ss:Vertical="Bottom"/>
					<Borders/>
					<Font x:CharSet="204"/>
					<Interior/>
					<NumberFormat/>
					<Protection/>
				</Style>
				<Style ss:ID="s16" ss:Name="Финансовый">
					<NumberFormat
					 ss:Format="_-* #,##0.00_р_._-;\-* #,##0.00_р_._-;_-* &quot;-&quot;??_р_._-;_-@_-"/>
				</Style>
				<Style ss:ID="s21">
					<NumberFormat ss:Format="dd/mm/yyyy;@"/>
				</Style>
				<Style ss:ID="s22">
					<Font x:CharSet="204"/>
					<NumberFormat ss:Format="Standard"/>
				</Style>
				<Style ss:ID="s23">
					<Font x:CharSet="204"/>
				</Style>
				<Style ss:ID="s24">
					<Alignment ss:Horizontal="Center" ss:Vertical="Center"/>
					<Font x:CharSet="204" ss:Bold="1"/>
				</Style>
				<Style ss:ID="s25">
					<Alignment ss:Horizontal="CenterAcrossSelection" ss:Vertical="Top"
					 ss:WrapText="1"/>
					<Font x:CharSet="204" x:Family="Swiss" ss:Bold="1" ss:Italic="1"/>
				</Style>
				<Style ss:ID="s26">
					<Alignment ss:Horizontal="CenterAcrossSelection" ss:Vertical="Top"
					 ss:WrapText="1"/>
				</Style>
				<Style ss:ID="s28">
					<Alignment ss:Horizontal="Left" ss:Vertical="Top"/>
					<Font x:CharSet="204"/>
				</Style>
				<Style ss:ID="s30">
					<Alignment ss:Horizontal="Center" ss:Vertical="Center" ss:WrapText="1"/>
					<Borders>
						<Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
						<Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
						<Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
						<Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
					</Borders>
					<Font x:CharSet="204" ss:Bold="1"/>
				</Style>

				<Style ss:ID="s400" ss:Name="YearTotal">
					<Alignment ss:Horizontal="Left" ss:Vertical="Center"/>
					<Borders>
						<Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
						<Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
						<Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
					</Borders>
					<Font x:CharSet="204" ss:Color="#0000FF"/>
				</Style>
				<Style ss:ID="s401" ss:Parent="s400">
					<Alignment ss:Horizontal="Right" ss:Vertical="Center"/>
					<NumberFormat ss:Format="General"/>
				</Style>
				<Style ss:ID="s4015" ss:Parent="s400">
					<Alignment ss:Horizontal="Right" ss:Vertical="Center"/>
					<NumberFormat ss:Format="0.0##\%"/>
				</Style>
				<Style ss:ID="s402" ss:Parent="s400">
					<Alignment ss:Horizontal="Center" ss:Vertical="Center"/>
					<Font ss:Color="#0000FF" ss:Bold="1"/>
				</Style>

				<Style ss:ID="s310" ss:Name="Белая дата">
					<Alignment ss:Horizontal="Center" ss:Vertical="Center"/>
					<Borders>
						<Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
						<Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
						<Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
					</Borders>
					<NumberFormat ss:Format="Short Date"/>
				</Style>
				<Style ss:ID="s320" ss:Name="Бел. раб. дней">
					<Alignment ss:Horizontal="Center" ss:Vertical="Center"/>
					<Borders>
						<Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
						<Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
					</Borders>
					<Font x:CharSet="204" ss:Color="#0000FF"/>
				</Style>
				<Style ss:ID="s330" ss:Name="Бел. СЧА, ДДС">
					<Alignment ss:Horizontal="Right" ss:Vertical="Center"/>
					<Borders>
						<Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
						<Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
					</Borders>
					<Font ss:Color="#0000FF" />
					<NumberFormat ss:Format="Standard"/>
				</Style>
				<Style ss:ID="s340" ss:Parent="s330">
					<Font ss:Color="#0000FF" ss:Bold="1"/>
					<NumberFormat ss:Format="##0.0000"/>
				</Style>


				<Style ss:ID="s311" ss:Parent="s310" >
					<Font x:CharSet="204" ss:Color="#0000FF" ss:Bold="1"/>
					<Interior ss:Color="#FFFF00" ss:Pattern="Solid"/>
				</Style>
				<Style ss:ID="s321" ss:Parent="s320">
					<Interior ss:Color="#FFFF00" ss:Pattern="Solid"/>
				</Style>
				<Style ss:ID="s331" ss:Parent="s330">
					<Font ss:Color="#0000FF" ss:Bold="1"/>
					<Interior ss:Color="#FFFF00" ss:Pattern="Solid"/>
				</Style>
				<Style ss:ID="s341" ss:Parent="s331">
					<Font ss:Color="#000000" ss:Bold="1"/>
					<NumberFormat ss:Format="##0.0000"/>
				</Style>

				<Style ss:ID="s332" ss:Parent="s330">
					<Font ss:Color="#FF0000"/>
				</Style>

				<Style ss:ID="s333" ss:Parent="s331">
					<Font ss:Color="#FF0000" ss:Bold="1"/>
				</Style>

				<Style ss:ID="s82">
					<Alignment ss:Vertical="Top"/>
					<Font x:CharSet="204" x:Family="Swiss" ss:Italic="1"/>
				</Style>
				<Style ss:ID="s83">
					<Alignment ss:Vertical="Top"/>
					<Font x:CharSet="204" x:Family="Swiss" ss:Italic="1"/>
					<NumberFormat ss:Format="dd/mm/yy;@"/>
				</Style>
				<Style ss:ID="s84">
					<Alignment ss:Horizontal="Right" ss:Vertical="Top"/>
				</Style>

			</Styles>

			<xsl:call-template name="CreateWorksheet" />

		</Workbook>
	</xsl:template>

	<xsl:template name="CreateWorksheet" xml:space="preserve" >
		<Worksheet><xsl:attribute name="ss:Name"><xsl:value-of select="@Caption"/></xsl:attribute>
			
			<Table ss:ExpandedColumnCount="256" x:FullColumns="1" x:FullRows="1" ss:StyleID="s22">
				<Column ss:StyleID="s22" ss:AutoFitWidth="0" ss:Width="121.25"/>
				<Column ss:StyleID="s22" ss:AutoFitWidth="0" ss:Width="57.75"/>
				<Column ss:StyleID="s22" ss:AutoFitWidth="0" ss:Width="102"/>
				<Column ss:StyleID="s22" ss:AutoFitWidth="0" ss:Width="87.75"/>
				<Column ss:StyleID="s22" ss:AutoFitWidth="0" ss:Width="90.25"/>
				<Row ss:AutoFitHeight="0" ss:Height="2.5" /> 
				<Row ss:AutoFitHeight="0" ss:Height="2.5" /> 
				<Row ss:AutoFitHeight="0" ss:Height="2.5" /> 
				<Row ss:AutoFitHeight="0" ss:Height="2.5" /> 
				<Row ss:AutoFitHeight="0" ss:Height="2.5" /> 
				<Row ss:AutoFitHeight="0" ss:Height="2.5" /> 
				<Row ss:AutoFitHeight="0" ss:Height="2.5" /> 
				<Row ss:Height="25.5">
					<Cell ss:StyleID="s25">
					  <Data ss:Type="String">Расчет доходности по пенсионным <xsl:choose><xsl:when test="ReportView/@InsuranceType='NPO'">резервам</xsl:when><xsl:otherwise>накоплениям</xsl:otherwise></xsl:choose> НПФ &quot;РЕГИОНФОНД&quot; согласно данным <xsl:choose><xsl:when test="ReportView/@InsuranceType='NPO'">бухгалтерии</xsl:when><xsl:otherwise>спецдепозитария ЗАО &quot;ИК &quot;ПЭКО-ИНВЕСТ&quot;</xsl:otherwise></xsl:choose></Data></Cell>
					<Cell ss:StyleID="s26"/>
					<Cell ss:StyleID="s26"/>
					<Cell ss:StyleID="s26"/>
					<Cell ss:StyleID="s26"/>
				</Row>          
				<Row ss:Index="11">
					<Cell ss:StyleID="s28"><Data ss:Type="String"><xsl:value-of select="ReportView/@DateBegin"/> - <xsl:value-of select="ReportView/@DateEnd"/></Data></Cell>
				</Row>			
				<Row ss:Index="13" ss:Height="38.25">
					<Cell ss:StyleID="s30"><Data ss:Type="String">Дата</Data></Cell>
					<Cell ss:StyleID="s30"><Data ss:Type="String">Рабочие дни</Data></Cell>
					<Cell ss:StyleID="s30"><Data ss:Type="String">СЧА на конец каждого рабочего дня (руб.)</Data></Cell>
					<Cell ss:StyleID="s30"><Data ss:Type="String">Сумма поступления / изъятия за день</Data></Cell>
					<Cell ss:StyleID="s30"><Data ss:Type="String">(СЧАi-Si) / CЧА(i-1)</Data></Cell>
				</Row>				
				<xsl:for-each select="ReportView/ReportRow">
					<xsl:sort select="@Date" order="ascending" data-type="text"/>
					<xsl:choose>
						<xsl:when test="ms:format-date(@Date, $DateFormat) = ms:format-date(helper:AddDays(-1, @YearPeriodBegin), $DateFormat) or ms:format-date(@Date, $DateFormat) = ms:format-date(@YearPeriodEnd, $DateFormat)">
							<Row>
								<Cell ss:StyleID="s311"><Data ss:Type="DateTime"><xsl:value-of select="@Date"/></Data></Cell>
								<Cell ss:StyleID="s321"><Data ss:Type="Number"><xsl:value-of select="@WorkDays"/></Data></Cell>
								<Cell ss:StyleID="s331"><Data ss:Type="Number"><xsl:value-of select="@Value"/></Data></Cell>
								<Cell><xsl:attribute name="ss:StyleID"><xsl:choose><xsl:when test="@CashFlow &lt; 0">s333</xsl:when><xsl:otherwise>s331</xsl:otherwise></xsl:choose></xsl:attribute><Data ss:Type="Number"><xsl:value-of select="@CashFlow"/></Data></Cell>
								<Cell ss:StyleID="s341"><Data><xsl:attribute name="ss:Type"><xsl:choose><xsl:when test="@R">Number</xsl:when><xsl:otherwise>String</xsl:otherwise></xsl:choose></xsl:attribute><xsl:value-of select="@R"/></Data></Cell>
							</Row>							
						</xsl:when>
						<xsl:otherwise>
							<Row>
								<Cell ss:StyleID="s310"><Data ss:Type="DateTime"><xsl:value-of select="@Date"/></Data></Cell>
								<Cell ss:StyleID="s320"><Data ss:Type="Number"><xsl:value-of select="@WorkDays"/></Data></Cell>
								<Cell ss:StyleID="s330"><Data ss:Type="Number"><xsl:value-of select="@Value"/></Data></Cell>
								<Cell><xsl:attribute name="ss:StyleID"><xsl:choose><xsl:when test="@CashFlow &lt; 0">s332</xsl:when><xsl:otherwise>s330</xsl:otherwise></xsl:choose></xsl:attribute><Data ss:Type="Number"><xsl:value-of select="@CashFlow"/></Data></Cell>
								<Cell ss:StyleID="s340"><Data><xsl:attribute name="ss:Type"><xsl:choose><xsl:when test="@R">Number</xsl:when><xsl:otherwise>String</xsl:otherwise></xsl:choose></xsl:attribute><xsl:value-of select="@R"/></Data></Cell>
							</Row>							
						</xsl:otherwise>
					</xsl:choose>
				</xsl:for-each>
				<Row/>
				<xsl:for-each select="YearTotal/YearTotalRow">
					<xsl:if test="count(../YearTotalRow) &gt; 1">
						<Row>
							<Cell ss:StyleID="s402" ss:MergeAcross="4"><Data ss:Type="String"><xsl:value-of select="ms:format-date(@YearPeriodBegin, $DateFormat)"/>-<xsl:value-of select="ms:format-date(@YearPeriodEnd, $DateFormat)"/></Data></Cell>
						</Row>
					</xsl:if>	
					<Row>
						<Cell ss:StyleID="s400" ss:MergeAcross="1"><Data ss:Type="String">Произведение (n=<xsl:value-of select="@C"/>)*</Data></Cell>
						<Cell ss:StyleID="s400"><Data ss:Type="String"></Data></Cell>
						<Cell ss:StyleID="s400"><Data ss:Type="String"></Data></Cell>
						<Cell ss:StyleID="s401"><Data ss:Type="Number"><xsl:value-of select="@MultiplyR"/></Data></Cell>
					</Row>
					<Row>
						<Cell ss:StyleID="s400" ss:MergeAcross="1"><Data ss:Type="String">Произведение-1</Data></Cell>
						<Cell ss:StyleID="s400"><Data ss:Type="String"></Data></Cell>
						<Cell ss:StyleID="s400"><Data ss:Type="String"></Data></Cell>
						<Cell ss:StyleID="s401"><Data ss:Type="Number"><xsl:value-of select="@MultiplyRMinusOne"/></Data></Cell>
					</Row>
					<Row>
						<Cell ss:StyleID="s400" ss:MergeAcross="1"><Data ss:Type="String">(Произведение-1) * 365 / <xsl:value-of select="helper:GetTimeSpanInDays(@YearPeriodBegin, @YearPeriodEnd)+1"/></Data></Cell>
						<Cell ss:StyleID="s400"><Data ss:Type="String"></Data></Cell>
						<Cell ss:StyleID="s400"><Data ss:Type="String"></Data></Cell>
						<Cell ss:StyleID="s4015"><Data ss:Type="Number"><xsl:value-of select="@R"/></Data></Cell>
					</Row>		
					<Row/>
				</xsl:for-each>
			<Row>
				<Cell><Data ss:Type="String">* n - количество дней, в которые происходили поступления/изъятия средств пенсионных</Data></Cell>
			</Row>
			<Row>
				<Cell><Data ss:Type="String">резервов в расчетном периоде, плюс один день (дата конца расчетного периода)</Data></Cell>
			</Row>
			<Row />
			<Row />
			<Row>
				<Cell ss:StyleID="s82"><Data ss:Type="String">Итого: доходность по пенсионным <xsl:choose><xsl:when test="ReportView/@InsuranceType='NPO'">резервам</xsl:when><xsl:otherwise>накоплениям</xsl:otherwise></xsl:choose> за <xsl:value-of select="ReportView/@DateBegin"/> - <xsl:value-of select="ReportView/@DateEnd"/> составляет: <xsl:value-of select="helper:FormatNumber(Total/TotalRow[position()=1]/@R, $NumberFormat)"/></Data></Cell>
			</Row>
			<Row />
			<Row />
			<Row>
				<Cell><Data ss:Type="String">Начальник отдела персонифицированного</Data></Cell>
				<Cell ss:Index="4" ss:StyleID="s84"><Data ss:Type="String">Николаева Н.А.</Data></Cell>
			</Row>
			<Row>
			<Cell><Data ss:Type="String">учета и выплат</Data></Cell>
			</Row>				
		</Table>
			
		<WorksheetOptions xmlns="urn:schemas-microsoft-com:office:excel">
				<Selected/>
				<ProtectObjects>False</ProtectObjects>
				<ProtectScenarios>False</ProtectScenarios>
			</WorksheetOptions>
		</Worksheet>
		
	</xsl:template>

</xsl:stylesheet>
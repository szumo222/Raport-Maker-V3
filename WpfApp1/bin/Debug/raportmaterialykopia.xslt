<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:output method="text" />
<xsl:template match="/">
<xsl:for-each select="Show">
<xsl:variable name="audycja_data" select="substring(Time_Start,0,11)"/>
<xsl:variable name="audycja_godzina_start" select="substring(Time_Start,12,5)"/>
<xsl:variable name="audycja_tytul" select="Name"/>
<xsl:for-each select="Track[Number='1000']">
<xsl:for-each select="Element[Class='News']">
<xsl:if test="SendState != 'Skipped'">
<xsl:if test="SendState != 'Cleared'">
	<xsl:if test="Time_Duration &lt; 3600000">
	<xsl:variable name="element_godzina_start" select="substring(Time_Start,12,5)"/>
		<xsl:if test="Time_RealDuration &gt; 1000">
			<xsl:value-of select="$audycja_data"/>;<xsl:value-of select="$audycja_godzina_start"/>;<xsl:value-of select="$element_godzina_start"/>;<xsl:value-of select="$audycja_tytul"/>;<xsl:value-of select="substring(Time_Start,12,8)"/>;<xsl:value-of select="format-number(floor(Time_RealDuration div 3600000),'00')"/>:<xsl:value-of select="format-number(floor(Time_RealDuration div 60000),'00')"/>:<xsl:value-of select="format-number(round(Time_RealDuration div 1000 mod 60),'00')"/>;<xsl:value-of select="translate(Title,';',' ')"/>;<xsl:value-of select="translate(News_Author,';',' ')"/>;
</xsl:if>
		
			<xsl:if test="Time_RealDuration &lt; 0">
				<xsl:variable name="realtime" select= "(Fade_LinkOut - Time_RealDuration)"/>
					<xsl:value-of select="$audycja_data"/>;<xsl:value-of select="$audycja_godzina_start"/>;<xsl:value-of select="$element_godzina_start"/>;<xsl:value-of select="$audycja_tytul"/>;<xsl:value-of select="substring(Time_Start,12,8)"/>;<xsl:value-of select="floor(Time_RealDuration div 3600000)"/>:<xsl:value-of select="floor(Time_RealDuration div 60000)"/>:<xsl:value-of select="round(Time_RealDuration div 1000 mod 60)"/>;<xsl:value-of select="Title"/>;<xsl:value-of select="News_Author"/>;
</xsl:if>
		</xsl:if>
	</xsl:if>
</xsl:if>

</xsl:for-each>
</xsl:for-each>
</xsl:for-each>

</xsl:template>
</xsl:stylesheet>

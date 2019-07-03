<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:output method="text"/>
<xsl:template match="/">
<xsl:for-each select="Show">
<xsl:variable name="audycja_data" select="substring(Time_Start,0,11)"/>
<xsl:variable name="audycja_godzina_start" select="substring(Time_Start,12,5)"/>
<xsl:variable name="audycja_tytul" select="Name"/>
<xsl:for-each select="Track[Number='1000']">
<xsl:for-each select="Element[Class='Music'] | Element[Class='Cart']">
<xsl:if test="PERSONALRADIO = '1'">
<xsl:if test="SendState != 'Skipped'">
<xsl:if test="SendState != 'Cleared'">
	<xsl:if test="Music_Composer!=''">
	<xsl:if test="Music_Composer!='#'">
	<xsl:if test="Music_Performer!=''">
	<xsl:if test="Music_Performer!='#'">
	<xsl:if test="Time_Duration &lt; 3600000">
	<xsl:variable name="element_godzina_start" select="substring(Time_Start,12,5)"/>
		<xsl:if test="Time_RealDuration &gt; 2000">
		<xsl:value-of select="$element_godzina_start"/>|<xsl:value-of select="Music_Performer"/>|<xsl:value-of select="Title"/>|<xsl:value-of select="floor(Time_RealDuration div 3600000)"/>:<xsl:value-of select="floor(Time_RealDuration div 60000)"/>:<xsl:value-of select="round(Time_RealDuration div 1000 mod 60)"/>|L.nad.|<xsl:value-of select="Music_Album"/>|Nr.Kat.|<xsl:value-of select="Music_Publisher"/>|<xsl:value-of select="substring(Music_RecordDate,0,5)"/>|PL|<xsl:value-of select="Music_GemaID"/>|
</xsl:if>
			<xsl:if test="Time_RealDuration &lt;= 2000">
				<xsl:variable name="realtime" select="(Time_RealDuration)"/>
				<xsl:value-of select="$element_godzina_start"/>|<xsl:value-of select="Music_Performer"/>|<xsl:value-of select="Title"/>|<xsl:value-of select="floor($realtime div 3600000)"/>:<xsl:value-of select="floor($realtime div 60000)"/>:<xsl:value-of select="round($realtime div 1000 mod 60)"/>|L.nad.|<xsl:value-of select="Music_Album"/>|Nr.Kat.|<xsl:value-of select="Music_Publisher"/>|<xsl:value-of select="substring(Music_RecordDate,0,5)"/>|PL|<xsl:value-of select="Music_GemaID"/>|
</xsl:if>
	</xsl:if>
	</xsl:if>
	</xsl:if>
	</xsl:if>
	</xsl:if>
	</xsl:if>
</xsl:if>
</xsl:if>
</xsl:for-each>
</xsl:for-each>
</xsl:for-each>
</xsl:template>
</xsl:stylesheet>
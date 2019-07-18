<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:output method="text" />
<xsl:template match="/">
<xsl:for-each select="Show">
<xsl:variable name="audycja_data" select="substring(Time_Start,0,11)"/>
<xsl:variable name="audycja_godzina_start" select="substring(Time_Start,12,5)"/>
<xsl:variable name="audycja_tytul" select="Name"/>
<xsl:for-each select="Track[Number='1000']">
<xsl:for-each select="Element">
<xsl:variable name="part_of_title" select="substring(Title,0,LengthOfTheTitleWrittenByUser)"/>
<xsl:if test="$part_of_title='NameOfTheTitleWrittenByUser'">
	<xsl:variable name="element_godzina_start" select="substring(Time_Start,12,5)"/>
		<xsl:value-of select="$audycja_data"/>|<xsl:value-of select="$audycja_godzina_start"/>|<xsl:value-of select="$element_godzina_start"/>|<xsl:value-of select="$audycja_tytul"/>|<xsl:value-of select="Title"/>|<xsl:value-of select="News_Author"/>|<xsl:value-of select="floor(Fade_MarkOut div 3600000)"/>:<xsl:value-of select="floor(Fade_MarkOut div 60000)"/>:<xsl:value-of select="round(Fade_MarkOut div 1000 mod 60)"/>|<xsl:value-of select="Fade_MarkOut"/>|
</xsl:if>
</xsl:for-each>
</xsl:for-each>
</xsl:for-each>
</xsl:template>
</xsl:stylesheet>
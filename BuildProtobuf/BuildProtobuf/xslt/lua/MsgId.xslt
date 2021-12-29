<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
    <xsl:output method="text" indent="yes"/>

  <xsl:template match="/">-- ***该文件为自动生成的***
    <xsl:apply-templates/>
  </xsl:template>
  
  <xsl:template match="Protobuf">
local package = "<xsl:value-of select="PackageName"/>"
local m = {<xsl:for-each select="Messages/*/Message">
      [<xsl:value-of select="Index+1"/>] = package .. ".<xsl:value-of select="Name"/>"<xsl:if test="position()!=last()">,</xsl:if>
</xsl:for-each>
}

return {
    cs = {
        idToMsg = {<xsl:for-each select="Messages/CS/Message">
            [<xsl:value-of select="Id"/>] = m[<xsl:value-of select="Index +1"/>]<xsl:if test="position()!=last()">,</xsl:if>              
            </xsl:for-each>
        },
        idToName = {<xsl:for-each select="Messages/CS/Message">
            [<xsl:value-of select="Id"/>] = "<xsl:value-of select="Name"/>"<xsl:if test="position()!=last()">,</xsl:if>              
            </xsl:for-each>
        },
        nameToMsg = {<xsl:for-each select="Messages/CS/Message">
            ["<xsl:value-of select="UsedName"/>"] = m[<xsl:value-of select="Index+1"/>]<xsl:if test="position()!=last()">,</xsl:if>
            </xsl:for-each>
        },
        nameToId = {<xsl:for-each select="Messages/CS/Message">
            ["<xsl:value-of select="UsedName"/>"] = <xsl:value-of select="Id"/><xsl:if test="position()!=last()">,</xsl:if>
            </xsl:for-each>
        }
    },
    sc = {
        idToMsg = {<xsl:for-each select="Messages/SC/Message">
            [<xsl:value-of select="Id"/>] = m[<xsl:value-of select="Index+1"/>]<xsl:if test="position()!=last()">,</xsl:if>
            </xsl:for-each>
        },
        idToName = {<xsl:for-each select="Messages/SC/Message">
            [<xsl:value-of select="Id"/>] = "<xsl:value-of select="Name"/>"<xsl:if test="position()!=last()">,</xsl:if>              
            </xsl:for-each>
        },
        nameToMsg = {<xsl:for-each select="Messages/SC/Message">
            ["<xsl:value-of select="UsedName"/>"] = m[<xsl:value-of select="Index+1"/>]<xsl:if test="position()!=last()">,</xsl:if>
            </xsl:for-each>
        },
        nameToId = {<xsl:for-each select="Messages/SC/Message">
            ["<xsl:value-of select="UsedName"/>"] = <xsl:value-of select="Id"/><xsl:if test="position()!=last()">,</xsl:if>
            </xsl:for-each>
        }
    }
}

  </xsl:template>


</xsl:stylesheet>

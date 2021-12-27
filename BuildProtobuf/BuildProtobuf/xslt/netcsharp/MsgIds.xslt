<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
                xmlns:tpl="urn:templates"
>
	<xsl:output method="text" indent="yes"/>
	<xsl:param name="ClassName"/>
	
	<xsl:template match="/">//***该文件为自动生成的***
using System;
using System.Collections.Generic;
<xsl:apply-templates/>
	</xsl:template>
	
	<xsl:template match="Protobuf">
namespace <xsl:value-of select="PackageName"/>
{
    public class <xsl:value-of select="$ClassName"/>
    {
        public class CS
        {
            public readonly static Dictionary&lt;int, Type&gt; IdToType;
            public readonly static Dictionary&lt;Type, int&gt; TypeToId;

            static CS()
            {
                IdToType = new Dictionary&lt;int, Type&gt;();
                TypeToId = new Dictionary&lt;Type, int&gt;();
                <xsl:for-each select="Messages/Message">
                <xsl:if test="ClientToServer='true'">
                Register(<xsl:value-of select="Id"/>, typeof(<xsl:value-of select="TypeName"/>));</xsl:if>
            </xsl:for-each>
           }
		   
		   <xsl:call-template name="ClassMethods"/>
        }

        public class SC
        {
            public readonly static Dictionary&lt;int, Type&gt; IdToType;
            public readonly static Dictionary&lt;Type, int&gt; TypeToId;

            static SC()
            {
                IdToType = new Dictionary&lt;int, Type&gt;();
                TypeToId = new Dictionary&lt;Type, int&gt;();
<xsl:for-each select="Messages/Message">
    <xsl:if test="ClientToServer='false'">
                Register(<xsl:value-of select="Id"/>, typeof(<xsl:value-of select="TypeName"/>));</xsl:if>
</xsl:for-each>
            }
		
		<xsl:call-template name="ClassMethods"/>
        }
    }
}
	</xsl:template>

	<xsl:template match="Message">
		<xsl:apply-templates select="Name" />
		<xsl:if test="position()!=last()">,</xsl:if>
	</xsl:template>
	
	<xsl:template name="ClassMethods">
            public static void Register(int id, Type type)
            {
               IdToType[id] = type;
               TypeToId[type] = id;
            }

            public static bool TryGetType(int id, out Type type)
            {
                return IdToType.TryGetValue(id, out type);
            }

            public static bool TryGetId(Type type, out int id)
            {
                return TypeToId.TryGetValue(type, out id);
            }
	</xsl:template>
</xsl:stylesheet>

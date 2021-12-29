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
        public Dictionary&lt;int, Type&gt; IdToType { get; private set; } = new Dictionary&lt;int, Type&gt;();
        public Dictionary&lt;Type, int&gt; TypeToId { get; private set; } = new Dictionary&lt;Type, int&gt;();
        public Dictionary&lt;string, int&gt; NameToId { get; private set; } = new Dictionary&lt;string, int&gt;();
        public Dictionary&lt;int, string&gt; IdToName { get; private set; } = new Dictionary&lt;int, string&gt;();

        public static MsgIds CS { get; private set; }
        public static MsgIds SC { get; private set; }

        static MsgIds()
        {
            CS = new MsgIds();<xsl:for-each select="Messages/CS/Message">
            CS.Register(<xsl:value-of select="Id"/>, typeof(<xsl:value-of select="TypeName"/>), "<xsl:value-of select="UsedName"/>");</xsl:for-each>

            SC = new MsgIds();<xsl:for-each select="Messages/SC/Message">
            SC.Register(<xsl:value-of select="Id"/>, typeof(<xsl:value-of select="TypeName"/>), "<xsl:value-of select="UsedName"/>");</xsl:for-each>
        }

        public void Register(int id, Type type, string name)
        {
            IdToType[id] = type;
            TypeToId[type] = id;
            NameToId[name] = id;
            IdToName[id] = name;
        }
        
        public bool TryGetId(string name, out int id)
        {
            return NameToId.TryGetValue(name, out id);
        }

        public bool TryGetId(Type type, out int id)
        {
            return TypeToId.TryGetValue(type, out id);
        }
        
        public bool TryGetName(int id, out string name)
        {
            return IdToName.TryGetValue(id, out name);
        }
        
        public bool TryGetType(int id, out Type type)
        {
            return IdToType.TryGetValue(id, out type);
        }

    }
}
	</xsl:template>

</xsl:stylesheet>

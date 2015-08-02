<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
          <xsl:template match="/">
            <Customers>
              <xsl:for-each select="Persons/Person">
                <Customer>
                  <xsl:value-of select="FirstName"/>
                </Customer>
              </xsl:for-each>
            </Customers>
          </xsl:template>
        </xsl:stylesheet>
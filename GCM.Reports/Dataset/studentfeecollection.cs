using System.Xml.Linq;

using System;

namespace GCM.Reports.Dataset
{
}

namespace GCM.Reports.Dataset
{
}
<? xml version = "1.0" encoding = "UTF-8" ?>
< xs : schema xmlns: xs = "http://www.w3.org/2001/XMLSchema" elementFormDefault = "qualified" >

	< xs:element name = "studentfeecollection" >

		< xs:complexType >

			< xs:sequence >

				< xs:element name = "Student" maxOccurs="unbounded">
                    <xs:complexType >

						< xs:sequence >

							< xs:element name = "GovernmentFee" type="xs:decimal" minOccurs="0"/>
                            <xs:element name = "GovAmount" type="xs:decimal" minOccurs="0"/>
                            <xs:element name = "PrivateFee" type="xs:decimal" minOccurs="0"/>
                            <xs:element name = "PrivAmount" type="xs:decimal" minOccurs="0"/>
                            <xs:element name = "GovernmentTotal" type="xs:decimal" minOccurs="0"/>
                            <!-- Dynamic fee heads will be included here based on SubHeadMaster -->
                        </xs:sequence >

						< xs:attribute name = "StudentId" type="xs:integer" use="required"/>
                        <xs:attribute name = "FeeRecieptNo" type="xs:string" use="required"/>
                        <xs:attribute name = "StudentName" type="xs:string" use="required"/>
                        <xs:attribute name = "Gender" type="xs:string" use="required"/>
                        <xs:attribute name = "Semester" type="xs:string" use="required"/>
                    </xs:complexType >

				</ xs:element >

			</ xs:sequence >

		</ xs:complexType >

	</ xs:element >
</ xs:schema >
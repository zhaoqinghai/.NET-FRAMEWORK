
//---------------------------------------------------------------------------
// 
// File: IXamlContentHandler.cs
//
// Copyright (C) Microsoft Corporation.  All rights reserved.
//
// Description: Inteface Xaml content handler.
//
//---------------------------------------------------------------------------

namespace System.Windows.Documents
{
    internal interface IXamlContentHandler
    {
        //------------------------------------------------------
        //
        //  Internal Methods
        //
        //------------------------------------------------------

        #region Internal Methods

        XamlToRtfError StartDocument();

        XamlToRtfError EndDocument();

        XamlToRtfError StartPrefixMapping(string prefix, string uri);

        XamlToRtfError StartElement(string nameSpaceUri, string localName, string qName, IXamlAttributes attributes);

        XamlToRtfError EndElement(string nameSpaceUri, string localName, string qName);

        XamlToRtfError Characters(string characters);

        XamlToRtfError IgnorableWhitespace(string characters);

        XamlToRtfError ProcessingInstruction(string target, string data);

        XamlToRtfError SkippedEntity(string name);

        #endregion Internal Methods
    }
}

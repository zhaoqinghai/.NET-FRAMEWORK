//------------------------------------------------------------------------------
//  Microsoft Avalon
//  Copyright (c) Microsoft Corporation, All Rights Reserved
//
//  File: IconBitmapDecoder.cs
//
//------------------------------------------------------------------------------

using System;
using System.IO;
using System.Collections;
using System.Security;
using System.Security.Permissions;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Reflection;
using Microsoft.Win32.SafeHandles;
using MS.Internal;
using System.Diagnostics;
using System.Windows.Media;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Windows.Media.Imaging
{
    #region IconBitmapDecoder

    /// <summary>
    /// The built-in Microsoft Icon (Bitmap) Decoder.
    /// </summary>
    public sealed class IconBitmapDecoder : BitmapDecoder
    {
        /// <summary>
        /// Don't allow construction of a decoder with no params
        /// </summary>
        private IconBitmapDecoder()
        {
        }

        /// <summary>
        /// Create a IconBitmapDecoder given the Uri
        /// </summary>
        /// <param name="bitmapUri">Uri to decode</param>
        /// <param name="createOptions">Bitmap Create Options</param>
        /// <param name="cacheOption">Bitmap Caching Option</param>
        /// <SecurityNote>
        /// Critical - access critical resource
        /// PublicOK - inputs verified or safe
        /// </SecurityNote>
        [SecurityCritical ]
        public IconBitmapDecoder(
            Uri bitmapUri,
            BitmapCreateOptions createOptions,
            BitmapCacheOption cacheOption
            ) : base(bitmapUri, createOptions, cacheOption, MILGuidData.GUID_ContainerFormatIco)
        {
        }

        /// <summary>
        /// If this decoder cannot handle the bitmap stream, it will throw an exception.
        /// </summary>
        /// <param name="bitmapStream">Stream to decode</param>
        /// <param name="createOptions">Bitmap Create Options</param>
        /// <param name="cacheOption">Bitmap Caching Option</param>
        /// <SecurityNote>
        /// Critical - access critical resource
        /// PublicOK - inputs verified or safe
        /// </SecurityNote>
        [SecurityCritical ]
        public IconBitmapDecoder(
            Stream bitmapStream,
            BitmapCreateOptions createOptions,
            BitmapCacheOption cacheOption
            ) : base(bitmapStream, createOptions, cacheOption, MILGuidData.GUID_ContainerFormatIco)
        {
        }

        /// <summary>
        /// Internal Constructor
        /// </summary>
        /// <SecurityNote>
        /// Critical: Uses a SafeFileHandle, which is a SecurityCritical type (in v4).
        ///     Calls SecurityCritical base class constructor.
        /// </SecurityNote>
        [SecurityCritical]
        internal IconBitmapDecoder(
            SafeMILHandle decoderHandle,
            BitmapDecoder decoder,
            Uri baseUri,
            Uri uri,
            Stream stream,
            BitmapCreateOptions createOptions,
            BitmapCacheOption cacheOption,
            bool insertInDecoderCache,
            bool originalWritable,
            Stream uriStream,
            UnmanagedMemoryStream unmanagedMemoryStream,
            SafeFileHandle safeFilehandle
            ) : base(decoderHandle, decoder, baseUri, uri, stream, createOptions, cacheOption, insertInDecoderCache, originalWritable, uriStream, unmanagedMemoryStream, safeFilehandle)
        {
        }

        #region Internal Abstract

        /// Need to implement this to derive from the "sealed" object
        internal override void SealObject()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    #endregion
}


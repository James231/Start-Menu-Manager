// -------------------------------------------------------------------------------------------------
// Start Menu Manager - © Copyright 2020 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

#pragma warning disable

// Copyright (c) 2014 AlphaSierraPapa for the SharpDevelop Team
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.
namespace HL.Xshtd
{
	using System;
	using System.Collections.Generic;
	using ICSharpCode.AvalonEdit.Utils;

	/// <summary>
	/// An Xml highlighting theme element.
	/// 
	/// <see cref="XmlHighlightingThemeDefinition"/> for equivalent run-time object.
	/// </summary>
	[Serializable]
	public class XhstdThemeDefinition : XshtdElement
	{
		/// <summary>
		/// Creates a new XhstdThemeDefinition object.
		/// </summary>
		public XhstdThemeDefinition()
		{
			this.Elements = new NullSafeCollection<XshtdElement>();
			this.GlobalStyleElements = new XshtdGlobalStyles();
		}

		/// <summary>
		/// Gets/sets the highlighting theme definition name (eg. 'Dark', 'TrueBlue')
		/// as stated in the Name attribute of the xshtd (xs highlighting theme definition) file.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets the collection of elements.
		/// </summary>
		public IList<XshtdElement> Elements { get; private set; }

		/// <summary>
		/// Gets the collection of elements.
		/// </summary>
		public XshtdGlobalStyles GlobalStyleElements { get; private set; }

		/// <summary>
		/// Applies the visitor to all elements.
		/// </summary>
		public override object AcceptVisitor(IXshtdVisitor visitor)
		{
			foreach (XshtdElement element in Elements)
			{
				element.AcceptVisitor(visitor);
			}

			// Visit Global Styles
			foreach (XshtdElement element in GlobalStyleElements.Elements)
			{
				element.AcceptVisitor(visitor);
			}

			return null;
		}
	}
}

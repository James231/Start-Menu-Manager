// -------------------------------------------------------------------------------------------------
// Start Menu Manager - © Copyright 2020 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

#pragma warning disable

namespace HL.Xshtd
{
	using System;
	using System.Collections.Generic;
	using ICSharpCode.AvalonEdit.Utils;

	/// <summary>
	/// A &lt;GlobalStyles&gt; element.
	/// </summary>
	[Serializable]
	public class XshtdGlobalStyles : XshtdElement
	{
		/// <summary>
		/// Creates a new XshtdSyntaxDefinition object.
		/// </summary>
		public XshtdGlobalStyles()
		{
			this.Elements = new NullSafeCollection<XshtdElement>();
		}

		/// <summary>
		/// Gets the collection of elements.
		/// </summary>
		public IList<XshtdElement> Elements { get; private set; }

		/// <summary>
		/// Applies the visitor to all elements.
		/// </summary>
		public void AcceptElements(IXshtdVisitor visitor)
		{
			foreach (XshtdElement element in Elements)
			{
				element.AcceptVisitor(visitor);
			}
		}

		/// <summary>
		/// Applies the visitor to this element.
		/// </summary>
		/// <param name="visitor"></param>
		/// <returns></returns>
		public override object AcceptVisitor(IXshtdVisitor visitor)
		{
			return visitor.VisitGlobalStyles(this);
		}
	}
}

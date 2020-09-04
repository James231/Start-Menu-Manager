// -------------------------------------------------------------------------------------------------
// Start Menu Manager - © Copyright 2020 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

#pragma warning disable

namespace HL.Xshtd
{
	using System;
	using System.Windows.Media;

	/// <summary>
	/// An element contained in a &lt;GlobalStyles&gt; element.
	/// </summary>
	[Serializable]
	public class XshtdGlobalStyle : XshtdElement
	{
		#region fields
		private readonly XshtdGlobalStyles _styles;
		#endregion fields

		#region ctors
		/// <summary>
		/// Creates a new XshtdSyntaxDefinition object.
		/// </summary>
		/// <param name="styles">Parent collection of styles in which this style occurs.</param>
		public XshtdGlobalStyle(XshtdGlobalStyles styles)
			: this()
		{
			_styles = styles;
		}

		/// <summary>
		/// Hidden class constructor
		/// </summary>
		protected XshtdGlobalStyle()
		{
		}
		#endregion ctors

		/// <summary>
		/// Gets/sets the style definition name
		/// </summary>
		public string TypeName { get; set; }

		/// <summary>
		/// Gets/sets the style definition name
		/// </summary>
		public Color? foreground { get; set; }

		/// <summary>
		/// Gets/sets the style definition name
		/// </summary>
		public Color? background { get; set; }

		/// <summary>
		/// Gets/sets the style definition name
		/// </summary>
		public Color? bordercolor { get; set; }

		#region methods

		/// <summary>
		/// Applies the visitor to this element.
		/// </summary>
		/// <param name="visitor"></param>
		/// <returns></returns>
		public override object AcceptVisitor(IXshtdVisitor visitor)
		{
			return visitor.VisitGlobalStyle(_styles, this);
		}
		#endregion methods
	}
}

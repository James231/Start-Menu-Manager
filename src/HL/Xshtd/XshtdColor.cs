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
	using ICSharpCode.AvalonEdit.Highlighting;
	using System;
	using System.Runtime.Serialization;
	using System.Security.Permissions;
	using System.Windows;

	/// <summary>
	/// A color in an Xshd file.
	/// </summary>
	[Serializable]
	public class XshtdColor : XshtdElement, ISerializable
	{
		#region fields
		private readonly XshtdSyntaxDefinition _syntax;
		#endregion fields

		#region ctors
		/// <summary>
		/// Creates a new XshdColor instance that is part of a <see cref="XshtdSyntaxDefinition"/>.
		/// </summary>
		public XshtdColor(XshtdSyntaxDefinition syntax)
		{
			_syntax = syntax;
		}

		/// <summary>
		/// Deserializes an XshdColor.
		/// </summary>
		protected XshtdColor(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
				throw new ArgumentNullException("info");

			this.Name = info.GetString("Name");
			this.Foreground = (HighlightingBrush)info.GetValue("Foreground", typeof(HighlightingBrush));
			this.Background = (HighlightingBrush)info.GetValue("Background", typeof(HighlightingBrush));

			if (info.GetBoolean("HasWeight"))
				this.FontWeight = System.Windows.FontWeight.FromOpenTypeWeight(info.GetInt32("Weight"));

			if (info.GetBoolean("HasStyle"))
				this.FontStyle = (FontStyle?)new FontStyleConverter().ConvertFromInvariantString(info.GetString("Style"));

			this.ExampleText = info.GetString("ExampleText");

			if (info.GetBoolean("HasUnderline"))
				this.Underline = info.GetBoolean("Underline");
		}
		#endregion ctors

		#region properties
		/// <summary>
		/// Gets/sets the name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets/sets the foreground brush.
		/// </summary>
		public HighlightingBrush Foreground { get; set; }

		/// <summary>
		/// Gets/sets the background brush.
		/// </summary>
		public HighlightingBrush Background { get; set; }

		/// <summary>
		/// Gets/sets the font weight.
		/// </summary>
		public FontWeight? FontWeight { get; set; }

		/// <summary>
		/// Gets/sets the underline flag
		/// </summary>
		public bool? Underline { get; set; }

		/// <summary>
		/// Gets/sets the font style.
		/// </summary>
		public FontStyle? FontStyle { get; set; }

		/// <summary>
		/// Gets/Sets the example text that demonstrates where the color is used.
		/// </summary>
		public string ExampleText { get; set; }
		#endregion properties

		/// <summary>
		/// Serializes this XshdColor instance.
		/// </summary>
#if DOTNET4
		[System.Security.SecurityCritical]
#else
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
#endif
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
				throw new ArgumentNullException("info");

			info.AddValue("Name", this.Name);
			info.AddValue("Foreground", this.Foreground);
			info.AddValue("Background", this.Background);
			info.AddValue("HasUnderline", this.Underline.HasValue);

			if (this.Underline.HasValue)
				info.AddValue("Underline", this.Underline.Value);

			info.AddValue("HasWeight", this.FontWeight.HasValue);

			if (this.FontWeight.HasValue)
				info.AddValue("Weight", this.FontWeight.Value.ToOpenTypeWeight());

			info.AddValue("HasStyle", this.FontStyle.HasValue);

			if (this.FontStyle.HasValue)
				info.AddValue("Style", this.FontStyle.Value.ToString());

			info.AddValue("ExampleText", this.ExampleText);
		}

		/// <summary>
		/// Applies the visitor to this element.
		/// </summary>
		/// <param name="visitor"></param>
		/// <returns></returns>
		public override object AcceptVisitor(IXshtdVisitor visitor)
		{
			return visitor.VisitColor(_syntax, this);
		}
	}
}

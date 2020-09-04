// -------------------------------------------------------------------------------------------------
// Start Menu Manager - © Copyright 2020 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

#pragma warning disable

namespace HL.Xshtd
{
	using HL.HighlightingTheme;
	using HL.Xshtd.interfaces;
	using ICSharpCode.AvalonEdit.Highlighting;
	using System.Collections.Generic;
	using System.Diagnostics;

	/// <summary>
	/// Implements a highlighting theme definition object that provides all run-time
	/// relevant properties and methods to work with themes in the context of highlightings.
	/// 
	/// <see cref="XhstdThemeDefinition"/> for equivalent Xml persistance layer object.
	/// </summary>
	internal class XmlHighlightingThemeDefinition : IHighlightingThemeDefinition
	{
		#region fields
		private Dictionary<string, SyntaxDefinition> syntaxDefDict;
		private Dictionary<string, GlobalStyle> _GlobalStyles;
		private readonly XhstdThemeDefinition _xshtd;
		#endregion fields

		#region ctors
		/// <summary>
		/// Class constructor
		/// </summary>
		public XmlHighlightingThemeDefinition(XhstdThemeDefinition xshtd,
											  IHighlightingThemeDefinitionReferenceResolver resolver)
			: this()
		{
			// Create HighlightingRuleSet instances
			xshtd.AcceptVisitor(new RegisterNamedElementsVisitor(this));

			// Translate elements within the rulesets (resolving references and processing imports)
			xshtd.AcceptVisitor(new TranslateElementVisitor(this, resolver));

			_xshtd = xshtd;
		}

		/// <summary>
		/// Class constructor
		/// </summary>
		protected XmlHighlightingThemeDefinition()
		{
			syntaxDefDict = new Dictionary<string, SyntaxDefinition>();
			_GlobalStyles = new Dictionary<string, GlobalStyle>();
		}
		#endregion ctors

		#region properties
		/// <summary>
		/// Gets/sets the highlighting theme definition name (eg. 'Dark', 'TrueBlue')
		/// as stated in the Name attribute of the xshtd (xs highlighting theme definition) file.
		/// </summary>
		public string Name { get { return _xshtd.Name; } }

		/// <summary>
		/// Gets all global stayles in the collection of global styles.
		/// </summary>
		public IEnumerable<GlobalStyle> GlobalStyles
		{
			get
			{
				return _GlobalStyles.Values;
			}
		}
		#endregion properties

		#region methods
		/// <summary>
		/// Gets the syntaxdefinition colors that should be applied for a certain highlighting (eg 'C#')
		/// within this theme (eg TrueBlue).
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public SyntaxDefinition GetNamedSyntaxDefinition(string name)
		{
			SyntaxDefinition item = null;
			syntaxDefDict.TryGetValue(name, out item);

			return item;
		}

		/// <summary>
		/// Gets a highlighting color based on the name of the syntax definition
		/// and the name of the color that should be contained in it.
		/// </summary>
		/// <param name="synDefName"></param>
		/// <param name="colorName"></param>
		/// <returns></returns>
		public HighlightingColor GetNamedColor(string synDefName, string colorName)
		{
			var synDef = GetNamedSyntaxDefinition(synDefName);
			if (synDef == null)
				return null;

			return synDef.ColorGet(colorName);
		}

		/// <summary>
		/// Gets an enumeration of all highlighting colors that are defined
		/// for this highlighting pattern (eg. C#) as part of a highlighting theme (eg 'True Blue').
		/// </summary>
		public IEnumerable<HighlightingColor> NamedHighlightingColors(string synDefName)
		{
			var synDef = GetNamedSyntaxDefinition(synDefName);
			if (synDef == null)
				return new List<HighlightingColor>();

			return synDef.NamedHighlightingColors;
		}

		/// <summary>
		/// Helper method to generate a <see cref="HighlightingDefinitionInvalidException"/>
		/// containing more insights (line number, coloumn) to verify the actual problem.
		/// </summary>
		/// <param name="element"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		private static System.Exception Error(XshtdElement element, string message)
		{
			if (element.LineNumber > 0)
				return new HighlightingDefinitionInvalidException(
					"Error at line " + element.LineNumber + ":\n" + message);
			else
				return new HighlightingDefinitionInvalidException(message);
		}
		#endregion methods

		#region private classes
		/// <summary>
		/// Implements the visitor pattern based on the <see cref="IXshtdVisitor"/> interface
		/// to register all elements of a given XML element tree and check whether their syntax
		/// is as expected or not.
		/// </summary>
		sealed class RegisterNamedElementsVisitor : IXshtdVisitor
		{
			#region fields
			private readonly XmlHighlightingThemeDefinition def;
			#endregion fields

			#region ctors
			/// <summary>
			/// Class constructor
			/// </summary>
			public RegisterNamedElementsVisitor(XmlHighlightingThemeDefinition def)
				: this()
			{
				this.def = def;
			}

			/// <summary>
			/// Class constructor
			/// </summary>
			private RegisterNamedElementsVisitor()
			{
			}
			#endregion ctors

			#region methods
			/// <summary>
			/// Implements the visitor for the <see cref="XshtdSyntaxDefinition"/> object.
			/// </summary>
			/// <param name="syntax"></param>
			/// <returns></returns>
			public object VisitSyntaxDefinition(XshtdSyntaxDefinition syntax)
			{
				if (syntax.Name != null)
				{
					if (syntax.Name.Length == 0)
						throw Error(syntax, "Name must not be the empty string");

					if (def.syntaxDefDict.ContainsKey(syntax.Name))
						throw Error(syntax, "Duplicate syntax definition name '" + syntax.Name + "'.");

					def.syntaxDefDict.Add(syntax.Name, new SyntaxDefinition());
				}

				syntax.AcceptElements(this);

				return null;
			}

			/// <summary>
			/// Implements the visitor for a named color (<see cref="XshtdColor"/> object)
			/// that is contained in a <see cref="XshtdSyntaxDefinition"/> object.
			/// 
			/// Method checks if given color name is unique and adds the color into the internal
			/// collection of inique colors if it is.
			/// </summary>
			/// <param name="syntax"></param>
			/// <param name="color"></param>
			/// <returns>Always returns null. Throws a <see cref="HighlightingDefinitionInvalidException"/>
			/// if color name is a duplicate.</returns>
			public object VisitColor(XshtdSyntaxDefinition syntax, XshtdColor color)
			{
				if (color.Name != null)
				{
					if (color.Name.Length == 0)
						throw Error(color, "Name must not be the empty string");

					if (syntax == null)
						throw Error(syntax, "Syntax Definition for theme must not be null");

					SyntaxDefinition synDef;
					if (def.syntaxDefDict.TryGetValue(syntax.Name, out synDef) == false)
						throw Error(syntax, "Themed Syntax Definition does not exist '" + syntax.Name + "'.");

					if (synDef.ColorGet(color.Name) == null)
						synDef.ColorAdd(new HighlightingColor() { Name = color.Name });
					else
						throw Error(color, "Duplicate color name '" + color.Name + "'.");
				}

				return null;
			}

			/// <summary>
			/// Implements the visitor for the <see cref="XshtdGlobalStyles"/> object.
			/// </summary>
			/// <param name="globStyles">the element to be visited.</param>
			/// <returns></returns>
			public object VisitGlobalStyles(XshtdGlobalStyles globStyles)
			{
				globStyles.AcceptElements(this);

				return null;
			}

			public object VisitGlobalStyle(XshtdGlobalStyles globStyles, XshtdGlobalStyle style)
			{
				if (style.TypeName != null)
				{
					if (style.TypeName.Length == 0)
						throw Error(style, "Name must not be the empty string");

					if (globStyles == null)
						throw Error(globStyles, "GlobalStyles parameter must not be null");

					GlobalStyle globDef;
					if (def._GlobalStyles.TryGetValue(style.TypeName, out globDef) == true)
						throw Error(style, "GlobalStyle definition '" + style.TypeName + "' has duplicates.");

					globDef = new GlobalStyle(style.TypeName);
					globDef.backgroundcolor = style.background;
					globDef.foregroundcolor = style.foreground;
					globDef.bordercolor = style.bordercolor;

					globDef.Freeze();

					def._GlobalStyles.Add(style.TypeName, new GlobalStyle(style.TypeName));
				}

				return null;
			}
			#endregion methods
		}

		/// <summary>
		/// Implements the visitor pattern based on the <see cref="IXshtdVisitor"/> interface
		/// to convert the content of a <see cref="XshtdSyntaxDefinition"/> object into a
		/// <see cref="XmlHighlightingThemeDefinition"/> object.
		/// </summary>
		sealed class TranslateElementVisitor : IXshtdVisitor
		{
			#region fields
			readonly XmlHighlightingThemeDefinition def;
			#endregion fields

			#region ctors
			/// <summary>
			/// Class constructor.
			/// </summary>
			/// <param name="def"></param>
			/// <param name="resolver"></param>
			public TranslateElementVisitor(XmlHighlightingThemeDefinition def,
										   IHighlightingThemeDefinitionReferenceResolver resolver)
			{
				Debug.Assert(def != null);
				Debug.Assert(resolver != null);
				this.def = def;
			}
			#endregion ctors

			#region methods
			/// <summary>
			/// Implements the visitor for the <see cref="XshtdSyntaxDefinition"/> object.
			/// </summary>
			/// <param name="syntax"></param>
			/// <returns></returns>
			public object VisitSyntaxDefinition(XshtdSyntaxDefinition syntax)
			{
				SyntaxDefinition c;
				if (syntax.Name != null)
					c = def.syntaxDefDict[syntax.Name];
				else
				{
					if (syntax.Extensions == null)
						return null;
					else
						c = new SyntaxDefinition(syntax.Name);
				}

				// Copy extensions to highlighting theme object
				foreach (var item in syntax.Extensions)
					c.Extensions.Add(item);

				syntax.AcceptElements(this);

				return c;
			}

			/// <summary>
			/// Implements the visitor for a named color (<see cref="XshtdColor"/> object)
			/// that is contained in a <see cref="XshtdSyntaxDefinition"/> object.
			/// </summary>
			/// <param name="syntax"></param>
			/// <param name="color"></param>
			/// <returns></returns>
			public object VisitColor(XshtdSyntaxDefinition syntax, XshtdColor color)
			{
				if (color.Name == null)
					throw Error(color, "Name must not be null");

				if (color.Name.Length == 0)
					throw Error(color, "Name must not be the empty string");

				if (syntax == null)
					throw Error(syntax, "Syntax Definition for theme must not be null");

				SyntaxDefinition synDef;
				HighlightingColor highColor;
				if (def.syntaxDefDict.TryGetValue(syntax.Name, out synDef) == false)
					throw Error(syntax, "Themed Syntax Definition does not exist '" + syntax.Name + "'.");

				highColor = synDef.ColorGet(color.Name);
				if (highColor == null)
				{
					highColor = new HighlightingColor() { Name = color.Name };
					synDef.ColorAdd(highColor);
				}

				highColor.Foreground = color.Foreground;
				highColor.Background = color.Background;
				highColor.Underline = color.Underline;
				highColor.FontStyle = color.FontStyle;
				highColor.FontWeight = color.FontWeight;

				return highColor;
			}

			/// <summary>
			/// Implements the visitor for the <see cref="XshtdGlobalStyles"/> object.
			/// </summary>
			/// <param name="globStyles">the element to be visited.</param>
			/// <returns></returns>
			public object VisitGlobalStyles(XshtdGlobalStyles globStyles)
			{
				globStyles.AcceptElements(this);

				return null;
			}

			public object VisitGlobalStyle(XshtdGlobalStyles globStyles, XshtdGlobalStyle style)
			{
				if (style.TypeName == null)
					throw Error(style, "Name must not be null");

				if (style.TypeName.Length == 0)
					throw Error(style, "Name must not be the empty string");

				if (globStyles == null)
					throw Error(globStyles, "GlobalStyles parameter must not be null");

				GlobalStyle globDef;
				if (def._GlobalStyles.TryGetValue(style.TypeName, out globDef) == false)
					throw Error(style, "Style definition '" + style.TypeName + "' does not exist in collection of GlobalStyles.");

				globDef.backgroundcolor = style.background;
				globDef.foregroundcolor = style.foreground;
				globDef.bordercolor = style.bordercolor;
				globDef.Freeze();

				return globDef;
			}
			#endregion methods
		}
		#endregion private classes
	}
}

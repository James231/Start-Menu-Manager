// -------------------------------------------------------------------------------------------------
// Start Menu Manager - © Copyright 2020 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

#pragma warning disable

namespace HL.Manager
{
	using System.Collections.ObjectModel;
	using HL.HighlightingTheme;
	using HL.Xshtd;
	using HL.Xshtd.interfaces;
	using ICSharpCode.AvalonEdit.Highlighting;

	/// <summary>
	/// Defines a highlighting theme which is based on a WPF theme (eg. 'Light')
	/// with a corresponding set of highlighting definitions (eg. 'XML', 'C#' etc)
	/// to ensure that highlightings are correct in the contecxt of
	/// (different background colors) WPF themes.
	/// </summary>
	public interface IHLTheme
	{
		#region properties
		/// <summary>
		/// Gets the display independent key value that should by unique in an
		/// overall collection of highlighting themes and should be used for retrieval purposes.
		/// </summary>
		string Key { get; }

		/// <summary>
		/// Gets the prefix of the XSHD resources that should be used to lookup
		/// the actual resource for this theme.
		/// 
		/// This property is null for a derived highlighting theme since finding its
		/// base highlighting should by performed through <see cref="HLBaseKey"/>
		/// and the corresponding <see cref="HLBasePrefix"/> property of that entry.
		/// </summary>
		string HLBasePrefix { get; }

		/// <summary>
		/// Gets the name of theme (eg. 'Dark' or 'Light' which is used as
		/// the base of a derived highlighting theme.
		/// 
		/// This property has the same value as the <see cref="Key"/> property
		/// if the highlighting is GENERIC (since these highlightings come without
		/// additional theme resources).
		/// </summary>
		string HLBaseKey { get; }

		/// <summary>
		/// Gets the prefix of the resource under which a theme resource definition
		/// file xshTd can be found (eg 'HL.Resources.Themes').
		/// </summary>
		string HLThemePrefix { get; }

		/// <summary>
		/// Gets the file name under which a theme resource definition
		/// file xshTd can be found (eg 'Dark.xshtd').
		/// </summary>
		string HLThemeFileName { get; }

		/// <summary>
		/// Gets the name of theme (eg. 'Dark', 'Light' or 'True Blue' for display purposes in the UI.
		/// </summary>
		string DisplayName { get; }

		/// <summary>
		/// Gets a copy of all highlightings defined in this object.
		/// </summary>
		ReadOnlyCollection<IHighlightingDefinition> HighlightingDefinitions { get; }

		/// <summary>
		/// Gets the theme highlighting definition for this theme
		/// or null (highlighting definition is generic and not based on a theme).
		/// </summary>
		IHighlightingThemeDefinition HlTheme { get; }

		/// <summary>
		/// Gets/sets whether built-in themes have already been registered or not
		/// Use this to avoid registration of built-in themes twice for one and the
		/// same highlighting theme.
		/// </summary>
		bool IsBuiltInThemesRegistered { get; set; }
		#endregion properties

		#region methods
		/// <summary>
		/// Gets the highlighting definition by name, or null if it is not found.
		/// </summary>
		IHighlightingDefinition GetDefinition(string name);

		/// <summary>
		/// Gets a highlighting definition by extension.
		/// Returns null if the definition is not found.
		/// </summary>
		IHighlightingDefinition GetDefinitionByExtension(string extension);

		/// <summary>
		/// Registers a highlighting definition.
		/// </summary>
		/// <param name="name">The name to register the definition with.</param>
		/// <param name="extensions">The file extensions to register the definition for.</param>
		/// <param name="highlighting">The highlighting definition.</param>
		void RegisterHighlighting(string name, string[] extensions, IHighlightingDefinition highlighting);

		/// <summary>
		/// Gets the highlighting theme definition  by name, or null if it is not found.
		/// </summary>
		/// <param name="highlightingName"></param>
		SyntaxDefinition GetThemeDefinition(string highlightingName);

		/// <summary>
		/// Converts a XSHD reference from namespace prefix and themename
		/// into a <see cref="XhstdThemeDefinition"/> object and returns it.
		/// </summary>
		/// <param name="hLPrefix"></param>
		/// <param name="hLThemeName"></param>
		/// <returns></returns>
		XhstdThemeDefinition ResolveHighLightingTheme(string hLPrefix, string hLThemeName);
		#endregion methods
	}
}
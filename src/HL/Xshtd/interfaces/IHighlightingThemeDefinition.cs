// -------------------------------------------------------------------------------------------------
// Start Menu Manager - © Copyright 2020 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

#pragma warning disable

namespace HL.Xshtd.interfaces
{
	using HL.HighlightingTheme;
	using System.Collections.Generic;

	/// <summary>
	/// A highlighting definition.
	/// </summary>
	public interface IHighlightingThemeDefinition
	{
		/// <summary>
		/// Gets the name of the highlighting theme definition.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Gets a named highlighting color.
		/// </summary>
		/// <returns>The highlighting color, or null if it is not found.</returns>
		////HighlightingColor GetNamedColor(string name);
		SyntaxDefinition GetNamedSyntaxDefinition(string name);

		/// <summary>
		/// Gets all global stayles in the collection of global styles.
		/// </summary>
		IEnumerable<GlobalStyle> GlobalStyles { get; }
	}
}

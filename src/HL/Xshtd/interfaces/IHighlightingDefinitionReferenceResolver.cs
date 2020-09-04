// -------------------------------------------------------------------------------------------------
// Start Menu Manager - © Copyright 2020 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

#pragma warning disable

namespace HL.Xshtd.interfaces
{
	using HL.HighlightingTheme;

	/// <summary>
	/// Defines a resolver interface that can find highlighting theme definitions
	/// based on a highlighting name (searches within the current highlighting theme)
	/// or based on a highlighting name and name of highlighting theme that should
	/// contain the highlighting definition.
	/// </summary>
	public interface IHighlightingThemeDefinitionReferenceResolver
	{
		/// <summary>
		/// Gets a highlighting definition within the current highlighting theme
		/// by name, or null.
		/// </summary>
		/// <param name="highlightingName"></param>
		/// <returns></returns>
		SyntaxDefinition GetThemeDefinition(string highlightingName);

		/// <summary>
		/// Gets a highlighting theme definition by name from a given highlighting
		/// theme obtained via <paramref name="hlThemeName"/> or null.
		/// </summary>
		/// <param name="hlThemeName"></param>
		/// <param name="highlightingName"></param>
		SyntaxDefinition GetThemeDefinition(string hlThemeName,
											string highlightingName);
	}
}

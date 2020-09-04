// -------------------------------------------------------------------------------------------------
// Start Menu Manager - © Copyright 2020 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

#pragma warning disable

namespace HL.Manager
{
	using HL.HighlightingTheme;
	using HL.Interfaces;
	using HL.Resources;
	using HL.Xshtd.interfaces;
	using ICSharpCode.AvalonEdit.Highlighting;
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;

	/// <summary>
	/// Implements a Highlighting Manager that associates syntax highlighting definitions with file extentions
	/// (*.cs -> 'C#') with consideration of the current WPF App theme
	/// 
	/// Extension  App Theme   SyntaxHighlighter
	/// (*.cs  +   'Dark')  -> 'C#' (with color definitions for 'Dark')
	/// </summary>
	public class ThemedHighlightingManager : IThemedHighlightingManager
	{
		#region fields
		/// <summary>
		/// Defines the root namespace under which the built-in xshd highlighting
		/// resource files can be found
		/// (eg all highlighting for 'Light' should be located here).
		/// </summary>
		public const string HL_GENERIC_NAMESPACE_ROOT = "HL.Resources.Light";

		/// <summary>
		/// Defines the root namespace under which the built-in additional xshtd highlighting theme
		/// resource files can be found
		/// (eg 'Dark' and 'TrueBlue' themes should be located here).
		/// </summary>
		public const string HL_THEMES_NAMESPACE_ROOT = "HL.Resources.Themes";

		private readonly object lockObj = new object();
		private readonly Dictionary<string, IHLTheme> _ThemedHighlightings;
		#endregion fields

		#region ctors
		/// <summary>
		/// Class constructor
		/// </summary>
		public ThemedHighlightingManager()
		{
			_ThemedHighlightings = new Dictionary<string, IHLTheme>();
		}
		#endregion ctors

		#region properties
		/// <summary>
		/// Gets the current highlighting theme (eg 'Light' or 'Dark') that should be used as
		/// a base for the syntax highlighting in AvalonEdit.
		/// </summary>
		public IHLTheme CurrentTheme { get; private set; }

		/// <summary>
		/// Gets the default HighlightingManager instance.
		/// The default HighlightingManager comes with built-in highlightings.
		/// </summary>
		public static IThemedHighlightingManager Instance
		{
			get
			{
				return DefaultHighlightingManager.Instance;
			}
		}
		#endregion properties

		#region methods
		/// <summary>
		/// Gets the highlighting definition by name, or null if it is not found.
		/// </summary>
		IHighlightingDefinition IHighlightingDefinitionReferenceResolver.GetDefinition(string name)
		{
			lock (lockObj)
			{
				if (CurrentTheme != null)
					return CurrentTheme.GetDefinition(name);

				return null;
			}
		}

		/// <summary>
		/// Gets an (ordered by Name) list copy of all highlightings defined in this object
		/// or an empty collection if there is no highlighting definition available.
		/// </summary>
		public ReadOnlyCollection<IHighlightingDefinition> HighlightingDefinitions
		{
			get
			{
				lock (lockObj)
				{
					if (CurrentTheme != null)
						return CurrentTheme.HighlightingDefinitions;

					return new ReadOnlyCollection<IHighlightingDefinition>(new List<IHighlightingDefinition>());
				}
			}
		}

		/// <summary>
		/// Gets a highlighting definition by extension.
		/// Returns null if the definition is not found.
		/// </summary>
		public IHighlightingDefinition GetDefinitionByExtension(string extension)
		{
			lock (lockObj)
			{
				IHLTheme theme;
				if (_ThemedHighlightings.TryGetValue(CurrentTheme.Key, out theme) == true)
				{
					return theme.GetDefinitionByExtension(extension);
				}

				return null;
			}
		}

		/// <summary>
		/// Registers a highlighting definition for the <see cref="CurrentTheme"/>.
		/// </summary>
		/// <param name="name">The name to register the definition with.</param>
		/// <param name="extensions">The file extensions to register the definition for.</param>
		/// <param name="highlighting">The highlighting definition.</param>
		public void RegisterHighlighting(string name, string[] extensions, IHighlightingDefinition highlighting)
		{
			if (highlighting == null)
				throw new ArgumentNullException("highlighting");

			lock (lockObj)
			{
				if (this.CurrentTheme != null)
				{
					CurrentTheme.RegisterHighlighting(name, extensions, highlighting);
				}
			}
		}

		/// <summary>
		/// Registers a highlighting definition.
		/// </summary>
		/// <param name="name">The name to register the definition with.</param>
		/// <param name="extensions">The file extensions to register the definition for.</param>
		/// <param name="lazyLoadedHighlighting">A function that loads the highlighting definition.</param>
		public void RegisterHighlighting(string name, string[] extensions, Func<IHighlightingDefinition> lazyLoadedHighlighting)
		{
			if (lazyLoadedHighlighting == null)
				throw new ArgumentNullException("lazyLoadedHighlighting");

			RegisterHighlighting(name, extensions, new DelayLoadedHighlightingDefinition(name, lazyLoadedHighlighting));
		}

		/// <summary>
		/// Sets the current highlighting based on the name of the given highöighting theme.
		/// (eg: WPF APP Theme 'TrueBlue' -> Resolve highlighting 'C#' to 'TrueBlue'->'C#')
		/// 
		/// Throws an <see cref="IndexOutOfRangeException"/> if the WPF APP theme is not known.
		/// </summary>
		/// <param name="themeNameKey"></param>
		public void SetCurrentTheme(string themeNameKey)
		{
			SetCurrentThemeInternal(themeNameKey);
			HLResources.RegisterBuiltInHighlightings(DefaultHighlightingManager.Instance, CurrentTheme);
		}

		/// <summary>
		/// Adds another highlighting theme into the current collection of highlighting themes.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="theme"></param>
		public void ThemedHighlightingAdd(string key, IHLTheme theme)
		{
			lock (lockObj)
			{
				_ThemedHighlightings.Add(key, theme);
			}
		}

		/// <summary>
		/// Removes a highlighting theme from the current collection
		/// of highlighting themes.
		/// </summary>
		/// <param name="removekey"></param>
		public void ThemedHighlightingRemove(string removekey)
		{
			lock (lockObj)
			{
				_ThemedHighlightings.Remove(removekey);
			}
		}

		/// <summary>
		/// Initializes the current default theme available at start-up of application
		/// (without registration of highlightings).
		/// </summary>
		/// <param name="themeNameKey"></param>
		protected void SetCurrentThemeInternal(string themeNameKey)
		{
			CurrentTheme = _ThemedHighlightings[themeNameKey];
		}

		/// <summary>
		/// Helper method to find the correct namespace of an internal xshd resource
		/// based on the name of a (WPF) theme (eg. 'TrueBlue') and an internal
		/// constant (eg. 'HL.Resources')
		/// </summary>
		/// <param name="themeNameKey"></param>
		/// <returns></returns>
		protected virtual string GetPrefix(string themeNameKey)
		{
			lock (lockObj)
			{
				IHLTheme theme;
				if (_ThemedHighlightings.TryGetValue(themeNameKey, out theme) == true)
				{
					return theme.HLBasePrefix;
				}
			}

			return null;
		}

		/// <summary>
		/// Gets the highlighting theme definition by name, or null if there is none to be found.
		/// </summary>
		/// <param name="highlightingName"></param>
		SyntaxDefinition IHighlightingThemeDefinitionReferenceResolver.GetThemeDefinition(string highlightingName)
		{
			lock (lockObj)
			{
				if (CurrentTheme != null)
					return CurrentTheme.GetThemeDefinition(highlightingName);

				return null;
			}
		}

		/// <summary>
		/// Gets the highlighting theme definition by name of the theme (eg 'Dark2' or 'TrueBlue')
		/// and the highlighting, or null if there is none to be found.
		/// </summary>
		/// <param name="hlThemeName"></param>
		/// <param name="highlightingName"></param>
		SyntaxDefinition IHighlightingThemeDefinitionReferenceResolver.GetThemeDefinition(string hlThemeName,
																						  string highlightingName)
		{
			lock (lockObj)
			{
				IHLTheme highlighting;
				this._ThemedHighlightings.TryGetValue(hlThemeName, out highlighting);

				if (highlighting != null)
					return highlighting.GetThemeDefinition(hlThemeName);

				return null;
			}
		}
		#endregion methods
	}
}

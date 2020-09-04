// -------------------------------------------------------------------------------------------------
// Start Menu Manager - © Copyright 2020 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

#pragma warning disable

namespace HL.Manager
{
	using HL.HighlightingTheme;
	using HL.Resources;
	using HL.Xshtd;
	using HL.Xshtd.interfaces;
	using ICSharpCode.AvalonEdit.Highlighting;
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.IO;
	using System.Linq;
	using System.Xml;

	/// <summary>
	/// Implements a highlighting theme which is based on a WPF theme (eg. 'Light')
	/// with a corresponding set of highlighting definitions (eg. 'XML', 'C#' etc)
	/// to ensure that highlightings are correct in the contecxt of
	/// (different background colors) WPF themes.
	/// </summary>
	internal class HLTheme : IHLTheme
	{
		#region fields
		private readonly object lockObj = new object();
		private Dictionary<string, IHighlightingDefinition> highlightingsByName = new Dictionary<string, IHighlightingDefinition>();
		private Dictionary<string, IHighlightingDefinition> highlightingsByExtension = new Dictionary<string, IHighlightingDefinition>(StringComparer.OrdinalIgnoreCase);
		private List<IHighlightingDefinition> allHighlightings = new List<IHighlightingDefinition>();
		private bool _HLThemeIsInitialized;

		private XhstdThemeDefinition _xshtd;
		private XmlHighlightingThemeDefinition _hlTheme;
		private readonly IHighlightingThemeDefinitionReferenceResolver _hLThemeResolver;
		#endregion fields

		#region ctors
		/// <summary>
		/// Class constructor for GENERIC highlighting definitions.
		/// 
		/// Generic highlighting definitions ar usually defined in xshd
		/// files and stand on their own (do not need additional processing/resources
		/// to compute highlighting rules and formating information).
		/// </summary>
		/// <param name="paramKey"></param>
		/// <param name="paramHLBasePrefix"></param>
		/// <param name="paramDisplayName"></param>
		public HLTheme(string paramKey,
					   string paramHLBasePrefix,
					   string paramDisplayName)
			: this()
		{
			Key = paramKey;
			HLBasePrefix = paramHLBasePrefix;  // This Highlighting is GENERIC - based on 'itself'
			HLBaseKey = paramKey;

			DisplayName = paramDisplayName;
		}

		/// <summary>
		/// Class constructor for derived highlighting themes.
		/// 
		/// Derived highlighting themes have a base highlighting (eg 'Light')
		/// and an 'overwritting' highlighting themes definition using an xshTd file resource.
		/// </summary>
		/// <param name="paramKey"></param>
		/// <param name="paramHLBaseKey"></param>
		/// <param name="paramDisplayName"></param>
		/// <param name="paramHLThemePrefix"></param>
		/// <param name="paramHLThemeName"></param>
		/// <param name="themeResolver"></param>
		public HLTheme(string paramKey,
			string paramHLBaseKey,
					   string paramDisplayName,
					   string paramHLThemePrefix, string paramHLThemeName,
					   IHighlightingThemeDefinitionReferenceResolver themeResolver)
			: this()
		{
			Key = paramKey;
			HLBaseKey = paramHLBaseKey;

			HLThemePrefix = paramHLThemePrefix;
			HLThemeFileName = paramHLThemeName;

			_hLThemeResolver = themeResolver;

			DisplayName = paramDisplayName;
		}

		/// <summary>
		/// Hidden class constructor
		/// </summary>
		protected HLTheme()
		{
		}
		#endregion ctors

		#region properties
		/// <summary>
		/// Gets the display independent key value that should by unique in an
		/// overall collection of highlighting themes and should be used for retrieval purposes.
		/// </summary>
		public string Key { get; }

		/// <summary>
		/// Gets the prefix of the XSHD resources that should be used to lookup
		/// the actual resource for this theme.
		/// 
		/// This property is null for a derived highlighting theme since finding its
		/// base highlighting should by performed through <see cref="HLBaseKey"/>
		/// and the corresponding <see cref="HLBasePrefix"/> property of that entry.
		/// </summary>
		public string HLBasePrefix { get; }

		/// <summary>
		/// Gets the name of theme (eg. 'Dark' or 'Light' which is used as
		/// the base of a derived highlighting theme.
		/// 
		/// This property has the same value as the <see cref="Key"/> property
		/// if the highlighting is GENERIC (since these highlightings come without
		/// additional theme resources).
		/// </summary>
		public string HLBaseKey { get; }

		/// <summary>
		/// Gets the prefix of the resource under which a theme resource definition
		/// file xshTd can be found (eg 'HL.Resources.Themes').
		/// </summary>
		public string HLThemePrefix { get; }

		/// <summary>
		/// Gets the file name under which a theme resource definition
		/// file xshTd can be found (eg 'Dark.xshtd').
		/// </summary>
		public string HLThemeFileName { get; }

		/// <summary>
		/// Gets the name of theme (eg. 'Dark', 'Light' or 'True Blue' for display purposes in the UI.
		/// </summary>
		public string DisplayName { get; }

		/// <summary>
		/// Gets an (ordered by Name) list copy of all highlightings defined in this object.
		/// </summary>
		public ReadOnlyCollection<IHighlightingDefinition> HighlightingDefinitions
		{
			get
			{
				lock (lockObj)
				{
					return Array.AsReadOnly(allHighlightings.OrderBy(x => x.Name).ToArray());
				}
			}
		}

		/// <summary>
		/// Gets the theme highlighting definition for this theme
		/// or null (highlighting definition is generic and not based on a theme).
		/// </summary>
		public IHighlightingThemeDefinition HlTheme
		{
			get
			{
				ResolveHighLightingTheme();

				return _hlTheme;
			}
		}

		/// <summary>
		/// Gets/sets whether built-in themes have already been registered or not
		/// Use this to avoid registration of built-in themes twice for one and the
		/// same highlighting theme.
		/// </summary>
		public bool IsBuiltInThemesRegistered { get; set; }
		#endregion properties

		#region methods
		/// <summary>
		/// Gets the highlighting definition by name, or null if it is not found.
		/// </summary>
		public IHighlightingDefinition GetDefinition(string name)
		{
			lock (lockObj)
			{
				this.ResolveHighLightingTheme();

				IHighlightingDefinition rh;
				if (highlightingsByName.TryGetValue(name, out rh))
					return rh;
				else
					return null;
			}
		}

		/// <summary>
		/// Gets the highlighting theme definition  by name, or null if it is not found.
		/// </summary>
		public SyntaxDefinition GetThemeDefinition(string highlightingName)
		{
			lock (lockObj)
			{
				this.ResolveHighLightingTheme();

				return _hlTheme.GetNamedSyntaxDefinition(highlightingName);
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
				this.ResolveHighLightingTheme();

				IHighlightingDefinition rh;
				if (highlightingsByExtension.TryGetValue(extension, out rh))
					return rh;
				else
					return null;
			}
		}

		/// <summary>
		/// Registers a highlighting definition.
		/// </summary>
		/// <param name="name">The name to register the definition with.</param>
		/// <param name="extensions">The file extensions to register the definition for.</param>
		/// <param name="highlighting">The highlighting definition.</param>
		public void RegisterHighlighting(string name,
										 string[] extensions,
										 IHighlightingDefinition highlighting)
		{
			lock (lockObj)
			{
				// Perform an update if this highlighting happens to be available already 
				var itemInList = allHighlightings.FirstOrDefault(i => name == i.Name);
				if (itemInList != null)
					allHighlightings.Remove(itemInList);

				allHighlightings.Add(highlighting);
				if (name != null)
				{
					highlightingsByName[name] = highlighting;
				}

				if (extensions != null)
				{
					foreach (string ext in extensions)
					{
						highlightingsByExtension[ext] = highlighting;
					}
				}
			}
		}

		/// <summary>
		/// Loads the highlighting theme for this highlighting definition
		/// (if an additional theme was configured)
		/// </summary>
		protected virtual void ResolveHighLightingTheme()
		{
			if (_hlTheme != null || _HLThemeIsInitialized == true)
				return;

			_HLThemeIsInitialized = true;            // Initialize this at most once

			// Load the highlighting theme and setup converter to XmlHighlightingThemeDefinition
			_xshtd = ResolveHighLightingTheme(HLThemePrefix, HLThemeFileName);

			if (_hLThemeResolver == null || _xshtd == null)
				return;

			_hlTheme = new XmlHighlightingThemeDefinition(_xshtd, _hLThemeResolver);
		}

		/// <summary>
		/// Converts a XSHTD reference from namespace prefix and themename
		/// into a <see cref="XhstdThemeDefinition"/> object and returns it.
		/// </summary>
		/// <param name="hLPrefix"></param>
		/// <param name="hLThemeName"></param>
		/// <returns></returns>
		public XhstdThemeDefinition ResolveHighLightingTheme(string hLPrefix, string hLThemeName)
		{
			if (string.IsNullOrEmpty(hLPrefix) || string.IsNullOrEmpty(hLThemeName))
				return null;

			using (Stream s = HLResources.OpenStream(hLPrefix, hLThemeName))
			{
				using (XmlTextReader reader = new XmlTextReader(s))
				{
					return HighlightingThemeLoader.LoadXshd(reader, false);
				}
			}
		}
		#endregion methods
	}
}

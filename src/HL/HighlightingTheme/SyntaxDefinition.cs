// -------------------------------------------------------------------------------------------------
// Start Menu Manager - © Copyright 2020 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

#pragma warning disable

namespace HL.HighlightingTheme
{
	using HL.Xshtd.interfaces;
	using ICSharpCode.AvalonEdit.Highlighting;
	using ICSharpCode.AvalonEdit.Utils;
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Implements the object that keeps track of each syntax definition reference
	/// within a highlighting theme definition.
	/// </summary>
	public class SyntaxDefinition : AbstractFreezable, IFreezable
	{
		#region fields
		string _Name;
		private readonly Dictionary<string, HighlightingColor> _NamedHighlightingColors;
		#endregion fields

		#region ctors
		/// <summary>
		/// Class constructor
		/// </summary>
		public SyntaxDefinition(string paramName)
			: this()
		{
			this._Name = paramName;
		}

		/// <summary>
		/// Class constructor
		/// </summary>
		public SyntaxDefinition()
		{
			this.Extensions = new NullSafeCollection<string>();
			_NamedHighlightingColors = new Dictionary<string, HighlightingColor>();
		}
		#endregion ctors

		#region properties
		/// <summary>
		/// Gets/Sets the name of the color.
		/// </summary>
		public string Name
		{
			get
			{
				return _Name;
			}
			set
			{
				if (IsFrozen)
					throw new InvalidOperationException();

				_Name = value;
			}
		}

		/// <summary>
		/// Gets the associated extensions.
		/// </summary>
		public IList<string> Extensions { get; private set; }

		/// <summary>
		/// Gets an enumeration of all highlighting colors that are defined
		/// for this highlighting pattern (eg. C#) as part of a highlighting theme (eg 'True Blue').
		/// </summary>
		public IEnumerable<HighlightingColor> NamedHighlightingColors
		{
			get
			{
				return _NamedHighlightingColors.Values;
			}
		}
		#endregion properties

		#region methods
		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return "[" + GetType().Name + " " + (string.IsNullOrEmpty(this.Name) ? string.Empty : this.Name) + "]";
		}

		/// <summary>
		/// Gets a named color definition or null.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public HighlightingColor ColorGet(string name)
		{
			HighlightingColor color;
			if (_NamedHighlightingColors.TryGetValue(name, out color))
				return color;

			return null;
		}

		/// <summary>
		/// Adds another named color definition.
		/// Exceptions:
		///   <see cref="System.ArgumentNullException"/>
		///     key is null.
		///
		///   <see cref="System.ArgumentException"/>
		///     An element with the same key already exists in the System.Collections.Generic.Dictionary`2.
		/// </summary>
		/// <param name="color"></param>
		public void ColorAdd(HighlightingColor color)
		{
			_NamedHighlightingColors.Add(color.Name, color);
		}

		internal void ColorReplace(string name, HighlightingColor themeColor)
		{
			_NamedHighlightingColors.Remove(name);
			_NamedHighlightingColors.Add(name, themeColor);
		}
		#endregion methods
	}
}

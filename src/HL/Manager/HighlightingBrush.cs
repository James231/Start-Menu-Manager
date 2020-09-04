// -------------------------------------------------------------------------------------------------
// Start Menu Manager - © Copyright 2020 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

#pragma warning disable

namespace HL.Manager
{
	using System;
	using System.Diagnostics;
	using System.Reflection;
	using System.Runtime.Serialization;
	using System.Windows;
	using System.Windows.Media;
	using ICSharpCode.AvalonEdit.Highlighting;
	using ICSharpCode.AvalonEdit.Rendering;

	/// <summary>
	/// HighlightingBrush implementation that finds a brush using a resource.
	/// </summary>
	[Serializable]
	sealed class SystemColorHighlightingBrush : HighlightingBrush, ISerializable
	{
		readonly PropertyInfo property;

		public SystemColorHighlightingBrush(PropertyInfo property)
		{
			Debug.Assert(property.ReflectedType == typeof(SystemColors));
			Debug.Assert(typeof(Brush).IsAssignableFrom(property.PropertyType));
			this.property = property;
		}

		public override Brush GetBrush(ITextRunConstructionContext context)
		{
			return (Brush)property.GetValue(null, null);
		}

		public override string ToString()
		{
			return property.Name;
		}

		SystemColorHighlightingBrush(SerializationInfo info, StreamingContext context)
		{
			property = typeof(SystemColors).GetProperty(info.GetString("propertyName"));
			if (property == null)
				throw new ArgumentException("Error deserializing SystemColorHighlightingBrush");
		}

		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("propertyName", property.Name);
		}

		public override bool Equals(object obj)
		{
			SystemColorHighlightingBrush other = obj as SystemColorHighlightingBrush;
			if (other == null)
				return false;
			return object.Equals(this.property, other.property);
		}

		public override int GetHashCode()
		{
			return property.GetHashCode();
		}
	}

}

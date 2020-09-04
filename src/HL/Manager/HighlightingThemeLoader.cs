// -------------------------------------------------------------------------------------------------
// Start Menu Manager - © Copyright 2020 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

#pragma warning disable

namespace HL.Manager
{
	using HL.Xshtd;
	using HL.Xshtd.interfaces;
	using ICSharpCode.AvalonEdit.Highlighting;
	using System;
	using System.Xml;
	using System.Xml.Schema;

	/// <summary>
	/// Static class with helper methods to load XSHTD highlighting files.
	/// </summary>
	static class HighlightingThemeLoader
	{
		#region XSHD loading
		/// <summary>
		/// Lodas a syntax definition from the xml reader.
		/// </summary>
		public static XhstdThemeDefinition LoadXshd(XmlReader reader)
		{
			return LoadXshd(reader, false);
		}

		internal static XhstdThemeDefinition LoadXshd(XmlReader reader, bool skipValidation)
		{
			if (reader == null)
				throw new ArgumentNullException("reader");
			try
			{
				reader.MoveToContent();
				if (reader.NamespaceURI == XshtdLoader.Namespace)
				{
					return XshtdLoader.LoadDefinition(reader, skipValidation);
				}

				throw new ArgumentOutOfRangeException(reader.NamespaceURI);
			}
			catch (XmlSchemaException ex)
			{
				throw WrapException(ex, ex.LineNumber, ex.LinePosition);
			}
			catch (XmlException ex)
			{
				throw WrapException(ex, ex.LineNumber, ex.LinePosition);
			}
		}

		static Exception WrapException(Exception ex, int lineNumber, int linePosition)
		{
			return new HighlightingDefinitionInvalidException(FormatExceptionMessage(ex.Message, lineNumber, linePosition), ex);
		}

		internal static string FormatExceptionMessage(string message, int lineNumber, int linePosition)
		{
			if (lineNumber <= 0)
				return message;
			else
				return "Error at position (line " + lineNumber + ", column " + linePosition + "):\n" + message;
		}

		internal static XmlReader GetValidatingReader(XmlReader input, bool ignoreWhitespace, XmlSchemaSet schemaSet)
		{
			XmlReaderSettings settings = new XmlReaderSettings();
			settings.CloseInput = true;
			settings.IgnoreComments = true;
			settings.IgnoreWhitespace = ignoreWhitespace;
			if (schemaSet != null)
			{
				settings.Schemas = schemaSet;
				settings.ValidationType = ValidationType.Schema;
			}
			return XmlReader.Create(input, settings);
		}

		internal static XmlSchemaSet LoadSchemaSet(XmlReader schemaInput)
		{
			XmlSchemaSet schemaSet = new XmlSchemaSet();
			schemaSet.Add(null, schemaInput);
			schemaSet.ValidationEventHandler += delegate (object sender, ValidationEventArgs args)
			{
				throw new HighlightingDefinitionInvalidException(args.Message);
			};
			return schemaSet;
		}
		#endregion

		#region Load Highlighting from XSHD
		/// <summary>
		/// Creates a highlighting definition from the XSHD file.
		/// </summary>
		public static IHighlightingThemeDefinition Load(XhstdThemeDefinition syntaxDefinition,
														IHighlightingThemeDefinitionReferenceResolver resolver)
		{
			if (syntaxDefinition == null)
				throw new ArgumentNullException("syntaxDefinition");

			return new XmlHighlightingThemeDefinition(syntaxDefinition, resolver);
		}

		/// <summary>
		/// Creates a highlighting definition from the XSHD file.
		/// </summary>
		public static IHighlightingThemeDefinition Load(XmlReader reader,
														IHighlightingThemeDefinitionReferenceResolver resolver)
		{
			return Load(LoadXshd(reader), resolver);
		}
		#endregion
	}
}

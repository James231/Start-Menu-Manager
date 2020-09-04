// -------------------------------------------------------------------------------------------------
// Start Menu Manager - © Copyright 2020 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

#pragma warning disable

namespace HL.Manager
{
	using HL.HighlightingTheme;
	using ICSharpCode.AvalonEdit.Highlighting;
	using ICSharpCode.AvalonEdit.Highlighting.Xshd;
	using System;
	using System.Xml;
	using System.Xml.Schema;

	/// <summary>
	/// Static class with helper methods to load XSHD highlighting files.
	/// </summary>
	public static class HighlightingLoader
	{
		#region XSHD loading
		/// <summary>
		/// Lodas a syntax definition from the xml reader.
		/// </summary>
		public static XshdSyntaxDefinition LoadXshd(XmlReader reader)
		{
			return LoadXshd(reader, false);
		}

		internal static XshdSyntaxDefinition LoadXshd(XmlReader reader, bool skipValidation)
		{
			if (reader == null)
				throw new ArgumentNullException("reader");
			try
			{
				reader.MoveToContent();
				////                if (reader.NamespaceURI == V2Loader.Namespace)
				////                {
				return V2Loader.LoadDefinition(reader, skipValidation);
				////                }
				////                else
				////                {
				////                    return V1Loader.LoadDefinition(reader, skipValidation);
				////                }
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
		public static IHighlightingDefinition Load(XshdSyntaxDefinition syntaxDefinition,
												   IHighlightingDefinitionReferenceResolver resolver)
		{
			if (syntaxDefinition == null)
				throw new ArgumentNullException("syntaxDefinition");

			return new XmlHighlightingDefinition(syntaxDefinition, resolver);
		}

		/// <summary>
		/// Loads a highlighting definition base on a:
		/// </summary>
		/// <param name="themedHighlights">
		/// Themed Highlighting Definition
		/// (This contains the color definition for a highlighting in this theme)
		/// </param>
		/// <param name="syntaxDefinition">
		/// A Highlighting definition
		/// (This contains the pattern matching and color definitions where the later
		///  is usually overwritten be a highlighting theme)
		/// </param>
		/// <param name="resolver">An object that can resolve a highlighting definition by its name.</param>
		/// <returns></returns>
		public static IHighlightingDefinition Load(SyntaxDefinition themedHighlights,
												   XshdSyntaxDefinition syntaxDefinition,
												   IHighlightingDefinitionReferenceResolver resolver
												   )
		{
			if (syntaxDefinition == null)
				throw new ArgumentNullException("syntaxDefinition");

			return new XmlHighlightingDefinition(themedHighlights, syntaxDefinition, resolver);
		}

		/// <summary>
		/// Creates a highlighting definition from the XSHD file that is already initialled
		/// in the <see cref="XmlReader"/> instance of the <paramref name="reader"/> parameter.
		/// </summary>
		public static IHighlightingDefinition Load(XmlReader reader,
												   IHighlightingDefinitionReferenceResolver resolver)
		{
			return Load(LoadXshd(reader), resolver);
		}
		#endregion
	}
}

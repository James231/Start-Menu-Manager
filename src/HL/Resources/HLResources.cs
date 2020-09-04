// -------------------------------------------------------------------------------------------------
// Start Menu Manager - © Copyright 2020 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

#pragma warning disable

namespace HL.Resources
{
	using System.IO;
	using HL.Manager;

	internal class HLResources
	{
		/// <summary>
		/// Open a <see cref="Stream"/> object to an internal resource (eg: xshd file)
		/// to load its contents from an 'Embedded Resource'.
		/// </summary>
		/// <param name="prefix"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public static Stream OpenStream(string prefix, string name)
		{
			string fileRef = prefix + "." + name;

			Stream s = typeof(HLResources).Assembly.GetManifestResourceStream(fileRef);
			if (s == null)
				throw new FileNotFoundException("The resource file '" + fileRef + "' was not found.");

			return s;
		}

		/// <summary>
		/// Registers the built-in highlighting definitions on first time request for a definition
		/// or when the application changes its WPF Theme (eg. from 'Light' to 'Dark') to load the
		/// appropriate highlighting resource when queried for it.
		/// </summary>
		/// <param name="hlm"></param>
		/// <param name="theme"></param>
		internal static void RegisterBuiltInHighlightings(
			DefaultHighlightingManager hlm,
			IHLTheme theme)
		{
			// This registration was already performed for this highlighting theme
			if (theme.IsBuiltInThemesRegistered == true)
				return;

			hlm.RegisterHighlighting(theme, "XmlDoc", null, "XmlDoc.xshd");

			hlm.RegisterHighlighting(theme, "C#", new[] { ".cs" }, "CSharp-Mode.xshd");
			hlm.RegisterHighlighting(theme, "Gcode", new[] { ".nc" }, "Gcode.xshd");

			hlm.RegisterHighlighting(theme, "GRazor", new[] { ".grazor" }, "GRazor-Mode.xshd");


			hlm.RegisterHighlighting(theme, "JavaScript", new[] { ".js" }, "JavaScript-Mode.xshd");
			hlm.RegisterHighlighting(theme, "HTML", new[] { ".htm", ".html" }, "HTML-Mode.xshd");
			hlm.RegisterHighlighting(theme, "ASP/XHTML", new[] { ".asp", ".aspx", ".asax", ".asmx", ".ascx", ".master" }, "ASPX.xshd");

			hlm.RegisterHighlighting(theme, "Boo", new[] { ".boo" }, "Boo.xshd");
			hlm.RegisterHighlighting(theme, "Coco", new[] { ".atg" }, "Coco-Mode.xshd");
			hlm.RegisterHighlighting(theme, "CSS", new[] { ".css" }, "CSS-Mode.xshd");
			hlm.RegisterHighlighting(theme, "C++", new[] { ".c", ".h", ".cc", ".cpp", ".hpp" }, "CPP-Mode.xshd");
			hlm.RegisterHighlighting(theme, "Java", new[] { ".java" }, "Java-Mode.xshd");
			hlm.RegisterHighlighting(theme, "Patch", new[] { ".patch", ".diff" }, "Patch-Mode.xshd");
			hlm.RegisterHighlighting(theme, "PowerShell", new[] { ".ps1", ".psm1", ".psd1" }, "PowerShell.xshd");
			hlm.RegisterHighlighting(theme, "PHP", new[] { ".php" }, "PHP-Mode.xshd");
			hlm.RegisterHighlighting(theme, "Python", new[] { ".py", ".pyw" }, "Python-Mode.xshd");
			hlm.RegisterHighlighting(theme, "TeX", new[] { ".tex" }, "Tex-Mode.xshd");
			hlm.RegisterHighlighting(theme, "TSQL", new[] { ".sql" }, "TSQL-Mode.xshd");
			hlm.RegisterHighlighting(theme, "VB", new[] { ".vb" }, "VB-Mode.xshd");
			hlm.RegisterHighlighting(theme, "XML", (".xml;.xsl;.xslt;.xsd;.manifest;.config;.addin;" +
											 ".xshd;.wxs;.wxi;.wxl;.proj;.csproj;.vbproj;.ilproj;" +
											 ".booproj;.build;.xfrm;.targets;.xaml;.xpt;" +
											 ".xft;.map;.wsdl;.disco;.ps1xml;.nuspec").Split(';'),
											 "XML-Mode.xshd");

			hlm.RegisterHighlighting(theme, "MarkDown", new[] { ".md" }, "MarkDown-Mode.xshd");

			// Additional Highlightings

			hlm.RegisterHighlighting(theme, "ActionScript3", new[] { ".as" }, "AS3.xshd");
			hlm.RegisterHighlighting(theme, "BAT", new[] { ".bat", ".dos" }, "DOSBATCH.xshd");
			hlm.RegisterHighlighting(theme, "F#", new[] { ".fs" }, "FSharp-Mode.xshd");
			hlm.RegisterHighlighting(theme, "HLSL", new[] { ".fx" }, "HLSL.xshd");
			hlm.RegisterHighlighting(theme, "INI", new[] { ".cfg", ".conf", ".ini", ".iss" }, "INI.xshd");
			hlm.RegisterHighlighting(theme, "LOG", new[] { ".log" }, "Log.xshd");
			hlm.RegisterHighlighting(theme, "Pascal", new[] { ".pas" }, "Pascal.xshd");
			hlm.RegisterHighlighting(theme, "PLSQL", new[] { ".plsql" }, "PLSQL.xshd");
			hlm.RegisterHighlighting(theme, "Ruby", new[] { ".rb" }, "Ruby.xshd");
			hlm.RegisterHighlighting(theme, "Scheme", new[] { ".sls", ".sps", ".ss", ".scm" }, "scheme.xshd");
			hlm.RegisterHighlighting(theme, "Squirrel", new[] { ".nut" }, "squirrel.xshd");
			hlm.RegisterHighlighting(theme, "TXT", new[] { ".txt" }, "TXT.xshd");
			hlm.RegisterHighlighting(theme, "VTL", new[] { ".vtl", ".vm" }, "vtl.xshd");

			theme.IsBuiltInThemesRegistered = true;
		}
	}
}

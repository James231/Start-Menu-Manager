// -------------------------------------------------------------------------------------------------
// Start Menu Manager - © Copyright 2020 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

#pragma warning disable

namespace HL.Manager
{
	using ICSharpCode.AvalonEdit.Highlighting;
	using System;
	using System.Collections.Generic;

	internal sealed class DelayLoadedHighlightingDefinition : IHighlightingDefinition
	{
		readonly object lockObj = new object();
		readonly string name;
		Func<IHighlightingDefinition> lazyLoadingFunction;
		IHighlightingDefinition definition;
		Exception storedException;

		public DelayLoadedHighlightingDefinition(string name, Func<IHighlightingDefinition> lazyLoadingFunction)
		{
			this.name = name;
			this.lazyLoadingFunction = lazyLoadingFunction;
		}

		public string Name
		{
			get
			{
				if (name != null)
					return name;
				else
					return GetDefinition().Name;
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
														 Justification = "The exception will be rethrown")]
		IHighlightingDefinition GetDefinition()
		{
			Func<IHighlightingDefinition> func;
			lock (lockObj)
			{
				if (this.definition != null)
					return this.definition;
				func = this.lazyLoadingFunction;
			}
			Exception exception = null;
			IHighlightingDefinition def = null;
			try
			{
				using (var busyLock = BusyManager.Enter(this))
				{
					if (!busyLock.Success)
						throw new InvalidOperationException("Tried to create delay-loaded highlighting definition recursively. Make sure the are no cyclic references between the highlighting definitions.");
					def = func();
				}
				if (def == null)
					throw new InvalidOperationException("Function for delay-loading highlighting definition returned null");
			}
			catch (Exception ex)
			{
				exception = ex;
			}
			lock (lockObj)
			{
				this.lazyLoadingFunction = null;
				if (this.definition == null && this.storedException == null)
				{
					this.definition = def;
					this.storedException = exception;
				}
				if (this.storedException != null)
					throw new HighlightingDefinitionInvalidException("Error delay-loading highlighting definition", this.storedException);
				return this.definition;
			}
		}

		public HighlightingRuleSet MainRuleSet
		{
			get
			{
				return GetDefinition().MainRuleSet;
			}
		}

		public HighlightingRuleSet GetNamedRuleSet(string name)
		{
			return GetDefinition().GetNamedRuleSet(name);
		}

		public HighlightingColor GetNamedColor(string name)
		{
			return GetDefinition().GetNamedColor(name);
		}

		public IEnumerable<HighlightingColor> NamedHighlightingColors
		{
			get
			{
				return GetDefinition().NamedHighlightingColors;
			}
		}

		public override string ToString()
		{
			return this.Name;
		}

		public IDictionary<string, string> Properties
		{
			get
			{
				return GetDefinition().Properties;
			}
		}
	}
}

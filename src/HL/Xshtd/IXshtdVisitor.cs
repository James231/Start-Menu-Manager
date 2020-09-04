// -------------------------------------------------------------------------------------------------
// Start Menu Manager - © Copyright 2020 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

#pragma warning disable

// Copyright (c) 2014 AlphaSierraPapa for the SharpDevelop Team
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.
namespace HL.Xshtd
{
	/// <summary>
	/// Implements an interface for usage in a Visitor pattern based implementation.
	/// This visitor pattern can be used to visit the elements of an XSHTD element tree.
	/// 
	/// A visitor pattern can be used in many ways, here its used for syntax checks and
	/// object conversion (from POCO XML object to non-xml .net object).
	/// </summary>
	public interface IXshtdVisitor
	{
		/// <summary>
		/// Implements the visitor for a named color (<see cref="XshtdColor"/> object)
		/// that is contained in a <see cref="XshtdSyntaxDefinition"/> object.
		/// </summary>
		/// <param name="syntax"></param>
		/// <param name="color"></param>
		/// <returns></returns>
		object VisitColor(XshtdSyntaxDefinition syntax, XshtdColor color);

		/// <summary>
		/// Implements the visitor for the <see cref="XshtdSyntaxDefinition"/> object.
		/// </summary>
		/// <param name="syntax">the element to be visited.</param>
		/// <returns></returns>
		object VisitSyntaxDefinition(XshtdSyntaxDefinition syntax);

		/// <summary>
		/// Implements the visitor for the <see cref="XshtdGlobalStyles"/> object.
		/// </summary>
		/// <param name="globStyles">the element to be visited.</param>
		/// <returns></returns>
		object VisitGlobalStyles(XshtdGlobalStyles globStyles);

		/// <summary>
		/// Implements the visitor for the <see cref="XshtdGlobalStyle"/> object
		/// contained in a <see cref="XshtdGlobalStyles"/> object.
		/// </summary>
		/// <param name="globStyles"></param>
		/// <param name="style"></param>
		/// <returns></returns>
		object VisitGlobalStyle(XshtdGlobalStyles globStyles, XshtdGlobalStyle style);
	}
}

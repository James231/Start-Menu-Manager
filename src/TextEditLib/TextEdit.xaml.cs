// -------------------------------------------------------------------------------------------------
// Start Menu Manager - © Copyright 2020 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

#pragma warning disable

namespace TextEditLib
{
	using ICSharpCode.AvalonEdit;
    using System.Windows;
	using System.Windows.Media;
	using TextEditLib.Extensions;

	/// <summary>
	/// Implements an AvalonEdit control textedit control with extensions.
	/// </summary>
	public class TextEdit : TextEditor
	{
		#region fields
		#region EditorCurrentLine Highlighting Colors
		public static readonly DependencyProperty EditorCurrentLineBackgroundProperty =
			DependencyProperty.Register("EditorCurrentLineBackground",
										 typeof(Brush),
										 typeof(TextEdit),
										 new UIPropertyMetadata(new SolidColorBrush(Colors.Transparent)));

		public static readonly DependencyProperty EditorCurrentLineBorderProperty =
			DependencyProperty.Register("EditorCurrentLineBorder", typeof(Brush),
				typeof(TextEdit), new PropertyMetadata(new SolidColorBrush(
					Color.FromArgb(0x60, SystemColors.HighlightBrush.Color.R,
										 SystemColors.HighlightBrush.Color.G,
										 SystemColors.HighlightBrush.Color.B))));

		public static readonly DependencyProperty EditorCurrentLineBorderThicknessProperty =
			DependencyProperty.Register("EditorCurrentLineBorderThickness", typeof(double),
				typeof(TextEdit), new PropertyMetadata(2.0d));
		#endregion EditorCurrentLine Highlighting Colors
		#endregion fields

		#region ctors
		/// <summary>
		/// Static class constructor
		/// </summary>
		static TextEdit()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(TextEdit),
				new FrameworkPropertyMetadata(typeof(TextEdit)));
		}

		/// <summary>
		/// Class constructor
		/// </summary>
		public TextEdit()
		{
			this.Loaded += TextEdit_Loaded;
		}
		#endregion ctors

		#region properties
		#region EditorCurrentLine Highlighting Colors
		/// <summary>
		/// Style the background color of the current editor line
		/// </summary>
		public Brush EditorCurrentLineBackground
		{
			get { return (Brush)GetValue(EditorCurrentLineBackgroundProperty); }
			set { SetValue(EditorCurrentLineBackgroundProperty, value); }
		}

		public Brush EditorCurrentLineBorder
		{
			get { return (Brush)GetValue(EditorCurrentLineBorderProperty); }
			set { SetValue(EditorCurrentLineBorderProperty, value); }
		}

		public double EditorCurrentLineBorderThickness
		{
			get { return (double)GetValue(EditorCurrentLineBorderThicknessProperty); }
			set { SetValue(EditorCurrentLineBorderThicknessProperty, value); }
		}
		#endregion EditorCurrentLine Highlighting Colors
		#endregion properties

		#region methods
		/// <summary>
		/// Method is invoked when the control is loaded for the first time.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TextEdit_Loaded(object sender, RoutedEventArgs e)
		{
			AdjustCurrentLineBackground();
		}

		/// <summary>
		/// Reset the <seealso cref="SolidColorBrush"/> to be used for highlighting the current editor line.
		/// </summary>
		private void AdjustCurrentLineBackground()
		{
			HighlightCurrentLineBackgroundRenderer oldRenderer = null;

			// Make sure there is only one of this type of background renderer
			// Otherwise, we might keep adding and WPF keeps drawing them on top of each other
			foreach (var item in this.TextArea.TextView.BackgroundRenderers)
			{
				if (item != null)
				{
					if (item is HighlightCurrentLineBackgroundRenderer)
					{
						oldRenderer = item as HighlightCurrentLineBackgroundRenderer;
					}
				}
			}

			if (oldRenderer != null)
				this.TextArea.TextView.BackgroundRenderers.Remove(oldRenderer);

			this.TextArea.TextView.BackgroundRenderers.Add(new HighlightCurrentLineBackgroundRenderer(this));
		}
		#endregion methods
	}
}

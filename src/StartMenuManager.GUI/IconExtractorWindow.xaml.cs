// -------------------------------------------------------------------------------------------------
// Start Menu Manager - © Copyright 2020 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

#pragma warning disable

using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using ImageMagick;
using WebImageExtractor;

namespace StartMenuManager.GUI
{
    /// <summary>
    /// Interaction logic for IconExtractorWindow.xaml
    /// </summary>
    public partial class IconExtractorWindow : Window
    {
        private int numRunning = 0;
        private MainWindow window;
        private ShortcutControl shortcutControl;

        public IconExtractorWindow(MainWindow mainWindow, ShortcutControl control)
        {
            window = mainWindow;
            shortcutControl = control;

            InitializeComponent();

            IconExtractorWindow_TitleBarControl.InitEvents(this);

            Loaded += (s, e) =>
            {
                Closing += OnWindowClose;
            };
        }

        private static IconExtractorWindow CurrentWindow { get; set; }

        private static CancellationTokenSource cancellationTokenSource;

        public static void CreateOrFocusInstance(MainWindow window, ShortcutControl control)
        {
            if (CurrentWindow != null)
            {
                if (ContainsWindow(Application.Current.Windows, CurrentWindow))
                {
                    CurrentWindow.Focus();
                    return;
                }
            }

            CurrentWindow = new IconExtractorWindow(window, control);
            CurrentWindow.Show();
        }

        public static bool FocusInstance()
        {
            if (CurrentWindow != null)
            {
                if (ContainsWindow(Application.Current.Windows, CurrentWindow))
                {
                    CurrentWindow.Focus();
                    return true;
                }
            }

            return false;
        }

        private static bool ContainsWindow(WindowCollection windows, Window myWindow)
        {
            foreach (Window w in windows)
            {
                if (w == myWindow)
                {
                    return true;
                }
            }

            return false;
        }

        private void OnWindowClose(object sender, CancelEventArgs e)
        {
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
            }
        }

        private async void SearchButtonPressed(object sender, RoutedEventArgs e)
        {
            if (!UrlInput.Text.StartsWith("http://") && !UrlInput.Text.StartsWith("https://"))
            {
                ErrorText.Visibility = Visibility.Visible;
                LoadingSpinner.Visibility = Visibility.Collapsed;
                return;
            }

            ErrorText.Visibility = Visibility.Collapsed;
            LoadingSpinner.Visibility = Visibility.Visible;

            ExtractionSettings settings = new ExtractionSettings()
            {
                SvgOnly = false,
                RecurseUri = true,
                RecurseHyperlinks = true,
                HyperlinkRecursionDepth = 3,
                LazyDownload = true,
                GetMetaTagImages = true,
                GetLinkTagImages = true,
                GetInlineBackgroundImages = true,
                UseCorsAnywhere = false,
                DisableValidityCheck = false,
            };

            settings.OnFoundImage += async (WebImage image) =>
            {
                try
                {
                    MagickImage img = await image.GetImageAsync();

                    if (img.Format != MagickFormat.Svg)
                    {
                        if (img.Width < 64 || img.Height < 64 || img.Width > 512 || img.Height > 512)
                        {
                            return;
                        }

                        var size = new MagickGeometry(256, 256);
                        size.IgnoreAspectRatio = false;
                        img.Resize(size);
                        MagickColor mc = new MagickColor(0, 0, 0, 0);
                        img.Transparent(mc);
                        img.Format = MagickFormat.Bmp;
                    }
                    else
                    {
                        Density de = new Density(256, 256);
                        MagickColor mc = new MagickColor(0, 0, 0, 0);
                        img.Transparent(mc);
                        img.Density = de;
                        img.Settings.TextAntiAlias = true;
                        img.Format = MagickFormat.Bmp;
                    }

                    byte[] bytes = img.ToByteArray();
                    BitmapImage bitmapImage = LoadImage(bytes);

                    ListBoxItem boxItem = new ListBoxItem();
                    Image imageControl = new Image();
                    imageControl.Width = 96;
                    imageControl.Height = 96;
                    imageControl.Source = bitmapImage;
                    boxItem.Content = imageControl;
                    boxItem.Selected += (s, args) =>
                    {
                        window.SelectedWebsiteImage(img, shortcutControl);
                        Close();
                    };
                    ImageParent.Items.Add(boxItem);
                }
                catch (Exception)
                {
                }
            };

            ImageParent.Items.Clear();

            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
            }

            numRunning++;
            cancellationTokenSource = new CancellationTokenSource();
            try
            {
                await Extractor.GetAllImages(UrlInput.Text.Trim(), settings, cancellationTokenSource.Token);
            } catch (Exception)
            {
            }

            numRunning--;
            if (numRunning == 0)
            {
                LoadingSpinner.Visibility = Visibility.Collapsed;
            }
        }

        private static BitmapImage LoadImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
            {
                return null;
            }

            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }

            image.Freeze();
            return image;
        }
    }
}

# Start Menu Manager

[![Donate](https://img.shields.io/badge/Donate-PayPal-green.svg)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=MLD56V6HQWCKU&source=url)

<img align="left" width="64" height="64" src="https://cdn.jam-es.com/img/start-menu-manager/icon.png">

Windows 10 App to improve you productivity with shortcuts. See the features below:  

:star: :star: :star: And if you like it ... please star it! :star: :star: :star:  

Introduction Video:

[<img width="640" height="360" src="https://cdn.jam-es.com/img/start-menu-manager/thumbnail.png">](https://youtu.be/22APfw-ZSxI)

### Add Shorcuts to Start Menu

The app lets you add any kind of shortcut to the Start Menu:  
- Website Shorcuts  
- Software Shorcuts  
- File Shorcuts  
- Folder Shorcuts  
- Shorcuts to run Commands/Scripts  
- 'Group' Shortcuts which open multiple things at the same time  


### Add Shortcuts Anywhere

Once created, those shorcuts can be moved or added elsewhere. They can be pinned to the Start Menu, added to the Taskbar, added to your Desktop, or put anywhere you like.  
  
### Fixes Windows 10 Bugs

Windows 10 Search is buggy and inconsistent. Sometimes you might type in the exact name of an app, but get redirected to Bing search results inside a web brower.  
  
Start Menu Manager fixes this. All shorctuts are treated like apps so they get priority in Windows 10 Search and appear at the top of the search results.  

### Additional Features

 - Clean modern Graphical Interface with light/dark themes.  
 - Want an icon for your shortcut? The app can extract images from your favorite websites to use as shortcut icons.  
 - Or provide custom icons as `.ico` files.  
 - Shortcuts can be saved to JSON format so you can copy them between devices.  
 - You can also generate the shortcuts from JSON using a terminal to avoid the GUI.
 - If you don't like it, uninstallation removes all shortcuts and leaves no 'junk' behind.  
 - No performance loss as no background processes are used. Apps will start with an almost unnoticable overhead.

## Screenshots

Click for full size.

[<img width="108" height="103" src="https://cdn.jam-es.com/img/start-menu-manager/screen1.png">](https://cdn.jam-es.com/img/start-menu-manager/screen1.png)

[<img width="129" height="83" src="https://cdn.jam-es.com/img/start-menu-manager/screen2.png">](https://cdn.jam-es.com/img/start-menu-manager/screen2.png)

[<img width="154" height="80" src="https://cdn.jam-es.com/img/start-menu-manager/screen3.png">](https://cdn.jam-es.com/img/start-menu-manager/screen3.png)

## Installation

Requires Windows 10 with recent updates.  

**Disclaimer:** The app needs to run with Administrator privileges, so it can access the directories to place the shortcuts. If you don't have Administrator privileges, then it won't work.

1. Go to the [GitHub Releases page](https://github.com/James231/Start-Menu-Manager/releases).  
2. Scroll Down and download the `.msi` file in the 'Assets' section.  
3. Run the installer to install the software.  

## How to Use

It should be intuitive from the app, but just in case ...  
  
1. Open the 'Start Menu Manager' app through the Start Menu.  
2. Click the 'Add Shortcut' button to begin creating a shorcut.  
3. Change the 'Shortcut Name'. This will be the name which appears in the Start Menu and Windows Search.  
4. Change the 'Shortcut Type' to the kind of shortcut you want to create. And fill in the details for that type. E.g. Set the 'Website Url' for Web shortcuts.  
5. Next to 'Icon' press 'Select from Website' to pick an icon from your favorite website.  
6. Create as many shortcuts as you want in the same way.  
7. Press 'Generate' to create the shortcuts. You'll see them added to the start menu under 'Recently Added'.  
  
**Optional:**  
8. In the Start Menu, right click on the shortcut to pin it, or add it to the Taskbar.  
9. Naviagate to the Start Menu folder through the App Settings and copy the shortcut to wherever you want it.  
  
**Note:** When generating the shorcuts, icons might not immediatley be displayed correctly in the Start Menu. This is a bug with Windows. One fix is to look in the 'Display' settings in the Windows 10 Settings App. Change the 'Scale and Layout' percentage, wait a few seconds, then change it back again.  

## Uninstall

Open the 'Apps & Features' page in the Windows 10 Settings app. Select 'Start Menu Manager' and select 'Uninstall'. Everything is removed, including the shorctuts you created with the tool.

## Building the App from Source

If you want to get your hands dirty you can build from the source code. Just clone the repository and open the solution (`.sln`) in Visual Studio 2019 or later.  
  
The `StartMenuManager.GUI` project contains the main WPF application. `StartMenuManager.Builder` is a console app which creates the shorcuts a from JSON file. `StartMenuManager.Runner` is a console app used to run the Shortcuts when they are clicked. There are a few other class libraries, a project which acts as an uninstallation step, and an Wix installer project.  

VS may prompt you to open as an administrator, since running the app requires those privileges.  
  
Once built, the various console apps (`.exe` files with `.dll`s) need to be put in the correct locations relative to each other. See the `Program Files` of an installed version of the app to see the correct setup.  
  
## License

This code is released under MIT license. This means you can use this for whatever you want. Modify, distribute, sell, fork, and use this as much as you like. Both for personal and commercial use. I hold no responsibility if anything goes wrong.  
  
If you use this, you don't need to refer to this repo, or give me any kind of credit but it would be appreciated. At least a :star: would be nice.  

It took a lot of work to make this available for free. If you are feeling more generous, perhaps you could consider donating?

[![paypal](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=MLD56V6HQWCKU&source=url)

## Contributing

Pull Requests are welcome. But, note that by creating a pull request you are giving me permission to merge your code and release it under the MIT license mentioned above. At no point will you be able to withdraw merged code from the repository, or change the license under which it has been made available.

## References

This wouldn't have been possible without ...

[Material Design In Xaml](http://materialdesigninxaml.net/) - The WPF styles used in this app.  
  
[AvalonEdit](http://avalonedit.net/) - The code editor WPF control used for the JSON editing in the app.  
  
[AvalonEditHighlightingThemes](https://github.com/Dirkster99/AvalonEditHighlightingThemes) - Implementation of Themes in AvalonEdit. Used for light/dark JSON editing themes.  

[FontAwesome.WPF](https://www.nuget.org/packages/FontAwesome.WPF/) - Only used for the loading spinner on the Icon Extractor page.
  
[Json.NET](https://www.newtonsoft.com/json) - JSON serializer.  

[JsonSubTypes](https://www.newtonsoft.com/json) - JSON SubType implementation for Json.NET.  

[Wix Toolset](https://wixtoolset.org/) - Used to create the `.msi` installer.

... and obvious credit to Microsoft for C#, WPF, .NET, and the best OS in existence :)

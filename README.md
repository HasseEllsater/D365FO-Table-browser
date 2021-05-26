# D365FO-Table-browser
Simple table browser for D365FO
This is a simple table browser for D365FO, the intent is to make it quick and easy to browse tables in D365FO whitout having to edit the urls all the time.

Feel free to download and contribute. The UI is based on MahApps.Metro (great library!) and ControlzEx components.

You will find some info on how to use it on my blog https://memyselfandax.wordpress.com/simple-d365fo-table-browser/

# Visual Studio 2019
This is built with visual studio 2019, use nuget to get the referenced librarys used by this app.
The solution also consist of a setup program that let you easily distribute the app within your project. To be able to load the setup program in Visual Studio the Extension "Microsoft Visual Studio Installer Project" must be installed first. 

If you download this code and wish to use it, remember to restore all the nuget packages

# No support
Use this with care, I have written this for my own use in my spare time, it is provided AS IS and it can break anytime... As I think this a useful tool, I want to help and share it with anybody who needs it.

# Installation
I have published a MSI file and a setup.exe file, it is important that you run the setup.exe to get all the dependencies. You will find it in the releases section https://github.com/HasseEllsater/D365FO-Table-browser/releases

# Known issues
There has been some issues with the serialization of data when adding new records to settings tables in the browser. I will investigate

/Hasse

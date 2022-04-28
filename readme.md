# Steam Profile Background Switch Script

Few things you have to do to use it:

1. Change the username and password in Program.cs file in line 35 and 40.
2. Get image url of each background in steam profile and add it to imageUrls string array.
3. Write whatever logic you want to tell program how to pick image for "selectedImage" variable
4. Run (Enter TFA, it will give you 1 min)

When you re-run the script, you'll notice that it won't prompt for TFA anymore, it because it would save the cookie required for authentication though cookie can expire, if that happens, you need to delete Cookie.txt.

Also if you update Firefox, you may need to update geckodriver from here: https://github.com/mozilla/geckodriver/releases

All of this is from Dotnet SDK tool.

To build and run: `dotnet run` within repo directory in the terminal.

This script was written in a whim in about 7 min. Use as you please, this script is licensed under MIT License and Geckodriver is licensed under Mozilla Public License 2.0: https://www.mozilla.org/en-US/MPL/2.0/

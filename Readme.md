This illustrates the issue with Visual Studio 2022 (`ITextTemplating` and `ITextTemplatingCallback`) described in the following issue:

https://developercommunity.visualstudio.com/t/MicrosoftVisualStudioTextTemplatingVS/10567518?port=1025&fsid=63ef71a3-6803-44c0-805f-b4a5a295b596&ref=native&refTime=1706269716754&refUserId=2a22f212-76ff-66af-8946-afb13e8cdc50

To reproduce:

1. Open `MySpecialPackage.sln`, build and install the resulting vsix file (which installs the code generator).

2. Open `MyProject.sln`, and transform `MyTemplate.tt`.

> Please check the comments in the 2 solutions.
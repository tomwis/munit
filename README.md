# MUnit

MUnit is unit test framework for mobile platforms.
If we want to run unit tests that include platform specific code, we need to do that with runner supported by given platform. 
For Android/iOS these take form of simple apps that are run on device or emulator, execute tests and present results inside the app.
Example of such runner can be https://github.com/tomwis/nunit.maui

Downside of such runners is that they are not very convenient to use and functionality is limited.

MUnit's goal is to resolve this and make running unit tests on platforms more convenient. 
To achieve that, it will only install agent on mobile device to run tests, but results will be handled in IDE by NUnit or other framework.
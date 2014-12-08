MealManager
===========

Meal Planner and Kitchen Assistant for CS 4911-B Fall 2014


Install
  Download the setup.exe from the Setup Folder
  Follow the Instructions in the installer
  Right now, the installer only installs the meal manager app. In the future, a small MySQL db and .NET 4.0 will be included.

Source
  Source is organized into projects, all included in one solution, titled Inventory.sln.  This solution can be loaded in Visual studio in order to debug more effectively. In order to run the debugger without error, .NET 4.0 and MYSQL Connector for ADO.NET need to be running on the computer, and the Inventory.WPF project needs to be set as the startup project. Lastly, a connection string warning may be thrown when connecting to the remote db, however, this is due to VS not acknowledging the sparation between front and back end in this architecture, and should not cause any behavioral issues in the application.
  



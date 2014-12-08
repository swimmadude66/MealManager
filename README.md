MealManager
===========

<h3>Install</h3>

  Download the setup.exe from the Setup Folder
  
  Follow the Instructions in the installer
  
  Right now, the installer only installs the meal manager app. In the future, a small MySQL db and .NET 4.0 will be included.

<h3>Source</h3>

  Source is organized into projects, all included in one solution, titled Inventory.sln.  This solution can be loaded in Visual studio in order to debug more effectively. In order to run the debugger without error, .NET 4.0 and MYSQL Connector for ADO.NET need to be running on the computer, and the Inventory.WPF project needs to be set as the startup project. Lastly, a connection string warning may be thrown when connecting to the remote db, however, this is due to VS not acknowledging the sparation between front and back end in this architecture, and should not cause any behavioral issues in the application.
  
<h3>Projects</h3>
  The application is divided into projects, each with a specific role in the stack
  
  <h5>WPF</h5>
    The WPF project holds all the front-end UI files and their respective code-behind. The XAML files hold the info for object layout in a control, while the C# code controls the data to/from the control. Many of these XAML/C# pairs are instantiated as objects inside a large control titled MainWindow. It represents the app's actual presence in the OS and is the top-level front-end element.  
  <h5>Models</h5>
    Models are data-carrying objects, used to pass relevant information up and down the stack without the need for mass refactoring on data changes.
  <h5>Tools</h5>
    Tools are widely used methods, available to all layers of the stack. These methods were consolidated off stack for ease of debugging.
  <h5>Managers</h5>
    Managers perform business logic on models. If no operations are needed on the models, they also serve to pass data between the Data layer and the front end.
  <h5>Factory</h5>
    Factories produce Managers and Data Access Objecs. The static factory is called anytime a manager or DAO is needed, in order to avoid the use of the "new" keyword as much as possible.  Using the factory allows for better scalability, since if a manager or DAO changes constructors, it only needs to be changed in the factory, rather than across the entire app.
  <h5>Data</h5>
    The Data project is the bridge between the application on the database, it handles all calls to the db through the Entity Framewrok, and maps SQL result objects to respective models.
  <h5>Interfaces</h5>
    Managers and DAOs all have Interfaces named with an "I" then the name of the implementing class. The Interface is yeat another scalability addition, allowing for drastic changes to the managers and DAOs, without any changes in the rest of the stack, so long as the method signatures are the same.
  <h5>Tests</h5>
    The final project is the Test Suite. Every completed task has required passing unit tests to be accepted in to master, and those tests are held here.

# Pawnshop
WPF .NET application for pawnshop as part of an engineering thesis

# Main Functions
<li>Creating,Renewing,Ending,Searching Contracts</li>
<li>Printing contract based on fillable PDF using Adobe PDF Reader</li>
<li>Creating,Editing,Searching Clients</li>
<li>Selling,Searching Items</li>
<li>Managing Workers - Creating,Editing,Deleting,Privileges</li>
<li>Setting Pawnshop settings, Lending Rates, Application Theme</li>
<li>Currency,Gold,Sell,Renew Charts</li>
<li>Login/Logout system</li>

# Installation Instruction
1. Download and run setup msi from Release. It will install Pawnshop application and .Net 5.0.13 Desktop runtime if it is not on the computer.
2. Application uses Adobe PDF Reader to printing deal documents. Make sure it is installed on the computer and set as a deafult application for .pdf files.
5. In application folder you will find DB backup with tables,schemas, indexes and one Worker.
6. Restore backup with Managament Studio or another software to the sql server.
7. Add environmental variable with name Iterations and value 1000. (Iterations for hashing algorithm)
8. Add environmental variable with name Pepper and value XXXXX.
9. Set the correct connection string (PawnShopDatabaseProduction) in App.config file in the application folder.
10. First launch the application has to be with administrator privileges. (To set a value in app config). At the first lunch app will create user settings file, folder in documents for printed deal documents and will set path to that folder and to the DealDocument Fillable pdf which you will find in the application folder. (I couldn't manage to do this in setup, I was getting dll errors)
11. Login to the app with ID:admin and PW:admin, change the Pepper and Iterations environment variable according to your preferences and go to the Worker tab in application and set the new password.
12. App is ready to use.

# Screenshots
[![image.png](https://i.postimg.cc/4yfZgy9L/image.png)](https://postimg.cc/mz5K3bVC)
[![image.png](https://i.postimg.cc/mrSB67cz/image.png)](https://postimg.cc/GB9W48xd)
[![image.png](https://i.postimg.cc/GpYtqBR6/image.png)](https://postimg.cc/sBj32gvJ)
[![image.png](https://i.postimg.cc/MHhK7x4z/image.png)](https://postimg.cc/ZvxSZk4Q)
[![image.png](https://i.postimg.cc/Cxnrr0QW/image.png)](https://postimg.cc/RW9Tqk9R)

<p>
<br>
Very short description for now.
Readme to be contuined..







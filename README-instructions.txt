Instructions:

- Extract the zip file.
- Open the folder and find "COLLATEFINAL.sln" then open it in visual studio.
- Open the "Microsoft SQL Server Management Studio" to connect the tables in the database server.

HOW TO CONNECT?
- Inside SSMS, choose the available server name, then click 'connect'. (Don't forget to copy the server name)
- Inside the visual studio, find the solution explorer tab then check for the filename "appsettings.json"
- Look for "Server=(PASTE YOUR SERVER NAME HERE)" then paste the server name you copied a while ago.
- Save the file.
- On the Top Corner, look for "Tools", under tools, look for nuGet Package Manager, and go to "Package Manager Console".
- enter this commnand "update-database"
- It will prompt "Done", and its good to go!
# Store API coding challenge
Hello there

Ok, this is how this small demo work, you might find some commented code, those where ideas I wanted to implement based on my experience with Retail software that I worked on my early years, but time did not let me finish the implementation and I decided to stick to de basic of the challenge.

SETTING UP
----------------------------------------------------------------------------------
I recommend to open the source code in Visual studio 2019, Since I created it there, I haven't try to run in on Visual Studio Code.

so please follow these steps

1. in the NuGet console (Package Manager Console) run: Add-Migration InitialCreate
2. in the NuGet console (Package Manager Console) run: Update-Database (his step might take some time)
    this will create and populate the database with some data when it's done, you should have the database ready

3. run the program.

FIRST RUN 
----------------------------------------------------------------------------------
Note for postman: for some reason, you need to turn off 'SSL certificate verification' in Settings > General
is the first time I use Postman, SoapUI, the one I normally use did not have that problem.

the API will launch opening the auto-generated Documentation located in the following URL witch it might change depending on the computer you are running this API Source:

https://localhost:44302/docs/

1. sadly you will have to create the customer since this software use JWT Beared tokens for authentication :) (my bad), to do let create the admin user.
    https://localhost:44302/api/Register
    {
        "email":"admin@admin.com",
        "password":"12345678"
    }

    the API will detect the "admin@" string and it will add the user to the administrator role, I know this is not the ideal solution, but that was the simple way to add an administrator user in short notice, if the user does not have that string It will become a Customer, by the way, the password as has to be 8 characters long.

LOGIN/ BEARER TOKEN
----------------------------------------------------------------------------------
Since you have created to users, Admin and the rest of the customers lets login to the API to get the JTW Beared token you will have to use to validate or authenticate the rest of the API.

1. load the Login API
    https://localhost:44302/api/login
    {
        "username": "admin@admin.com",
        "password":"12345678"
    }
    
    this will return the token and the expiration date of it similar to this
    
    {
        "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJhZG1pbkBhZG1pbi5jb20iLCJleHAiOjE1NjYwNjM5MTIsImlzcyI6Imh0dHA6Ly93d3cuR2Vla29zUGxheS5jb20iLCJhdWQiOiJodHRwOi8vd3d3LkdlZWtvc1BsYXkuY29tIn0.VZcx1G0d0oRmRVXXKPjGK2NyK9EUmsvYYvEmZI1U1OA",
        "expiration": "2019-08-17T17:45:12Z"
    }

Now that you have your users and token is time to have fun with the API.
----------------------------------------------------------------------------------

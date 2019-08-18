Hello there

Ok this is how this small demo work, you might find some commented code, those where ideas I wanted to implement based on my experience with Retail software that I worked on my early years, but time did not let me finish the implementation and I decided to stick to de basic of the challenge.

SETTING UP
----------------------------------------------------------------------------------
I recommend to open the source code in Visual studio 2019, Since I created it there, I haven't try to run in on Visual Studio Code.

so please follow this steps

1. in the nuget console (Package Manager Console) run: Add-Migration InitialCreate
2. in the nuget console (Package Manager Console) run: Update-Database (his step might take some time)
	this will create and populate the database with some data, when is done, you should have the database ready

3. run the program.

FIRST RUN 
----------------------------------------------------------------------------------
Note for postman: for some reason , you need to turn off 'SSL certificate verification' in Settings > General
is the first time I use Postman, SoapUI, the one I normally use did not have that problem.

the API will launch opening the auto-genetated Documentation located in the folowing URL witch it might change depending on the computer you are running this API Source:

https://localhost:44302/docs/

1. sadly you will have to create the customer since this software use JWT Beared tokens for authentication :) (my bad), to do let create the admin user.
	https://localhost:44302/api/Register
	{
		"email":"admin@admin.com",
		"password":"12345678"
	}

	the API will detect the "admin@" string and it will add the user to the administrator role, I know this is not the Ideal solution, but that was the simple way to add and admin int this shot notice, if the user does not have that string I will become a Customer, by the way the password as has to be 8 character long.

LOGIN/ BEARER TOKEN
----------------------------------------------------------------------------------
Since you have created to users, Admin and the rest of the customers lets login to the API to get the JTW Beared token you will have to use to valite the resto of the API

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

now that you have you users and token, is time to have fun with the API to validate the folowing

REMARKS
----------------------------------------------------------------------------------
The price can be changed jus sending the new price with out sending the rest of the product 

{
   "productPrice": "300.00"
}





	# Store API coding challenge
	This challenge is designed to put your skills to the test by designing and building a good REST API to manage a small snacks store using .net Core.
	## Requirements
	The API should allow:
	* Adding/Removing products and set their stock quantity.
	* Modify the price of the products
	* Save a log of the price updates for a product.
	* Buy a product
	* Buying a product should reduce its stock.
	* Keep a log of all the purchases (who bought it, how many, when).
	* Liking a product
	* Obtain a list of all the available products.
	* The list should be sortable by name (default), and by popularity (likes) 
	* The list should have pagination functionality
	* Search through the products by name.
	## Security requirements
	* Add login functionality so: 
	* Only admins can Add/remove products.
	* Only admins can modify price of the products.
	* Only logged in users can buy a product.
	* Any logged in users could *like* a product.
	* Everyone (logged in or not logged in) can get a list of all the products.
	* Everyone (logged in or not logged in) can use the search feature.
	### Extra credit
	* Build a small front-end application that:
		  * Connects to the API to show a list of the products
		  * The app should allow searching products by name 
	### Keep in mind
	*  You are free to use any package, framework, library and weapons for the battle as long as you can justify their use.
	* You may use any kind of database you like but you need to use Code-First approach.
	* Provide a database dump so we can replicate the database locally.
	* POSTMAN will be used to evaluate the API. It would be great if you can provide us with a collection to test your API
	* Follow best Rest API practices.
	* Use git and do small commits to facilitate the evaluation of progress.
	* Include a *readme.md* file with instructions on how to setup your project locally to test it. (This is super important, if we cannot install it and run it easily we cannot evaluate it)
	* Upload your solution to your GitHub account and share a link with your evaluator
	* The test has been designed with enough time to do a good job, so don’t cut any corners, take your time and watch for quality
	### Time to complete: 5 days (Due Date: Monday 19, 2019)
	Looking forward to seeing your code
	Good luck!
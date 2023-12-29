![Thumbnail GitHub](E:\Desenvolvimento\Projetos\StreamberryMoviesApi\Img\StreamBerryBanner.png)

## StreamBerry API

StreamBerryAPI is a C# API created using ASP.NET and MySql. Through it, it is possible to CRUD users, films, streaming services, link films to streaming services and carry out evaluations on films.

The API also has some specific endpoints to obtain data that can be used for new film productions.

## 🔨 Project features

The API has endpoints to input and retrieve user-related data. This documentation will only contain a superficial overview of the API's functionalities. 

Call examples can be found through Swagger.

#### 1) MOVIES:  `/MOVIE`
- `POST`: Creates a new movie in the database.
- `DEL`: Deletes a movie from the movie data table. Requires movie ID to run.
- `PUT`: Completely updates a movie. Requires movie ID to run.
- `PATCH`: Updates specific properties of a movie by ID.
- `GET`: Search movies. There are three options:
	1. `/Movie`: Retrieves a list of movies with optional pagination.
	2. `/Movie/{id}`: Retrieves a specific movie by ID.
	3. `/Movie/genre/{movieGenre}`: Retrieves a list of films by genre with optional pagination.


#### 2) STREAMING PLATFORM: `/StreamingPlatform`
- `POST`: Adds a new streaming platform to the database.
- `DEL`: Deletes a streaming platform by ID.
- `PUT`: Updates a streaming platform by ID.
- `GET`: Search streaming platforms. There are two options:
	1. `/StreamingPlatform`: Retrieves a list of streaming platforms with optional pagination.
	2. `/Movie/{id}`: Retrieves a specific streaming platform by ID.

#### 3) ASSOCIATE A MOVIE TO STREAMING PLATFORMS `/MovieStreamingAssociation`
- `POST`: Adds a new movie streaming association to the database.
- `DEL`: Deletes a movie streaming association by ID.
- `GET`: Search associations between movies and streaming platforms. There are two options:
	1. `/MovieStreamingAssociation`: Retrieves a list of movie streaming associations with optional pagination.
	2. `/MovieStreamingAssociation/{movieId}/{streamingServiceId}`: Retrieves a specific movie streaming association by ID.

#### 4) USER `/User`
- `POST`: There is two types:
	1. `/User/register`: Adds a new user to the database.
	2. `/User/login`: Logs in to the API and returns a JWT token.
- `GET`: Search users in database. There are two options:
	1. `/User/GetAll`: Retrieves a list of users with optional pagination.
	2. `/User/GetById`: Retrieves a specific user by ID.
- `DEL`: Deletes a user by ID. Deletes a user by ID. It can only be performed by users authenticated (logged in) to the system. The bearer token provided in the *`POST`* of *`Login`* is required

#### 5) RATING: `/Rating`
`All RATING operations require the user to be authenticated. It is necessary to inform the bearer token generated in USER/LOGIN`
- `POST`: Adds a new rating to the database.
- `DEL`: Deletes a rating by ID.
- `PUT`: Updates a rating by ID.
- `PATCH`: Updates specific properties of a rating by ID.
- `GET`: Search ratings. There are two options:
	1. `/Rating`: Retrieves a list of ratings with optional pagination.
	2. `/Rating/{id}`: Retrieves a specific rating by ID.

#### 5) STATS: `/Stats/`
All this operations are GETs and inform determinated informations:
- `GET`: Search ratings. There are two options:
	1. `/Stats/streams-by-movie`: Gets the number of streaming services available for a specific movie.
	2. `/Stats/average-rating-of-all-movies`: Gets the average rating of all movies.
	3. `/Stats/movies-per-year`: Gets the number of movies released per year.
	4. `/Stats/movies-per-rate-and-or-commentary`: Gets movies based on their rating, commentary, or both.
	5. `/Stats/average-rating-per-genre-and-per-year`: Gets the average rating per genre and per release year.

#### Validates JWT token
- Each JWT token generated has 10 minutes of validity. After that, you need to generate a new token.
- `Endpoint`: `https://localhost:7287/access`

---

## 🛠️ Open and run the project

#### User-Secrets Configuration
* To run the API in a testing environment, it is necessary to install and configure the MySql database.
You must enter the connection string and encryption key in the “secrets.json” file present in the *“AppData\Roaming\Microsoft\UserSecrets”* folder.

* `Creation of 'secret.json':`
```powershell
dotnet user-secrets init
```
* `Addition of 'SymmetricSecurityKey' variable:`
```powershell
dotnet user-secrets set "SymmetricSecurityKey" "1234567890123456789abcdefhijklmnopqrstuvxwyz"
```
* `Addition of variable 'ConnectionStrings:UserConnection':`
```powershell
dotnet user-secrets set "ConnectionStrings:UserConnection": "server=localhost;database=userdb;user=root;password=root"
```

* It is important to check that the "UserSecretsId" in CSPROJ is the same as the name of the folder that will be generated in *“AppData\Roaming\Microsoft\UserSecrets”*.

#### Database structure generation
* It is possible to use *Migrations* to generate the database structure. To do this, you need to follow these steps:
	1. With the project open, you will need to open the NuGet Console;			
	2. After, run the *`Update-Database`* command and the database structure should be created.

`It is important that the connection string and *User-Secret* have been set correctly for this to work correctly.rectly configurated to this works properly.`
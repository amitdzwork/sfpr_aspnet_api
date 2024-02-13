
# sfpr_aspnet_api
#WebAPI
sfpr_aspnet_api is an Asp.net Core Web API written in C# that interacts with a MongoDB database to query information about facilities maintained by the SF Recreation and Parks Department. 
The dataset is provided by DataSF and contains details about structural or physical amenities within property boundaries.

## Installation

Make sure you have the following prerequisites installed before setting up the project:

- [.NET SDK](https://dotnet.microsoft.com/download)
- [MongoDB](https://www.mongodb.com/try/download/community)

### Steps

1. Clone the repository:

    ```bash
    git clone https://github.com/amitdzwork/sfpr_aspnet_api.git
    cd sfpr_aspnet_api
    ```

2. Restore the dependencies and build the project:

    ```bash
    dotnet restore
    dotnet build
    ```

3. Configure MongoDB Connection:

    Update the MongoDB connection string in `appsettings.json` with your MongoDB server details.

4. Run the application:

    ```bash
    dotnet run
    ```

The API will be accessible at `http://localhost:4001` by default.

## Usage
https://localhost:4001/api/facilities/type/Administration%20Building

### Endpoints

- **GET /api/facilities:** Get a list of all facilities.
- **GET /api/facilities/{facilitytype}:** Get all facilities of a specific facilityType.
- **POST /api/facilities:** Create a new facility.
- **PUT /api/facilities/{facilityname}:** Update details of a specific facility by Name.
- **DELETE /api/facilities/{facilityname}:** Delete a facility by Name/


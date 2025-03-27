# Upload Image Manipulation APIs

This project provides APIs for image manipulation (Add/Delete/GetAll) using .NET 8, EF Core 8, and MSSQL.

## Tech Stack
- .NET Core APIs (.NET 8)
- Entity Framework Core 8
- MSSQL

## Image Path
Images are stored in the following format:
```
BaseUrl/Uploads/image_name.extension
```

## API Endpoints

### Get All Products
**Endpoint:** `GET api/Product`

**Response:**
```json
[
  {"id":1,"productName":"Product 1","productImage":"https://localhost:7084/Uploads/20-03-2025_17-39_5b7c9e4d-3324-4f71-9b4c-a938d7edafde.jpg"},
  {"id":2,"productName":"Product 2","productImage":"https://localhost:7084/Uploads/20-03-2025_17-39_b60b6b24-8c6a-44e6-aebc-00df60f9f40f.jpg"}
]
```

### Get Product by ID
**Endpoint:** `GET api/Product/{id}`

**Response:**
```json
{
  "id": 3,
  "productName": "Product 31",
  "productImage": "https://localhost:7084/Uploads/20-03-2025_17-39_hdfsubdsb24-8c6a-44e6-aebc-00df60f9f40f.jpg"
}
```

### Add Product
**Endpoint:** `POST api/Product`

**Content Type:** `multipart/form-data`

**Request Body:**
- `ProductName`: STRING | REQUIRED | MaxLength: 30
- `ImageFile`: FILE | REQUIRED | MaxSize: 1MB (Allowed file types: .jpg, .jpeg, .png)

**Response:**
```json
{
  "id": 1,
  "productName": "Product 1",
  "productImage": "https://localhost:7084/Uploads/20-03-2025_17-39_5b7c9e4d-3324-4f71-9b4c-a938d7edafde.jpg"
}
```



### Delete Product
**Endpoint:** `DELETE api/Product/{id}`

**Response:** 
```json
{
  "id": 1,
  "productName": "Product 1",
  "productImage": "https://localhost:7084/Uploads/20-03-2025_17-39_5b7c9e4d-3324-4f71-9b4c-a938d7edafde.jpg"
}
```

---

## How to Run This Project?

### Prerequisites
Ensure you have the following installed:
- **Visual Studio 2022** (Latest as of March 2024)
- **Microsoft SQL Server Management Studio (SSMS)** (Using MSSQL Server 2022)

### Steps to Run
1. Open the command prompt and navigate to the directory where you want to clone the project.
2. Clone the repository:
   ```sh
   git clone https://github.com/krishbadhiwala/Upload_Image_.git
   ```

3. Open the `appsettings.json` file and update the connection string:
   ```json
   "ConnectionStrings": {
   "DefaultConnection": "Server=Your_Server_name;DatabaseYour_Database_Name;Trusted_Connection=True;TrustServerCertificate=True"
   }
   ```
5. Install three packages
```
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
```



	
6. Delete the `Migrations` folder.
   
7. Open **Tools > Package Manager > Package Manager Console**.
   
8. Run the following commands:
   ```sh
   add-migration init
   update-database
   ```
   
9. Now, you can run the project.



## Note

Repository & Service Pattern

Repository ```(ProductRepository.cs)```

- Handles data access logic.
  
Service ```(FileService.cs)```

- Contains File Save And Delete logic and calls the repository.

---

Thank you for ⭐ and supporting this project! 😅

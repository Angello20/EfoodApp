# eFood Commerce

This project is an e-commerce platform developed in ASP.NET Core and C#. The goal of this project is to provide an online food shopping solution with a smooth user experience and comprehensive functionalities.

## Requirements

- .NET 5.0 SDK or higher.
- SQL Server.
- Visual Studio 2019 or higher (optional for development).

## Getting Started

1. **Clone the Repository**

   ```bash
   git clone https://github.com/Angello20/EfoodApp.git
   cd EfoodApp
   ```

2. **Navigate to the Project Directory**

   ```bash
   cd EfoodApp
   ```

3. **Configure the Database**

   - Ensure you have SQL Server installed and running.
   - Update the connection string in `appsettings.json` with your SQL Server configuration.

4. **Run Migrations**

   ```bash
   dotnet ef database update
   ```

5. **Run the Application**

   ```bash
   dotnet run
   ```

6. **Access the Application**

   - Open your web browser and go to `http://localhost:5078/`.

## Features

- **Authentication and Authorization**: User registration, login, and profile management.
- **Product Management**: Create, edit, delete, and list products.
- **Shopping Cart**: Add products to the cart and manage orders.
- **Product Search**: Search for products by name, category, and price.
- **Order History**: View order history and current order status.

## Technologies Used

- **Backend**: ASP.NET Core 5.0, C#
- **Frontend**: HTML5, CSS3, JavaScript
- **Database**: SQL Server
- **Authentication**: ASP.NET Identity

## Results

- Main view of the application with featured products.


![Main View](Images/EfoodAppImg1.PNG)

- Adding a product with its quantity.


![Main View](Images/EfoodAppImg2.PNG)












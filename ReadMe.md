# **🌱 GrowthPulse**

**GrowthPulse is a complete web-based marketplace application designed for buying and selling plants. It provides a seamless experience for customers to browse products and manage their collections, while offering robust tools for administrators to manage inventory, sales, and users.**

## **\#\# Core Features**

**The application is built with a role-based access control system, providing experiences for different user types.**

### **🧑‍💼 Admin & Manager Features**

* **Product Management: Full CRUD (Create, Read, Update, Delete) functionality for marketplace listings.**  
* **Inventory Control: Easily update stock quantities for each product.**  
* **"Sell This Plant" Feature: Quickly create a new marketplace listing from an existing plant in the system's collection.**

### **🧑 Customer Features**

* **User Authentication: Secure registration and login system.**  
* **Marketplace: Browse all available plants, with clear pricing, images, and stock levels.**  
* **"Cart" Functionality: Add to cart any quantity of an in-stock plant directly from the marketplace and buy from cart.**  
* **Order History: Customers can view a complete history of all their past orders and see detailed receipts.**  
* **Plant Gallery: View a gallery of all plants being tracked within the GrowthPulse community.**

---

## **\#\# Technology Stack 🛠️**

* **Backend: ASP.NET Core MVC (.NET)**  
* **Database: Microsoft SQL Server**  
* **Object-Relational Mapper (ORM): Entity Framework Core**  
* **Authentication: ASP.NET Core Cookie Authentication**  
* **Frontend: HTML, CSS, JavaScript, Bootstrap 5**  
* **Architecture: Follows the Model-View-Controller (MVC) pattern with a Repository and Unit of Work design pattern for data access.**

---

## **\#\# Setup & Installation 🚀**

**To get a local copy up and running, follow these simple steps.**

**Clone the repository:**  
**Bash**  
1. **git clone https://github.com/your-username/GrowthPulse.git**
2. **Configure the Database Connection:**  
   * **Open the `appsettings.json` file.**  
   * **Modify the `DefaultConnection` string to point to your local SQL Server instance.**  
3. **Apply Migrations:**  
   * **Open the Package Manager Console in Visual Studio.**
    **Run the following command to create the database schema:**  
    **Bash**  
    **dotnet ef database update**  
4. **Run the Application:**
  **Build and run the project from Visual Studio (press `F5`) or use the .NET CLI:**  
  **Bash**  
  **dotnet run**

---

## **\#\# Usage Workflow**

1. **Register the First User: The very first user to register will automatically be assigned the Admin role.**  
2. **Log in as Admin: Use the Admin account to create new Manager accounts or to start adding products to the marketplace via the Listing Controller.**  
3. **Register as a Customer: Create a new user account, which will be assigned the Customer role.**  
4. **Shop: Log in as the Customer, browse the marketplace, and purchase a plant.After purchase, the customer will be redirected to their "Market Place”.**

**Contributions:-**  
**Meet Chaudhari:** Developed the core backend modules for Plant and Account management. This includes user roles, authentication (registration and login), and the foundational CRUD operations for plants.

**Vismay Chaudhari:** Implemented the  e-commerce workflow. This covers the product marketplace, the Buy order processing system, and the customer's order history page.

**Pushpraj Adajaniya:** Handled the entire front-end and user experience. This includes all CSS styling, the 'Add to Cart' functionality, and the development of the shopping cart and Listing  feature.  

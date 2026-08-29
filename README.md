# Full Stack Fashion Store

A full-stack e-commerce application for a fashion store built with modern web technologies :)
The application features a Vue 3 frontend with a robust ASP.NET Core backend, complete with product management, user authentication, shopping cart functionality, and admin controls.

## 🌟 Features

- **User Authentication & Authorization**
  - JWT-based authentication
  - Role-based access control (Admin, User)
  - Secure login/registration

- **Product Management**
  - Browse products with detailed information
  - Filter products by category
  - Product reviews and comments
  - Admin product management panel

- **Categories**
  - Organize products by categories
  - Admin category manager
  - Dynamic category filtering

- **Shopping Cart**
  - Add/remove items from cart
  - Persistent cart management
  - Cart checkout functionality

- **User Management**
  - User profile management
  - Role assignment system
  - Admin user management panel

- **Admin Dashboard**
  - Product administration
  - Category management
  - User role management
  - Comment/review moderation

## 🛠️ Tech Stack

### Frontend
- **Framework:** Vue 3
- **State Management:** Pinia
- **Authentication:** JWT with jwt-decode
- **Language:** JavaScript/TypeScript
- **Node.js:** v20.19.0 or >=22.12.0

### Backend
- **Framework:** ASP.NET Core 8.0
- **Database:** SQL Server
- **ORM:** Entity Framework Core 9.0
- **Authentication:** JWT Bearer
- **API Documentation:** Swagger/OpenAPI
- **Language:** C#

<img width="1035" height="812" alt="main" src="https://github.com/user-attachments/assets/956b9ffc-c7cc-4ff8-8ba9-a155811798d1" />
<img width="1070" height="742" alt="panel1" src="https://github.com/user-attachments/assets/78415295-05ac-4dab-a8f2-32b650e803bd" />
<img width="1317" height="702" alt="login" src="https://github.com/user-attachments/assets/e00bd5e9-8c35-46d0-b0fe-34ca17ce110b" />
<img width="1037" height="557" alt="koszyk" src="https://github.com/user-attachments/assets/fbb1ca7c-b619-4848-890e-53657b1c281c" />
<img width="1077" height="686" alt="panel2" src="https://github.com/user-attachments/assets/9441d8ba-9b4e-4fa3-b3d4-929f16472060" />
<img width="1252" height="857" alt="product" src="https://github.com/user-attachments/assets/4a16df9c-fdb3-4f07-80e8-fe0092687381" />


## 📦 Project Structure

```
FullStack-Fashion-Store/
├── client/                          # Frontend (Vue 3)
│   ├── src/
│   │   ├── components/             # Vue components
│   │   │   ├── AdminCategoryManager.vue
│   │   │   ├── AdminProductForm.vue
│   │   │   ├── AdminRoleManager.vue
│   │   │   ├── AuthForm.vue
│   │   │   ├── CartItemCard.vue
│   │   │   ├── Modal.vue
│   │   │   ├── navbar.vue
│   │   │   ├── ProductAdminPanel.vue
│   │   │   ├── ProductCard.vue
│   │   │   ├── ProductInfoDetails.vue
│   │   │   └── UserProfile.vue
│   │   ├── DTOs/                   # Data transfer objects
│   │   ├── mappers/                # Data mappers
│   │   ├── router/                 # Vue Router configuration
│   │   ├── services/               # API services
│   │   ├── store/                  # Pinia store
│   │   ├── styles/                 # Global styles
│   │   ├── App.vue
│   │   └── main.js
│   ├── package.json
│   ├── vite.config.js
│   └── index.html
│
└── server/                          # Backend (ASP.NET Core)
    ├── Program.cs                  # Application entry point
    ├── Data/
    │   └── DataContext.cs          # EF Core DbContext
    ├── DTOs/                       # Data transfer objects
    ├── Endpoints/                  # API endpoints
    │   ├── AuthEndpoints.cs
    │   ├── CategoryEndpoints.cs
    │   ├── CommentEndpoints.cs
    │   ├── ProductEndpoints.cs
    │   └── UserEndpoints.cs
    ├── Mappers/                    # Entity mappers
    ├── Migrations/                 # EF Core migrations
    ├── Models/                     # Database models
    │   ├── Comment.cs
    │   ├── Product.cs
    │   ├── ProductCategory.cs
    │   ├── Role.cs
    │   └── User.cs
    ├── server.csproj              # Project configuration
    └── appsettings.json           # Configuration file
```

## 🚀 Getting Started

### Prerequisites
- **Node.js:** v20.19.0 or >=22.12.0
- **.NET SDK:** 8.0 or higher
- **SQL Server:** 2019 or higher (or use LocalDB)

### Backend Setup

1. Navigate to the server directory:
   ```bash
   cd server
   ```

2. Install dependencies:
   ```bash
   dotnet restore
   ```

3. Configure the database connection string in `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=.;Database=FashionStore;Trusted_Connection=true;TrustServerCertificate=true;"
     }
   }
   ```

4. Apply database migrations:
   ```bash
   dotnet ef database update
   ```

5. Run the server:
   ```bash
   dotnet run
   ```

   The API will be available at `http://localhost:5000` (check `launchSettings.json` for actual port).
   Swagger documentation: `http://localhost:5000/swagger`

### Frontend Setup

1. Navigate to the client directory:
   ```bash
   cd client
   ```

2. Install dependencies:
   ```bash
   npm install
   ```

3. Configure API endpoint (if needed) in your services or `.env`:
   ```
   VITE_API_URL=http://localhost:5000
   ```

4. Start the development server:
   ```bash
   npm run dev
   ```

   The application will be available at `http://localhost:5173`


## 📚 API Endpoints

The backend provides REST endpoints for:

- **Authentication** (`/api/auth`)
  - POST `/login` - User login
  - POST `/register` - User registration

- **Products** (`/api/products`)
  - GET `/` - Get all products
  - GET `/{id}` - Get product details
  - POST `/` - Create product (Admin)
  - PUT `/{id}` - Update product (Admin)
  - DELETE `/{id}` - Delete product (Admin)

- **Categories** (`/api/categories`)
  - GET `/` - Get all categories
  - POST `/` - Create category (Admin)
  - PUT `/{id}` - Update category (Admin)
  - DELETE `/{id}` - Delete category (Admin)

- **Users** (`/api/users`)
  - GET `/{id}` - Get user profile
  - PUT `/{id}` - Update user profile
  - POST `/assign-role` - Assign role to user (Admin)

- **Comments** (`/api/comments`)
  - GET `/product/{productId}` - Get product comments
  - POST `/` - Create comment
  - DELETE `/{id}` - Delete comment

For complete API documentation, visit the Swagger UI at `/swagger` when the server is running.

## 🔐 Authentication

The application uses JWT (JSON Web Tokens) for authentication:

1. User logs in with credentials
2. Server validates credentials and returns a JWT token
3. Client stores the token (typically in localStorage)
4. Token is sent with each request in the `Authorization` header
5. Server validates the token for protected endpoints

Role-based authorization controls access to admin features.


# 🎨 MTKPM E-Commerce Frontend Design Brief

**Version:** 1.0  
**Date:** 2024  
**Project:** MTKPM E-Commerce Platform  
**Tech Stack:** React 18 + TypeScript + Vite + TailwindCSS  
**Target:** Full-stack e-commerce system with admin & user interfaces

---

## 📑 Table of Contents

1. [System Overview](#system-overview)
2. [Project Structure](#project-structure)
3. [Technology Stack](#technology-stack)
4. [API Integration Guide](#api-integration-guide)
5. [User Roles & Permissions](#user-roles--permissions)
6. [Page Requirements](#page-requirements)
7. [Component Architecture](#component-architecture)
8. [State Management](#state-management)
9. [Authentication Flow](#authentication-flow)
10. [Design System](#design-system)
11. [File Structure & Naming Conventions](#file-structure--naming-conventions)
12. [Development Workflow](#development-workflow)

---

## System Overview

### 🎯 Project Goals
- E-commerce platform with user shopping experience
- Admin dashboard for business management
- Real-time cart management
- Multiple payment methods
- Discount/pricing strategies
- User favorite system
- Order tracking

### 👥 User Types
1. **Anonymous Users** - Can browse products, view categories
2. **Registered Users** - Can shop, manage cart, checkout, track orders
3. **Admin Users** - Can manage products, categories, orders, users

### 💡 Key Features
- ✅ Product catalog with pagination & search
- ✅ Shopping cart with discount calculation
- ✅ Multiple payment methods (Credit Card, Bank Transfer, COD)
- ✅ Order management
- ✅ User authentication & profile
- ✅ Favorite products
- ✅ Admin dashboard
- ✅ Real-time notifications
- ✅ Responsive design

---

## Project Structure

```
mtkpm-frontend/
├── src/
│   ├── components/
│   │   ├── common/           # Reusable components
│   │   │   ├── Header.tsx
│   │   │   ├── Footer.tsx
│   │   │   ├── Navbar.tsx
│   │   │   ├── Sidebar.tsx
│   │   │   ├── Modal.tsx
│   │   │   ├── Toast.tsx
│   │   │   ├── LoadingSpinner.tsx
│   │   │   ├── Pagination.tsx
│   │   │   ├── Badge.tsx
│   │   │   └── Breadcrumb.tsx
│   │   ├── product/
│   │   │   ├── ProductCard.tsx
│   │   │   ├── ProductGrid.tsx
│   │   │   ├── ProductFilter.tsx
│   │   │   └── ProductImage.tsx
│   │   ├── cart/
│   │   │   ├── CartItem.tsx
│   │   │   ├── CartSummary.tsx
│   │   │   └── CartEmpty.tsx
│   │   ├── order/
│   │   │   ├── OrderCard.tsx
│   │   │   ├── OrderTimeline.tsx
│   │   │   └── OrderDetails.tsx
│   │   ├── auth/
│   │   │   ├── LoginForm.tsx
│   │   │   ├── RegisterForm.tsx
│   │   │   └── AuthGuard.tsx
│   │   └── admin/
│   │       ├── DataTable.tsx
│   │       ├── StatCard.tsx
│   │       └── Chart.tsx
│   ├── pages/
│   │   ├── auth/
│   │   │   ├── Login.tsx
│   │   │   ├── Register.tsx
│   │   │   └── ForgotPassword.tsx
│   │   ├── user/
│   │   │   ├── Home.tsx
│   │   │   ├── Products.tsx
│   │   │   ├── ProductDetail.tsx
│   │   │   ├── Cart.tsx
│   │   │   ├── Checkout.tsx
│   │   │   ├── Orders.tsx
│   │   │   ├── OrderDetail.tsx
│   │   │   ├── Profile.tsx
│   │   │   └── Favorites.tsx
│   │   ├── admin/
│   │   │   ├── Dashboard.tsx
│   │   │   ├── Products.tsx
│   │   │   ├── ProductForm.tsx
│   │   │   ├── Categories.tsx
│   │   │   ├── CategoryForm.tsx
│   │   │   ├── Orders.tsx
│   │   │   ├── OrderDetail.tsx
│   │   │   ├── Users.tsx
│   │   │   └── Analytics.tsx
│   │   └── NotFound.tsx
│   ├── hooks/
│   │   ├── useAuth.ts
│   │   ├── useProducts.ts
│   │   ├── useCart.ts
│   │   ├── useOrders.ts
│   │   ├── useUser.ts
│   │   ├── useFetch.ts
│   │   ├── useLocalStorage.ts
│   │   └── useNotification.ts
│   ├── services/
│   │   ├── apiClient.ts
│   │   ├── auth.service.ts
│   │   ├── product.service.ts
│   │   ├── cart.service.ts
│   │   ├── order.service.ts
│   │   ├── payment.service.ts
│   │   ├── category.service.ts
│   │   ├── user.service.ts
│   │   └── notification.service.ts
│   ├── stores/
│   │   ├── authStore.ts
│   │   ├── cartStore.ts
│   │   ├── productStore.ts
│   │   ├── uiStore.ts
│   │   └── notificationStore.ts
│   ├── middleware/
│   │   ├── ProtectedRoute.tsx
│   │   ├── RoleRoute.tsx
│   │   ├── authMiddleware.ts
│   │   └── errorHandler.ts
│   ├── utils/
│   │   ├── format.ts
│   │   ├── validation.ts
│   │   ├── constants.ts
│   │   ├── helpers.ts
│   │   └── api.ts
│   ├── types/
│   │   ├── api.types.ts
│   │   ├── auth.types.ts
│   │   ├── product.types.ts
│   │   ├── order.types.ts
│   │   ├── user.types.ts
│   │   └── ui.types.ts
│   ├── styles/
│   │   ├── index.css
│   │   ├── tailwind.config.ts
│   │   └── globals.css
│   ├── App.tsx
│   └── main.tsx
├── public/
│   ├── images/
│   ├── icons/
│   └── favicon.ico
├── package.json
├── vite.config.ts
├── tsconfig.json
├── tailwind.config.js
└── README.md
```

---

## Technology Stack

### Core Dependencies
```json
{
  "react": "^18.2.0",
  "react-dom": "^18.2.0",
  "react-router-dom": "^6.19.0",
  "typescript": "^5.2.0"
}
```

### State Management & Data
```json
{
  "zustand": "^4.4.0",
  "axios": "^1.6.2"
}
```

### Forms & Validation
```json
{
  "react-hook-form": "^7.47.0",
  "zod": "^3.22.4",
  "@hookform/resolvers": "^3.3.2"
}
```

### Styling
```json
{
  "tailwindcss": "^3.3.0",
  "postcss": "^8.4.31",
  "autoprefixer": "^10.4.16"
}
```

### Utilities
```json
{
  "date-fns": "^2.30.0",
  "clsx": "^2.0.0"
}
```

### Dev Tools
```json
{
  "vite": "^4.4.0",
  "prettier": "^3.0.3",
  "eslint": "^8.x"
}
```

---

## API Integration Guide

### Base URL Configuration
```typescript
// src/services/apiClient.ts
const API_BASE_URL = process.env.REACT_APP_API_URL || 'http://localhost:5000/api';
const API_TIMEOUT = 30000;

export const apiClient = axios.create({
  baseURL: API_BASE_URL,
  timeout: API_TIMEOUT,
  headers: {
    'Content-Type': 'application/json',
  },
});
```

### Authentication Header
```typescript
// All requests automatically include JWT token
apiClient.interceptors.request.use((config) => {
  const token = authStore.getState().accessToken;
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});
```

### API Response Structure
```typescript
interface ApiResponse<T> {
  success: boolean;
  data: T;
  message: string;
}
```

### All Available Endpoints

#### 🔐 Authentication Endpoints
```
POST   /api/auth/register           - User registration
POST   /api/auth/login              - User login
POST   /api/auth/refresh-token      - Refresh access token
POST   /api/auth/logout             - Logout user
POST   /api/auth/change-password    - Change password
```

#### 📦 Product Endpoints
```
GET    /api/products                - Get products (paginated)
GET    /api/products/all            - Get all products
GET    /api/products/{id}           - Get product by ID
GET    /api/products/category/{id}  - Get products by category
GET    /api/products/search         - Search products
POST   /api/products                - Create product (Admin)
PUT    /api/products/{id}           - Update product (Admin)
DELETE /api/products/{id}           - Delete product (Admin)
PATCH  /api/products/{id}/stock     - Update stock (Admin)
```

#### 🏷️ Category Endpoints
```
GET    /api/categories              - Get all categories
GET    /api/categories/{id}         - Get category by ID
POST   /api/categories              - Create category (Admin)
PUT    /api/categories/{id}         - Update category (Admin)
DELETE /api/categories/{id}         - Delete category (Admin)
```

#### 🛒 Cart Endpoints
```
GET    /api/cart                    - Get user cart
GET    /api/cart/count              - Get cart item count
POST   /api/cart                    - Add to cart
PUT    /api/cart/{cartItemId}       - Update cart item
DELETE /api/cart/{cartItemId}       - Remove from cart
DELETE /api/cart                    - Clear cart
```

#### 📋 Order Endpoints
```
GET    /api/orders/my-orders        - Get user's orders
GET    /api/orders/{id}             - Get order by ID
GET    /api/orders/number/{number}  - Get order by number
POST   /api/orders                  - Create order
POST   /api/orders/{id}/cancel      - Cancel order
PATCH  /api/orders/{id}/status      - Update order status (Admin)
POST   /api/orders/{id}/mark-paid   - Mark as paid (Admin)
```

#### 💳 Payment Endpoints
```
POST   /api/payment/process         - Process payment
```

#### 🎟️ Discount Endpoints
```
POST   /api/discount/calculate      - Calculate discount
GET    /api/discount/available      - Get available discounts
GET    /api/discount/guide          - Get discount guide
```

#### 💰 Pricing Endpoints
```
POST   /api/pricing/calculate       - Calculate price
GET    /api/pricing/strategies      - Get pricing strategies
```

#### 🔔 Notification Endpoints
```
GET    /api/notification/subscribers     - Get subscribers (Admin)
POST   /api/notification/test/...        - Test notifications (Admin)
GET    /api/notification/guide           - Get notification guide
```

#### 👤 User Endpoints
```
GET    /api/users/me                - Get current user
PUT    /api/users/me                - Update profile
GET    /api/users/favourites        - Get favorites
POST   /api/users/favourites        - Add to favorites
DELETE /api/users/favourites/{id}   - Remove from favorites
GET    /api/users                   - Get all users (Admin)
```

---

## User Roles & Permissions

### Anonymous User
- ✅ Browse products
- ✅ View product details
- ✅ Browse categories
- ✅ Search products
- ❌ Add to cart
- ❌ Checkout
- ❌ View orders

### Registered User (Role: "User")
- ✅ All Anonymous permissions
- ✅ Register & Login
- ✅ Manage cart
- ✅ Create orders
- ✅ View own orders
- ✅ Cancel own orders
- ✅ Update profile
- ✅ Manage favorites
- ❌ Create/Edit products
- ❌ Manage users
- ❌ Access admin panel

### Admin User (Role: "Admin")
- ✅ All User permissions
- ✅ Create products
- ✅ Edit products
- ✅ Delete products
- ✅ Manage categories
- ✅ View all orders
- ✅ Update order status
- ✅ View all users
- ✅ Access admin dashboard
- ✅ View analytics
- ✅ Manage discounts

---

## Page Requirements

### 🏠 User Pages

#### 1. Home Page
- **Path:** `/`
- **Layout:** Full width with header & footer
- **Components:**
  - Hero banner
  - Featured products grid
  - Categories section
  - Call-to-action buttons
- **Data:** Fetch featured products on load
- **State:** Use `productStore` for featured products

#### 2. Products Page
- **Path:** `/products`
- **Layout:** Sidebar + main content
- **Features:**
  - Product grid with card display
  - Sidebar filters (category, price range, ratings)
  - Pagination
  - Sort options (newest, popular, price)
  - Search integration
- **Components:**
  - ProductGrid, ProductCard, ProductFilter, Pagination
- **Data:** Fetch from `GET /api/products`
- **State:** `productStore` for filters & pagination

#### 3. Product Detail Page
- **Path:** `/products/:id`
- **Components:**
  - Product image gallery
  - Product details
  - Price & stock info
  - Add to cart button
  - Quantity selector
  - Reviews section (optional)
  - Related products
- **Data:** `GET /api/products/:id`
- **Actions:** Add to cart, Add to favorites

#### 4. Cart Page
- **Path:** `/cart`
- **Components:**
  - Cart items list
  - Item quantity controls
  - Remove item buttons
  - Cart summary
  - Discount code input
  - Proceed to checkout button
  - Empty cart message
- **Data:** `GET /api/cart`
- **State:** `cartStore` for local cart state
- **Features:**
  - Discount calculation
  - Real-time total update
  - Continue shopping link

#### 5. Checkout Page
- **Path:** `/checkout`
- **Protected:** Yes (requires authentication)
- **Components:**
  - Shipping address form
  - Billing address form
  - Payment method selector
  - Order review
  - Place order button
- **Forms:** React Hook Form with validation
- **Actions:**
  - Validate addresses
  - Create order
  - Process payment
- **Success:** Redirect to order confirmation

#### 6. Orders Page
- **Path:** `/orders`
- **Protected:** Yes
- **Components:**
  - Orders list
  - Order cards with status
  - Filters & search
  - Pagination
- **Data:** `GET /api/orders/my-orders`
- **Actions:** View order details, cancel order

#### 7. Order Detail Page
- **Path:** `/orders/:id`
- **Protected:** Yes
- **Components:**
  - Order header (number, date, status)
  - Order timeline
  - Items list
  - Shipping info
  - Payment info
  - Cancel button (if applicable)
- **Data:** `GET /api/orders/:id`

#### 8. Profile Page
- **Path:** `/profile`
- **Protected:** Yes
- **Tabs:**
  - Personal Info
  - Address Book
  - Password Change
  - Preferences
- **Forms:** Update profile, change password
- **Data:** `GET /api/users/me`
- **Actions:** Update user info

#### 9. Favorites Page
- **Path:** `/favorites`
- **Protected:** Yes
- **Components:**
  - Favorite products grid
  - Remove from favorites button
  - Add to cart from favorites
- **Data:** `GET /api/users/favourites`
- **State:** `cartStore` & `productStore`

### 🔐 Authentication Pages

#### 1. Login Page
- **Path:** `/login`
- **Public:** Yes (redirect if already logged in)
- **Components:**
  - Email/username input
  - Password input
  - Remember me checkbox
  - Login button
  - Register link
  - Forgot password link
- **Form Validation:** Email & password required
- **Error Handling:** Display API errors
- **Success:** Store tokens, redirect to home/previous page

#### 2. Register Page
- **Path:** `/register`
- **Public:** Yes (redirect if already logged in)
- **Components:**
  - Username input
  - Email input
  - Password input
  - Confirm password input
  - Phone number input (optional)
  - Terms & conditions checkbox
  - Register button
  - Login link
- **Validation:**
  - Email format
  - Password strength (min 8 chars, uppercase, number)
  - Passwords match
  - Terms accepted
- **Error Handling:** Display validation & API errors
- **Success:** Auto-login or redirect to login

### 📊 Admin Pages

#### 1. Admin Dashboard
- **Path:** `/admin`
- **Protected:** Yes (Admin role required)
- **Components:**
  - Stat cards (total products, orders, users, revenue)
  - Charts (sales, orders, revenue)
  - Recent orders table
  - Top products list
- **Data:**
  - `GET /api/products/all` (count)
  - `GET /api/orders` (recent, count)
  - `GET /api/users` (count)
- **Refresh:** Auto-refresh every 30 seconds or manual refresh

#### 2. Products Management
- **Path:** `/admin/products`
- **Protected:** Yes (Admin)
- **Components:**
  - Products data table
  - Search & filter
  - Add product button
  - Edit button (row action)
  - Delete button (row action)
  - Pagination
- **Features:**
  - Bulk delete
  - Export to CSV (optional)
- **Data:** `GET /api/products/all`
- **Actions:** Create, update, delete

#### 3. Product Form
- **Path:** `/admin/products/new` or `/admin/products/:id/edit`
- **Components:**
  - Name input
  - Description textarea
  - Price input
  - Stock quantity input
  - Category dropdown
  - Image URL input
  - Submit button
- **Form Validation:** All fields required
- **Actions:**
  - Create: `POST /api/products`
  - Update: `PUT /api/products/:id`
- **Success:** Redirect to products list

#### 4. Categories Management
- **Path:** `/admin/categories`
- **Features:**
  - Categories table
  - Add, edit, delete
  - Search & pagination
- **Data:** `GET /api/categories`
- **Actions:** CRUD operations

#### 5. Orders Management
- **Path:** `/admin/orders`
- **Features:**
  - Orders table
  - Status filter
  - Search by order number
  - View details
  - Update status
  - Mark as paid
- **Data:** `GET /api/orders` (all orders)
- **Columns:** Order #, Customer, Amount, Status, Date

#### 6. Users Management
- **Path:** `/admin/users`
- **Features:**
  - Users table
  - Search by name/email
  - Pagination
  - View user details
- **Data:** `GET /api/users`

---

## Component Architecture

### 🎯 Component Hierarchy

```
App
├── AuthGuard
├── Layout
│   ├── Header
│   │   ├── Logo
│   │   ├── SearchBar
│   │   ├── Navigation
│   │   └── UserMenu
│   ├── Sidebar (admin only)
│   ├── Content
│   │   └── Pages / Components
│   └── Footer
├── Modal (portal)
├── Toast (notification)
└── LoadingSpinner
```

### Common Components

#### Header
```typescript
interface HeaderProps {
  sticky?: boolean;
  transparent?: boolean;
}

Features:
- Logo & brand name
- Search bar
- Category menu
- Cart icon (with count)
- User menu (login/profile)
- Admin link (if admin)
```

#### Footer
```typescript
Features:
- Company info
- Links (About, Help, Contact)
- Social media
- Newsletter signup
- Copyright
```

#### ProductCard
```typescript
interface ProductCardProps {
  product: ProductDto;
  onAddCart: (productId: number) => void;
  onAddFavorite?: (productId: number) => void;
  isFavorite?: boolean;
}

Features:
- Product image
- Product name
- Price
- Stock indicator
- Rating (if available)
- Add to cart button
- Favorite button
```

#### CartItem
```typescript
interface CartItemProps {
  item: CartItemDto;
  onUpdateQuantity: (quantity: number) => void;
  onRemove: () => void;
}

Features:
- Product image
- Product name
- Price
- Quantity selector
- Subtotal
- Remove button
```

#### Modal
```typescript
interface ModalProps {
  isOpen: boolean;
  title: string;
  children: React.ReactNode;
  onClose: () => void;
  footer?: React.ReactNode;
}

Features:
- Backdrop
- Close button
- Header & footer
- Body content
```

#### Toast
```typescript
interface Toast {
  id: string;
  type: 'success' | 'error' | 'warning' | 'info';
  message: string;
  duration?: number;
}

Features:
- Auto-dismiss
- Action button (optional)
- Icon based on type
```

---

## State Management

### Using Zustand Stores

#### authStore
```typescript
// src/stores/authStore.ts
interface AuthState {
  // State
  user: User | null;
  accessToken: string | null;
  refreshToken: string | null;
  isLoading: boolean;
  error: string | null;

  // Actions
  login(email: string, password: string): Promise<void>;
  register(data: RegisterDto): Promise<void>;
  logout(): void;
  refreshAccessToken(): Promise<void>;
  setUser(user: User): void;
  clearError(): void;

  // Selectors
  isAuthenticated(): boolean;
  isAdmin(): boolean;
}
```

#### cartStore
```typescript
interface CartState {
  // State
  items: CartItemDto[];
  total: number;
  itemCount: number;

  // Actions
  addItem(productId: number, quantity: number): Promise<void>;
  removeItem(cartItemId: number): Promise<void>;
  updateItem(cartItemId: number, quantity: number): Promise<void>;
  clearCart(): Promise<void>;
  setItems(items: CartItemDto[]): void;

  // Selectors
  getTotal(): number;
  getItemCount(): number;
  hasItem(productId: number): boolean;
}
```

#### productStore
```typescript
interface ProductState {
  // State
  products: ProductDto[];
  selectedProduct: ProductDto | null;
  filters: ProductFilters;
  pagination: PaginationState;
  isLoading: boolean;

  // Actions
  fetchProducts(filters?: ProductFilters): Promise<void>;
  fetchProductById(id: number): Promise<void>;
  setFilters(filters: ProductFilters): void;
  setPagination(page: number, size: number): void;
  searchProducts(term: string): Promise<void>;
}
```

#### uiStore
```typescript
interface UIState {
  // State
  isSidebarOpen: boolean;
  notifications: Toast[];
  modals: Record<string, boolean>;

  // Actions
  toggleSidebar(): void;
  addNotification(toast: Toast): void;
  removeNotification(id: string): void;
  openModal(modalName: string): void;
  closeModal(modalName: string): void;
}
```

---

## Authentication Flow

### 📋 Login Flow

```
1. User enters credentials
2. Submit login form
3. Call authService.login(email, password)
4. Server returns { accessToken, refreshToken, user }
5. Store tokens in authStore
6. Store user in authStore
7. Save tokens to localStorage (optional)
8. Redirect to previous page or home
```

### 📝 Register Flow

```
1. User fills registration form
2. Submit register form
3. Call authService.register(userData)
4. Server returns { accessToken, refreshToken, user }
5. Auto-login with returned tokens
6. Redirect to home or profile
```

### 🔄 Token Refresh

```
API Request
  ↓
Check if token expired
  ↓ (if expired)
Call POST /api/auth/refresh-token
  ↓
Get new accessToken
  ↓
Retry original request
  ↓
Return response
```

### ❌ Logout Flow

```
1. User clicks logout
2. Call authService.logout()
3. POST /api/auth/logout
4. Clear authStore
5. Clear localStorage
6. Redirect to login page
```

---

## Design System

### Color Palette

```typescript
const colors = {
  // Primary
  primary: '#007bff',      // Blue
  primaryDark: '#0056b3',
  primaryLight: '#cfe2ff',

  // Secondary
  secondary: '#6c757d',    // Gray
  secondaryDark: '#5a6268',
  secondaryLight: '#e2e3e5',

  // Status Colors
  success: '#28a745',      // Green
  danger: '#dc3545',       // Red
  warning: '#ffc107',      // Yellow
  info: '#17a2b8',         // Cyan

  // Neutral
  white: '#ffffff',
  black: '#000000',
  gray50: '#f9fafb',
  gray100: '#f3f4f6',
  gray200: '#e5e7eb',
  gray300: '#d1d5db',
  gray400: '#9ca3af',
  gray500: '#6b7280',
  gray600: '#4b5563',
  gray700: '#374151',
  gray800: '#1f2937',
  gray900: '#111827',
};
```

### Typography

```typescript
const typography = {
  h1: {
    fontSize: '2.5rem',    // 40px
    fontWeight: 700,
    lineHeight: 1.2,
  },
  h2: {
    fontSize: '2rem',      // 32px
    fontWeight: 700,
    lineHeight: 1.3,
  },
  h3: {
    fontSize: '1.5rem',    // 24px
    fontWeight: 600,
    lineHeight: 1.4,
  },
  body: {
    fontSize: '1rem',      // 16px
    fontWeight: 400,
    lineHeight: 1.5,
  },
  small: {
    fontSize: '0.875rem',  // 14px
    fontWeight: 400,
    lineHeight: 1.5,
  },
};
```

### Spacing Scale

```typescript
const spacing = {
  xs: '0.25rem',   // 4px
  sm: '0.5rem',    // 8px
  md: '1rem',      // 16px
  lg: '1.5rem',    // 24px
  xl: '2rem',      // 32px
  '2xl': '3rem',   // 48px
  '3xl': '4rem',   // 64px
};
```

### Border Radius

```typescript
const borderRadius = {
  sm: '0.25rem',   // 4px
  md: '0.375rem', // 6px
  lg: '0.5rem',    // 8px
  xl: '1rem',      // 16px
  full: '9999px',  // Circle
};
```

### Shadow System

```typescript
const shadows = {
  sm: '0 1px 2px 0 rgba(0, 0, 0, 0.05)',
  md: '0 4px 6px -1px rgba(0, 0, 0, 0.1)',
  lg: '0 10px 15px -3px rgba(0, 0, 0, 0.1)',
  xl: '0 20px 25px -5px rgba(0, 0, 0, 0.1)',
};
```

---

## File Structure & Naming Conventions

### File Naming
```
├── Components
│   ├── ComponentName.tsx              (PascalCase)
│   ├── ComponentName.module.css       (kebab-case for CSS)
│   └── ComponentName.types.ts         (types file)
├── Hooks
│   └── useHookName.ts                 (camelCase with 'use' prefix)
├── Services
│   └── serviceName.service.ts         (camelCase)
├── Stores
│   └── entityStore.ts                 (camelCase)
├── Types
│   └── entityName.types.ts            (kebab-case)
└── Utils
    └── utilityName.ts                 (camelCase)
```

### Import Paths (using aliases)
```typescript
// ❌ Bad
import Component from '../../../components/product/ProductCard';

// ✅ Good
import { ProductCard } from '@components/product';
import { useProducts } from '@hooks/useProducts';
import { productService } from '@services/product.service';
import { ProductDto } from '@types/product.types';
```

### tsconfig.json aliases
```json
{
  "compilerOptions": {
    "baseUrl": ".",
    "paths": {
      "@/*": ["src/*"],
      "@components/*": ["src/components/*"],
      "@pages/*": ["src/pages/*"],
      "@hooks/*": ["src/hooks/*"],
      "@services/*": ["src/services/*"],
      "@stores/*": ["src/stores/*"],
      "@types/*": ["src/types/*"],
      "@utils/*": ["src/utils/*"],
      "@middleware/*": ["src/middleware/*"],
      "@layouts/*": ["src/layouts/*"]
    }
  }
}
```

---

## Development Workflow

### 🚀 Getting Started

```bash
# Clone repository
git clone https://github.com/sngkagnwho/Ecommercial-shop.git
cd mtkpm-frontend

# Install dependencies
npm install

# Start dev server
npm run dev

# Build for production
npm run build

# Preview production build
npm run preview

# Run type checking
npm run type-check

# Lint code
npm run lint

# Format code
npm run format
```

### 📝 Creating a New Feature

#### 1. Create Page Component
```typescript
// src/pages/user/NewFeature.tsx
import React from 'react';
import { Header, Footer } from '@components/common';

export const NewFeaturePage: React.FC = () => {
  return (
    <div>
      <Header />
      <main>
        {/* Page content */}
      </main>
      <Footer />
    </div>
  );
};
```

#### 2. Add Route
```typescript
// Update src/App.tsx
import { NewFeaturePage } from '@pages/user/NewFeature';

const routes = [
  {
    path: '/feature',
    element: <ProtectedRoute><NewFeaturePage /></ProtectedRoute>,
  },
];
```

#### 3. Create Service (if needed)
```typescript
// src/services/newFeature.service.ts
import { apiClient } from './apiClient';

export const newFeatureService = {
  getItems: async () => {
    const response = await apiClient.get('/endpoint');
    return response.data.data;
  },
};
```

#### 4. Create Store (if needed)
```typescript
// src/stores/newFeatureStore.ts
import { create } from 'zustand';

interface State {
  items: any[];
  setItems: (items: any[]) => void;
}

export const useNewFeatureStore = create<State>((set) => ({
  items: [],
  setItems: (items) => set({ items }),
}));
```

#### 5. Create Hook (if needed)
```typescript
// src/hooks/useNewFeature.ts
import { useNewFeatureStore } from '@stores/newFeatureStore';
import { newFeatureService } from '@services/newFeature.service';

export const useNewFeature = () => {
  const { items, setItems } = useNewFeatureStore();

  const fetchItems = async () => {
    const data = await newFeatureService.getItems();
    setItems(data);
  };

  return { items, fetchItems };
};
```

### 🧪 Testing Strategy

```typescript
// Example: src/pages/user/NewFeature.test.tsx
import { render, screen } from '@testing-library/react';
import { NewFeaturePage } from './NewFeature';

describe('NewFeaturePage', () => {
  it('renders correctly', () => {
    render(<NewFeaturePage />);
    expect(screen.getByText(/feature/i)).toBeInTheDocument();
  });
});
```

### 🐛 Debugging

```typescript
// Enable debug mode
localStorage.setItem('DEBUG', 'true');

// Log API requests
apiClient.interceptors.request.use((config) => {
  console.log('Request:', config);
  return config;
});

// Log API responses
apiClient.interceptors.response.use((response) => {
  console.log('Response:', response);
  return response;
});
```

---

## Error Handling

### API Error Handler
```typescript
// src/middleware/errorHandler.ts
export const handleApiError = (error: AxiosError) => {
  if (error.response?.status === 401) {
    // Token expired - redirect to login
    window.location.href = '/login';
  } else if (error.response?.status === 403) {
    // Unauthorized - show error
    showNotification('Access denied', 'error');
  } else {
    // Generic error
    showNotification(error.response?.data?.message || 'An error occurred', 'error');
  }
};
```

### Try-Catch Pattern
```typescript
try {
  const data = await service.fetchData();
  setData(data);
} catch (error) {
  if (isAxiosError(error)) {
    setError(error.response?.data?.message);
  } else {
    setError('Unknown error occurred');
  }
}
```

---

## Performance Optimization

### Code Splitting
```typescript
// Lazy load pages
import { lazy, Suspense } from 'react';

const AdminDashboard = lazy(() => 
  import('@pages/admin/Dashboard').then(m => ({ default: m.AdminDashboard }))
);

<Suspense fallback={<LoadingSpinner />}>
  <AdminDashboard />
</Suspense>
```

### Memoization
```typescript
// Prevent unnecessary re-renders
import { memo } from 'react';

export const ProductCard = memo(({ product }: Props) => {
  return <div>{product.name}</div>;
});
```

### Image Optimization
```typescript
// Use WebP format, lazy loading
<img 
  src="image.webp" 
  alt="Description"
  loading="lazy"
  width={300}
  height={300}
/>
```

---

## 📚 Additional Resources

### API Documentation
- Swagger UI: `http://localhost:5000`
- API_ENDPOINTS_COMPLETE.md

### Backend Repository
- https://github.com/sngkagnwho/Ecommercial-shop

### Frontend Port
- Development: `http://localhost:3000`
- Admin Panel: `http://localhost:3000/admin`

---

## 🎯 Development Checklist

### Phase 1: Setup & Core
- [ ] Project initialization
- [ ] Folder structure
- [ ] Configure Tailwind CSS
- [ ] Setup routing
- [ ] Create layout components

### Phase 2: Authentication
- [ ] Login page
- [ ] Register page
- [ ] Auth service
- [ ] Auth store
- [ ] Protected routes

### Phase 3: User Features
- [ ] Home page
- [ ] Products page
- [ ] Product detail page
- [ ] Cart functionality
- [ ] Checkout flow
- [ ] Orders management
- [ ] User profile

### Phase 4: Admin Features
- [ ] Admin dashboard
- [ ] Products management
- [ ] Categories management
- [ ] Orders management
- [ ] Users management

### Phase 5: Polish
- [ ] Error handling
- [ ] Loading states
- [ ] Notifications
- [ ] Responsive design
- [ ] Performance optimization
- [ ] Testing
- [ ] Documentation

---

## 🤝 Collaboration Tips

### For Your Friend (Frontend Developer)
1. **Read this document first** - Get familiar with the entire system
2. **Review API endpoints** - Understand what data is available
3. **Check backend code** - See DTOs and response formats
4. **Start with simple pages** - Build home page first
5. **Test with Swagger UI** - Verify API responses before implementing
6. **Use Zustand stores** - All state management in one place
7. **Follow naming conventions** - Keep code consistent
8. **Ask questions** - Don't hesitate about API behavior

### Best Practices
- ✅ Always validate user input
- ✅ Handle loading & error states
- ✅ Use TypeScript for type safety
- ✅ Keep components small & reusable
- ✅ Test API integration thoroughly
- ✅ Document complex logic
- ✅ Keep CSS organized with Tailwind
- ✅ Use meaningful variable names

---

## 📞 Support & Questions

**Backend Endpoints:** `http://localhost:5000`  
**Swagger Documentation:** `http://localhost:5000`  
**API Response Format:** See `API_ENDPOINTS_COMPLETE.md`  

**Common Issues:**
- CORS errors → Configure proxy in vite.config.ts
- Token expired → Refresh token automatically
- 404 errors → Check API endpoint spelling
- CSRF errors → Include credentials in requests

---

**Document Version:** 1.0  
**Last Updated:** 2024  
**Status:** Ready for Frontend Development  

Good luck building! 🚀

Data Flow Between Microservices - E-Commerce Product Catalog & Order Tracking System

User Service
Customers, Sellers, and Admins can register and log in.
JWT tokens are issued for authentication upon successful login.
Admins can access system-wide data using admin-specific endpoints secured via authorization policies.

Product Service
Sellers can add, update, or delete products through secured endpoints requiring JWT authentication.
Customers can view available products via public APIs.
When an order is placed, the Order Service communicates with the Product Service via RabbitMQ to check stock.
Product Service verifies inventory and responds with availability.
If available, Product Service reduces stock once the order is confirmed.

Order Service
Customers place orders via the API Gateway.
On receiving an order request, Order Service sends a stock check request to Product Service using RabbitMQ.
If stock is available, Order Service confirms the order, generates an Order ID, updates stock, and sets the status to 'Processing'.
Notification about the order is published to RabbitMQ for the Notification Service to handle.
Order status (e.g., Processing to Shipped) can be updated by the seller, which also triggers another notification.

Notification Service
Listens for messages from RabbitMQ related to order events such as new orders and status updates.
Sends email or SMS notifications to the customer based on the message received.

API Gateway (YARP)
Acts as a single entry point for all incoming requests.
Validates JWT tokens and enforces authorization policies for roles like Customer, Seller, and Admin.

Security Flow
All APIs except product viewing require JWT authentication.
Authorization policies control access based on user roles:
Customer: Can place and view orders, browse products.
Seller: Can manage product listings.
Admin: Can oversee and manage users and product data across the system.

Database Per Service
Each microservice maintains its own database to ensure data encapsulation and reduce coupling:
User DB: Manages user credentials and roles.
Product DB: Stores product information and inventory levels.
Order DB: Tracks order details and status.

Notification DB (optional): Can be used to log messages sent or for auditing.
This separation and flow ensure scalability, fault isolation, and clearer boundaries between business capabilities.
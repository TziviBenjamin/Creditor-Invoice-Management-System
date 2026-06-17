# Creditor Invoice Management System

A professional solution designed to manage and track property-related creditor invoices. This system provides a secure environment for property owners to view invoice history and for administrators to manage user access and property assignments.

## Features
* **Secure Authentication:** User login system managed by administrators, ensuring property owners only access their relevant data.
* **Invoice Overview:** Comprehensive search and filtering capabilities, including filtering by property, creditor, and date ranges.
* **Document Management:** Direct access to view attached PDF invoices associated with specific creditor records.
* **Admin Dashboard:** Centralized management for creating user accounts and assigning specific properties to owners.
* **Data Integrity:** A structured database design ensuring accurate linking between users, properties, and invoices.

## System Components
* **Login Page:** Secure entry point for authenticated property owners.
* **Home Page:** The primary interface for searching and viewing invoice records linked to the user's properties.
* **Admin Page:** The control panel for administrators to manage users and property assignments.

## Database Structure
* **Users:** Stores username and password credentials.
* **Properties:** Contains details of properties managed by the system.
* **User-Property Mapping:** Links users to their specific assigned properties.
* **Creditor Invoices:** Stores invoice details (including creditor number, dates) and the associated PDF file linked to a specific Property ID.

## Technologies Used
* **Language/Framework:** C# / ASP.NET Core
* **Database:** SQL Server
* **Environment:** Visual Studio

---
*Developed for property management and invoice tracking purposes.*

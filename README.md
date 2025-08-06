📧 Email Prompt Submission App
A full-stack web application that allows users to submit prompts, which are processed and sent via AI-generated emails. The system provides role-based authentication, real-time status tracking, and an admin dashboard for request management.

🚀 Features
✅ Angular Frontend – Responsive UI for users and admins

✅ ASP.NET Core Backend – REST API with SQL Server integration

✅ JWT Authentication – Secure login for Users and Admins

✅ AI Email Generation – Automated, intelligent email responses

✅ Prompt Status Tracking – Manage and monitor submissions

✅ Background Email Scheduler – Sends pending emails every 5 minutes

✅ Admin Dashboard – View all user requests and manage roles

🛠 Tech Stack
Frontend: Angular 16+, TypeScript, Bootstrap

Backend: ASP.NET Core 8, C#

Database: Microsoft SQL Server

Authentication: JWT (JSON Web Token)

Email Service: AI-generated email logic (e.g., OpenAI, custom logic)

Scheduler: Background task in .NET for email delivery

📂 Project Structure
bash
Copy
Edit
/email-prompt-app
│
├── frontend/         # Angular code
├── backend/          # ASP.NET Core API
│   ├── Controllers/
│   ├── Models/
│   ├── Services/
│   └── Data/
└── README.md

📬 How It Works
User Signup/Login using JWT Authentication

Submit Prompt through the Angular dashboard

Store Prompt in SQL Server with Pending status

Background Scheduler checks every 5 mins to send email

AI Generates Email and sends via email API

Status Updates to Submitted after success

🛡️ Security
Role-based authentication (Admin & User)

JWT tokens for session security

Input validation and SQL injection protection

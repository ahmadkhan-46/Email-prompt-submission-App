📧 Email Prompt Submission App

A full-stack application that allows users to **submit prompts**, automatically **convert them into professional emails using AI (Gemini API)**, and then **send the emails to the intended recipient** after a short delay.  
The app includes **user and admin roles**, prompt tracking, and background processing.

---

## 🚀 Features
✔ **User Signup & Login (JWT Authentication)**  
✔ **Role-Based Access (User & Admin)**  
✔ **Submit Prompts with Recipient Email**  
✔ **Background Service**:  
   - Stores prompts in the database with **Pending** status  
   - Processes them every 5 minutes  
   - Uses **Google Gemini AI** to generate a formal email  
   - Sends the email to the recipient and updates status to **Submitted**  
✔ **User Dashboard** – View all previously submitted prompts  
✔ **Admin Dashboard** – View all prompts and user activities  
✔ **Responsive UI (Angular)**  
✔ **REST API (ASP.NET Core 8)**  
✔ **Email Sending via SMTP**  

---

## 🛠 Tech Stack
### Frontend
- Angular 19+
- Bootstrap 5
- TypeScript

### Backend
- ASP.NET Core 8 Web API
- Entity Framework Core
- Microsoft SQL Server
- Background Services (HostedService)
- Gemini AI API (Google Generative Language API)
- SMTP Email Service (Gmail)

---

 📂 Project Structure

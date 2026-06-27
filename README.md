# PROG6221-Part-3
Portfolio of Evidence
# CyberChatBot

## Overview

CyberChatBot is a desktop application developed using **C#**, **Windows Presentation Foundation (WPF)**, and the **Model-View-ViewModel (MVVM)** architecture. The application serves as an intelligent cybersecurity assistant that allows users to interact with a chatbot, manage daily tasks, complete cybersecurity quizzes, and monitor user activities. All user information, tasks, and quiz data are stored in a **MySQL** database.

---

# Features

###  Intelligent Chatbot

* Converses naturally with users.
* Responds to cybersecurity-related questions.
* Detects user intent using Natural Language Processing (NLP).
* Provides contextual responses.

###  Sentiment Analysis

* Detects whether the user's message is:

  * Positive
  * Neutral
  * Negative
* Displays the detected sentiment within the application.

### Task Management

Users can:

* Add new tasks
* Add task descriptions
* Set reminder dates
* View all saved tasks
* Mark tasks as completed
* Delete tasks

###  Cybersecurity Quiz

* Multiple Choice questions
* True/False questions
* Instant feedback
* Score tracking
* Explanations after each answer
* Quiz reset functionality

###  Activity Logging

The chatbot records user activities including:

* Messages sent
* Tasks created
* Tasks completed
* Tasks deleted
* Quiz attempts
* System events

###  Database Integration

The application automatically creates the following MySQL tables:

* Users
* Tasks
* ActivityLogs
* QuizQuestions

Sample cybersecurity quiz questions are automatically inserted when the application starts for the first time.

---

# Technologies Used

* C#
* .NET
* Windows Presentation Foundation (WPF)
* MVVM Design Pattern
* MySQL
* MySql.Data
* XAML
* ObservableCollection
* ICommand
* Data Binding

---

# Project Structure

```text
CyberChatBot
│
├── App
│   ├── Views
│   │   ├── ChatView.xaml
│   │   ├── TaskView.xaml
│   │   └── QuizView.xaml
│   │
│   ├── ViewModels
│   │   ├── ChatViewModel.cs
│   │   ├── TaskViewModel.cs
│   │   └── QuizViewModel.cs
│
├── Data
│   └── DbContext.cs
│
├── Models
│   ├── User.cs
│   ├── TaskItem.cs
│   ├── QuizQuestion.cs
│   └── ActivityLog.cs
│
├── Services
│   ├── ResponseService.cs
│   ├── NLPService.cs
│   ├── SentimentService.cs
│   ├── TaskService.cs
│   ├── QuizService.cs
│   └── ActivityLogService.cs
│
└── Database
```

---

# Database

The application automatically creates the database named:

```sql
cyberchatbot
```

It contains four tables:

### Users

Stores user information.

| Column           | Description               |
| ---------------- | ------------------------- |
| Id               | User ID                   |
| Name             | User Name                 |
| CurrentSentiment | Latest detected sentiment |
| LastInteraction  | Last interaction date     |

---

### Tasks

Stores user tasks.

| Column       | Description       |
| ------------ | ----------------- |
| Id           | Task ID           |
| Title        | Task title        |
| Description  | Task description  |
| ReminderDate | Reminder date     |
| CreatedDate  | Date created      |
| IsCompleted  | Completion status |
| UserId       | User reference    |

---

### ActivityLogs

Stores all user activities.

| Column    | Description            |
| --------- | ---------------------- |
| Id        | Activity ID            |
| UserId    | User reference         |
| Action    | Action performed       |
| Details   | Additional information |
| Timestamp | Date and time          |

---

### QuizQuestions

Stores cybersecurity quiz questions.

| Column        | Description                   |
| ------------- | ----------------------------- |
| Id            | Question ID                   |
| Question      | Quiz question                 |
| Type          | Multiple Choice or True/False |
| Options       | Answer choices                |
| CorrectAnswer | Correct answer                |
| Explanation   | Answer explanation            |

---

# How the Application Works

## Chat Module

The chatbot:

1. Receives user input.
2. Detects user sentiment.
3. Identifies the user's intent using NLP.
4. Generates an appropriate response.
5. Records the interaction.
6. Updates the user's status.

---

## Task Manager

The user can:

1. Enter a task title.
2. Enter a description.
3. Select a reminder date.
4. Save the task.
5. Mark the task as complete.
6. Delete unwanted tasks.

All task information is stored in MySQL.

---

## Quiz Module

The quiz:

1. Loads cybersecurity questions.
2. Displays one question at a time.
3. Accepts the user's answer.
4. Checks correctness.
5. Displays feedback.
6. Updates the score.
7. Shows the final result.

---

# Installation

## Prerequisites

Install the following software:

* Visual Studio 2022
* .NET SDK
* MySQL Server
* MySQL Workbench (optional)
* MySql.Data NuGet Package

---

## Clone the Project

```bash
git clone https://github.com/yourusername/CyberChatBot.git
```

Open the solution in Visual Studio.

---

## Configure Database

Open:

```text
DbContext.cs
```

Update the connection string:

```csharp
_connectionString =
"Server=localhost;Database=cyberchatbot;Uid=root;Pwd=YOUR_PASSWORD;";
```

Replace:

```
YOUR_PASSWORD
```

with your MySQL password.

---

## Install Required Package

Using NuGet Package Manager:

```powershell
Install-Package MySql.Data
```

---

## Run the Project

Press

```
F5
```

or click

```
Start
```

Visual Studio will automatically:

* Create the database
* Create all tables
* Insert quiz questions
* Launch the chatbot

---

# Future Improvements

Possible enhancements include:

* Voice recognition
* Speech synthesis
* User authentication
* Password encryption
* Email reminders
* AI-powered chatbot responses
* Cloud database integration
* Dark mode
* Mobile application version

---

# Learning Outcomes

This project demonstrates knowledge of:

* Object-Oriented Programming
* MVVM Architecture
* WPF Development
* MySQL Database Integration
* CRUD Operations
* Data Binding
* Commands
* Observable Collections
* Exception Handling
* Natural Language Processing
* Sentiment Analysis

---

# Author

**Name:** SEDUMA MMP

**Institution:** Rosebank International

**Module:** Programming / Software Development Project

**Project:** CyberChatBot

---

# License

This project is intended for educational purposes and may be modified or extended for learning and research.

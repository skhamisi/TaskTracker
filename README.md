# Task Tracker System

A full CRUD application, including login, adding/deleting users, updating user information, admin/user credentials, adding tasks, editing tasks, viewing current tasks, completing tasks, receiving notification on pending/complete tasks, completion percentages of tasks. 

## Requirements
1. [Docker Desktop](https://docs.docker.com/desktop/setup/install/windows-install/)
1. [Powershell](https://learn.microsoft.com/en-us/powershell/scripting/install/installing-powershell-on-windows?view=powershell-7.4)
1. Optional: [SSMS](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16)

## Local Docker Setup
1. Install and run docker desktop (ensure it is running in Linux configuration)
1. Open a powershell window and navigate to the project root
1. Run: `docker build -f dev.dockerfile -t tasktrackerdev .`
1. Open dockerdesktop and go to Images
1. Ensure that there is an image named 'tasktrackerdev' with a tag of 'latest'
1. Use the same powershell window or open another window and navigate to the project root
1. Run: `docker compose up -d --build`
1. Open a browser and navigate to '[http://localhost:5168](http://localhost:5168/)'

### Initial Credentials:
- Username: admin
- Password: Password123!

## Hosted Site
1. Reach out to an admin and request an account. 
1. Navigate to [https://mytasktrackerpro.site](https://mytasktrackerpro.site)
1. Use assigned credentials to log in

## Current Admins
- skhamisi@outlook.com

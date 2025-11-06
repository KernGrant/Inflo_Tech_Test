# User Management System – Tech Test Submission

## Overview
This project implements a user management system with full CRUD operations, user action logging, and comprehensive test coverage. 
The system demonstrates clean architecture, separation of concerns, and scalability considerations, while being fully unit-testable.


### Key Features Implemented
- **User Management**: Create, Read, Update, Delete users with proper validation and navigation.
- **Logging**: Logs user actions (Create/Update/Delete) with timestamps and details.
- **Seeding**: Initial set of logs seeded automatically at startup.
- Pagination & Filtering: Logs page handles large datasets gracefully.
- **Tests**: Unit tests cover services, controllers, and DataContext operations.
- **Architecture**: Clean separation of concerns and proper dependency injection.
- **UX Enhancements**:
 - Users can navigate to a specific page or filter by UserId.
 - Logs overview page persists page/filter when navigating back from an individual log.
 - Back navigation on user details page, consistent redirection after actions.
 - Log Details Handling: Shows log details, cuts off (in the overview page) if the section goes beyond 50 characters, and provides a generic message ("No additional information available") when details are missing.
 - Deleted User Handling: When a user referenced by a log has been deleted, the system shows a Deleted User Summary, displaying historical logs instead of a 404 error.
 - Logs now include a "View User Details" button, allowing easy navigation from a log entry to the corresponding user’s details or their Deleted User Summary.


### Optional / Future Enhancements
- Extend filtering & pagination to User overview pages.
- Implement search for Users and Logs (e.g., name, email, action details, date ranges).
- Role-based permissions for actions on logs/users.
- Centralized event logging service for platform-level auditing.
- Background log processing for high-scale systems.
- Integration with multiple storage backends (SQL Server, Azure Cosmos DB, etc.).
- Advanced architectural patterns: CQRS, Event Sourcing, MediatR.

### Notes
- Logs seeded at startup: 50 generic + 3 specific actions.
- CRUD operations are fully unit-tested.
- Logging actions are verified through mock-based tests in `LogServiceTests` and `UserControllerTests`.
- Clean architecture and testability were a priority.


---------------------------------------------------------------------------------------------------------------------------
# User Management Technical Exercise

The exercise is an ASP.NET Core web application backed by Entity Framework Core, which faciliates management of some fictional users.
We recommend that you use [Visual Studio (Community Edition)](https://visualstudio.microsoft.com/downloads) or [Visual Studio Code](https://code.visualstudio.com/Download) to run and modify the application. 

**The application uses an in-memory database, so changes will not be persisted between executions.**

## The Exercise
Complete as many of the tasks below as you feel comfortable with. These are split into 4 levels of difficulty 
* **Standard** - Functionality that is common when working as a web developer
* **Advanced** - Slightly more technical tasks and problem solving
* **Expert** - Tasks with a higher level of problem solving and architecture needed
* **Platform** - Tasks with a focus on infrastructure and scaleability, rather than application development.

### 1. Filters Section (Standard)

The users page contains 3 buttons below the user listing - **Show All**, **Active Only** and **Non Active**. Show All has already been implemented. Implement the remaining buttons using the following logic:
* Active Only – This should show only users where their `IsActive` property is set to `true`
* Non Active – This should show only users where their `IsActive` property is set to `false`

### 2. User Model Properties (Standard)

Add a new property to the `User` class in the system called `DateOfBirth` which is to be used and displayed in relevant sections of the app.

### 3. Actions Section (Standard)

Create the code and UI flows for the following actions
* **Add** – A screen that allows you to create a new user and return to the list
* **View** - A screen that displays the information about a user
* **Edit** – A screen that allows you to edit a selected user from the list  
* **Delete** – A screen that allows you to delete a selected user from the list

Each of these screens should contain appropriate data validation, which is communicated to the end user.

### 4. Data Logging (Advanced)

Extend the system to capture log information regarding primary actions performed on each user in the app.
* In the **View** screen there should be a list of all actions that have been performed against that user. 
* There should be a new **Logs** page, containing a list of log entries across the application.
* In the Logs page, the user should be able to click into each entry to see more detail about it.
* In the Logs page, think about how you can provide a good user experience - even when there are many log entries.

### 5. Extend the Application (Expert)

Make a significant architectural change that improves the application.
Structurally, the user management application is very simple, and there are many ways it can be made more maintainable, scalable or testable.
Some ideas are:
* Re-implement the UI using a client side framework connecting to an API. Use of Blazor is preferred, but if you are more familiar with other frameworks, feel free to use them.
* Update the data access layer to support asynchronous operations.
* Implement authentication and login based on the users being stored.
* Implement bundling of static assets.
* Update the data access layer to use a real database, and implement database schema migrations.

### 6. Future-Proof the Application (Platform)

Add additional layers to the application that will ensure that it is scaleable with many users or developers. For example:
* Add CI pipelines to run tests and build the application.
* Add CD pipelines to deploy the application to cloud infrastructure.
* Add IaC to support easy deployment to new environments.
* Introduce a message bus and/or worker to handle long-running operations.

## Additional Notes

* Please feel free to change or refactor any code that has been supplied within the solution and think about clean maintainable code and architecture when extending the project.
* If any additional packages, tools or setup are required to run your completed version, please document these thoroughly.

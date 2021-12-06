# Roommates
----------------------------------
# Practice

**IMPORTANT NOTE BEFORE YOU START!!!** 
    
The process of using ADO.NET is lengthy and verbose. You will undoubtedly be tempted to copy and paste code from one repository to another. We _highly_ _HIGHLY_ **HIGHLY** suggest that you do not do this. Doing this will not only lead to bugs in your code, it will hinder you from building muscle memory and impact your understanding of the code we're writing. Instead, reference code from other repositories, but type out your code by hand and leave comments if it helps you. It will pay off.

## Create a Chore Repository

1. Create a new file in the Repositories folder called `ChoreRepository` and implement the same methods as we did with the `RoomRepository`. After implementing each method, update the `Main` method to add an option in the menu.

1. Create a `RoommateRepository` and implement only the `GetById` method. It should take in a `int id` as a parameter and return a `Roommate` object. The trick: When you add a menu option for searching for a roommate by their Id, the output to the screen should output their first name, their rent portion, and _the name of the room they occupy_. Hint: You'll want to use a JOIN statement in your SQL query

1. Add a method to `ChoreRepository` called `GetUnassignedChores`. It should not accept any parameters and should return a list of chores that don't have any roommates already assigned to them. After implementing this method, add an option to the menu so the user can see the list of unassigned chores.

1. Add a `RoommateRepository` and define a `GetAll` method on it, but don't add a menu option to view all roommates yet. Next create a method in the `ChoreRepository` named `AssignChore`. It should accept 2 parameters--a roommateId and a choreId. Finally, add an option to the menu for "Assign chore to roommate". When the user selects that option, the program should first show a list of all chores and prompt the user to select the Id of the chore they want. Next the program should show a list of all roommates and prompt the user to select the Id of the roommate they want assigned to that chore. After the roommate has been assigned to the chore the program should print a message to the user to let them know the operation was successful.

### Advanced Challenge

Before you begin, add a few more records to the Chore and RoommateChore tables.

Inside the `ChoreRepository` create a method called `GetChoreCounts`. It's purpose will be to eventually help print out a report to the user that shows how many chores have been assigned to each roommate. i.e.

```
Wilma: 3
Juan: 4
Karen: 1
```

Helpful tips: 
- It may be tempting to make a SQL query to fetch all the chores and programmatically count them in your C# code; but that would be inefficient, and if you've made it this far you're better than that! The better way to do this is using a GROUP BY clause in your SQL query
- The shape of the data that will be returned by your GROUP BY won't match the shape of any of your model classes. You'll need to make another class whose properties better represent this data.

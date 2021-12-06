# Roommates
----------------------------------
# Create a Chore Repository
Create a new file in the Repositories folder called ChoreRepository and implement the same methods as we did with the RoomRepository. After implementing each method, update the Main method to add an option in the menu.

Create a RoommateRepository and implement only the GetById method. It should take in a int id as a parameter and return a Roommate object. The trick: When you add a menu option for searching for a roommate by their Id, the output to the screen should output their first name, their rent portion, and the name of the room they occupy. Hint: You'll want to use a JOIN statement in your SQL query

Add a method to ChoreRepository called GetUnassignedChores. It should not accept any parameters and should return a list of chores that don't have any roommates already assigned to them. After implementing this method, add an option to the menu so the user can see the list of unassigned chores.

Add a RoommateRepository and define a GetAll method on it, but don't add a menu option to view all roommates yet. Next create a method in the ChoreRepository named AssignChore. It should accept 2 parameters--a roommateId and a choreId. Finally, add an option to the menu for "Assign chore to roommate". When the user selects that option, the program should first show a list of all chores and prompt the user to select the Id of the chore they want. Next the program should show a list of all roommates and prompt the user to select the Id of the roommate they want assigned to that chore. After the roommate has been assigned to the chore the program should print a message to the user to let them know the operation was successful.

# Advanced Challenge
Before you begin, add a few more records to the Chore and RoommateChore tables.

Inside the ChoreRepository create a method called GetChoreCounts. It's purpose will be to eventually help print out a report to the user that shows how many chores have been assigned to each roommate. i.e.

Wilma: 3
Juan: 4
Karen: 1

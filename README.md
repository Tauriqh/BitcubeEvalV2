# BitcubeEvalV2
 New repository/project for BitcuebEval because I am having trouble with the ASP.Net Identity on the previous one.(the validation on the edit page)

## Repository for Bitcube Assessment Project

### This web app does the following:

#### Create a registration page that allows users to register to use the website. When a user registers, the following criteria should be met:

  * Email address must be unique
  * User must supply at least their first name and last name
  * Password must have at least 1 uppercase character, 1 lowercase character, 1 special character,1 number and must be at least 6 characters long
  * Password must be encrypted before being saved


#### Create a login page, where a user can log in using their email address and password.

  * There should be an option for the user to tell the site to remember their email address, so they don’t have to complete it every time they want to log in.
  * Once a user successfully logs in, they should be directed to the profile page.


#### Each page that a user sees once they have been validated should have navigation buttons with the following buttons:

  * Profile – redirects to the profile page.
  * Friends – redirects to the friends page.
  * Logout – Should abandon the user’s current session and redirect them to the login page.

#### Profile page:
 * Displays the information that a user completed when they registered, except for their password.
 * Should provide an edit function, that allows the user to update the information they provided when they registered, except for password.
 * Should provide a change password function, that allows a user to change their password, after providing their current password again.

#### Friends page:
 * Displays all of the user's friends whoare registered on the website.
 * Users can Unfriend any of their friends.
 
#### Find Friends page:
 * Displays all the registered users that is not the loged in users friend.
 * Here the loged in user can send a friend request to another user.
 
#### Friend Request page:
 * Displays any friend request that the user has.
 * Here the user can either accept the friend request or decline it.

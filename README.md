 PROG 2500 WINDOWS PROGRAMMING - ASSIGNMENT 1
WPF APPLICATIONS

Assignment Value: 10% of overall course mark.
Due Dates: 	See assigned due date on Assignment 1 dropbox  – Marked by code review during scheduled class time
Late submissions will receive the standard late submission penalty as stated in the course outline. (5% overall deduction per day late, and 0% after assignment handed back to the class.)
Assignment Instructions: 
In Visual Studio, use XAML/C# to create a WPF application as described by the requirements of this assignment.
Submissions:
Submission of work will be received through your GitHub repository. Please ensure that the commit you wish to have marked is labelled with an appropriate Commit Comment in your repo, and a text file containing the Commit ID is uploaded to the Assignment 1 dropbox on Brightspace. Unless otherwise specified, your most recent commit prior to the assignment deadline will be the one chosen for evaluation.
Evaluation:
To insure the greatest chance of success on this assignment, be sure to check the marking rubrics at the end of this document or in Brightspace. The rubrics contain the criteria your instructor will be assessing when marking your assignment.








 
Program – Media Player/MP3 Tagger

Design and write a program allows users to select and play an MP3 audio file, as well as edit and save some of the MP3 file’s tagged metadata.

On application start, the user can open a file dialog, browse to an MP3 audio file, and open it. When an MP3 has been opened, the user can use either a menu option or toolbar buttons to control the music’s playback state (Play, Pause or Stop). After a song is selected, the primary song metadata (Title, Artist, Album, Year) shall be displayed in a “Now Playing” screen. The song’s current time progress should also be shown in a timer, as well as use a slider control to visually display the progress and allow the user to move to any point in the song.
Once a song has been selected, the user should be able to toggle the display between the “Now Playing” screen and a tag-editing screen. If the user makes changes and saves any tag data, the changes should be written back into the MP3 file’s tag metadata.
TECHNICAL REQUIREMENTS
Your solution should be built to include the following technical specs:
•	WPF application using XAML and C#.
•	Use CommandBindings for the media and application controls
•	Use at least one User Control (Suggested use: Now Playing and Tag Editor screens)
•	Use at least three layout managers to create an intuitive and flexible user interface.
•	At minimum, the app should be able to read and write the following tag data: Song Title, Artist, Album, Year. Other tags can be used as desired.
•	Implement reasonable exception handling to avoid program crashes.

TECHNICAL RESOURCES
For accessing and editing ID3 tag metadata from the Mp3, it is suggested that you install the TagLib-Sharp package shown below. This third-party package allows reading and writing of ID3 tag data in MP3 files. Documentation and examples can be found on their website: http://taglib.org/api/ 
 
Important Note: If you refer to work or code from a website or other resources, whether you copy any code or not, it MUST be cited in your work. The references provided are not intended for you to just copy-paste… you are expected to cite anything from resource sites, and will be asked to explain how they work.



SAMPLE SCREENSHOTS
File Menu 	Media Menu 
Now Playing screen
 	Tag Editor screen 
Open File dialog
 


# HomeCinema
Simple application which is using users actions(upvote,downvote,download) to make recommended system for the user(userbased and itembased recommended system).
Onion architecture,code first approach.
# Motivation
Internship project.
# Tech/framework used
<b>Built with</b>
- .NET Core 2.1
- JavaScript (AJAX)
- CSS
- LINQ
- C#
- JQuery
# Code overview
- ASP.NET Identity(LogIn,LogOut,Register,CheckUsername and CheckEmail)
- User actions(upvote,downvote,download,view(actually it is not downloading or viewing the movie, this is just data for recommender system))
- User based(if user has action like download in User actions) and item based(if user is only registred and has no User actions) recommender(using PearsoneCorrelation and rating for movies to get suggestions for watching)


#ListeningTo
ListeningTo is a .NET Web API app that provides RESTful access to Last.fm user information including:

- Recently Played Tracks
- Top Artists 

It provides two benefits over using the last.fm REST API directly


- It hides the Last.fm API key in a configuration file server side so it is not visible in the browser

- It caches last.fm information for one minute. Ensuring that there is not more than one Last.fm API call per minute violating their terms of service.

##Dependencies
-	Rhino Mocks
-	NUnit
-	[LastfmClient](http://www.github.com/asciamanna/LastfmClient "LastfmClient") (my last.fm REST client also on github)

## Contact
**Anthony Sciamanna**  
Email: asciamanna@gmail.com  
Web: [http://www.anthonysciamanna.com](http://www.anthonysciamanna.com)  
Twitter: @asciamanna

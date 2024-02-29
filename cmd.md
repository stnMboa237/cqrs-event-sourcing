/*INSTALL .NET 6 SDK AND THEN CREATE FOLDERS HIERARCHY
  CQRS-ES = Command Query Responsability Segregation - Event Sourcing 
  SM = Social Media

  The scope is to create a social media App where users can create a post, like a post, update a post and delete a post
*/


cd CQRS-ES/

dotnet --version
dotnet --list-sdks
dotnet new globaljson /*the open the global.json file generated and paste my preferred .Net version at the place of the old version*/
dotnet --version >> to verify that the to project that we will create is pointing to the correct .Net version
dotnet new classlib -o CQRS.Core

/*CREATE THE SOLUTION*/
cd ../SM-Post/
dotnet new sln

/*CREATE POST.CMD PROJECT*/
cd Post.Cmd/
dotnet new webapi -o Post.Cmd.Api
dotnet new classlib -o Post.Cmd.Domain
dotnet new classlib -o Post.Cmd.Infrastructure

/*CREATE POST.QUERY PROJECT*/
cd ../Post.Query/
dotnet new webapi -o Post.Query.Api
dotnet new classlib -o Post.Query.Domain
dotnet new classlib -o Post.Query.Infrastructure

/*CREATE THE COMMON PROJECT WHICH IS WHERE WE WILL ADD OUR EVENTS OBJECTS BECAUSE EVENT OBJECTS NEEDS TO BE SHARED BETWEEN 
THE POST COMMAND AND THE POST QUERY APIs*/
cd .. /*now we are into SM-Post folder*/
dotnet new classlib -o Post.Common

/*ADD ALL PROJECTS TO SOLUTION*/
dotnet sln add ../CQRS-ES/CQRS.Core/CQRS.Core.csproj
dotnet sln add Post.Cmd/Post.Cmd.Api/Post.Cmd.Api.csproj
dotnet sln add Post.Cmd/Post.Cmd.Infrastructure/Post.Cmd.Infrastructure.csproj

dotnet sln add Post.Query/Post.Query.Api/Post.Query.Api.csproj
dotnet sln add Post.Query/Post.Query.Domain/Post.Query.Domain.csproj
dotnet sln add Post.Query/Post.Query.Infrastructure/Post.Query.Infrastructure.csproj


/*ADD REFERENCES BETWEEN PROJECTS*/
dotnet add Post.Cmd/Post.Cmd.Api/Post.Cmd.Api.csproj reference ../CQRS-ES/CQRS.Core/CQRS.Core.csproj
dotnet add Post.Cmd/Post.Cmd.Api/Post.Cmd.Api.csproj reference Post.Cmd/Post.Cmd.Domain/Post.Cmd.Domain.csproj
dotnet add Post.Cmd/Post.Cmd.Api/Post.Cmd.Api.csproj reference Post.Cmd/Post.Cmd.Infrastructure/Post.Cmd.Infrastructure.csproj
dotnet add Post.Cmd/Post.Cmd.Api/Post.Cmd.Api.csproj reference Post.Common/Post.Common.csproj 

dotnet add Post.Cmd/Post.Cmd.Domain/Post.Cmd.Domain.csproj reference ../CQRS-ES/CQRS.Core/CQRS.Core.csproj 
dotnet add Post.Cmd/Post.Cmd.Domain/Post.Cmd.Domain.csproj reference Post.Common/Post.Common.csproj 

dotnet add Post.Cmd/Post.Cmd.Infrastructure/Post.Cmd.Infrastructure.csproj reference ../CQRS-ES/CQRS.Core/CQRS.Core.csproj 
dotnet add Post.Cmd/Post.Cmd.Infrastructure/Post.Cmd.Infrastructure.csproj reference Post.Cmd/Post.Cmd.Domain/Post.Cmd.Domain.csproj

dotnet add Post.Common/Post.Common.csproj reference ../CQRS-ES/CQRS.Core/CQRS.Core.csproj

dotnet add Post.Query/Post.Query.Api/Post.Query.Api.csproj reference ../CQRS-ES/CQRS.Core/CQRS.Core.csproj
dotnet add Post.Query/Post.Query.Api/Post.Query.Api.csproj reference Post.Query/Post.Query.Domain/Post.Query.Domain.csproj
dotnet add Post.Query/Post.Query.Api/Post.Query.Api.csproj reference Post.Query/Post.Query.Infrastructure/Post.Query.Infrastructure.csproj
dotnet add Post.Query/Post.Query.Api/Post.Query.Api.csproj reference Post.Common/Post.Common.csproj

dotnet add Post.Query/Post.Query.Domain/Post.Query.Domain.csproj reference ../CQRS-ES/CQRS.Core/CQRS.Core.csproj
dotnet add Post.Query/Post.Query.Domain/Post.Query.Domain.csproj reference Post.Common/Post.Common.csproj

dotnet add Post.Query/Post.Query.Infrastructure/Post.Query.Infrastructure.csproj reference ../CQRS-ES/CQRS.Core/CQRS.Core.csproj
dotnet add Post.Query/Post.Query.Infrastructure/Post.Query.Infrastructure.csproj reference Post.Common/Post.Common.csproj

/* ADD NUGET PACKAGES
  Confluent.Kafka sera utilisé dans POST.CMD et POST.QUERY afin d'ecrire et lire les messages
  MongoDB (NoSQL DB) sera utilisé dans POST.CMD car c'est une DB rapide en écriture
  Microsoft.EntityFrameworkCore.sqlServer sera utilisé dans POST.QUERY car c'est une DB rapide en Lecture
*/

MongoDB.driver version 2.16.1 dans le projet CQRS.Core
Confluent.kafka version 1.9.0 dans le projet Post.Cmd.Infrastructure
Microsoft.extensions.options version 6.0.0 (doit matcher avec la version de .net du projet) dans le projet Post.Cmd.Infrastructure
MongoDB.driver version 2.16.1 dans le projet Post.Cmd.Infrastructure

Microsoft.EntityFrameworkCore.sqlServer version 6.0.6 dans le project Post.Query.Infrastructure
Microsoft.Extensions.Hosting version 6.0.1 dans le projet Post.Cmd.Infrastructure
Confluent.kafka version 1.9.0 dans le projet Post.Cmd.Infrastructure

/*Restore all our packages*/
dotnet restore

/*Config du debug dans VsCode*/
Ctrl + Shift + p >>.Net: Generate Assets for building Debug >> Post.Cmd.Api 
  >> into 'configurations' field: rename 'name': 'Post.Cmd.Api'
  >> into the 'env' field: add a new field: "ASPNETCORE_URL": "http://localhost:5010"
  >> Copy the first entry of 'configurations', and paste it below with params:
      "name": "Post.Query.Api",
      "ASPNETCORE_URL": "http://localhost:5011"
create a new configurations entry for the post.Query.Api following the example of the first entry
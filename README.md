BookTheRoom - fullstack booking web-service made by Max Sheludchenko

Service provides abilities for booking rooms in selected by user hotel and booking a private apartment owned by another authorized user.



Technologies used:

	Backend (Clean Architecture with elements of DDD - domain driven design):

		- Web development & language: ASP.NET Core & C# (.NET 8)

		- Containerization: Docker & Docker compose

		- Redis - distributed cache, docker image: redis

		- Database: Postgres, docker image: postgres

		- External services: 
 
				Braintree - payment service (test environment)

				Cloudinary - cloud storage for media files (photos, videos etc.)

				MimeKit - library that uses SMTP for sending emails

		- Libraries & frameworks used:
				
				MediatR - creating pipelines (validation, logging) and impementation of CQRS
				
				FluentValidation - validation

				EntityFramework - selected ORM, provides built-in protection from SQL-injection

				AspNetIdentity & EntityFramework.AspNetIdentity - user management in db

				Microsoft.AspNetCore.Authentication.JwtBearer - for JWT Bearer Authorization

	Frontend & Client:

		- SPA library / framework: React (18.3.1) & Node: 16.14.0

		- CSS framework: Tailwind CSS

		- Libraries & modules used:

			Axios - for sending request to API (fetch, post, put etc.)

			Chakra UI - library with components

			Fontawesome - icons & badges etc.

			yet-another-react-lightbox - library for lightbox

			slick-carousel - library for carousel

			js-cookie - library for storaging data in cookies (in this case - auth tokents: access, refresh; user data)
	 
All rights are secured
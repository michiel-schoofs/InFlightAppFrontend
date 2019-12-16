

# InFlightApp

Dit software pakket is een combinatie van een ASP.NET Core backend en een UWP applicatie ontwikkeld voor een luchtvaartmaatschappij. De applicatie zal zowel door de passagiers als de crewleden worden gebruikt. De passagiers kunnen er producten op bestellen, vlucht informatie bekijken, media afspelen en met hun reisgezelschap chatten. De crewleden kunnen orders afhandelen, notificaties versturen en passagiers van zitplaatsen wisselen.

## Aan De Slag

Onderstaande instructies gaan over het opzetten van het project op basis van de broncode.

### Vereisten
Het is belangrijk om het project te openen in Visual Studio versie 2019!

### Installeren
Als het project is geopend in VS, klik dan rechtermuisknop op de solution  en open propperties. In het tabblad  Startup Project, kies Multiple startup projects. 

Selecteer `Start without debugging` voor de backend en `Start` voor de App.

#### Databank
Onze backend maakt gebruik van een Default Microsoft SQL Server configuratie. Indien je een SQLEXPRESS server gebruikt, kan best in de startup van de backend, de volgende lijn (lijn 43):
```
options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]);
```
in commentaar zetten en onderstaande lijn (lijn 44) uit commentaar zetten:
```
options.UseSqlServer(Configuration["ConnectionStrings:SQLEXPRESSConnection"]);
```
Het is ook aangeraden om eens te controleren of de connectionstring in `appsettings.json` evereen komt met de instellingen van je SQL Server.

### Login
Bij het opstarten krijg je de keuze om in te loggen als passagier of crew.
Als passagier moet je de stoel kiezen waarop je zit. Hierbij zal je de gegevens van de passagier die op de stoel is geregistreerd te zien krijgen. Als crew gebruik je onderstaande login gegevens:

Gebruikersnaam:
```
Bram.De_Bleecker
```

Paswoord:
```
password-123
```
## Auteurs

* **Michiel Schoofs** - [michiel-schoofs](https://github.com/michiel-schoofs)
* **Tybo Vanderstraeten** - [TyboVanderstraeten](https://github.com/TyboVanderstraeten)
* **Lars Vandenberghe** - [LarsVandenberghe](https://github.com/LarsVandenberghe)

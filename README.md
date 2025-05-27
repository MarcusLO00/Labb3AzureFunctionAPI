# Azure Function REST API – Labb 3

# GET - Hämtar all snus i DB
/api/items 

# POST - Skapar en ny snus i DB

Anrop kräver Function Key

# Exempel med Postman

GET https://labb3azurefunction.azurewebsites.net/api/items?code=<din-key>

POST https://labb3azurefunction.azurewebsites.net/api/CreateItem?code=<din-key>
Content-Type: application/json

{
  "name": "Ett nytt snus",
  "amount": 69
}

Function Key finns i azure. Välj CreateItem / GetItems och sen klicka på "Function Keys", kopiera och placera in mellan <din-key> som visat ovan.

# BunkersAndBadasses

This is an unofficial [Loot Generator](https://bunkers-and-badasses-app.azurewebsites.net/) for 2K and Nerdvana Games <a href="https://nerdvanagames.myshopify.com/">Bunkers and Badasses</a> TTRPG Game. Feel free to use the data files for your own purpoises. (See what i did there?)

## Running it locally 
You'll need [Node.js](https://nodejs.org/en/download/) and the [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download).
This project was an excuse to play a bit with Blazor, so I recommend using Visual Studio Code in case you'd like to debug.

### Loot API

The `LootApi` project exposes the services in `Business/Services` as HTTP endpoints. The Blazor app and API both read the shared game data from the root `Configuration/` directory. Run the API locally over HTTPS with:

```shell
dotnet dev-certs https --trust
dotnet run --project LootApi
```

The API listens at `https://localhost:7443`; its OpenAPI document is available at `https://localhost:7443/openapi/v1.json` in Development. HTTP requests are redirected to HTTPS.

Build and run the API container from the repository root:

```shell
docker build -f LootApi/Dockerfile -t loot-api .
docker run --rm -p 8080:8080 loot-api
```

The container listens on HTTP port `8080`; terminate HTTPS at a trusted reverse proxy or hosting platform rather than placing a development certificate in the image.

If you want to compile a desktop version from a command line run the command

dotnet electronize build /target win /PublishReadyToRun true /Config Release

You can target win and linux as release targets

## Copyright
Copyright 2022 Alberto Malavassi Viales
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

<div align="center">
    <img src="https://logos-world.net/wp-content/uploads/2022/01/NET-Framework-Symbol.png" width="200" alt=".NET Framework Logo"/><br>
    <b style="font-size:25px">AlgoliaCrawler</b><br>
    <a href="https://dotnet.microsoft.com/en-us/download/dotnet/8.0"><img src="https://img.shields.io/badge/8-v8a0dc?label=.NET%20Core&style=flat&logo=dotnet" alt=".NET Core Logo"/></a>
    <a href="https://github.com/KeirLoire/AlgoliaCrawler/commits/main"><img src="https://github.com/keirLoire/AlgoliaCrawler/actions/workflows/ci.yml/badge.svg" alt="GitHub CI/CD"/></a>
</div>

## About

Crawls a website and sends pages to Algolia for indexing.

## Prerequisites
This binary requires prerequisites in order to run.

- None, all dependencies are embedded in the binary.

## Usage

- Configure appsettings.json by adding your Algolia Application ID, Write API Key, Index name, and the Website URL you want to crawl.
```json
    "Applications": [
      {
        "Id": "YourAlgoliaAppId",
        "WriteApiKey": "blablablabla",
        "Index": "relevance",
        "Url": "https://www.example.com",
        "Enabled": true
      }
    ]

```

- Run the binary

```powershell
.\AlgoliaCrawler.exe
```
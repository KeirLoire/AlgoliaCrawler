<div align="center">
    <img src="https://upload.wikimedia.org/wikipedia/commons/e/ee/.NET_Core_Logo.svg" width="150" alt=".NET Core Logo"/><br>
    <b style="font-size:25px">algolia-crawler</b><br>
    <a href="https://dotnet.microsoft.com/en-us/download/dotnet/8.0"><img src="https://img.shields.io/badge/8-v8a0dc?label=.NET%20Core&style=flat&logo=dotnet" alt=".NET Core Logo"/></a>
    <a href="https://github.com/KeirLoire/algolia-crawler/commits/main"><img src="https://github.com/keirLoire/algolia-crawler/actions/workflows/ci.yml/badge.svg" alt="GitHub CI/CD"/></a>
</div>

## About

Crawls a website and sends pages to Algolia for indexing.

The crawler used on this project is [AbotX](https://github.com/sjdirect/abotx).

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
# pbc-bot

A Telegram bot for the secret Post-Apocalyptic B. community. This community appeared in the beginning of 2016 in the [OnApp Ltd.](http://www.onapp.com) in the result of combined efforts of the vCloud & Application Server teams members.

### Available Commands

- ex - exchange currency. Format amount XXX in YYY
- ud - define word using Urban Dictionary
- go - search on DuckDuckGo (beta)
- dice - roll a basic dice
- l - find a location by address
- weekday - current day of week (in Europe, Kiev timezone)
- timein - gets time at specific location
- ball - ask Magic Ball a question
- weather - get weather in location (pas as argument)


### Goals

The main goal is to help all the members of the community to prepare to possible post-apocalyptic future of Earth.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. See deployment for notes on how to deploy the project on a live system.

### Prerequisites

- Development Tools
- .NET 5.0
- Docker
- Docker Compose

### Installing

In order to install the bot, you need
1. Get the bot-api-key from @BotFather
2. Get the Google Map api key
3. Add these keys and host (on which bot is running) to `appsettings.json` file
4. Fill the data about lunches account at the [OnApp Ltd.](http://www.onapp.com)

## Running the tests

Currently tests couldnot be running. It would be greate to make them working.

### Coding style tests

When you build the app, you can experience some warnings which relates to codding style. This tests were added in order to make the app maintainable and make all code look similar.

Currently I'm working on making all this tests passed.

## Deployment

For deploy using docker as a delivery tool. You need to build and publish the app. Then build the docker container, deliver it to the server with other configs (like nginx.conf) and run

```docker compose up -d```

## Built With

* [.NET Core](https://github.com/dotnet/core) - The framework used
* [Telegram.Bot](https://github.com/TelegramBots/telegram.bot) - Telegram Client for .NET Core
* [Api.Forex.Sharp](https://github.com/ApiForex/Api.Forex.Sharp) - Used for getting currency rates
* [GeoTimeZone](https://github.com/mj1856/GeoTimeZone) - Used for getting timezone by location

## Contributing

Please read [CONTRIBUTING.md](https://gist.github.com/PurpleBooth/b24679402957c63ec426) for details on our code of conduct, and the process for submitting pull requests to us.

## Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/deril/jewish-bot-c/tags).

## Authors

* **Dmytro Bihniak** - *Initial work* - [deril](https://github.com/deril)

See also the list of [contributors](https://github.com/deril/jewish-bot-c/contributors) who participated in this project.

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details

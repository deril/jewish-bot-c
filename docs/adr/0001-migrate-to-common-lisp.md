# ADR 0001: Rewrite the bot in Common Lisp, with an intent matcher for plain-text requests

- **Status:** Accepted
- **Date:** 2026-10-01
- **Deciders:** Dmytro Bihniak

## Context

### History

The bot has served the Post-Apocalyptic B. group chat since 2016:

| Years | Version | Repository |
|---|---|---|
| 2016 | Clojure | `deril/jewish-bot` |
| 2016–2022 | C# / ASP.NET Core, webhook | this repository (`deril/jewish-bot-c`) |

Commits per year in this repository: 2016: 7, 2017: 47, 2018: 18, 2019: 5, 2020: 14,
2021: 1, 2022: 5. The last commit is from 2022-01-29.

### Where it runs

Checked on 2026-10-01 on the Oracle Cloud server (`oracle`):

- The `jewish-bot` container has been up since 2024-01-03, from image
  `ghcr.io/deril/jewish-bot-c:0da9fac`. It sits behind Traefik at
  `https://jewishbot.derilok.xyz/WebHook/Post`, next to `ukrainiancuisine`.
- It runs .NET 6, which went out of support in November 2024. Telegram.Bot is pinned at
  17.0.0.
- **Usage is unknown.** The log level is `Warning`, so incoming messages (logged at
  `Information`) were never written. The whole log since 2024-01-03 is one startup line
  ("Webhook set on the server"). There are no `Cannot execute command` errors and no
  restarts. Traefik has no access log for the route.

### What forces a decision

Home apps are moving to Common Lisp
([UkrainianCuisine ADR 0001](https://github.com/deril/UkrainianCuisine/blob/master/docs/adr/0001-migrate-to-common-lisp.md),
and cleanslate, repository `deril/ourhome`), and hosting is moving to FreeBSD jails. A
Docker image of a .NET 6 app doesn't move there easily, so "leave it running" only lasts
as long as the current server.

### What we want from the bot

The bot stays. Besides porting it, the rewrite changes how people talk to it: the useful
requests should work as **plain text in the chat**, not only as commands.

| Today | Wanted |
|---|---|
| `/ex 200 uah in usd` | `200 uah in usd`, `200 грн в доларах` |
| (none) | `4 cup flour in grams`, `2 склянки борошна в грамах` |
| `/weather Kyiv` | `погода в Києві`, `weather in Lviv` |
| `/timein London` | `котра година в Лондоні`, `time in London` |

## Decision

Rewrite the bot as a small standalone Common Lisp program on **cl-telegram-bot2** with
**long polling**. Plain-text requests go through **one general intent matcher**; the
remaining playful commands stay `/commands`.

### Stack

| Concern | Choice | Notes |
|---|---|---|
| Implementation | SBCL | |
| Dependencies | qlot (`qlfile` + lock) | Pin `40ants/cl-telegram-bot` with a `github` line. The Quicklisp dist of 2026-01-01 predates v0.18+ |
| Telegram | [cl-telegram-bot2](https://40ants.com/cl-telegram-bot/) | Maintained (v0.21.0, 2026-03-10). The low-level API is generated from Telegram's JSON spec. Registers commands with Telegram, so the `/` menu stays in sync. Handles `/cmd@botname` in groups. Non-command text reaches the state's `:on-update` handler |
| Getting updates | **Long polling** (`start-polling`) | See *Why polling* |
| Plain text | Own intent matcher (`defintent`) | See *Intent matcher* |
| HTTP client | dexador (already pulled in by cl-telegram-bot2) | Weather, geocoding, rates, Urban Dictionary |
| Currency rates | [National Bank of Ukraine API](https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?json) | Free, no key, UAH rates for every currency. Cached once a day. Replaces Api.Forex |
| Time | local-time | `weekday`, `котра година в` |
| Config | Environment variables | `TELEGRAM_TOKEN`, `GOOGLE_API_KEY`. Nothing secret in the repository |
| Distribution | Docker image on `ghcr.io/deril/jewish-bot-c`, plus the plain binary | See *Docker image* |
| Tests | FiveAM | Intents and commands are tested without Telegram |
| Logging | log4cl at Information | Every matched intent and command is logged with chat id and name (not the message text), so usage can be measured from now on |

**A thin wrapper layer.** Each command and each intent's reply is a plain function from
its arguments to the reply text. One starting state registers the commands as
`global-command`s and sends other text to the matcher. This keeps cl-telegram-bot2's state
machine and actors out of the bot's own code: the docs say its high-level API "is still
incomplete and may change", and with this layer a breaking change touches one file.

### Commands and intents

| Today | Becomes | External dependency |
|---|---|---|
| `ex` | Intent `convert` (currency) | NBU API |
| (new) | Intent `convert` (units, ingredients) | Unit and density tables |
| `weather` | Intent `weather` | `wttr.in` |
| `timein` | Intent `time-in` | Google Geocoding + Google Time Zone API (the offline GeoTimeZone library has no CL equivalent) |
| `dice`, `ball`, `weekday`, `echo`, `hey` | `/commands`, as today | none |
| `ud` | `/ud`, as today | `api.urbandictionary.com`, kept if the API still answers |
| `l` | `/l`, as today | Google Maps Geocoding, kept if used |
| `go` | Dropped | The DuckDuckGo instant-answer API returns nothing for most queries |

### Intent matcher

A small ELIZA-style pattern matcher over token lists (as in Norvig's *Paradigms of AI
Programming*, chapter 5). Each intent declares patterns with **typed slots**:

```lisp
(defintent weather
  (:patterns ("погода" (:opt "в" "у") ?place)
             ("яка" "погода" (:opt "в" "у") ?place)
             ("weather" "in" ?place))
  (:slots (?place :rest))              ; the rest of the message
  (:reply (weather-for ?place)))

(defintent convert
  (:patterns (?amount ?from (:or "in" "to" "в" "у") ?to)
             (?amount ?from ?what (:or "in" "to" "в" "у") ?to))
  (:slots (?amount :number)            ; 200, 1,5, 1/2, ½
          (?from :unit) (?to :unit)    ; uah, грн, $, cup, склянка, г
          (?what :ingredient))         ; flour, борошно
  (:reply (convert ?amount ?from ?to :ingredient ?what)))
```

How a message is handled:

1. **Normalize and tokenize.** Lower-case, split `200грн` into `200 грн`, and read `1,5`,
   `1.5`, `1/2`, `½` and `1 1/2` as rationals.
2. **Match.** Literal words must match exactly; `(:opt …)` makes words optional and
   `(:or …)` gives synonyms. A typed slot only matches if its parser accepts the token:
   `:number` rejects "привіт", `:unit` and `:ingredient` look the word up in their tables,
   `:rest` takes the rest of the message.
3. **Choose.** Only matches that cover the **whole message** count. If several intents
   match, the one with the most literal words wins. If none match, the bot **stays quiet**.

Typed slots and whole-message matching are what keep the bot quiet in normal
conversation: "I spent 5 kg in total" doesn't match, because `total` is not a unit.

Rules for the data behind the slots:

- **Unit names** are listed with their word forms in English and Ukrainian (`гривня`,
  `гривні`, `гривень`; `долар`, `долари`, `доларів`, `доларах`; `склянка`, `склянки`,
  `склянок`). Listing forms is tedious but predictable; stem matching is a later option.
- **Units** are a fixed table of dimension and factor to a base unit (g, kg, ml, l, cup,
  tbsp, tsp, oz, lb), the same design as `src/units.lisp` in UkrainianCuisine.
- **Volume to weight** ("4 cup flour in grams") needs grams per cup for each ingredient.
  A table of 20–30 kitchen ingredients covers most questions. An unknown ingredient gets
  "I don't know the density of X", never a guess.
- **Ambiguous units** get one default, named in the reply: "4 cups (US, 240 ml) flour ≈
  500 g".
- **Places in Ukrainian cases.** `?place` captures "Києві", not "Київ". The bot tries the
  word as written first, then a few common case endings, before saying it can't find the
  place.

**Share the units with UkrainianCuisine.** Kitchen conversions and ingredient densities
are what the recipe app needs for scaling and merging lists. The unit and density tables
go into a small shared system that both projects load, instead of two copies.

### Telegram privacy mode

By default a bot in a group only receives commands, mentions and replies to itself. Plain
text needs privacy mode **turned off** in @BotFather (`/setprivacy` → Disable), followed by
removing the bot from the group and adding it back (or making it a group admin). The bot
then receives every message in the chat. It keeps none of them: unmatched text is dropped
and not logged.

### Docker image

The repository is public and the image is already published, so a Docker image stays the
supported way for anyone else to run the bot. The author's own deployment may use the
plain binary (a FreeBSD jail can't run Docker images); both come from the same build.

- **Multi-stage `Dockerfile`.** The build stage installs SBCL and qlot, runs
  `qlot install` from the lock file, runs the FiveAM suite and saves an executable with
  `sb-ext:save-lisp-and-die`. The final stage is a slim Debian image with the binary,
  OpenSSL (for `cl+ssl`) and CA certificates. No Lisp toolchain in the final image.
- **No ports, volumes or proxy labels.** With polling the container only makes outgoing
  connections. Configuration is environment variables only; the rates cache lives in
  memory.
- **`docker-compose.yml`** becomes one service with `env_file` and `restart`, as the
  example in the README.
- **A GitHub Actions workflow** builds the image on pushes to `master` and tags (tests run
  inside the build) and pushes it to `ghcr.io`, tagged with the commit SHA and `latest`.
  Today the image is built by hand. The CodeQL workflow (C#) is removed with the .NET code.
- **Multi-arch** (`linux/amd64`, `linux/arm64`) with `docker buildx`, since Oracle's free
  Ampere A1 shapes are arm64.

### Why polling

None of the Common Lisp Telegram libraries checked has a webhook receiver; all of them
only poll:

| Library | Last push | How it gets updates |
|---|---|---|
| 40ants/cl-telegram-bot (v2) | 2026-09 | `start-polling` |
| aartaka/cl-telegram-bot-auto-api | 2025-05 | `tga:start`, a `get-updates` loop |
| gzip4/cl-telebot | 2021 | `telebot:long-polling` |
| sovietspaceship and other v1 forks | 2019–2023 | Polling, unmaintained |

For one bot in a few chats, polling costs at most about a second of delay. In return:

- the Traefik route, TLS certificate, domain and open port go away;
- the bot only makes outgoing connections, which is the simplest setup for a FreeBSD jail;
- it uses only the library's supported, exported API.

## Options considered

### Platform

1. **Keep the .NET bot running as it is.** No work now. It stays on an unsupported runtime
   and can't follow the move to FreeBSD jails. Rejected.
2. **Upgrade to .NET 8+ and keep the webhook.** Probably the least work for a working bot.
   Rejected on preference: the author's home projects are moving to one stack, Common Lisp.
3. **Retire the bot.** Considered, because usage is unknown and the code hasn't changed
   since 2022. Rejected: the bot is wanted, and plain-text requests make it more useful
   than it was.

### Telegram library and transport

4. **cl-telegram-bot2 with a webhook.** A Clack/Ningle POST route that checks
   `X-Telegram-Bot-Api-Secret-Token`, parses the JSON into an `update` and passes it to the
   library's dispatch. It works in principle, but `process-update-in-actor`,
   `start-actors` and the JSON parser `parse-as` are internal (`pkg::name`) and can change
   in any release. Rejected.
5. **Own Telegram client** (dexador + a JSON library, `getUpdates` and `sendMessage`). About
   100 lines, with no 15+ dependency tree (sento, fset, 40ants-*). Kept as the **fallback**
   if cl-telegram-bot2 doesn't load cleanly or its threads misbehave (its own code has a
   TODO about threads not being cleaned up). It would also be the way to keep a webhook.
6. **cl-telegram-bot-auto-api.** A thinner generated binding, but older, less used, and
   it gives no command menu or command routing. Rejected.
7. **cl-telegram-bot2 with long polling.** Chosen.

### Understanding plain text

8. **A separate grammar per command** (cl-ppcre or esrap for each phrase). Fine for one or
   two phrases, but every new request means a new parser. Rejected in favour of 10.
9. **An LLM that turns the message into a structured request.** Handles free wording
   ("how many grams is a glass of rice") for free. Against it: every group message goes to
   an outside service, results vary between runs, each message costs money, and the unit
   tables and rates API are still needed because the model must not do the arithmetic or
   know the rates. Rejected as the main path. **Revisit** as a fallback only for messages
   that mention the bot and match no intent.
10. **One general intent matcher with typed slots.** Deterministic, testable, sends nothing
    outside, and a new intent is a few patterns plus a reply function. Chosen.

## Consequences

### Positive

- One stack with the other home apps; layout, qlot and FiveAM habits carry over.
- People ask in plain words, in English or Ukrainian, instead of remembering command syntax.
- New requests are cheap to add: a `defintent` with patterns and a reply function.
- Unit and density tables are shared with UkrainianCuisine.
- Less infrastructure: no reverse proxy route, certificate or public endpoint.
- Usage becomes visible, since matched intents and commands are logged.

### Negative / risks

- **Phrasing must be anticipated.** "а яка там завтра погода у Львові?" matches only if a
  pattern allows the extra words. Patterns grow with real use; truly free wording needs
  option 9.
- **False positives annoy a group chat.** Whole-message matching, typed slots and a test
  suite of everyday sentences that must not match are the safeguards.
- **The bot reads every group message** once privacy mode is off. It drops what doesn't
  match and doesn't log message text.
- **Data entry is most of the work**: word forms, densities, currency names and symbols.
- **Dependency weight.** cl-telegram-bot2 brings 15+ systems and an actor runtime for a
  bot that keeps no per-chat state. The wrapper layer and the fallback (option 5) limit the
  damage. One main maintainer (40ants); read the lock diff when updating.
- **External services may be gone** (Urban Dictionary, wttr.in). Commands without a
  working service are dropped, not ported.
- **Switching from webhook to polling needs one `deleteWebhook` call.** Telegram refuses
  `getUpdates` with 409 while a webhook is set.
- **One bot process only.** Two polling processes with the same token compete for updates.
  Stop the old container before starting the new one.

## Implementation plan

Estimated 4–5 focused days (36–45 h) in total. Phase 1 alone is about 1–1.5 days.

| Step | Estimate |
|---|---|
| Skeleton: `.asd`, `qlfile`, config, `start-polling`, logging | 2–3 h |
| Getting cl-telegram-bot2 to load and learning its state DSL | 2–4 h |
| Wrapper layer: commands, and text routed to the matcher | 1–2 h |
| `dice`, `ball`, `weekday`, `echo`, `hey` with FiveAM tests | 3–4 h |
| Intent matcher: tokenizer, numbers, `defintent`, matching and choosing, tests | ~1 day |
| `convert`: unit table, densities, word forms, NBU rates with a daily cache | 1 day |
| `weather` and `time-in` intents, place-name case fallback | 3–4 h |
| `ud`, `l` commands (if kept) | ~1 h each |
| Negative test suite: everyday sentences that must not match | 2–3 h |
| Docker image, compose example, GitHub Actions workflow | 3–4 h |
| Deploy on Oracle and cut over | 2–3 h |

Phases, one PR each:

1. **Skeleton and commands.** Runs locally with polling, tests green.
2. **Intent matcher** with the `convert` intent (currency first, then units and
   ingredients) and the negative test suite.
3. **More intents** (`weather`, `time-in`) and the remaining commands.
4. **Docker image and cutover on Oracle.** Add the `Dockerfile`, compose example and
   image workflow. Turn privacy mode off in @BotFather and re-add the bot to the group.
   Stop and remove the .NET container, call `deleteWebhook`, start the new image, remove
   the Traefik labels and the DNS record. Delete the .NET code, `nginx.conf`,
   `Dockerfile.nginx` and the CodeQL workflow in this PR; they stay in git history.
5. **Shared units system** with UkrainianCuisine, once its phase 3 (shopping lists) needs
   unit conversion.
6. **FreeBSD jail.** Planned separately, together with the other home apps.

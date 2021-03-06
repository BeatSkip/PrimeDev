
# PrimeDev

This is my attempt at re-imagining the development process for the HP Prime.
the goal is an easy web-based Connectivity kit and development IDE for both
python and PPL programs. Later on optimization for mobile and a program sharing 
feature would be nice.

## Status
Very alpha, right now you can connect to your prime and read the contents. note: this 'only' works for HP Prime G2 for now. G1 users, sorry, if i get more traction i might add support later on. although Connectivity kit 2022 is in the works, so let's see what the HP has in store for us.

demo version online at: https://primedev.polyform.wiki/

note: *USE AT YOUR OWN RISK; I'M NOT RESPONSIBLE FOR ANY BRICKING OF DEVICES! IT SHOULD BE FINE, BUT ESPECIALLY WITH THE G1, ALL BETS ARE OFF*

**Latest changes**

*24/03/2022* - Ported over Sysend.Blazor for initial architecture compatibility with future integrated Prime emulator

*13/03/2022* - starting to nail down final data handling, phasing out legacy experimental parsers

*10/03/2022* - internal finetuning, Variables tab added, More work on internal data formats

*01/03/2022* - complete rework of ui, see screenshot for result.

*27/02/2022* - Code editor layout, UI design, internal optimization, debugging

*25/02/2022* - Code editor fully implemented, hpapp file parsing partially done. enough to actually read text files from prime. added changelog (https://github.com/BeatSkip/PrimeDev/blob/master/Changelog.md)

*23/02/2022* - Designed all standard prime icons in SVG format. Fixed decompression.

*23/02/2022* - Fixed up Backup procedure and added all required data types with help from @Cyrille-de-Brebisson, slimmed down protocol code (again) removed connection success message

## Screenshots

![newui](https://github.com/BeatSkip/PrimeDev/blob/master/img/rework_ui.png?raw=true)

![intro](https://github.com/BeatSkip/PrimeDev/blob/master/img/screenshot_intro.png?raw=true)


![ide](https://github.com/BeatSkip/PrimeDev/blob/master/img/screenshot_ide1.png?raw=true)


![screenshot](https://github.com/BeatSkip/PrimeDev/blob/master/img/img_multipartcompressedtransfer.png?raw=true)


## Dependencies

- The communication library (PrimeWeb) is basically a copy of PrimeComm and it's my intention to adapt and extend this library to work with modern hardware and firmware
- The actual communication to the WebHID API is using a wonderfully written library called Blazm.HID


## Usage

for now just open and run, nothing too much interesting until i get the protocol working...

## Known issues

## Getting help
Feel free to post issues or to directly contact me, altough for the time being, everything is development in progress and is provided 'As-is'
If you have questions, concerns, bug reports, etc, please file an issue in this repository's Issue Tracker. or pitch it into the discussions

## Getting involved
If you want to get involved, great! please contact me and we'll think of something as there's still tonnes to be done

## Credits and references 

1. Thanks to the dev's whose libraries I generously based this code off of.

https://github.com/EngstromJimmy/Blazm.HID

https://github.com/eried/PrimeComm

https://github.com/debrouxl/hplp

2. Special thanks to Stefan Wolfrum for some insights

https://github.com/metawops

# PrimeDev

This is my attempt at re-imagining the development process for the HP Prime.
the goal is an easy web-based Connectivity kit and development IDE for both
python and PPL programs. Later on optimization for mobile and a program sharing 
feature would be nice.

## Status
Very very alpha, and currently working on the initial communication between the SPA and the prime itself..

**Latest changes**
- Initial bring-up
- Scan and discover USB HID devices
- Connect to single HP Prime via HID
- Send initial test message to actual hardware
- Receive data back from actual hardware
- Receive message, parse content and fire message received event
- Initial work done to reverse engineer V2 protocol
- Multipart compressed message transfer via V2 protocol!!
- Discover files on Prime
- Request calculator information and display on top bar
- Change protocol to undocumented V2 protocol
- Set up basics for completely reworked comms library
- receiving Screenshot from calculator with V2 Protocol working!
- ~~Sending multipart compressed messages!!!~~ (need to check back, compression seems to be broken again)
- 20/02/2022 - Started protocol Documentation [Prime protocol docs](https://github.com/BeatSkip/PrimeDev/wiki/HP-Prime---USB-HID-Packetizing-protocol)
- 20/02/2022 - Protocol handling refinements, intial design of SVG Icons, App discovery and enumeration
- 23/02/2022 - Fixed up Backup procedure and added all required data types with help from @Cyrille-de-Brebisson, slimmed down protocol code (again) removed connection success message
- 23/02/2022 - Designed all standard prime icons in SVG format. Fixed decompression. 




## Screenshots

![intro](https://github.com/BeatSkip/PrimeDev/blob/master/img/screenshot_intro.png?raw=true)


![return data](https://github.com/BeatSkip/PrimeDev/blob/master/img/screenshot_received.png?raw=true)


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
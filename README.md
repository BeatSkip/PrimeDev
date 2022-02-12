
# PrimeDev

  

**Description**: 
This is my attempt at re-imagining the development process for the HP Prime
the goal is to make an easy web-based Connectivity kit and development IDE for both
python and PPL programs. 

 

## Status

Very very alpha, and currently working on the initial communication between the SPA and the prime itself..

-  [x] Initial bring-up
-  [x] Scan and discover USB HID devices
-  [x] Connect to single HP Prime via HID
-  [x] Send initial test message to actual hardware
-  [x] Receive data back from actual hardware
-  [ ] Discover files on Prime

  
![initial proof](https://github.com/BeatSkip/PrimeDev/blob/master/img/photo_poc.jpg?raw=true)

## Screenshots

![intro](https://github.com/BeatSkip/PrimeDev/blob/master/img/screenshot_intro.png?raw=true)

![discovery](https://github.com/BeatSkip/PrimeDev/blob/master/img/screenshot_discovery.png?raw=true)

![return data](https://github.com/BeatSkip/PrimeDev/blob/master/img/screenshot_received.png?raw=true)



## Dependencies

- The communication library (PrimeWeb) is basically a copy of PrimeComm and it's my intention to adapt and extend this library to work with modern hardware and firmware
- The actual communication to the WebHID API is using a wonderfully written library called Blazm.HID


## Usage

for now just open and run, nothing too much interesting until i get the protocol working...

## Known issues

-

## Getting help

Feel free to post issues or to directly contact me, altough for the time being, everything is development in progress and is provided 'As-is'


If you have questions, concerns, bug reports, etc, please file an issue in this repository's Issue Tracker. or pitch it into the discussions

  

## Getting involved


If you want to get involved, great! please contact me and we'll think of something as there's still tonnes to be done


  
  

----

  

## Credits and references

  

1. Thanks to the dev's whose libraries i generously based this off of.

https://github.com/EngstromJimmy/Blazm.HID

https://github.com/eried/PrimeComm

https://github.com/debrouxl/hplp
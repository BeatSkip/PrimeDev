# PrimeDev

**Description**:  This is my attempt at re-imagining the development process for the HP Prime
the goal is to make an easy web-based Connectivity kit and development IDE for both
python and PPL programs. The current 

Other things to include:

  - **Technology stack**: Indicate the technological nature of the software, including primary programming language(s) and whether the software is intended as standalone or as a module in a framework or other ecosystem.
  - **Status**:  Very very alpha, and currently working on the initial communication between the SPA and the prime itself..
 
- [x] Initial bring-up
- [x] Scan and discover USB HID devices
- [x] Connect to single HP Prime via HID
- [ ] Send initial test message to actual hardware
- [ ] Discover files on Prime


**Screenshot**: 


## Dependencies

 - The communication library (PrimeWeb) is basically a copy of PrimeComm and it's my intention to adapt and extend this library to 
work with modern hardware and firmware

- The actual communication to the WebHID API is using a wonderfully written library called Blazm.HID



## Installation

T.B.D

## Configuration

T.B.D

## Usage

T.B.D

## Known issues

 -

## Getting help

Feel free to post issues or to directly contact me, altough for the time being, everything is development in progress and is provided 'As-is'

**Example**

If you have questions, concerns, bug reports, etc, please file an issue in this repository's Issue Tracker.

## Getting involved

If you want to get involved, great! please contact me and we'll think of something as there's still tonnes to be done


----

## Open source licensing info
1. [TERMS](TERMS.md)
2. [LICENSE](LICENSE)
3. [CFPB Source Code Policy](https://github.com/cfpb/source-code-policy/)


----

## Credits and references

1. Thanks to the dev's whose libraries i generously based this off of.
    https://github.com/EngstromJimmy/Blazm.HID
    https://github.com/eried/PrimeComm
    https://github.com/debrouxl/hplp

**27/02/2022** 
Code editor layout, UI design, internal optimization, debugging. Next step is saving edited data to prime!!

![ide](https://github.com/BeatSkip/PrimeDev/blob/master/img/screenshot_ide1.png?raw=true)

**25/02/2022**
Code editor fully implemented, hpapp file parsing partially done. enough to actually read text files from prime

![ide expanded](https://github.com/BeatSkip/PrimeDev/blob/master/img/screenshot_ide2.png?raw=true)

  

**23/02/2022**: Designed all standard prime icons in SVG format. Fixed decompression.

**23/02/2022**: Fixed up Backup procedure and added all required data types with help from @Cyrille-de-Brebisson, slimmed down protocol code (again) removed connection success message

**20/02/2022**:  Protocol handling refinements, intial design of SVG Icons, App discovery and enumeration

**20/02/2022**: Started protocol Documentation [Prime protocol docs](https://github.com/BeatSkip/PrimeDev/wiki/HP-Prime---USB-HID-Packetizing-protocol)


  
**08/02/2022 to 19/02/2022**:
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
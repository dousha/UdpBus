# UDP Bus - A UDP multiplexer

A simple library to forward UDP packets to different ports.

This piece of software is (originally) built for interconnecting multiple OSC applications (for VRChat) -- I couldn't find a proper way to do this so I figured I'll have to make one myself.

As for the initial draft version, it works. At least it could handle ~600 datagram/s without trouble. Performance-wise, it is not (yet) optimal.

The library basically works by CC-ing packets between downstream applications and upstream applications.

## UDPBusDriver - Graphical Frontend

Note: All UI strings are hard-coded right now. I18N support direly needed.

HolidaysApi
===========
[![Build status](https://ci.appveyor.com/api/projects/status/ykoj058ddh4ts52t?svg=true)](https://ci.appveyor.com/project/BjrnEinarBjarnes/holidaysapi)

[![Build Status](https://travis-ci.org/bjartwolf/HolidaysApi.svg?branch=Freya)](https://travis-ci.org/bjartwolf/HolidaysApi)


#Build and run 

## Windows
```
build.cmd
cd build
HolidaysApi.Server.exe 8000
```

## Linux
```
./build.sh
cd build
mono HolidaysApi.Server.exe 8000
```

## Check the api out
It's just not half-implemented yet because I am just learning Freya
The loopback address on my machine doesn't bind to localhost, only 127.0.0.1 works
Go to http://127.0.0.1:7000

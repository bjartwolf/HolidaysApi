namespace HolidaysApi.OwinApp
open Freya.Core
open HolidaysApi.App

type public EasterServer() =
            member __.Configuration () =
                  OwinAppFunc.ofFreya routes
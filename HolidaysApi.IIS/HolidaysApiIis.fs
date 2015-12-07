namespace HolidaysApi.Iis

open Owin
open Microsoft.Owin
open HolidaysApi.App

type Startup() =
    member x.Configuration(app: Owin.IAppBuilder) =
        app.Use(easterServer)

[<assembly: OwinStartup(typeof<Startup>)>]
do ()
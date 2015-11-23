open Microsoft.Owin.Hosting
open Arachne.Http
open Freya.Core
open System 
open Freya.Machine
open Freya.Machine.Router
open Arachne.Uri.Template 
open Freya.Router
open Freya.Machine.Extensions.Http
open Aklefdal.Holidays.HttpApi.Holidays

let mediaTypes = freya { return [ MediaType.Text ] }
let methods = freya { return [ GET ] }
let available = freya { return true }

let easter _ = freya {
    let! yearRaw = Freya.Lens.getPartial (Route.Atom_ "year")
    let couldParse, year = 
        match yearRaw with 
            | Some year -> Int32.TryParse year
            | None -> false, 2015
    return { Description = { Charset = Some Charset.Utf8
                                       Encodings = None
                                       MediaType = Some MediaType.Text
                                       Languages = None }
             Data = Text.Encoding.UTF8.GetBytes(sprintf "%A" (Seq.toList (Seq.take 5(HolidaysNO year)))) } }

let holidays _ = freya {
    return { Description = { Charset = Some Charset.Utf8
                                       Encodings = None
                                       MediaType = Some MediaType.Text
                                       Languages = None }
             Data = Text.Encoding.UTF8.GetBytes(sprintf "%A" (Seq.toList (Seq.take 3(HolidaysNO 2013)))) } }

let routeEaster = UriTemplate.Parse "/easter/{year}"
let routeHolidays = UriTemplate.Parse "/holidays/{country}/{year}"

let handlerEaster = freyaMachine { using http
                                   mediaTypesSupported mediaTypes
                                   methodsSupported methods
                                   serviceAvailable available
                                   handleOk easter } |> FreyaMachine.toPipeline

let holidaysHandler = freyaMachine { using http
                                     mediaTypesSupported mediaTypes
                                     methodsSupported methods
                                     serviceAvailable available
                                     handleOk holidays } |> FreyaMachine.toPipeline

let routes =
    freyaRouter { route (Methods [ GET ]) routeEaster handlerEaster
                  route (Methods [ GET ]) routeHolidays holidaysHandler } |> FreyaRouter.toPipeline

type EasterServer() =
    member __.Configuration () =
        OwinAppFunc.ofFreya (routes)

[<EntryPoint>]
let main _ =
    let _ = WebApp.Start<EasterServer> ("http://localhost:7000")
    let _ = System.Console.ReadLine ()
    0

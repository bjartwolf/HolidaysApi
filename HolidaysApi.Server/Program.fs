open Microsoft.Owin.Hosting
open Arachne.Http
open Freya.Core
open System 
open Freya.Machine
open Freya.Machine.Extensions.Http

open Aklefdal.Holidays.HttpApi.Holidays
// Properties
let mediaTypes = freya { return [ MediaType.Text ] } 
let methods = freya { return [ GET ] }
let available = freya { return true } // Handlers

let content _ = freya { 
    return { Description = { Charset = Some Charset.Utf8
                                       Encodings = None
                                       MediaType = Some MediaType.Text
                                       Languages = None }
             Data = Text.Encoding.UTF8.GetBytes(sprintf "%A" (Seq.toList (Seq.take 5(HolidaysNO 2015)))) } }
// Resource
let easter = freyaMachine { using http 
                            mediaTypesSupported mediaTypes
                            methodsSupported methods
                            serviceAvailable available
                            handleOk content } |> FreyaMachine.toPipeline

type EasterServer() =
    member __.Configuration () =
        OwinAppFunc.ofFreya (easter)

[<EntryPoint>]
let main _ = 
    let _ = WebApp.Start<EasterServer> ("http://localhost:7000")
    let _ = System.Console.ReadLine ()
    0
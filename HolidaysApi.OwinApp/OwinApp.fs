module HolidaysApi.App

open Arachne.Http
open Freya.Core
open System
open Freya.Machine
open Freya.Machine.Router
open Arachne.Uri.Template
open Freya.Router
open Freya.Machine.Extensions.Http
open Aklefdal.Holidays.HttpApi.Holidays
open Aklefdal.Holidays.HttpApi.Computus
open Aklefdal.Holidays.HttpApi
open Aklefdal.Holidays.HttpApi.CountryCode

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
             Data = Text.Encoding.UTF8.GetBytes(sprintf "%A"  (EasterDay year)) } }

let holidays _= freya {
    let! yearRaw = Freya.Lens.getPartial (Route.Atom_ "year")
    let couldParse, year =
        match yearRaw with
            | Some year -> Int32.TryParse year
            | None -> false, 2015
    let! countryRaw = Freya.Lens.getPartial (Route.Atom_ "country")
    let country = match countryRaw with
      | Some c -> match (CountryFromCode c) with
                      | Some c -> c
                      | None -> DefaultCountry
      | None -> DefaultCountry
    return { Description = { Charset = Some Charset.Utf8
                                       Encodings = None
                                       MediaType = Some MediaType.Text
                                       Languages = None }
             Data = Text.Encoding.UTF8.GetBytes(sprintf "%A" (Seq.toList (ForYear country year)))} }

let routeEaster = UriTemplate.Parse "/easter/{year}"
let routeHolidays = UriTemplate.Parse "/holidays/{country}/{year}"

let handler vacation = freyaMachine { using http
                                      mediaTypesSupported mediaTypes
                                      methodsSupported methods
                                      serviceAvailable available
                                      handleOk vacation } |> FreyaMachine.toPipeline

let routes =
    freyaRouter { route (Methods [ GET ]) routeEaster (handler easter)
                  route (Methods [ GET ]) routeHolidays (handler holidays) } |> FreyaRouter.toPipeline

type public EasterServer() =
            member __.Configuration () =
                  OwinAppFunc.ofFreya routes

let public easterServer =  OwinAppFunc.ofFreya routes
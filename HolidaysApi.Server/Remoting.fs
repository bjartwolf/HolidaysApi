namespace HolidaysApi.Server

open WebSharper
open System
open Aklefdal.Holidays.HttpApi.Computus 
module Server =

    [<Remote>]
    let DoSomething input =
        let findEaster x =
            let couldparse, year = Int32.TryParse(x)
            if couldparse && (year > 0) && year < 3000 then 
                (EasterDay year).ToString()
            else
                "A year between 0 and 3000 please" 
        async {
            return findEaster input
        }
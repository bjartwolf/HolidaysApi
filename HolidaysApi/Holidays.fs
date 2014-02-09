﻿namespace Aklefdal.Holidays.HttpApi

open System

module Holidays =
    let ForYear year =
        let easterday = Computus.EasterDay year
        seq {
            yield ("1. nyttårsdag", new DateTime(year, 1, 1))
            yield ("Palmesøndag", easterday.AddDays(float -7)) 
            yield ("Skjærtorsdag", easterday.AddDays(float -3)) 
            yield ("Langfredag", easterday.AddDays(float -2)) 
            yield ("1. påskedag", easterday) 
            yield ("2. påskedag", easterday.AddDays(float 1)) 
            yield ("Kristi Himmelfartsdag", easterday.AddDays(float 39)) 
            yield ("1. pinsedag", easterday.AddDays(float 49)) 
            yield ("2. pinsedag", easterday.AddDays(float 50)) 
            yield ("Offentlig høytidsdag", new DateTime(year, 5, 1)) 
            yield ("Grunnlovsdag", new DateTime(year, 5, 17)) 
            yield ("1. juledag", new DateTime(year, 12, 25)) 
            yield ("2. juledag", new DateTime(year, 12, 26)) 
            }
    
    let DatesForYear year =
        ForYear year |> Seq.map (fun (_, holiday) -> holiday)
    
    let IsHoliday(date : DateTime) =
        let holidays = DatesForYear date.Year
        Seq.exists (fun elem -> elem = date.Date) holidays

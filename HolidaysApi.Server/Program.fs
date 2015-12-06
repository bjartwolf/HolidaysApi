open System.Net
open Microsoft.Owin
open Microsoft.Owin.Hosting
open HolidaysApi.App

[<EntryPoint>]
let main [| port |] =
    let ip = "127.0.0.1"
    printf "Trying to start"
    printf "ip %s %s" ip port
    let hostname = Dns.GetHostName()
    printf "host %A" hostname
    let host = Dns.GetHostEntry(hostname)
    printf "%A" host
    for foo in host.AddressList do
        printf "ip %A" foo 
    printf "ip %s %s" ip port
    let _ = WebApp.Start<HolidaysApi.App.EasterServer> (sprintf "http://%s:%s" ip port)
    let _ = System.Console.ReadLine ()
    0

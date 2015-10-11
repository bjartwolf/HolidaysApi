namespace HolidaysApi.Server
open WebSharper.Html.Server
open WebSharper
open WebSharper.Sitelets
open Aklefdal.Holidays.HttpApi.Computus 
open Microsoft.Owin.Hosting

type EndPoint =
    | [<EndPoint "GET /">] Home

type ApiAction =
  | [<Method "GET"; CompiledName "easter">]
    GetEasterDate
  | [<Method "GET"; CompiledName "holidays">]
    GetHolidays

[<NamedUnionCases "result">]
type Result<'T> =
  | [<CompiledName "success">] Success of 'T
  | [<CompiledName "failure">] Failure of message: string

module Templating =
    open System.Web

    type Page =
        {
            Title : string
            MenuBar : list<Element>
            Body : list<Element>
        }

    let MainTemplate =
        Content.Template<Page>("~/Main.html")
            .With("title", fun x -> x.Title)
            .With("menubar", fun x -> x.MenuBar)
            .With("body", fun x -> x.Body)

    // Compute a menubar where the menu item for the given endpoint is active
    let MenuBar (ctx: Context<EndPoint>) endpoint =
        let ( => ) txt act =
             LI [if endpoint = act then yield Attr.Class "active"] -< [
                A [Attr.HRef (ctx.Link act)] -< [Text txt]
             ]
        [
            LI ["Home" => EndPoint.Home]
        ]

    let Main ctx endpoint title body : Async<Content<EndPoint>> =
        Content.WithTemplate MainTemplate
            {
                Title = title
                MenuBar = MenuBar ctx endpoint
                Body = body
            }

module Site =

    let HomePage ctx =
        Templating.Main ctx EndPoint.Home "Home" [
            H1 [Text "Say Hi to the server!"]
            Div [ClientSide <@ Client.Main() @>]
        ]

    [<Website>]
    let Main =
        Application.MultiPage (fun ctx action ->
            match action with
            | Home -> HomePage ctx
        )

module SelfHostedServer =

    open global.Owin
    open Microsoft.Owin.Hosting
    open Microsoft.Owin.StaticFiles
    open Microsoft.Owin.FileSystems
    open WebSharper.Owin

    type Startup() =
        member x.Configuration(app: Owin.IAppBuilder) =
                app.UseStaticFiles(
                            StaticFileOptions(
                                FileSystem = PhysicalFileSystem(".."))) |> ignore
                app.UseSitelet("..", Site.Main) |> ignore
 

    [<EntryPoint>]
    let Main args = 
            use server = WebApp.Start<Startup>("http://localhost:9000")
            stdin.ReadLine() |> ignore
            0
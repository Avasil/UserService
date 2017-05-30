namespace Main

open System
open System.Threading
open UserModel
open UserRepository
open UserRepository.UserRepositoryDb
open Controller
open Controller.UserController
open Suave
open MockRepository.MockRepository

module Main = 
    [<EntryPoint>]
    let main argv = 
        let user = {
            Username = "Piotr"
            Password = "123"
            Email = "mail@piotrgawrys.com"
            Street = "Piotrkowa"
            City = "Piotrkowo"
            PostCode = "12-345"
        }

        let result = mockRepositoryDb.Add user

        let user2 = {
            Username = "Konrad"
            Password = "123"
            Email = "mail@wajder.sznajder.com"
            Street = "Konradowa"
            City = "Konradowo"
            PostCode = "12-345"
        }
        let result2 = mockRepositoryDb.Add user2

        let cts = new CancellationTokenSource()
        let conf = { defaultConfig with cancellationToken = cts.Token }
        let listening, server = startWebServerAsync conf (userController mockRepositoryDb)

        Async.Start(server, cts.Token)
        printfn "Make requests now"
        Console.ReadKey true |> ignore

        cts.Cancel()

        0
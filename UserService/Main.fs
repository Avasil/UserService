namespace Main

open System
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

        startWebServer defaultConfig (userController mockRepositoryDb)
        0
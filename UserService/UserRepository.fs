namespace UserRepository

open UserModel
open System
open System.Collections.Generic
open System.Collections.Concurrent

type UserRepository = {
    Add : User -> Option<User>
    GetAll : unit -> seq<User>
    Find : string -> Option<User>
    Remove : string -> Option<User>
    Update : User -> Option<User>
    GetPassword : string -> Option<string>
}

module UserRepositoryDb = 

    let add user = None

    let getAll (key: unit) = 
        let u = {
            Username = ""
            Password = ""
            Email = ""
            Street = ""
            City = ""
            PostCode = ""
        }
        seq[u] 


    let find userId = None

    let remove userId = None
    
    let update user = None

    let getPassword userId = None

    let userRepositoryDb = {
        Add = add
        GetAll = getAll
        Find = find
        Remove = remove
        Update = update
        GetPassword = getPassword
    }
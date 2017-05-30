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
    UpdatePassword : string -> string -> Option<User>
    UpdateEmail : string -> string -> Option<User>
    UpdateStreet : string -> string -> Option<User>
    UpdateCity : string -> string -> Option<User>
    UpdatePostCode : string -> string -> Option<User>

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

    let updatePassword userId password  = None

    let updateEmail userId email = None

    let updateStreet userId street = None

    let updateCity userId city = None

    let updatePostCode userId postCode = None

    let userRepositoryDb = {
        Add = add
        GetAll = getAll
        Find = find
        Remove = remove
        Update = update
        UpdatePassword = updatePassword
        UpdateEmail = updateEmail
        UpdateStreet = updateStreet
        UpdateCity = updateCity
        UpdatePostCode = updatePostCode
    }
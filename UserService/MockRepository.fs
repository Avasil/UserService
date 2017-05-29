namespace MockRepository

open UserModel
open UserRepository
open System
open System.Collections.Generic
open System.Collections.Concurrent

module MockRepository = 
    let private users = new ConcurrentDictionary<string, User>()

    let add user =
        if users.TryAdd(user.Username, user) then
            Some(user)
        else 
            None

    let getAll (key: unit) = 
        users.Values |> Seq.cast

    let find userId = None

    let remove userId = None
    
    let update user = None

    let mockRepositoryDb = {
        Add = add
        GetAll = getAll
        Find = find
        Remove = remove
        Update = update
    }

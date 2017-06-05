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

    let find userId = 
        let (success, value) = users.TryGetValue(userId)
        if success then
            Some(value)
        else 
            None

    let remove userId = 
        let (success, value) = users.TryRemove(userId)
        if success then
            Some(value)
        else 
            None
    
    let update user = 
        let tryupdate current =
            if users.TryUpdate(user.Username, user, current) then
                Some(user)
            else 
                None
        find user.Username |> Option.bind tryupdate

    let getPassword userId = 
        let (success, value) = users.TryGetValue(userId)
        if success then
            Some(value.Password)
        else 
            None

    let mockRepositoryDb = {
        Add = add
        GetAll = getAll
        Find = find
        Remove = remove
        Update = update
        GetPassword = getPassword
    }

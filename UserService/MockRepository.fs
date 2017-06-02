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

    let updatePassword userId password  = 
        let tryupdate current =
            let newUser = {current with Password = password}
            if users.TryUpdate(userId, newUser, current) then
                Some(newUser)
            else 
                None
        find userId |> Option.bind tryupdate

    let getPassword userId = 
        let (success, value) = users.TryGetValue(userId)
        if success then
            Some(value.Password)
        else 
            None

    let updateEmail userId email  = 
        let tryupdate current =
            let newUser = {current with Email = email}
            if users.TryUpdate(userId, newUser, current) then
                Some(newUser)
            else 
                None
        find userId |> Option.bind tryupdate

    let updateStreet userId street = 
        let tryupdate current =
            let newUser = {current with Street = street}
            if users.TryUpdate(userId, newUser, current) then
                Some(newUser)
            else 
                None
        find userId |> Option.bind tryupdate

    let updateCity userId city = 
        let tryupdate current =
            let newUser = {current with City = city}
            if users.TryUpdate(userId, newUser, current) then
                Some(newUser)
            else 
                None
        find userId |> Option.bind tryupdate

    let updatePostCode userId postCode = 
        let tryupdate current =
            let newUser = {current with PostCode = postCode}
            if users.TryUpdate(userId, newUser, current) then
                Some(newUser)
            else 
                None
        find userId |> Option.bind tryupdate

    let mockRepositoryDb = {
        Add = add
        GetAll = getAll
        Find = find
        Remove = remove
        Update = update
        UpdatePassword = updatePassword
        GetPassword = getPassword
        UpdateEmail = updateEmail
        UpdateStreet = updateStreet
        UpdateCity = updateCity
        UpdatePostCode = updatePostCode
    }

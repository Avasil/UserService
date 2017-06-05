module MongoRepository

open UserModel
open UserRepository
open System
open System.Collections.Generic
open MongoDB.Bson
open MongoDB.Driver

module MongoRepository = 
    let private client = new MongoClient "mongodb://localhost"
 
    let private db = client.GetDatabase "ShopDB"
 
    let private collection = db.GetCollection<BsonDocument> "users"

    let private documentToUser (m: BsonDocument) = { 
        Username = m.GetElement("Username").Value.ToString()
        Password  = m.GetElement("Password").Value.ToString()
        Email  = m.GetElement("Email").Value.ToString()
        Street  = m.GetElement("Street").Value.ToString()
        City  = m.GetElement("City").Value.ToString()
        PostCode  = m.GetElement("PostCode").Value.ToString()
    }

    let private userToDocument (m: User) = BsonDocument ([ BsonElement("Username" , BsonValue.Create m.Username);
        BsonElement("Password" , BsonValue.Create m.Password );
        BsonElement("Email" , BsonValue.Create m.Email );
        BsonElement("Street" , BsonValue.Create m.Street );
        BsonElement("City" , BsonValue.Create m.City );
        BsonElement("PostCode" , BsonValue.Create m.PostCode )
    ])

    let private getQuery (filter: FilterDefinition<BsonDocument>) = 
        collection.Find(filter).ToListAsync()
        |> Async.AwaitTask
        |> Async.RunSynchronously
        |> Seq.map documentToUser

    let private exists (filter: FilterDefinition<BsonDocument>) = 
        collection.Find(filter).ToListAsync()
        |> Async.AwaitTask
        |> Async.RunSynchronously
        |> Seq.length
        |> (fun n -> n > 0)

    let private userQuery userId = 
        FilterDefinition<BsonDocument>.op_Implicit ("{\"Username\": \""+userId+"\" }")

    let private existsUser username = 
        userQuery username 
        |> exists

    let private existsPassword username password = 
        FilterDefinition<BsonDocument>.op_Implicit ("{\"Username\": \""+username+"\", \"Password\": \""+password+"\"}")
        |> exists

    let add user =
        if not (existsUser user.Username) then
            userToDocument user
            |> collection.InsertOne
            |> ignore
            Some(user)
        else 
            None

    let getAll (key: unit) = 
        let all = FilterDefinition<BsonDocument>.Empty
        getQuery(all)

    let find username = 
        getQuery (userQuery username)
        |> Seq.tryHead

    let remove userId = 
        userQuery userId
        |> collection.DeleteOne 
        |> ignore
        None
    
    let update user = 
        if (existsUser user.Username) then
            userToDocument user
            |> collection.InsertOne 
            |> ignore
            Some(user)
        else 
            None

    let getPassword userId = 
        find userId
        |> Option.map (fun u -> u.Password)

    let mongoRepositoryDb = {
        Add = add
        GetAll = getAll
        Find = find
        Remove = remove
        Update = update
        GetPassword = getPassword
    }
    

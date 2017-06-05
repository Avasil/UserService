namespace Controller

open Newtonsoft.Json
open Newtonsoft.Json.Serialization
open Suave
open Suave.Filters
open Suave.Operators
open UserModel
open UserRepository

module Controller = 
    let fromJson<'a> json =
        let obj = JsonConvert.DeserializeObject(json, typeof<'a>) 
        if isNull obj then
            None
        else
            Some(obj :?> 'a)

    let getResourceFromReq<'a> (req : HttpRequest) =
        let getString rawForm =
            System.Text.Encoding.UTF8.GetString(rawForm)
        req.rawForm |> getString |> fromJson<'a>

    let JSON value =
        let settings = new JsonSerializerSettings()
        settings.ContractResolver <- new CamelCasePropertyNamesContractResolver()

        JsonConvert.SerializeObject(value, settings)
        |> Successful.OK
        >=> Writers.setMimeType "application/json; charset=utf-8"
    
    let handleResource f requestError = function
        | Some r -> r |> f
        | _ -> requestError

    let handleResourceBADREQUEST = 
        (fun f -> handleResource f (RequestErrors.BAD_REQUEST "No Resource from request"))

    let handleResourceNOTFOUND = 
        (fun f -> handleResource f (RequestErrors.NOT_FOUND "Resource not found"))

    let handleResourceCONFLICT = 
        (fun f -> handleResource f (RequestErrors.CONFLICT "Resource already exists"))
  
module UserController = 

    let completeRequest result = 
        request (Controller.getResourceFromReq >> (Controller.handleResourceBADREQUEST result))

    let getAll db =
        warbler (fun _ -> db.GetAll() |> Controller.JSON)
    
    let find db =
        db.Find 
        >> (Controller.handleResourceNOTFOUND Controller.JSON)

    let add db =
        let addDb =
            db.Add 
            >> (Controller.handleResourceCONFLICT Controller.JSON)
        completeRequest addDb
    
    let update db key =
        let updateDb =
            db.Update
            >> (Controller.handleResourceNOTFOUND Controller.JSON)
        completeRequest updateDb

    let remove db =
        db.Remove 
        >> (Controller.handleResourceNOTFOUND Controller.JSON)

    let getPassword db =
        db.GetPassword
        >> (Controller.handleResourceNOTFOUND Controller.JSON)

    let userController (db:UserRepository) = 
        pathStarts "/api/users" >=> choose [
            DELETE >=> pathScan "/api/users/%s" (remove db)
            GET >=> pathScan "/api/users/%s/password" (getPassword db)
            PUT >=> pathScan "/api/users/%s" (update db)  
            POST >=> path "/api/users" >=> (add db)
            GET >=> path "/api/users" >=> (getAll db)
            GET >=> pathScan "/api/users/%s" (find db)
        ]


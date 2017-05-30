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

    let updatePassword db key =
        let updatePassword = 
            db.UpdatePassword key
            >> (Controller.handleResourceNOTFOUND Controller.JSON)
        completeRequest updatePassword

    let updateEmail db key =
        let updateEmail = 
            db.UpdateEmail key
            >> (Controller.handleResourceNOTFOUND Controller.JSON)
        completeRequest updateEmail

    let updateStreet db key =
        let updateStreet = 
            db.UpdateStreet key
            >> (Controller.handleResourceNOTFOUND Controller.JSON)
        completeRequest updateStreet

    let updateCity db key =
        let updateCity = 
            db.UpdateCity key
            >> (Controller.handleResourceNOTFOUND Controller.JSON)
        completeRequest updateCity

    let updatePostCode db key =
        let updatePostCode = 
            db.UpdatePostCode key
            >> (Controller.handleResourceNOTFOUND Controller.JSON)
        completeRequest updatePostCode

    let userController (db:UserRepository) = 
        pathStarts "/api/users" >=> choose [
            POST >=> path "/api/users" >=> (add db)
            GET >=> path "/api/users" >=> (getAll db)
            GET >=> pathScan "/api/users/%s" (find db)
            DELETE >=> pathScan "/api/users/%s" (remove db)
            PUT >=> pathScan "/api/users/%s/password" (updatePassword db)
            PUT >=> pathScan "/api/users/%s/email" (updateEmail db)
            PUT >=> pathScan "/api/users/%s/street" (updateStreet db)
            PUT >=> pathScan "/api/users/%s/city" (updateCity db)
            PUT >=> pathScan "/api/users/%s/postcode" (updatePostCode db)
            PUT >=> pathScan "/api/users/%s" (update db)  
        ]


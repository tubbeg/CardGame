module BattleScene
open PhaserUtility
open Fable.Core.JsInterop
open GameplayLoop
open Fable.Core
open Browser.Dom
open GameTypes
open Browser.Types

let myEmitterConfig = createObj [
    "speed" ==> 100
    "scale" ==> createObj [
        "start" ==> 1
        "end" ==> 0
    ]
    "blendMode" ==> "ADD"
]



let callbackTintSet (sprite : Sprite)  =
    sprite.setTint(0xff0000)
    EndTurnPress
 
let callbackTintClear (sprite : Sprite) : PlayerInput  =
    sprite.clearTint()
    EndTurnPress
(* 
let callbackTintSet2 (sprite : Sprite)  =
    sprite.setTint(0xff0000)
    PlayCard(Some(Card1), None)
 
let callbackTintClear2 (sprite : Sprite) : PlayerInput  =
    sprite.clearTint()
    PlayCard(Some(Card1), None)
*)

//use vscode launch with js debug

(*
    Adding JSX elements to Phaser turned out to be quite messy. So
    for now i'll work with simple html elements and css styling.

    I might try JSX later.
*)





//todo 

//add win and lose conditions

//add multiple enemies

//[<Import("updateHealthbarValue", "./healthbar.js")>]


//value is not available for HTMLElement, which is why we need emit
//an alternative would be to import a helper file/function
[<Emit("$0.value = $1")>]
let updateHealthbarValue (element : HTMLElement) (newval : int) = jsNative


let createZone(zone : GameObject) = (
    zone.setInteractive()
    zone.input.dropZone <- true
)


let updateProgressBar (root : option<DOMElement>) newvalue =
    match root with
    | None -> console.log("error! missing root element!")
    | Some(r) ->
        match (r.getChildByID "hp-bar") with
        | None -> console.log("error! missing HTML element!")
        | Some(element) ->
            updateHealthbarValue element newvalue


type Coordinate = int*int
//type ElementRecord = {Location:Coordinate;Character:NPC;Element:DOMElement;}
//type ElementTable = ElementRecord list
type Avatar =
    | Element of DOMElement
    | Image of GameObject
type InterfaceInfo =
    {Location:option<Position>
    ;Avatar:Coordinate*option<Avatar>
    ;Bar:Coordinate*option<DOMElement>}
and PositionMap = option<InterfaceInfo>
let hasPosition emap (enemy : NPC) =
    match emap with
    | Some(map) ->
        match enemy.Location with
        | None -> false
        | Some(location) ->
            match map.Location with
            | Some(l) when (l=location) -> true
            | _ -> false
    | _ ->
        console.log("Error! Missing position map!")
        false

let getPosMap (enemy : NPC) (mlist : PositionMap list) : option<PositionMap> =
    try
        let pm =
            mlist |>
            List.find(fun n -> hasPosition n enemy)
        Some(pm)
    with
        ex ->
            eprintfn "failure in getting position! %A" ex
            None

let cardKey = "card"
let locationKey = "location"

let setGameObjectLocation (object : GameObject) (npc : NPC) : unit =
    //console.log("Setting game location: " + npc.Location.ToString())
    let data = createObj [
        locationKey ==> npc.Location
    ]
    object.setData data

let setGameObjectCard (object : GameObject) (c : Card) : unit =
    let data = createObj [
        cardKey ==> c
    ]
    object.setData data

let setGameObjectZone (object : GameObject) : unit =
    let data = createObj [
        "zone" ==> "zone"
    ]
    object.setData data


let determineCard (gameobj : GameObject) : option<Card> =
    gameobj.getData(cardKey)

let determineTarget (object : GameObject) : option<Position> =
    match object.getData(locationKey) with
    | Some(p) -> p
    | None -> 
        match object.getData("zone") with
        | Some(z) -> Some(AllCharacters)
        | _ ->
            console.error("Missing game data!")
            None


type BattleScene() =
    class
        inherit Scene()
        let machine = new BattleStateMachine("bob the dude")
        //let mutable myPlayerInput : PlayerInput = GameTypes.PlayerInput.Idle

        //let mutable NPCtoUI : ElementTable = []

        //static coordinates for npc's, this limitation also means
        //that the number of enemies has to be static. But this is acceptable
        let mutable playerCoordinate : PositionMap =
            Some
                {Location=Some(PlayerPosition)
                ;Avatar=(300,400), None
                ;Bar=(300,300),None}
        let mutable enemyCoordinates = 
            [ Some{Location=Some(Position2);Avatar=(800, 300), None;Bar=(800,200), None}
            ; Some{Location=Some(Position3);Avatar=(800,500), None;Bar=(800,400), None}
            ; Some{Location=Some(Position4);Avatar=(800,700), None;Bar=(800,600), None}
            ; Some{Location=Some(Position4);Avatar=(800,900), None;Bar=(800,800), None}
            ; Some{Location=Some(Position4);Avatar=(800,1100), None;Bar=(800,1000), None}]

        let centerCoordinate : PositionMap =
            Some
                {Location=Some(AllCharacters)
                ;Avatar=(300,400), None
                ;Bar=(300,300),None}
        let endturnCoordinate =  (300,450)

        let battleResultCoordinate  = (450,450)
        let mutable battleResult = None

        let cardsCoordinate = 750, 700

        member this.createButton (element : GameObject) =
            let func input = this.updateUI input
            element.setInteractive()
            element.on "pointerup" (System.Func<_,_> (fun () -> func (Some EndTurnRelease)))
        //this should be a member function which updates the UI based on machine data
        member this.updateUI(action) =
            let updateResult result =
                match battleResult with
                | None ->
                    battleResult <- this.addWinLoseUI result
                | _ -> ()
            match machine.update(action) with
                | Some(s,t,p,enemies) ->
                    this.updateHealthbars p enemies
                    match s with
                    | Some(Running) ->
                        ()
                    | Some(Win) ->
                        updateResult "win"
                    | Some(GameOver) ->
                        updateResult "gameoverman"
                    | None ->
                        console.error("Cannot update UI because the state machine is uninitialized!")
                        ()
                    
                | _ ->
                    console.error("error! in update ui to statemachine!")

        member this.addWinLoseUI (status : string) : option<DOMElement> =
            Some((this.add.dom battleResultCoordinate).createFromCache status)

            
        
        member this.updateHealthbars player (enemies : Enemies) =
            let disableNPConDeath posmap npc =
                let _, avatarRoot = posmap.Avatar
                match npc.Life, avatarRoot with
                | Dead, Some(Image(r)) ->  
                    r.removeInteractive()
                | _ ->
                    ()
            enemies |> List.iter(
                fun e ->   
                    match (getPosMap e enemyCoordinates) with
                    | Some(Some(m)) ->
                        let _, root = m.Bar
                        disableNPConDeath m e
                        updateProgressBar root e.Health
                    | _ -> console.error("error in getting map!")
            )
            match playerCoordinate with
            | Some (pc) ->
                let _,playerroot = pc.Bar
                disableNPConDeath pc player
                updateProgressBar playerroot player.Health
            | _ -> console.error("Error! Player position map is missing!")
        member this.createCard (x, y) key card =
            console.log("Reminder to add different elements/images for each card")
            let myCard = this.add.image (x, y, key)
            myCard.setInteractive()
            this.input.setDraggable(myCard)
            setGameObjectCard myCard card
            //console.log("Created card")
            myCard
        
        override this.preload() =
            this.load.image "battleroom" "assets/platform.png"
            this.load.image "card" "assets/star.png"
            this.load.image "button" "assets/star.png"
            this.load.html ("healthbar", "assets/healthbar.html")
            this.load.html ("gameoverman", "assets/lose.html")
            this.load.html ("win", "assets/win.html")
            this.load.html ("endturn", "assets/endturn.html")
            this.load.html ("enemy", "assets/enemy.html")

        member this.recreateNPC (p : PositionMap) (npc : NPC) (avatarkey) : PositionMap =
            match p with 
            | Some(pmap) ->
                let barcoord, _ = pmap.Bar
                let bar = (this.add.dom barcoord).createFromCache "healthbar"
                let avatarcoord, _ = pmap.Avatar
                let x,y = avatarcoord
                let avatar = (this.add.image (x,y,avatarkey))
                setGameObjectLocation avatar npc
                //let avatar = (this.add.dom avatarcoord).createFromCache avatarkey
                match npc.Type with
                | Enemy -> (avatar |> createZone)
                | _ -> ()
                Some ({Location=pmap.Location
                ; Avatar=avatarcoord, Some(Image avatar)
                ; Bar=barcoord, Some(bar)})
            | _ -> None

        member this.initEnemies (enemies : Enemies) =
            let createForEachNPC npc = 
                match (getPosMap npc enemyCoordinates) with
                | Some(p) ->
                    this.recreateNPC p npc "card"
                | None ->
                    console.error("Missing position map!")
                    None
            enemyCoordinates <- enemies |> List.map(fun e -> createForEachNPC e)
            //NPCtoUI <- newlist
        member this.initPlayer (player : Player) =
            console.log("Reminder to fix custom image for the player")
            playerCoordinate <- this.recreateNPC playerCoordinate player "card"
        member this.initCards() =
            //the state machine is responsible for managing decks and classes
            //(essentially all game data, or non-ui stuff)
            let deck = machine.generateDeck(machine.getClass())
            console.log("Deck is: ")
            console.log(deck)
            match deck with
            | None -> console.error("Error! In recieving generated deck!")
            | Some(d) ->
                d |> List.iter(fun c ->
                    (this.createCard cardsCoordinate "card" c) |> ignore
                )
        
        member this.initEndTurn() = 
            let el = (this.add.dom endturnCoordinate).createFromCache "endturn"
            this.createButton el
            
        //probably going to need a table for keeping track of each UI element to each
        //record
        override this.create() =
            //create healthbars
            let gameData = machine.initialize(None)
            let createUIforNPC (data : GameData) =
                match data with
                | None -> console.log("Error! Missing game data!")
                | Some(d) ->
                    let status, turn, player, enemies = d
                    this.initPlayer player
                    this.initEnemies enemies
                    this.updateHealthbars player enemies
                    this.addDragAndDrop(None)
                    this.initEndTurn()
                    this.initCards()
            createUIforNPC gameData
        member this.addDragAndDrop(config : option<int>) =
            
            //cards which affect all enemies, or all characters needs a dropzone in the
            //center of the scene

            //what makes this a bit more complicated is that which dropzone that can
            //be used depends on the card

            //all-cards should play if

            //enemy-specific target cards should retract to hand if no target or an all
            //target is selected

            //create a dropzone specifically for Position.AllEnemies and Position.AllCharacters
            let addCenterZone() =
                match centerCoordinate with
                | Some(c) ->
                    let (x,y), _ = c.Avatar
                    let newObject = this.add.zone x y 500 500
                    newObject |> createZone
                    setGameObjectZone newObject
                | _ -> console.error("Failed to create center zone!")
            addCenterZone()

            let dragcb (pointer : unit) (gameObject : GameObject) dragX dragY =
                gameObject.x <- dragX
                gameObject.y <- dragY
            let dropcb (pointer : Pointer) (gameObject : GameObject) (dropZone : Zone) =
                //should only trigger if drop occurs
                
                console.log("Reminder to remove dropzones for dead enemies")
                let target = determineTarget dropZone
                let card = determineCard gameObject
                this.updateUI(Some (PlayCard (card, target)))
            this.input.on "drag" (System.Func<_,_,_,_,_> dragcb)
            this.input.on "drop" (System.Func<_,_,_,_> dropcb)


            //player idling should not trigger a state machine update
            //let updatedState : option<GameplayStates> =

            //enemy turns are no longer states, not sure if this is correct

            //but the program seems to behave in correct way

        (*
            Only player input should trigger a state machine update. Otherwise,
            the update function will have to do a lot of unnecessary lifting.

            

            Also another basic tip, edit the generated js files if you want to
            do some prototyping. This way you can figure out what you need, without
            defining types. And then later append types to F# (which makes it
            slightly easier to scale the application)

        *)


    end

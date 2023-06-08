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
    
let callbackTintSet2 (sprite : Sprite)  =
    sprite.setTint(0xff0000)
    PlayCard(Card1, None)
 
let callbackTintClear2 (sprite : Sprite) : PlayerInput  =
    sprite.clearTint()
    PlayCard(Card1, None)



let myCallback() = (
    console.log("using callback")
)

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

let errorNPC: NPC = {Id="Error";Health=(-1);Mana=(-1);Location=None;Type=Player}

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
type PositionMap =
    {Location:option<Position>
    ;Avatar:Coordinate*option<Avatar>
    ;Bar:Coordinate*option<DOMElement>}
let hasPosition emap (enemy : NPC) =
    match enemy.Location with
    | None -> false
    | Some(location) ->
        match emap.Location with
        | Some(l) when (l=location) -> true
        | _ -> false

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




type BattleScene() =
    class
        inherit Scene()
        let machine = new BattleStateMachine("bob the dude")
        //let mutable myPlayerInput : PlayerInput = GameTypes.PlayerInput.Idle

        //let mutable NPCtoUI : ElementTable = []

        //static coordinates for npc's, this limitation also means
        //that the number of enemies has to be static. But this is acceptable
        let mutable playerCoordinate =
            {Location=Some(Position1);Avatar=(300,400), None;Bar=(300,300),None}
        let mutable enemyCoordinates = 
            [ {Location=Some(Position2);Avatar=(800, 300), None;Bar=(800,200), None}
            ; {Location=Some(Position3);Avatar=(800,500), None;Bar=(800,400), None}
            ; {Location=Some(Position4);Avatar=(800,700), None;Bar=(800,600), None}]
        let endturnCoordinate =  (450,450)

        let cardsCoordinate = (450,800)

        //let createCards (numberOfCards : int) (gameClass : PlayableClasses) : Card list =
        //    [for i in 0 .. numberOfCards -> (Card1)]

        member this.createButton (element : GameObject) =
            element.setInteractive()
            element.on "pointerup" (System.Func<_,_> (fun () ->
                this.updateUI(Some EndTurnRelease)))
        //this should be a member function which updates the UI based on machine data
        member this.updateUI(action) =
            match machine.update(action) with
                | Some(s,t,p,enemies) ->
                    match s with
                    | Running ->
                        match p, enemies with
                        | p, enemies -> //this will break
                            this.updateHealthbars  p enemies
                        //| _ ->  console.log("missing health property!")
                    | Win ->
                        console.log("player won the game!")
                        console.log("note to self: fix UI elements for win/lose conditions")
                    | GameOver ->
                        console.log("player lost the game :/")
                        console.log("note to self: fix UI elements for win/lose conditions")
                    
                | None ->
                    console.log("error! in update ui to statemachine!")
        
        member this.updateHealthbars player (enemies : Enemies) =
            enemies |> List.iter(
                fun e ->   
                    match (getPosMap e enemyCoordinates) with
                    | None -> console.log("error in getting map!")
                    | Some(m) ->
                        let _, root = m.Bar
                        updateProgressBar root e.Health
            )
            let _,playerroot = playerCoordinate.Bar
            updateProgressBar playerroot player.Health
        member this.createCard x y key = (
            let myCard = this.add.image (x, y, key)
            myCard.setInteractive()
            this.input.setDraggable(myCard)
            myCard
        )
        override this.preload() =
            this.load.image "battleroom" "assets/platform.png"
            this.load.image "card" "assets/star.png"
            this.load.image "button" "assets/star.png"
            this.load.html ("healthbar", "assets/healthbar.html")
            this.load.html ("gameoverman", "assets/lose.html")
            this.load.html ("win", "assets/win.html")
            this.load.html ("endturn", "assets/endturn.html")
            this.load.html ("enemy", "assets/enemy.html")

        member this.recreateNPC (p : PositionMap) (npc : NPC) avatarkey =
            let barcoord, _ = p.Bar
            let bar = (this.add.dom barcoord).createFromCache "healthbar"
            let avatarcoord, _ = p.Avatar
            let x,y = avatarcoord
            let avatar = (this.add.image (x,y,avatarkey))
            //let avatar = (this.add.dom avatarcoord).createFromCache avatarkey
            match npc.Type with
            | Enemy -> (avatar |> createZone)
            | _ -> ()
            {Location=p.Location; Avatar=avatarcoord, Some(Image avatar); Bar=barcoord, Some(bar)}

        member this.initEnemies (enemies : Enemies) =
            let newlist : PositionMap list = enemies |> List.map(fun e ->
                let pm = getPosMap e enemyCoordinates
                match pm with
                | Some(p) ->
                    this.recreateNPC p e "card"
                | None -> 
                    {Location=None; Avatar=(0,0), None; Bar=(0,0), None}
                )
            enemyCoordinates <- newlist
            //NPCtoUI <- newlist
        member this.initPlayer (player : Player) =
            playerCoordinate <- this.recreateNPC playerCoordinate player "card"
            ()
        member this.initCards() =
            //this function should be affected by playable class as well as
            let cards = [for i in 0 .. 30 -> (this.createCard (60+i+5) (60+i) "card")]
            ()

        member this.initEndTurn() = 
            let el = (this.add.dom endturnCoordinate).createFromCache "endturn"
            this.createButton el
            

        

        //probably going to need a table for keeping track of each UI element to each
        //record
        override this.create() =
            //create healthbars
            let gameData : GameData = machine.initialize(None)
            let createUIforNPC (data : GameData) =
                match data with
                | None -> console.log("Error! Missing game data!")
                | Some(d) ->
                    let status, turn, player, enemies = d
                    //create ui elements for each player and npc
                    this.initPlayer player
                    this.initEnemies enemies
                    //each enemy npc has to be its own dropzone
                    //createZone(this.add.image (400, 300, "battleroom"))
                    //let enemyhp, playerhp =
                     //   (this.add.dom (600, 600)).createFromCache("healthbar"), (this.add.dom (500, 500)).createFromCache("healthbar")
                    this.addDragAndDrop(None)
                    //createButton  ((this.add.dom 900 400).createFromCache "endturn")enemyhp playerhp
                    this.initEndTurn()
                    this.initCards()
            createUIforNPC gameData
        member this.addDragAndDrop(config : option<int>) =
            
            let dragcb (pointer : unit) (gameObject : GameObject) dragX dragY =
                gameObject.x <- dragX
                gameObject.y <- dragY
            let dropcb (pointer : Pointer) (gameObject : GameObject) dropZone =
                //should only trigger if drop occurs
                //console.log(pointer.downTime)
                this.updateUI(Some (PlayCard (Card1, None)))
            this.input.on "drag" (System.Func<_,_,_,_,_> dragcb)
            this.input.on "drop" (System.Func<_,_,_,_> dropcb)
        //override this.update() = ()
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

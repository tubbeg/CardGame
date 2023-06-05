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
    PlayCard(Card1)
 
let callbackTintClear2 (sprite : Sprite) : PlayerInput  =
    sprite.clearTint()
    PlayCard(Card1)



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

let (errorNPC : NPC) = {Id="Error";Health=(-1);Mana=(-1)}

let updateProgressBar (root : DOMElement) newvalue =
    let bar = root.getChildByID "hp-bar"
    match bar with
    | None -> console.log("error! missing HTML element!")
    | Some(element) ->
        updateHealthbarValue element newvalue


type BattleScene() =
    class
        inherit Scene()
        let machine = new BattleStateMachine("bob the dude")
        let mutable myPlayerInput : PlayerInput = GameTypes.PlayerInput.Idle
        let createCards (numberOfCards : int) (gameClass : PlayableClasses) : Card list =
            [for i in 0 .. numberOfCards -> (Card1)]


        let dummyFunctionGetFirstEnemy (enemies : NPC list) =
            console.log("reminder to remove this function and actually fix the problem")
            match enemies with
            | e::_ -> e
            | _ ->
                console.log("no enemies detected in list")
                errorNPC

        let updateHealthbars ebar pbar player (enemies : NPC list) =
            let e = dummyFunctionGetFirstEnemy enemies
            updateProgressBar pbar player.Health
            updateProgressBar ebar e.Health
        let updateUI(enemybar, playerbar, action) =
            match machine.update(action) with
                | Some(s,t,p,enemies) ->
                    match s with
                    | Running ->
                        match p, enemies with
                        | p, enemies -> //this will break
                            updateHealthbars enemybar playerbar p enemies
                        //| _ ->  console.log("missing health property!")
                    | Win ->
                        console.log("player won the game!")
                        console.log("note to self: fix UI elements for win/lose conditions")
                    | GameOver ->
                        console.log("player lost the game :/")
                        console.log("note to self: fix UI elements for win/lose conditions")
                    
                | None ->
                    console.log("error! in update ui to statemachine!")
        

        let createButton (element : GameObject)  ebar pbar =
            element.setInteractive()
            element.on "pointerup" (System.Func<_,_> (fun () ->
                updateUI(ebar, pbar, Some EndTurnRelease)))
        

        member this.createCard x y key = (
            let myCard = this.add.image x y key
            myCard.setInteractive()
            this.input.setDraggable(myCard)
            myCard
        )
        override this.preload() =
            this.load.image "battleroom" "assets/platform.png"
            this.load.image "eye" "assets/star.png"
            this.load.image "button" "assets/star.png"
            this.load.html ("healthbar", "assets/healthbar.html")

        member this.initEnemies() = ()
        member this.initPlayer() = ()


        //probably going to need a table for keeping track of each UI element to each
        //record
        override this.create() =
            //create healthbars
            machine.initialize()
            let dom1, dom2 =
                this.add.dom 600 600, this.add.dom 500 500
            let enemyhp, playerhp =
                dom1.createFromCache("healthbar"), dom2.createFromCache("healthbar")
            this.addDragAndDrop(enemyhp, playerhp)
            //each enemy npc has to be its own dropzone
            createZone(this.add.image 400 300 "battleroom")
            createButton (this.add.image 30 30 "button") enemyhp playerhp
            //let myList = [for i in 0 .. 10 -> (this.add.image 30 30 "button")]
            let deck = [for i in 0 .. 30 -> (this.createCard (60+i+5) (60+i) "eye")]
            ()
        member this.addDragAndDrop(enemyhp, playerhp) =
            
            let dragcb (pointer : unit) (gameObject : GameObject) dragX dragY =
                gameObject.x <- dragX
                gameObject.y <- dragY
            let dropcb (pointer : Pointer) (gameObject : GameObject) dropZone =
                //should only trigger if drop occurs
                //console.log(pointer.downTime)
                updateUI(enemyhp, playerhp, Some (PlayCard Card1))
            this.input.on "drag" (System.Func<_,_,_,_,_> dragcb)
            this.input.on "drop" (System.Func<_,_,_,_> dropcb) //awesome!!
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

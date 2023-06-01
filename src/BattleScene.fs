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

    I'll use a plugin for GUI related functions.

    I might try JSX later.
*)



//todo 

//add cards

//add health bars for player and npc

//https://photonstorm.github.io/phaser3-docs/Phaser.GameObjects.Zone.html




[<Import("CountingComponent", "./healthbar.js")>]
let a(e : string, p : string) : obj = jsNative


[<Import("setCount", "./healthbar.js")>]
let setCount(num : int) : unit = jsNative

[<Import("count", "./healthbar.js")>]
let count() : int = jsNative


type BattleScene() =
    class
        inherit Scene()
        let machine = new BattleStateMachine("bob the dude")
        let mutable myPlayerInput : PlayerInput = GameTypes.PlayerInput.Idle
        let createCards (numberOfCards : int) (gameClass : PlayableClasses) : Card list = (
            let myList : Card list = [for i in 0 .. numberOfCards -> (Card1)]
            myList
        )

        let updateUI() = ()

        member this.createZone() = ()
        member this.createCard x y key = (
            let myCard = this.add.image x y key
            myCard.setInteractive()
            this.input.setDraggable(myCard)
            myCard
        )
        override this.preload() = (
            this.load.image "battleroom" "assets/platform.png"
            this.load.image "eye" "assets/star.png"
            this.load.image "button" "assets/star.png"
        )


        override this.create() = (
            let dom = this.add.dom 600 600
            dom.setElement(a("Enemy health", "Player Health"))
            //let el = dom.createFromCache("bar")
            
            //each enemy npc has to be its own dropzone
            let myZone = this.add.image 400 300 "battleroom"
            let myButton = this.add.image 30 30 "button"
            myButton.setInteractive()
            myButton.on "pointerup" (System.Func<_,_> (fun () -> (
                machine.update(Some(EndTurnRelease)) |> ignore)))
            let myList = [for i in 0 .. 10 -> (this.add.image 30 30 "button")]
            myZone.setInteractive()
            myZone.input.dropZone <- true
            let a = [for i in 0 .. 100 -> (this.createCard (50+i) (50+i) "eye")]
            this.onInput()
            ()
        )
        member this.onInput(el) = (
            
            let dragcb (pointer : unit) (gameObject : GameObject) dragX dragY = (
                gameObject.x <- dragX
                gameObject.y <- dragY
            )
            let dropcb (pointer : Pointer) (gameObject : GameObject) dropZone =
                //should only trigger if drop occurs
                console.log(pointer.downTime)
                let error = {Id="Error";Health=0;Mana=0}
                let result =
                    match machine.update(Some (PlayCard Card1)) with
                    | Some(t,p,e) -> t,p,e
                    | None -> (PlayerTurn, error, [error])
                let _,p,e = result
                let playerhealth = p.Health
                let enemyhealth =
                    match e with
                    | [en] -> en.Health
                    | _ ->  0
                setCount(enemyhealth)
                ()
            this.input.on "drag" (System.Func<_,_,_,_,_> dragcb)
            this.input.on "drop" (System.Func<_,_,_,_> dropcb) //awesome!!
        )
        override this.update() = ()
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

module BattleScene
open PhaserUtility
open Fable.Core.JsInterop
open GameplayLoop
open Fable.Core
open Browser.Dom
open GameTypes

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



//todo for ui

//1. add zone

//2. add checker for sprite (card) in zone

//3. add visual feedback for checker

//https://photonstorm.github.io/phaser3-docs/Phaser.GameObjects.Zone.html


type BattleScene() =
    class
        inherit Scene()
        let machine = new BattleStateMachine("bob the dude")
        let mutable myPlayerInput : PlayerInput = GameTypes.PlayerInput.Idle

        override this.preload() = (
            this.load.image "battleroom" "assets/platform.png"
            this.load.image "eye" "assets/star.png"
            this.load.image "button" "assets/star.png"
            //this.load.image "clicktoendturn" "assets/sample.png"
            //this.load.html("frame1", "html-assets/frame2.html")
            //this.load.htmlTexture "myButton" "assets/sample.html"
        )
        override this.create() = (
            let myZone = this.add.image 400 300 "battleroom"
            let myButton = this.add.image 30 30 "button"
            myButton.setInteractive()
            myButton.on "pointerup" (System.Func<_,_> (fun () -> (
                machine.update(Some(EndTurnRelease)) |> ignore)))
                
            myZone.setInteractive()
            myZone.input.dropZone <- true
            let a = this.add.image 15 15 "eye"
            a.setInteractive()
            this.input.setDraggable(a)
            this.onInput()
            ()
        )
        member this.onInput() = (
            let dragcb (pointer : unit) (gameObject : GameObject) dragX dragY = (
                gameObject.x <- dragX
                gameObject.y <- dragY
            )
            let dropcb (pointer : Pointer) (gameObject : GameObject) dropZone =
                //should only trigger if drop occurs
                console.log(pointer.downTime)
                machine.update(Some (PlayCard Card1 )) |> ignore
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

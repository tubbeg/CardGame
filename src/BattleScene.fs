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
    EndTurn
 
let callbackTintClear (sprite : Sprite) : PlayerInput  =
    sprite.clearTint()
    EndTurn
    
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
        do()
        override this.preload() = (
            this.load.image "battleroom" "assets/platform.png"
            this.load.image "eye" "assets/star.png"
            //this.load.image "clicktoendturn" "assets/sample.png"
            //this.load.html("frame1", "html-assets/frame2.html")
            //this.load.htmlTexture "myButton" "assets/sample.html"
        )
        override this.create() = (
            let myZone = this.add.image 400 300 "battleroom"
            myZone.setInteractive()
            myZone.input.dropZone <- true

            let a = this.add.image 15 15 "eye"
            a.setInteractive()
            this.input.setDraggable(a)
            //this.onInput("drag", ())
            //this.onInput2()
            //let myZone = this.add.zone 400 300 400 400
            //zone is working, due to interactive
            this.onInput()
            let overlapFunc zone block =
                console.log("it's overlapping")
            
            let ofUncurried = System.Func<_,_,_> overlapFunc
            this.physics.add.overlap myZone a (Some ofUncurried) None
            ()
        )
        //this is the other way that you can solve the problem
        [<Emit("this.input.on('drag',(pointer, gameObject, dragX, dragY) =>
        {gameObject.x = dragX; gameObject.y = dragY;})")>]
        member this.onInputEmit(event:string, func:_) = ()
        member this.onInput() = (
            let inputCallback pointer (gameObject : GameObject) dragX dragY = (
                gameObject.x <- dragX
                gameObject.y <- dragY
            )
            let dragEntercb pointer (gameObject : GameObject) dropZone = console.log("hello there")
            let dragEntercb2 pointer (gameObject : GameObject) dropZone = console.log("goodbye then")
            //Forcing Fable to uncurry the function
            //according to https://github.com/fable-compiler/Fable/issues/2436
            this.input.on "drag" (System.Func<_,_,_,_,_> inputCallback)
            this.input.on "dragleave" (System.Func<_,_,_,_> dragEntercb)
            this.input.on "dragenter" (System.Func<_,_,_,_> dragEntercb2)
        )
        override this.update() = (
            //player idling should not trigger a state machine update
            //let updatedState : option<GameplayStates> =
            match (myPlayerInput : GameTypes.PlayerInput) with
            | Idle -> None
            | (action : GameTypes.PlayerInput) -> machine.update(Some(action))
        )
    end

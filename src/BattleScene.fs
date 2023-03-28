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


type BattleScene() =
    class
        inherit Scene()
        let machine = new BattleStateMachine("bob the dude")
        let mutable myPlayerInput : PlayerInput = GameTypes.PlayerInput.Idle

        
        do()
        override this.preload() = (
            this.load.image "battleroom" "assets/battleroom.png"
            this.load.image "eye" "assets/sample.png"
            this.load.image "clicktoendturn" "assets/sample.png"
            this.load.html("frame1", "html-assets/frame2.html")
            //this.load.htmlTexture "myButton" "assets/sample.html"
        )
        override this.create() = (
            (this.add.image 400 300 "battleroom")
            //let mysprite = (this.add.sprite (200) (200) ("eye"))
            //let mysprite2 = (this.add.sprite (500) (500) ("clicktoendturn"))
            //let myBtn = getButton()
            //console.log(myBtn)
            //let btn = (this.add.dom 400 300)
            //btn.setElement(new Counter())
            //btn.setVisible(true)
            let element = this.add.dom 400 300
            //console.log(form)
            element.createFromCache "frame1"
            ()
        )














        
        override this.update() = (
            //player idling should not trigger a state machine update
            //let updatedState : option<GameplayStates> =
            match (myPlayerInput : GameTypes.PlayerInput) with
            | Idle -> None
            | (action : GameTypes.PlayerInput) -> machine.update(Some(action))
        )
    end
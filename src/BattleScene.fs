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


//https://photonstorm.github.io/phaser3-docs/Phaser.GameObjects.GameObjectFactory.html
//https://github.com/jsfehler/phaser-ui-tools

//använd vs code js debug, annars uppdateras inte filerna


//gör så enkelt som möjligt med knappar
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
        )
        override this.create() = (
            (this.add.image 400 300 "battleroom")
            let mysprite = (this.add.sprite (200) (200) ("eye"))
            let mysprite2 = (this.add.sprite (500) (500) ("clicktoendturn"))
            //let btn = (this.add.dom 400 300 button)
            //createClickListener button myCallback
            //myFunc.createClickListener button myCallback
            mysprite.setInteractive()
            mysprite2.setInteractive()
            mysprite.on "pointerdown" (fun _ -> (myPlayerInput <- callbackTintSet(mysprite)))
            mysprite.on "pointerout" (fun _ -> (myPlayerInput <- callbackTintClear(mysprite)))
            mysprite.on "pointerup" (fun _ -> (myPlayerInput <- callbackTintClear(mysprite)))
            mysprite2.on "pointerdown" (fun _ -> (myPlayerInput <- callbackTintSet2(mysprite)))
            mysprite2.on "pointerout" (fun _ -> (myPlayerInput <- callbackTintClear2(mysprite)))
            mysprite2.on "pointerup" (fun _ -> (myPlayerInput <- callbackTintClear2(mysprite)))
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

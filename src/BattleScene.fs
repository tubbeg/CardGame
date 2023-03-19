module BattleScene
open PhaserUtility
open Fable.Core.JsInterop

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
 
let callbackTintClear (sprite : Sprite)   =
    sprite.clearTint()
 
type BattleScene() =
    class
        inherit Scene()
        do()
        override this.preload() = (
            //this.load.setBaseURL "http://labs.phaser.io"
            //this.load.image "sky" "assets/skies/space3.png"
            //this.load.image "logo" "assets/sprites/phaser3-logo.png"
            this.load.image "battleroom" "assets/battleroom.png"
            this.load.image "eye" "assets/sample.png"
        )
        override this.update() = ()
        override this.create() = (















            (this.add.image 400 300 "battleroom")
            let mysprite = (this.add.sprite (400) (300) ("eye"))
            mysprite.setInteractive()
            mysprite.on "pointerdown" (fun _ -> callbackTintSet(mysprite))
            mysprite.on "pointerout" (fun _ -> callbackTintClear(mysprite))
            mysprite.on "pointerup" (fun _ -> callbackTintClear(mysprite))
            //mysprite.on "pointerdown" (fun _ -> (callbackTintSet(mysprite))))
            //mysprite.on "pointerup", (fun _ -> callbackTintClear(mysprite)))
            ()
        )
            //let cursors = this.input.keyboard.createCursorKeys()
    end
(*sadsdasdasd

var sprite = this.add.sprite(400, 300, 'eye').setInteractive();

    sprite.on('pointerdown', function (pointer) {

        this.setTint(0xff0000);

    });

    sprite.on('pointerout', function (pointer) {

        this.clearTint();

    });

    sprite.on('pointerup', function (pointer) {

        this.clearTint();

    });*)
module PhaserSceneExtension
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
type SceneExt() =
    class
        inherit Scene()
        do()
        override this.preload() = (
            this.load.setBaseURL "http://labs.phaser.io"
            this.load.image "sky" "assets/skies/space3.png"
            this.load.image "logo" "assets/sprites/phaser3-logo.png"
            this.load.image "red" "assets/particles/red.png"
        )
        override this.create() = (
            this.add.image 400 300 "sky" |> ignore
            let myParticles = this.add.particles "red"
            let emitter = myParticles.createEmitter myEmitterConfig
            let logo = this.physics.add.image 400 100 "logo"
            logo.setVelocity 100 200
            logo.setBounce 1 1
            logo.setCollideWorldBounds true
            emitter.startFollow logo
        )
    end



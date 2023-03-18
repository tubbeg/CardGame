module PhaserUtility

open Browser.Dom
open Fable.Core
open Fable.Core.JsInterop
open System.IO

type IPhaserRender = 
    | Auto
    | Canvas
    | WebGL

[<Import("ParticleEmitter", "phaser")>]
type ParticleEmitter() =
    class
        do()
        member this.startFollow gameObject = jsNative
    end
[<Import("ImageWithDynamicBody", "phaser")>]
type  ImageWithDynamicBody() =
    class
        do()
        member this.setVelocity x y = jsNative
        member this.setBounce x y = jsNative
        member this.setCollideWorldBounds foo = jsNative
    end

[<Import("AUTO", "phaser")>]
let phaserAuto: IPhaserRender = jsNative
[<Import("Factory", "phaser")>]
type  Factory() =
    class
        do()
        member this.image x y id : ImageWithDynamicBody =
            jsNative
    end

[<Import("ArcadePhysics", "phaser")>]
type ArcadePhysics() =
    class
        do()
        member val add : Factory = jsNative with get, set
    end


[<Import("ParticleEmitterManager", "phaser")>]
type ParticleEmitterManager() =
    class
        do()
        member this.createEmitter (config: obj) : ParticleEmitter =
            jsNative
    end


[<Import("GameObjectFactory", "phaser")>]
type GameObjectFactory() =
    class
        do()
        member this.image (x : int) (y : int) (id: string) =
            jsNative
        member this.particles (id: string) : ParticleEmitterManager =
            jsNative
    end



[<Import("LoaderPlugin", "phaser")>]
type LoaderWrapper() =
    class
        do()
        member this.image (param1 : string) (param2 : string) =
            jsNative
        member this.setBaseURL (url : string) =
            jsNative
    end
[<Import("SceneWrapper", "./PhaserSceneWrapper.js")>]
type Scene() =
    class
        abstract member preload: unit -> obj //DO NOT FORGET TO OVERRIDE. VERY IMPORTANT
        default this.preload() : obj = jsNative
        abstract member create: unit -> obj
        default this.create() : obj = jsNative
        member val load : LoaderWrapper = jsNative with get, set //THESE NEED TO BE PROPERTIES!! ALSO IMPORTANT
        member val add : GameObjectFactory = jsNative with get, set
        member val physics : ArcadePhysics = jsNative with get, set //not gameobjectfactory
        member this.getAdd : unit -> obj  = jsNative
        member this.getLoader : unit -> obj = jsNative
    end

let myFirstJsObj = createObj [
    "start" ==> 1
    "end" ==> 0
]

let myJsObj = createObj [
    "speed" ==> 100
    "scale" ==> myFirstJsObj
    "blendMode" ==> "ADD"
]

type SceneExt() =
    class
        inherit Scene()
        do()
        override this.preload() = (
            console.log("running preload in f#")
            let arr : (string*string) array = [|
                "sky", "assets/skies/space3.png"
                ;"logo", "assets/sprites/phaser3-logo.png"
                ;"red", "assets/particles/red.png"
                |]
            this.load.setBaseURL "http://labs.phaser.io"
            //this.loadImages(arr)
            this.load.image "sky" "assets/skies/space3.png"
            this.load.image "logo" "assets/sprites/phaser3-logo.png" //IT WORKS!!!
            this.load.image "red" "assets/particles/red.png"
        )
        
        override this.create() = (
            
            this.add.image 400 300 "sky"

            
            let myParticles = this.add.particles "red"
            
            let emitter = myParticles.createEmitter myJsObj
            let logo = this.physics.add.image 400 100 "logo"
            logo.setVelocity 100 200
            logo.setBounce 1 1
            logo.setCollideWorldBounds true
            emitter.startFollow logo
        )
    end

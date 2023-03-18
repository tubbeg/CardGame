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
[<Import("Scene", "phaser")>]
type Scene() =
    class
        abstract member preload: unit -> obj //DO NOT FORGET TO OVERRIDE. VERY IMPORTANT
        default this.preload() : obj = jsNative
        abstract member create: unit -> obj
        default this.create() : obj = jsNative
        member val load : LoaderWrapper = jsNative with get, set //THESE NEED TO BE PROPERTIES!! ALSO IMPORTANT
        member val add : GameObjectFactory = jsNative with get, set
        member val physics : ArcadePhysics = jsNative with get, set
    end

let myEmitterConfig = createObj [
    "speed" ==> 100
    "scale" ==> createObj [
        "start" ==> 1
        "end" ==> 0
    ]
    "blendMode" ==> "ADD"
]

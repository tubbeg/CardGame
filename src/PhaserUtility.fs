module PhaserUtility

open Browser.Dom
open Fable.Core
open Fable.Core.JsInterop
open System.IO

type IPhaserRender = 
    | Auto
    | Canvas
    | WebGL


//Might need this for passing data between scenes
[<Import("EventEmitter", "phaser")>]
type EventEmitter() =
    class
    end

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
        member this.overlap (object1:obj) (object2:obj) (collideCallback:option<_>) (processCallback:option<_>) : unit = jsNative
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




[<Import("LoaderPlugin", "phaser")>]
type LoaderWrapper() =
    class
        do()
        member this.image (param1 : string) (param2 : string) =
            jsNative
        member this.setBaseURL (url : string) =
            jsNative
        member this.htmlTexture (url : string) =
            jsNative
        member this.html (key:string, url : string) =
            jsNative
    end



[<Import("Sprite", "phaser")>]
type Sprite() =
    class
        do()
        member this.setInteractive() : unit =
            jsNative
        member this.on (event : string) (callback : (unit -> unit)) : unit =
            jsNative
        member this.setTint (topLeft : int ) : unit =
            jsNative
        member this.clearTint() : unit =
            jsNative//sadasiijdoa
    end

[<Import("DOMElement", "phaser")>]
type DOMElement() =
    class
        do()
        member this.addListener (id:string) : unit =
            jsNative
        member this.on (id:string) (cb:(unit->unit)) : unit =
            jsNative
        member this.createElement (tag:string) (typ:string) (inner:string) : unit =
            jsNative
        member this.setVisible (value:bool) : unit =
            jsNative
        member this.setElement (value:obj) : unit =
            jsNative
        member this.createFromHTML (form:obj) : unit =
            jsNative
        member this.createFromCache (form:obj) : unit =
            jsNative
    end


[<Import("GameObjectFactory", "phaser")>]
type InteractiveObject() =
    class
        do()
        member val dropZone : bool = jsNative  with get, set //
    end

[<Import("GameObjectFactory", "phaser")>]
type GameObject() =
    class
        do()
        member val x : int = jsNative with get, set //
        member val y : int = jsNative with get, set //
        member val input : InteractiveObject = jsNative with get, set //
        member this.setInteractive() : unit =
            jsNative
        member this.on (event:string) (myFunc : _): unit =
            jsNative
    end
[<Import("Body", "phaser")>]
type ArcadeBody() =
    class
        do()
        member this.setVisible (state:bool) : unit = jsNative
    end


[<Import("Zone", "phaser")>]
type Zone() =
    class
        do()
        member this.setVisible (state:bool) : unit = jsNative
        member this.setInteractive () : unit = jsNative
    end

[<Import("GameObjectFactory", "phaser")>]
type GameObjectFactory() =
    class
        do()
        member this.sprite (x:int) (y:int) (texture : string) : Sprite =
            jsNative
        member this.image (x : int) (y : int) (id: string) : GameObject =
            jsNative
        member this.particles (id: string) : ParticleEmitterManager =
            jsNative
        member this.dom (x:int) (y:int) : DOMElement =
            jsNative
        member this.zone (x:int) (y:int) (width:int) (height:int) : Zone =
            jsNative
        
    end



[<Import("Pointer", "phaser")>]
type Pointer() =
    class
        do()
        member val downTime : int = jsNative with get, set
    end

[<Import("InputPlugin", "phaser")>]
type InputPlugin() =
    class
        member this.on (event:string) (myFunc : _): unit =
            jsNative
        member this.setDraggable (gameObj : GameObject) : unit =
            jsNative
    end




[<Import("Scene", "phaser")>]
type Scene() =
    class
        abstract member preload: unit -> obj //DO NOT FORGET TO OVERRIDE. VERY IMPORTANT
        default this.preload() : obj = jsNative
        abstract member create: unit -> obj
        default this.create() : obj = jsNative
        abstract member update: unit -> obj
        default this.update() : obj = jsNative
        member val load : LoaderWrapper = jsNative with get, set //THESE NEED TO BE PROPERTIES!! ALSO IMPORTANT
        member val add : GameObjectFactory = jsNative with get, set
        member val physics : ArcadePhysics = jsNative with get, set
        member val input :  InputPlugin = jsNative with get, set
    end



module PhaserUtility

open Browser.Dom
open Fable.Core
open Fable.Core.JsInterop
open System.IO


//let (+/) (start : string) (endString : string) =
//    start + (string Path.DirectorySeparatorChar) + endString 

//let skyTuple = "sky", "assets" +/ "sky.png"

//let imagesToLoad  = [|skyTuple|]

//be careful when defining data types used by wrapper classes as
//some types might generate classes untintentionally
type SceneCallback = (unit -> unit)


//using wrapper classes is the easiest way to work with js in Fable

//otherwise you'll need to specify every single data type used
//by the library/game engine

[<Import("Stuff", "./PhaserSceneWrapper.js")>]

type Stuff() =
    class
        member this.getFive() : unit = jsNative
    end

[<Import("DummyClass", "./PhaserSceneWrapper.js")>]
type DummyClass() =
    class
        member this.getDummyFive() : unit = jsNative
    end
//[<Import("GameObjectFactory", "phaser")>]
//let s : IFact = jsNative

[<Import("SceneWrapper", "phaser")>]
type Scene() =
    class
        member this.getAdd : unit -> obj  = jsNative
        member this.getLoader : unit -> obj = jsNative
        member this.physics = jsNative
    end

[<Import("SceneWrapper", "./PhaserSceneWrapper.js")>]
type Scene() =
    class
        member this.getAdd : unit -> obj  = jsNative
        member this.getLoader : unit -> obj = jsNative
        member this.physics = jsNative
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
        member this.preload() = (
            console.log("running preload in f#")
            this.getLoader().setBaseURL("http://labs.phaser.io")
            this.getLoader().image("sky", "assets/skies/space3.png")
            this.getLoader().image("logo", "assets/sprites/phaser3-logo.png")
            this.getLoader().image("red", "assets/particles/red.png")
        )
        member this.create() = (
            
            this.getAdd().image(400, 300, "sky")
            let myParticles = this.getAdd().particles("sky")
            let emitter = myParticles.createEmitter(myJsObj)
            let logo = this.physics().add().image(400, 100, "logo")
            logo.setVelocity(100, 200)
            logo.setBounce(1, 1)
            logo.setCollideWorldBounds(true)
            emitter.startFollow(logo)
        )
        member this.dummyFunction(number) = number * 5
    end

//let sky: string = importMember "../public/assets/sky.png"
// JS: import { myString } from "my-lib"

type IPhaserRender = 
    | Auto
    | Canvas
    | WebGL



[<Import("AUTO", "phaser")>]
let phaserAuto: IPhaserRender = jsNative

[<Import("CANVAS", "phaser")>]
let phaserCanvas: IPhaserRender = jsNative

[<Import("AUTO", "phaser")>]
let phaserWebGL: IPhaserRender = jsNative


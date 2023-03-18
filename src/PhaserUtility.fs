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

[<Import("LoaderWrapper", "./PhaserSceneWrapper.js")>]
type LoaderWrapper() =
    class
        member this.image : string*string -> unit = jsNative
    end
[<Import("SceneWrapper", "./PhaserSceneWrapper.js")>]
type Scene() =
    class
        abstract member preload: unit -> obj
        default this.preload() : obj = jsNative
        member this.add : unit -> LoaderWrapper = jsNative
        member this.getAdd : unit -> obj  = jsNative
        member this.getLoader : unit -> obj = jsNative
        member this.physics = jsNative
        member this.loadImages arr : (string*string) array -> unit = jsNative
        member this.setBaseUrl (url : string) = jsNative
        member this.setAddImage (x,y, texture) : (int*int*string) -> unit = jsNative
        member this.addParticles texture : string -> obj = jsNative
        member this.createEmitter (particles, texture) : (string*string) -> unit = jsNative
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
            this.setBaseUrl "http://labs.phaser.io"
            this.loadImages(arr)
            //this.add().image("sky", "assets/skies/space3.png")
            //this.add().image("logo", "assets/sprites/phaser3-logo.png")
            //this.add().image("sky", "assets/particles/red.png")
        )
        (*
        member this.create() = (
            
            this.setAddImage (400, 300, "sky")

            
            let myParticles = this.getAdd().particles("sky")
            let emitter = myParticles.createEmitter(myJsObj)
            let logo = this.physics().add().image(400, 100, "logo")
            logo.setVelocity(100, 200)
            logo.setBounce(1, 1)
            logo.setCollideWorldBounds(true)
            emitter.startFollow(logo))*)
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


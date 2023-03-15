module App

open Browser.Dom
open Fable.Core
open Fable.Core.JsInterop
open Cardlogic
open GameTypes
open GameConfig
open PhaserUtility

(*
let myAuto = phaserAuto

let myCanvas = phaserCanvas

let myWebGL = phaserWebGL
*)
//let testPhaserScene = phaserScene


let testPhaserExtension : MyPhaserExtension = new MyPhaserExtension()

//let resultOfExtension = testPhaserExtension.addFive 10

//console.log(resultOfExtension)



let myClass = MagicianTank

type IPixi =
    abstract startAnim : unit -> unit

type IPhaser =
    abstract preload : unit -> unit
    abstract create : unit -> unit
    abstract preload2 : (string * string) array -> unit

type IAlert =
    abstract triggerAlert : message:string -> unit
    abstract someString: string

// Mutable variable to count the number of times we clicked the button
let mutable count = 0

// Get a reference to our button and cast the Element to an HTMLButtonElement
let myButton = document.querySelector(".my-button") :?> Browser.Types.HTMLButtonElement

// Register our listener
myButton.onclick <- fun _ ->
    count <- count + 1
    myButton.innerText <- sprintf "You clicked: %i time(s)" count

[<ImportAll("./alert.js")>]
let mylib: IAlert = jsNative

//[<ImportAll("./pixi_test.js")>]
//let myPixi: IPixi = jsNative

[<ImportAll("./phaser_test2.js")>]
let myPhaserLib: IPhaser = jsNative



(*
[<Import("default", "phaser")>]
type IScene (conf: string) =
    abstract update: data: 'T -> Promise<'T>
    default _.update(data: 'T): Promise<'T> = jsNative
*)

//mylib.triggerAlert ("Hey I'm calling my js library from Fable > " + mylib.someString)

//myPixi.startAnim()

let startFunctions =
    let testGame2 = new PhaserGame(phaserGameConfig)
    //let testScene = new PhaserScene()
    //let loader = testScene.load()
    //let testExt = new PhaserSceneExtension()
    //testExt.preload()
    //console.log(testExt.addFive 10)
    //myPhaserLib.preload()
    //myPhaserLib.create()
    ()



startFunctions




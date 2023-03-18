module GameConfig

open PhaserUtility
open Browser.Dom
open Fable.Core
open Fable.Core.JsInterop
open PhaserSceneExtension


type PhaserGameConfig = Config of obj

let createGravity =
    createObj [
        "y" ==> 300
    ]

let createArcade =
    createObj [
        "gravity" ==> createGravity
        "debug" ==> false
    ]

let createPhysics =
    createObj [
        "default" ==> "arcade"
        "arcade" ==> createArcade
    ]
    
let phaserGameConfig (scene : SceneExt) = 
    createObj [
        "type" ==> Auto
        "width" ==> 800
        "height" ==> 600
        "physics" ==> createPhysics
        "scene" ==> scene
    ]





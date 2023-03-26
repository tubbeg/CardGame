module GameConfig

open PhaserUtility
open Browser.Dom
open Fable.Core
open Fable.Core.JsInterop
open PhaserSceneExtension


let createContainer = createObj ["createContainer" ==> true]

let buildConfig scene = createObj [
    "parent" ==> "phaser"
    "dom" ==> createContainer
    "type" ==> Auto
    "width" ==> 800
    "height" ==> 600
    "physics" ==> createObj [
        "default" ==> "arcade"
        "arcade" ==> createObj [
            "gravity" ==> createObj [
                "y" ==> 300
            ]
            "debug" ==> false
        ]
    ]
    "scene" ==> scene
]

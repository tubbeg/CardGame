module GameConfig

open PhaserUtility
open Browser.Dom
open Fable.Core
open Fable.Core.JsInterop
open PhaserSceneExtension


let buildConfig scene = createObj [
    "parent" ==> "myPhaser"
    "dom" ==> createObj [
        "createContainer" ==> true
    ]
    "type" ==> phaserAuto
    "width" ==> 1600
    "height" ==> 800
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
    "fps" ==> createObj [
        "target" ==> 60 //use 1 for debug
        "forceSetTimeOut" ==> true
    ]
]

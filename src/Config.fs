module GameConfig

open PhaserUtility
open Browser.Dom
open Fable.Core
open Fable.Core.JsInterop
open PhaserSceneExtension

[<Import("RexUIPlugin", "./html-assets/rexuiplugin.min.js")>]
let RexUI : obj = jsNative


let details = createObj [
    "key" ==> "rexUI"
    "plugin" ==> RexUI
    "mapping" ==> "rexUI"
]

let constructRexPlugin = createObj [
    "scene" ==> [|
        details
        |]
    ]


let buildConfig scene = createObj [
    "parent" ==> "myPhaser"
    "dom" ==> createObj [
        "createContainer" ==> true
    ]
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
    //"plugins" ==> constructRexPlugin
]

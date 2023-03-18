module App

open Browser.Dom
open Fable.Core
open Fable.Core.JsInterop
open Cardlogic
open GameTypes
open GameConfig
open PhaserUtility
open PhaserSceneExtension

let testPhaserExtension  =
    new SceneExt()

let myClass = MagicianTank

[<Import("Game", "phaser")>]
type PhaserGame (config : obj) =
    class
    end



let testSceneExt = new SceneExt()
let testGame2 =
    new PhaserGame(buildConfig testPhaserExtension)


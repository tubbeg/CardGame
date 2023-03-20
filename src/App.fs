module App

open Browser.Dom
open Fable.Core
open Fable.Core.JsInterop
open Cardlogic
open GameTypes
open GameConfig
open PhaserUtility
open PhaserSceneExtension
open BattleScene
open GameplayLoop

let myScene  =
    new BattleScene()
[<Import("Game", "phaser")>]
type PhaserGame (config : obj) =
    class
    end

let testState =
    new BattleStateMachine("bob the player")

let testGame2 =
    new PhaserGame(buildConfig myScene)


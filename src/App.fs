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


let myScene  = new BattleScene()
[<Import("Game", "phaser")>]
type PhaserGame (config : obj) =
    class
    end


let testGame2() =
    new PhaserGame(buildConfig myScene)

let runGame =
    //a() |> ignore
    //let a = document.getElementById "hp-bar2"
    //let b: Browser.Types.HTMLElement = document.getElementById "mana-bar2"
    //a.dataset.Item "value" <- "0.3" //this successfully updates the progress, but does not yield any visual update...why?
    //testState()
    testGame2()
                


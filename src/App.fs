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
type IPhaser =
    abstract preload : unit -> unit
    abstract create : unit -> unit
    abstract preload2 : (string * string) array -> unit
[<Import("Game", "phaser")>]
type PhaserGame (config : obj) =
    class
    end

type IBuildConfig = obj

[<Import("buildConfig", "./BuildPhaserConfig.js")>]
let configBuilder (scene : obj) : IBuildConfig = jsNative

let testSceneExt = new SceneExt()
let testGame2 =
    new PhaserGame(configBuilder testSceneExt)


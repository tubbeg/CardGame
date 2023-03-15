module PhaserUtility

open Browser.Dom
open Fable.Core
open Fable.Core.JsInterop


type SceneCallback = unit -> unit


(*
det börjar bli ganska mycket jobb

gör så här istället:

1. definiera en vanlig js wrapper class som extendar
din scene

2. se till att den tar in callback funktioner som argument

3. definiera din funktioner i F# Fable

4. ???

4. common js wrapper + F# callback = profit
*)

[<Import("PhaserSceneWrapper", "./PhaserSceneWrapper.js")>]
type PhaserSceneWrapper (preCallback : SceneCallback, createCallback : SceneCallback) =
    class
    end

//let sky: string = importMember "../public/assets/sky.png"
// JS: import { myString } from "my-lib"

type IPhaserRender = 
    | Auto
    | Canvas
    | WebGL


[<Import("ParticleEmitterManager", "phaser")>]
type ParticleEmitterManager () =
    class
    end skit i detta

[<Import("LoaderPlugin", "phaser")>]
type PhaserLoaderPlugin () =
    class
        member _.image (key : string) (url : string) : unit = jsNative
        member _.setBaseURL (url : string) : unit = jsNative
    end

[<Import("GameObjectFactory", "phaser")>]
type GameObjectFactory (scene : obj) =
    class
        member _.image (x : int) (y : int) (id : string) : unit = jsNative
        member _.particles (id : string) : unit = jsNative
    end
[<Import("Game", "phaser")>]
type PhaserGame (config : obj) = class end

[<Import("Scene", "phaser")>]
type PhaserScene () =
    class
        abstract load : unit -> PhaserLoaderPlugin
        default _.load() : PhaserLoaderPlugin = jsNative
        abstract add : unit -> GameObjectFactory
        default _.add() : GameObjectFactory = jsNative
    end


type PhaserSceneExtension () as this =
    inherit PhaserScene()
    //this is a sort of weird solution...I should be able to call the base class normally
    //instead of relying on overriding
    override _.load() =
        base.load()
    member _.preload () = (
        this.load().setBaseURL "http://labs.phaser.io"
        this.load().image "sky" "assets/skies/space3.png"
        this.load().image "logo" "assets/sprites/phaser3-logo.png"
        this.load().image "red" "assets/particles/red.png"
        //this.load.spritesheet('dude', 'assets/dude.png', { frameWidth: 32, frameHeight: 48 });
    )
    member this.create () = (
        this.add().image 300 400 "sky"
        
        let particles = this.add.particles "red"

        let emitter = createObj[

        ]

    )
    member this.addFive number = number + 5

(*
type MyClassImplementation = // 1
  abstract awesomeInteger: int with get, set
  abstract isAwesome: unit -> bool
  
type PhaserSceneClass = // 2 
  [<Emit("new $0($1...)")>]
  abstract Create : awesomeInteger:int ->  MyClassImplementation //= jsNative  // takes a string parameter and does not return anything
  abstract getPI : unit-> float

[<Import("default", "../public/MyClass.js")>] // 3
let myClassStatic : MyClass = jsNative
*)

(*
function preload ()
    {
        this.load.setBaseURL('http://labs.phaser.io');

        this.load.image('sky', 'assets/skies/space3.png');
        this.load.image('logo', 'assets/sprites/phaser3-logo.png');
        this.load.image('red', 'assets/particles/red.png');
    }

    function create ()
    {
        this.add.image(400, 300, 'sky');

        var particles = this.add.particles('red');

        var emitter = particles.createEmitter({
            speed: 100,
            scale: { start: 1, end: 0 },
            blendMode: 'ADD'
        });

        var logo = this.physics.add.image(400, 100, 'logo');

        logo.setVelocity(100, 200);
        logo.setBounce(1, 1);
        logo.setCollideWorldBounds(true);

        emitter.startFollow(logo);
    }*)

type MyPhaserExtension =
    inherit PhaserScene
    new() = {}
    // We can do something with data before sending it to the base class
    //override _.update(data) =
    //    base.update(data)
    member this.preload = ()
    member this.create = ()



//[<Import("Phaser.Scene", "phaser")>]
//let phaserScene : PhaserScene = jsNative



[<Import("Phaser.AUTO", "phaser")>]
let phaserAuto: IPhaserRender = jsNative

[<Import("Phaser.CANVAS", "phaser")>]
let phaserCanvas: IPhaserRender = jsNative

[<Import("Phaser.AUTO", "phaser")>]
let phaserWebGL: IPhaserRender = jsNative
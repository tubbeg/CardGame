module PhaserUtility

open Browser.Dom
open Fable.Core
open Fable.Core.JsInterop

type IPhaserRender = 
    | Auto
    | Canvas
    | WebGL


[<Import("LoaderPlugin", "phaser")>]
type PhaserLoaderPlugin () =
    do ()
    member this.image (key : string) (url : string) = ()

[<Import("Game", "phaser")>]
type PhaserGame (config : obj) = do ()

[<Import("Scene", "phaser")>]
type PhaserScene () =
    do ()

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
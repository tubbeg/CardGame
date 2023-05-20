module GameTypes
open Browser.Dom
open Fable.Core
open Fable.Core.JsInterop

type PlayableClasses =
    | GunAssasin
    | SamuraiWarrior
    | MagicianTank


type Card =
    | Card1

type Deck = Cards of Card array

type NPC = {
    Id : string;
    Health : int;
    Mana : int
}
and Player = NPC
and Enemies = NPC array
type Passives =
    | Passive1
    | Passive2


type GameplayStates =
    //| Init
    //| EnemyTurn
    | PlayerTurn
and EndGame =
    | Win
    | GameOver
and PlayerInput =
    | PlayCard of Card
    | EndTurnPress
    | EndTurnRelease
    | Idle

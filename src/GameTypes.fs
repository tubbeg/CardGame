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
    | Card2
    | Card3
    | Card4
    | Card5

type Deck = Cards of Card array

type NPC = {
    Id : string;
    Health : int;
    Mana : int
}
and Player = NPC
and Enemies = NPC list
type Passives =
    | Passive1
    | Passive2


type GameplayStates =
    //| Init
    //| EnemyTurn
    | PlayerTurn
and GameStatus =
    | Win
    | GameOver
    | Running
and PlayerInput =
    | PlayCard of Card
    | EndTurnPress
    | EndTurnRelease
    | Idle

module GameTypes
open Browser.Dom
open Fable.Core
open Fable.Core.JsInterop

type PlayableClasses =
    | GunAssasin
    | SamuraiWarrior
    | MagicianTank


type EnemyClass =
    | NotYetImplemented

type Card =
    | Card1
    | Card2
    | Card3
    | Card4
    | Card5

type Deck = Cards of Card array

type Position =
    | Position1
    | Position2
    | Position3
    | Position4
    | Position5

type NPC = {
    Id : string;
    Health : int;
    Mana : int;
    Location : option<Position>
    Type : Type
}
and Type =
    | Player
    | Enemy
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
    | PlayCard of (Card*option<NPC>)
    | EndTurnPress
    | EndTurnRelease
    | Idle
and GameData =
    option<GameStatus * GameplayStates * Player * Enemies>
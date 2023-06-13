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

type Position =
    | PlayerPosition
    | Position2
    | Position3
    | Position4
    | Position5
    | AllCharacters //can also be used for all enemies, but this is dependent on the card itself



type CardAction = option<Position> * Effect
and Card =
    | Shoot of CardAction
    | Claw of CardAction
    | Lifesteal of CardAction
    | Burn of CardAction
    | Posion of CardAction
    | Bomb of CardAction
and Effect =
    | Effect1
    | Effect2
    | PosionEffect
    | BurnEffect
    | ExplosionEffect of Health
    | NormalDamage of Health
    | TrueDamage of Health*Mana
and Mana = int
and Health = int
type Deck = Card list
type NPC = {
    Id : string;
    Health : Health;
    Mana : Mana;
    Location : option<Position>
    Type : Type
    Passives : option<Passives list>
    Life : Life
}
and Life =
    | Alive
    | Dead
and Type =
    | Player
    | Enemy
and Player = NPC
and Enemies = NPC list
and Passives =
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
    | PlayCard of (option<Card>*option<Position>)
    | EndTurnPress
    | EndTurnRelease
    | Idle
and GameData =
    option<option<GameStatus> * GameplayStates * Player * Enemies>
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

type Passives =
    | Passive1
    | Passive2



type GameplayStates =
    | Init
    | CardStatusEffect
    | EnemyStatusEffect
    | EnemyTurn
    | Death
    | Win
    | Action of UpdateEntities
    | PlayerActions of PlayerStates
and UpdateEntities =
    | GameAction
    | UpdatePlayerStatus
    | UpdateEnemyStatus
and PlayerStates =
    | Await
    | EndTurn
    | PlayCard

type PlayerAction =
    | EndTurn
    | ChooseCard

type Acceptance =
    | OK
    | NotOK
module GameplayLoop
open GameTypes
open Browser.Dom
open Fable.Core


//let notYetImplemented() = eprintfn "not yet implemented"

let CardAction = None //support function maybe
let ValidatePlayerStatus player = None //check that player is alive
let ValidateEnemyStatus enemies = None //check that enemies are alive


let PlayCardEvent card =
    //notYetImplemented()
    PlayerTurn

let EndTurn() = 
    //notYetImplemented()
    EnemyTurn
(*





*)
let EnemyAction (enemies:Enemies) : GameplayStates =
    //notYetImplemented()
    ValidateEnemyStatus enemies |> ignore
    PlayerTurn

//the state machine should not govern the playerinput, but simply respond to it
type BattleStateMachine(playerName) =
    class
        do ()
        let mutable currentState : GameplayStates = PlayerTurn //default to player
        let mutable player : Player = {Id=playerName;Health=100;Mana=250}
        let mutable enemies : Enemies = [||] //initialize to empty array
        //this might need to be an asynchronous function, since it has to wait for animations
        //fix this later
        member this.update (playerinput : option<PlayerInput>) : option<GameplayStates>  = (
            match playerinput with
            | Some(input) -> (
                match (input, currentState) with
                //| _, Init -> (currentState <- PlayerTurn) //might call a function, or remove this later
                | EndTurn, PlayerTurn -> (
                    currentState <- EndTurn()
                    Some(currentState))
                | PlayCard(card), PlayerTurn -> (
                    currentState <- PlayCardEvent(card)
                    Some(currentState))
                | _, EnemyTurn -> (
                    currentState <- EnemyAction enemies
                    Some(currentState))
                | Idle, PlayerTurn -> (None)
                )
            | _ -> (
                eprintfn "error missing input"
                Some(currentState))
        )
    end
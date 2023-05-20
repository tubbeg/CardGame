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

let EnemyAction (enemies:Enemies) : GameplayStates =
    //notYetImplemented()
    ValidateEnemyStatus enemies |> ignore
    PlayerTurn

//the state machine should not govern the playerinput, but simply respond to it
type BattleStateMachine(playerName) =
    class
        do ()
        let mutable turn : GameplayStates = PlayerTurn //default to player
        let mutable player : Player = {Id=playerName;Health=100;Mana=250}
        let mutable enemies : Enemies = [||] //initialize to empty array
        //this might need to be an asynchronous function, since it has to wait for animations
        //fix this later


        let enemyTurn() =
            console.log("enemy turn")
            console.log("enemy does something!")
            turn <- EnemyAction enemies
            //console.log("current state is : " + turn.ToString())
            turn
        let EndTurnFunc() = 
        //notYetImplemented()
            enemyTurn()
        member this.update (playerinput : option<PlayerInput>) : option<GameplayStates>  = (

            match playerinput with
            | Some(input) -> (
                match (input, turn) with
                //| _, Init -> (currentState <- PlayerTurn) //might call a function, or remove this later
                | EndTurnRelease, PlayerTurn -> (
                    console.log("player end turn")
                    turn <- EndTurnFunc() //enemy turn is no longer a state
                    //console.log("current state is : " + turn.ToString())
                    Some(turn))
                | PlayCard(card), PlayerTurn -> (
                    console.log("player play card")
                    turn <- PlayCardEvent(card)
                    Some(turn))
                (*| _, EnemyTurn -> (
                    console.log("enemy turn")
                    turn <- EnemyAction enemies
                    console.log("enemy does something!")
                    console.log("current state is : " + turn.ToString())
                    Some(turn))*)
                | Idle, PlayerTurn -> (
                    //console.log("Player turn idling")
                    None)
                | EndTurnPress, PlayerTurn -> (None)
                )
            | _ -> (
                eprintfn "error missing input"
                Some(turn))
        )
    end
module GameplayLoop
open GameTypes


let notYetImplemented() = eprintfn "not yet implemented"

let PlayCardEvent card =
    notYetImplemented()

let EndTurn() = 
    notYetImplemented()


let EnemyAction() =
    notYetImplemented()
    PlayerTurn

let CardAction = None //support function maybe
let ValidatePlayerStatus = None //check that player is alive
let ValidateEnemyStatus = None //check that enemies are alive

type BattleStateMachine(playerName) =
    class
        do ()
        let mutable currentState : GameplayStates = Init
        member val player : Player = {Id=playerName;Health=100;Mana=250}
        member val enemies : Enemies = [||] //initialize to empty array
        //this might need to be an asynchronous function, since it has to wait for animations
        //fix this later
        member this.update (playerinput : option<PlayerInput>) = (
            match playerinput with
            | Some(input) -> (
                match (input, currentState) with
                | _, Init -> (currentState <- PlayerTurn) //might call a function, or remove this later
                | EndTurn, PlayerTurn -> (currentState <- EnemyTurn)
                | PlayCard(card), PlayerTurn -> (PlayCardEvent(card))
                | _, EnemyTurn -> (currentState <- EnemyAction())
                //| combo -> (eprintfn "error faulty input: %A" combo)
                )
            | _ -> (eprintfn "error missing input")
        )
        member this.getCurrentState = //remove this in production
            currentState
    end
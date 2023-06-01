module GameplayLoop
open GameTypes
open Browser.Dom
open Fable.Core


//let notYetImplemented() = eprintfn "not yet implemented"

let CardAction = None //support function maybe
let ValidatePlayerStatus player = None //check that player is alive
let ValidateEnemyStatus enemies = None //check that enemies are alive


let PlayCardEvent card player1 (enemies : Enemies) =
    //notYetImplemented()
    let playerUpdate = {Id=player1.Id;Health=(player1.Health);Mana=player1.Mana} //to be fixed
    //console.log(playerUpdate)
    let enemy =
        match enemies with
            | [e] -> {Id=e.Id;Health=(e.Health-10);Mana=e.Mana}
            | _ -> {Id="Error"; Health=0;Mana=0}
    //console.log("enemy health")
    //console.log(enemy.Health)
    let enemyUpdate = [enemy]  //notyetimplemented
    PlayerTurn, playerUpdate, enemyUpdate

let EnemyAction (player1 : Player) (enemies:Enemies) : GameplayStates*Player*Enemies =
    ValidateEnemyStatus enemies |> ignore
    let playerUpdate = {Id=player1.Id;Health=(player1.Health - 10);Mana=player1.Mana}
    let enemyUpdate = enemies //notyetimplemented
    //should add enemies as return value as well since they can hurt themselves
    PlayerTurn, playerUpdate, enemyUpdate

//the state machine should not govern the playerinput, but simply respond to it
type BattleStateMachine(playerName) =
    class
        do ()
        let mutable turn : GameplayStates = PlayerTurn //default to player
        let mutable player : Player = {Id=playerName;Health=100;Mana=250}
        let mutable enemies : Enemies = [{Id="Monster";Health=100;Mana=250}] //initialize to empty array
        //this might need to be an asynchronous function, since it has to wait for animations
        //fix this later


        let enemyTurn() =
            console.log("enemy turn")
            console.log("enemy does something!")
            let state, p, e =  EnemyAction player enemies  
            turn <- state
            player <- p
            enemies <- e
            console.log(player)
            //console.log("current state is : " + turn.ToString())
            turn
        let EndTurnFunc() = 
        //notYetImplemented()
            enemyTurn()
        member this.update (playerinput : option<PlayerInput>) : option<GameplayStates*Player*Enemies>  = (

            match playerinput with
            | Some(input) -> (
                match (input, turn) with
                //| _, Init -> (currentState <- PlayerTurn) //might call a function, or remove this later
                | EndTurnRelease, PlayerTurn -> (
                    console.log("player end turn")
                    turn <- EndTurnFunc() //enemy turn is no longer a state
                    //console.log("current state is : " + turn.ToString())
                    Some(turn,player,enemies))
                | PlayCard(card), PlayerTurn -> (
                    console.log("player play card")
                    let state, p, e = PlayCardEvent card player enemies
                    turn <- state
                    player <- p
                    enemies <- e
                    console.log("enemy status:")
                    console.log(enemies)
                    Some(turn,player,enemies))
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
                None)
        )
    end
module GameplayLoop
open GameTypes



let chooseCard (state : GameplayStates) : option<int> =
    match state with
    | PlayerActions(Await) -> Some(0)
    | _ -> None


type GameplayStateMachine() =
    class
        do ()
        member val currentState : GameplayStates = Init with get
        member val playerHealth : int = 0
        member val playerMagic : int = 0
        member val enemyHealth : int = 0
        //Returns the updated state
        ///
        member this.update (playerinput : option<PlayerAction>) : GameplayStates = (
            match playerinput with
            | Some(action) -> (
                match action with
                    | ChooseCard -> Init //call functions here
                    | EndTurn -> Init
                )
            | None -> this.currentState //do nothing on undefined behaviour
        )
    end
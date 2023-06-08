module GameplayLoop
open GameTypes
open Browser.Dom
open Fable.Core


//let notYetImplemented() = eprintfn "not yet implemented"

let CardAction = None //support function maybe
let ValidatePlayerStatus player = None //check that player is alive
let ValidateEnemyStatus enemies = None //check that enemies are alive

let damageAllCharacters (l : NPC list) damage =
    l |> List.map (fun e ->
        {
        Id=e.Id
        ;Health=e.Health-damage
        ;Mana=e.Mana
        ;Location=e.Location
        ;Type=Enemy})



let seriousDamage = 30





let isIdenticalNPC n1 (n2 : NPC) =
    match n2.Location with
    | l when (l=n1.Location) -> true
    | _ -> false

let damageEnemy enemy enemies damage : Enemies =
    enemies |> List.map(fun e -> (
        match (isIdenticalNPC e enemy) with
        | true -> {Id=e.Id;Health=e.Health - damage;Mana=e.Mana;Location=e.Location;Type=e.Type}
        | false -> e
    ))


let PlayCardEvent card (target : option<NPC>) player1 (enemies : Enemies) =
    //cards played may also affect the player
    let playerUpdate =
        {Id=player1.Id
        ;Health=(player1.Health)
        ;Mana=player1.Mana
        ;Location=player1.Location
        ;Type=Enemy} //to be fixed
    //cards may affect one enemy or several, this needs to be fixed
    let (eupdate : Enemies) =
        match target with
        | Some(npc) -> (damageEnemy npc enemies 30)
        | _ -> damageAllCharacters enemies seriousDamage //notyetimplemented
    PlayerTurn, playerUpdate, eupdate


let EnemyAction (player1 : Player) (enemies:Enemies) : GameplayStates*Player*Enemies =
    ValidateEnemyStatus enemies |> ignore
    let playerUpdate =
        {Id=player1.Id
        ;Health=(player1.Health - 10)
        ;Mana=player1.Mana
        ;Location=player1.Location
        ;Type=Player}
    let enemyUpdate = enemies //notyetimplemented
    //should add enemies as return value as well since they can hurt themselves
    PlayerTurn, playerUpdate, enemyUpdate


let isAlive n =
    match n.Health with
        | h when h <= 1 -> false
        | _ -> true

let areAlive (l : NPC List) =
    l |> List.forall (fun e -> isAlive e)

let validateGameStatus p e =
    let playerstatus = isAlive p
    let enemystatus = areAlive e
    match playerstatus, enemystatus with
    | false, _ -> GameOver
    | true, false -> Win
    | _ -> Running

//the state machine should not govern the playerinput, but simply respond to it
type BattleStateMachine(playerName) =
    class
        do ()
        let mutable turn : GameplayStates = PlayerTurn //default to player
        let mutable player : Player = {Id=playerName;Health=100;Mana=250;Location=Some(Position1);Type=Player}
        let mutable enemies : Enemies = [] //initialize to empty list
        //fix this later
        let mutable gameStatus : GameStatus = Running

            

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

        let validateInput playerinput =
            match (playerinput, turn, isAlive player, areAlive enemies) with
                | _,_,true, false ->
                    gameStatus <- Win
                    Some(gameStatus, turn, player, enemies)
                | _,_,false, _ ->
                    gameStatus <- GameOver
                    Some(gameStatus, turn, player, enemies)
                //| _, Init -> (currentState <- PlayerTurn) //might call a function, or remove this later
                | EndTurnRelease, PlayerTurn, true, true ->
                    console.log("player end turn")
                    turn <- EndTurnFunc() //enemy turn is no longer a state
                    //console.log("current state is : " + turn.ToString())
                    gameStatus <- validateGameStatus player enemies
                    Some(gameStatus, turn,player,enemies)
                | PlayCard(card, target), PlayerTurn, true, true ->
                    console.log("player play card")
                    let state, p, e = PlayCardEvent card target  player enemies
                    turn <- state
                    player <- p
                    enemies <- e
                    console.log("enemy status:")
                    console.log(enemies)
                    gameStatus <- validateGameStatus player enemies
                    Some(gameStatus, turn,player,enemies)
                (*| _, EnemyTurn -> *)
                | Idle, PlayerTurn, true, true ->
                    None
                | EndTurnPress, PlayerTurn, true, true ->
                    None
        //this might need to be an asynchronous function, since it has to wait for animations
        member this.initialize(config : option<int>) : GameData =
            //need a proper function for spawning enemies

            //the number of enemies has to be static

            let createEnemies: NPC list = [
                for i in 1 .. 3 ->
                    let e = {Id="Monster" + string(i);Health=100;Mana=250;Location=Some(Position2);Type=Enemy}
                    match i with
                    | 1 -> {Id=e.Id;Health=e.Health;Mana=e.Mana;Location=Some(Position3);Type=Enemy}
                    | 2 -> {Id=e.Id;Health=e.Health;Mana=e.Mana;Location=Some(Position4);Type=Enemy}
                    | _ -> e
                ]
            enemies <- createEnemies
            Some(gameStatus, turn, player, enemies)
        member this.update (playerinput : option<PlayerInput>) : GameData  =

            match playerinput, gameStatus with
            | Some input,Running ->
                validateInput input
            | _, GameOver ->
                Some(gameStatus, turn, player, enemies)
            | _, Win ->
                Some(gameStatus, turn, player, enemies)
            | i, s ->
                eprintfn "error in state machine! Input: %A GameStatus: %A" i s
                None
    end
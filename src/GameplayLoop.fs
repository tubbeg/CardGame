module GameplayLoop
open GameTypes
open Browser.Dom
open Fable.Core


//let notYetImplemented() = eprintfn "not yet implemented"

let errorNPC: NPC =
    {Id="Error"
    ;Health=(-1)
    ;Mana=(-1)
    ;Location=None
    ;Type=Player
    ;Passives=None
    ;Life=Dead}

let areDead (l : NPC List) =
    match l.Length with
    | n when n>1 ->
        l |> List.forall (fun e -> 
            match e.Life with
            | Dead -> true
            | _ -> false)
    | _ -> true

let damageAllCharacters (l : NPC list) damage =
    l |> List.map (fun e ->
        let newhealth = e.Health-damage
        let newlife =
            match newhealth with
            | n when n<1 -> Dead
            | _ -> Alive
        {
        Id=e.Id
        ;Health=newhealth
        ;Mana=e.Mana
        ;Location=e.Location
        ;Type=Enemy
        ;Passives=e.Passives
        ;Life=newlife})



let seriousDamage = 30


let getEnemy (position : Position) (elist : NPC list) =
    try
        let npc =
            elist |>
            List.find(fun n ->
                match n.Location with
                | Some(loc) when (loc=position) -> true
                | _ -> false
            )
        Some(npc)
    with
        ex ->
            eprintfn "failure in getting position! %A" ex
            None


let isIdenticalNPC n1 (n2 : NPC) =
    match n2.Location with
    | l when (l=n1.Location) -> true
    | _ -> false

let damageEnemy (enemy : option<NPC>) enemies damage : option<Enemies> =
    match enemy with
    | None ->
        console.log("Cannot damage enemy! Missing data!")
        None
    | Some(en) ->
        let elistUpdate : NPC list =
            enemies |> List.map(fun e -> (
                match (isIdenticalNPC e en) with
                | true ->
                    let newhealth = e.Health - damage
                    let life =
                        match newhealth with
                        | n when n<1 -> Dead
                        | _ -> Alive
                    {Id=e.Id
                    ;Health=newhealth
                    ;Mana=e.Mana
                    ;Location=e.Location
                    ;Type=e.Type
                    ;Passives=e.Passives
                    ;Life=life}
                | _ -> e
            ))
        Some(elistUpdate)


let determineCardEffect (card) : Mana*Health*Effect =
    match card with
    | Shoot(None, Effect1) -> 10, (-30), Effect1
    | _ -> (-30), (-10), Effect2

let updatePlayer player card =
    let mana,health,effect = determineCardEffect card
    let newhealth = player.Health - health
    let life =
        match newhealth with
        | n when n<1 -> Dead
        | _ -> Alive
    Some {Id=player.Id
        ;Health=newhealth
        ;Mana=player.Mana - mana
        ;Location=player.Location
        ;Type=Enemy
        ;Passives=player.Passives
        ;Life=life}

let updateEnemies enemies target card =
    let mana,health,effect = determineCardEffect card
    match target with
    | Some(pos) ->
        match pos with
        | p when (p=AllCharacters) -> //damaging all characters or just enemies should depend on the card
            console.log("Reminder to fix all damage")
            Some(damageAllCharacters enemies seriousDamage)
        | p when (p=AllCharacters) ->
            console.log("Error! Not Yet Implemented!")
            None
        | p ->
            let n = getEnemy p enemies
            (damageEnemy n enemies 30)
    | _ ->
        console.log("Error! No valid target!")
        None
    

let updateCharacters card player enemies target =
    let playerUpdate = updatePlayer player card //to be fixed
    //cards may affect one enemy or several, this needs to be fixed
    let enemiesUpdate = updateEnemies  enemies target card
            //Should return error on None
        //Added an AllEnemies Position, and an AllCharacters position
    match enemiesUpdate, playerUpdate with
        | Some(eupdate), Some(pupdate) ->
            Some(PlayerTurn, pupdate, eupdate)
        | _ ->
            console.log("Error in retrieving character updates! Aborting!")
            None


let PlayCardEvent card target player enemies =
    //cards played may also affect the player
    match card with
    | Some(c) -> updateCharacters c player enemies target
    | _ ->
        console.log("No valid card! Aborting!")
        None


let enemyActionUpdateEnemies enemies =
    console.log("Need a function for updating enemies during enemy action.
        Reminder to fix this function.")
    enemies

let EnemyAction (player1) (enemies:Enemies) effect : GameplayStates*Player*Enemies =
    let damage, sideeffect = effect
    let newhealth = player1.Health - damage
    let life =
        match newhealth with
        | n when n<1 -> Dead
        | _ -> Alive
    let playerUpdate =
        {Id=player1.Id
        ;Health=newhealth
        ;Mana=player1.Mana
        ;Location=player1.Location
        ;Type=Player
        ;Passives=player1.Passives
        ;Life=life}
    let enemyUpdate = enemyActionUpdateEnemies enemies //notyetimplemented
    //should add enemies as return value as well since they can hurt themselves
    PlayerTurn, playerUpdate, enemyUpdate



let validateGameStatus p e =
    match p.Health, (areDead e) with
    | n, _ when n<1 -> Some(GameOver)
    | n, true -> Some(Win)
    | _ -> Some(Running)

//the state machine should not govern the playerinput, but simply respond to it
type BattleStateMachine(playername) =
    class
        do ()
        let mutable turn : GameplayStates = PlayerTurn //default to player
        let mutable player =
            {Id=playername
            ;Health=0
            ;Mana=0
            ;Location=None
            ;Type=Player
            ;Passives=None
            ;Life=Dead}
        let mutable enemies : Enemies = [] //initialize to empty list
        //fix this later
        let mutable gameStatus : option<GameStatus> = None

        let mutable deck : option<Deck> = None

        let enemyTurn() =
            console.log("enemy turn")
            console.log("enemy does something!")
            let state, p, e =
                EnemyAction player enemies (30, None)
            turn <- state
            player <- p
            enemies <- e
            console.log(player)
                //console.log("current state is : " + turn.ToString())
            turn
        let EndTurnFunc() = 
        //notYetImplemented()
            enemyTurn()

        let defaultPlayer name = 
            {Id=name
            ;Health=100;Mana=250
            ;Location=Some(PlayerPosition)
            ;Type=Player
            ;Passives=None
            ;Life=Alive}

        let gunAssasinDeck: Deck =
            let bomb = [for i in 0 .. 2 -> Bomb(Some(AllCharacters),ExplosionEffect(30))]
            [for i in 0 .. 5 -> Shoot(Some(AllCharacters), NormalDamage(15))]
            |> List.append bomb

        let validateInput playerinput =
            match (playerinput, turn, player.Life, enemies |> areDead) with
                | _,_,Alive, true ->
                    gameStatus <- Some Win
                    Some(gameStatus, turn, player, enemies)
                | _,_,Dead, _ ->
                    gameStatus <- Some GameOver
                    Some(gameStatus, turn, player, enemies)
                //| _, Init -> (currentState <- PlayerTurn) //might call a function, or remove this later
                | EndTurnRelease, PlayerTurn, Alive, false ->
                    console.log("player end turn")
                    turn <- EndTurnFunc() //enemy turn is no longer a state
                    gameStatus <- validateGameStatus player enemies
                    Some(gameStatus, turn,player,enemies)
                | PlayCard(card, target), PlayerTurn, Alive, false ->
                    console.log("player play card")
                    let (result : option<GameplayStates*Player*Enemies>) =
                        PlayCardEvent card target  player enemies
                    match result with
                    | Some(state,p,e) ->
                        //let state, p, e = PlayCardEvent card target  player enemies
                        turn <- state
                        player <- p
                        enemies <- e
                        console.log("enemy status:")
                        console.log(enemies)
                        gameStatus <- validateGameStatus player enemies
                        Some(gameStatus, turn,player,enemies)
                    | _ ->
                        console.error("Error! Missing data in PlayCard input!")
                        None
            (*| _, EnemyTurn -> *)
                | Idle, PlayerTurn, Alive, false ->
                    None
                | EndTurnPress, PlayerTurn, Alive, false ->
                    None
        //this might need to be an asynchronous function, since it has to wait for animations
        member this.initialize(config : option<int>) : GameData =
            //need a proper function for spawning enemies
            //the number of enemies has to be static
            player <- defaultPlayer player.Id
            let createEnemy position identifier =
                {Id="Monster" + identifier
                ;Health=100;Mana=250
                ;Location=position
                ;Type=Enemy
                ;Passives=None
                ;Life=Alive}
            let defaultEnemy identifier = createEnemy (Some Position2) identifier
            let createEnemies: NPC list = [
                for i in 1 .. 3 ->
                    let id = i.ToString()
                    let e = defaultEnemy id
                    match i with
                    | 1 -> createEnemy (Some Position3) id
                    | 2 -> createEnemy (Some Position4) id
                    | _ -> e
                ]
            enemies <- createEnemies
            gameStatus <- Some(Running)
            Some(gameStatus, turn, player, enemies)
        member this.getClass() =
            console.log("Reminder to fix this classes")
            GunAssasin

        (*member this.identifyCards deck =
            console.log("Remove this function")
            deck |> List.iter(fun c ->
                match c with
                | Shoot(_) -> console.log("Detected shoot cards! :" + c.ToString())
                | Bomb(_) -> console.log("Detected bomb cards!")
                | _ -> console.log("Don't know what this is :/")
            )*)

        member this.generateDeck(playableClass : PlayableClasses) : option<Deck> =
            console.log("Reminder to fix this card generation")
            let newDeck =
                match deck with
                | Some(d) -> Some(d)
                | _ ->
                    match playableClass with
                    | GunAssasin ->
                        Some(gunAssasinDeck)
                    | SamuraiWarrior -> None
                    | MagicianTank -> None
            deck <- newDeck
            deck
        member this.update (playerinput : option<PlayerInput>) : GameData  =

            match playerinput, gameStatus with
            | Some input,Some(Running) ->
                validateInput input
            | _, Some(GameOver) ->
                Some(gameStatus, turn, player, enemies)
            | _, Some(Win) ->
                Some(gameStatus, turn, player, enemies)
            | i, s ->
                eprintfn "error in state machine! Input: %A GameStatus: %A" i s
                None
    end
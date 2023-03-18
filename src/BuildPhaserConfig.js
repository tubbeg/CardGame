import * as phaser from "phaser"


function buildConfig (myScene) {
    return {
        type: Phaser.AUTO,
        width: 800,
        height: 600,
        physics: {
            default: 'arcade',
            arcade: {
                gravity: { y: 300 },
                debug: false
            }
        },
        scene: myScene
    }
}


export {buildConfig};
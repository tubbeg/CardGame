import * as Phaser from "phaser";

class SceneWrapper extends Phaser.Scene{
    constructor(title) {
        super(title);
    }

    loadImages(loadArray){
        loadArray.forEach(
            id,path => this.load.image(id, path)
        );
    }

    getLoader(){
        return this.load;
    }

    getAdd(){
        return this.add;
    }

    getPhysics(){
        return this.physics;
    }
}

export {SceneWrapper};
import * as Phaser from "phaser";


class Stuff{
    constructor(){
        this.five = 100;
    }
    getFive(){return this.five}
}

class DummyClass {
    constructor(){
        this.myStuff = new Stuff();
    }
    getDummyFive () {
        return this.myStuff.getFive();
    }
}

class SceneWrapper extends Phaser.Scene{
    constructor(title) {
        super(title);
    }
    /*preload() {
        console.log("running preload");
        this.load.setBaseURL('http://labs.phaser.io');

        this.load.image('sky', 'assets/skies/space3.png');
        this.load.image('logo', 'assets/sprites/phaser3-logo.png');
        this.load.image('red', 'assets/particles/red.png');
        //this.myPreload(); uncomment this to get callbacks
    }
    create() {
        console.log("running create");
        this.add.image(400, 300, 'sky');

        var particles = this.add.particles('red');

        var emitter = particles.createEmitter({
            speed: 100,
            scale: { start: 1, end: 0 },
            blendMode: 'ADD'
        });

        var logo = this.physics.add.image(400, 100, 'logo');

        logo.setVelocity(100, 200);
        logo.setBounce(1, 1);
        logo.setCollideWorldBounds(true);

        emitter.startFollow(logo);
        //this.myCreate(); uncomment this to get callbacks
    }*/

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

export {SceneWrapper, Stuff, DummyClass};
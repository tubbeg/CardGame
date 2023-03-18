import * as Phaser from "phaser";


class LoaderWrapper extends Phaser.Loader.LoaderPlugin{

}

class SceneWrapper extends Phaser.Scene{
    constructor(title) {
        super(title);
    }

    loadImages(loadArray){
        loadArray.forEach(
            el => this.load.image(el[0], el[1])
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

    getNewEmitter(conf){
        return this.getPhysics().getEmitter(conf);
    }
    
    setBaseUrl(url){
        return this.load.setBaseURL(url);
    }

    setAddImage(x,y,texture){
        this.add.image(x,y,texture);
    }

    addParticles(texture){
        return this.add.particles(texture);
    }

    createEmitter(particles, configuration){
        return particles.createEmitter(configuration)
    }

    preload(){}
    create ()
    {
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
    }
}

export {SceneWrapper, LoaderWrapper};
import { Union } from "./fable_modules/fable-library.4.0.0-theta-018/Types.js";
import { class_type, union_type } from "./fable_modules/fable-library.4.0.0-theta-018/Reflection.js";
import { some } from "./fable_modules/fable-library.4.0.0-theta-018/Option.js";
import { SceneWrapper } from "./PhaserSceneWrapper.js";

export class IPhaserRender extends Union {
    constructor(tag, fields) {
        super();
        this.tag = tag;
        this.fields = fields;
    }
    cases() {
        return ["Auto", "Canvas", "WebGL"];
    }
}

export function IPhaserRender$reflection() {
    return union_type("PhaserUtility.IPhaserRender", [], IPhaserRender, () => [[], [], []]);
}

export const myFirstJsObj = {
    start: 1,
    end: 0,
};

export const myJsObj = {
    speed: 100,
    scale: myFirstJsObj,
    blendMode: "ADD",
};

export class SceneExt extends SceneWrapper {
    constructor() {
        super();
    }
    preload() {
        const this$ = this;
        console.log(some("running preload in f#"));
        const arr = [["sky", "assets/skies/space3.png"], ["logo", "assets/sprites/phaser3-logo.png"], ["red", "assets/particles/red.png"]];
        this$.load.setBaseURL("http://labs.phaser.io");
        const objectArg = this$.load;
        objectArg.image("sky", "assets/skies/space3.png");
        const objectArg_1 = this$.load;
        objectArg_1.image("logo", "assets/sprites/phaser3-logo.png");
        const objectArg_2 = this$.load;
        return objectArg_2.image("red", "assets/particles/red.png");
    }
    create() {
        const this$ = this;
        const objectArg = this$.add;
        objectArg.image(400, 300, "sky");
        const myParticles = this$.add.particles("red");
        const emitter = myParticles.createEmitter(myJsObj);
        let logo;
        const objectArg_1 = this$.physics.add;
        logo = objectArg_1.image(400, 100, "logo");
        logo.setVelocity(100, 200);
        logo.setBounce(1, 1);
        logo.setCollideWorldBounds(true);
        return emitter.startFollow(logo);
    }
}

export function SceneExt$reflection() {
    return class_type("PhaserUtility.SceneExt", void 0, SceneExt, class_type("PhaserUtility.Scene", void 0, SceneWrapper));
}

export function SceneExt_$ctor() {
    return new SceneExt();
}


import * as PIXI from 'pixi.js';
import { BitmapText } from 'pixi.js';


let app = new PIXI.Application({width: window.innerWidth, height: window.innerHeight, antialias: true ,transparent: true});
document.body.appendChild(app.view);

// Create the sprite and add it to the stage
let background = PIXI.Sprite.from("./assets/background.jpg")
let sprite = PIXI.Sprite.from('./assets/sample.png');
let sprite1 = PIXI.Sprite.from('./assets/sprite1.png');
let sprite2 = PIXI.Sprite.from('./assets/sprite2.png');
app.stage.addChild(background);
background.addChild(sprite1);
background.addChild(sprite2);
background.addChild(sprite);


sprite1.anchor.set(0, 0);
sprite2.anchor.set(1, 1);


/*
let frame = new PIXI.Graphics();
frame.beginFill(0x666666);
frame.lineStyle({ color: 0xffffff, width: 4, alignment: 0 });
frame.drawRect(0, 0, 208, 208);
frame.position.set(320 - 104, 180 - 104);
sprite.addChild(frame);
*/
// Add a ticker callback to move the sprite back and forth
let elapsed = 0.0;

function startAnimation(){
    if (true){
        app.ticker.add((delta) => {
            elapsed += delta;
            sprite.x = 100.0 + Math.cos(elapsed/50.0) * 100.0;
        });
    }
}

/*
PIXI.Assets.load('./assets/desyrel.xml').then(() => {
    const bitmapFontText = new PIXI.BitmapText(
        'bitmap fonts are supported!\nWoo yay!', {
            fontName: 'Desyrel',
            fontSize: 100,
            align: 'left',
        },
    );

    bitmapFontText.x = 50;
    bitmapFontText.y = 200;

    background.addChild(bitmapFontText);
});
*/

export {startAnimation as startAnim}
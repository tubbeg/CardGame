import { Union } from "./fable_modules/fable-library.4.0.2/Types.js";
import { union_type } from "./fable_modules/fable-library.4.0.2/Reflection.js";

export class IPhaserRender extends Union {
    "constructor"(tag, fields) {
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


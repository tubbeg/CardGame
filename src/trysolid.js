


import { StepLabel } from "@mui/material";
import { createSignal, onCleanup } from "solid-js";


export const CountingComponent = () => {
	const [count, setCount] = createSignal(0);
	const interval = setInterval(
		() => setCount(c => c + 1),
		1000
	);
	onCleanup(() => clearInterval(interval));
	return     (<div class="rpgui-content ">
    <div class="rpgui-container framed rpgui-draggable rpgui-cursor-point"
    style="height:550px; width:320px; top: 50px; left: 50px;">
        <div>Count value is {count()}</div>
    </div>
</div>);
};

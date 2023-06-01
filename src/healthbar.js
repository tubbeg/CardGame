import { createSignal } from "solid-js";


export const [count, setCount] = createSignal(100);

export const CountingComponent = (title) => {
	//this component can be reused for both player and enemy :)
	return     (
	<section class="nes-container with-title">
		<h3 class="title">{title}</h3>
		<div id="progress" class="item">
			<progress style="width:500px"class="nes-progress is-error" value={count()} max="100"></progress>
		</div>
	</section>);
};


<script>
	import '../app.postcss';
	import {browser} from '$app/environment'

	export const ssr = false;

	export let data;

	const {login} = data;

	if (browser) {
		// (не спрашивайте зачем)
		const searchParams = new URLSearchParams(location.search)
		searchParams.delete('code')
		const url = new URL(location.href)
		url.search = searchParams.toString()
		history.replaceState(null, '', url)
	}

	console.log(login)
</script>

<div class="">
	<div class="container min-h-screen mx-auto flex flex-col items-stretch bg-gray-200">
		<header class="m-5 p-2 rounded-lg shadow-xl bg-gray-400">
			<h1 class="text-4xl font-medium text-red-700">Kamod news</h1>
		</header>
		<main class="grow">
			<slot />
		</main>
		<footer class="m-5 p-2 rounded-lg shadow-xl bg-gray-400">
			<p>This a footer</p>
		</footer>
	</div>
</div>
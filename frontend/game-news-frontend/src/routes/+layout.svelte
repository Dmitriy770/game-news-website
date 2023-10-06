<script>
	import '../app.postcss';
	import UserProfile from './UserProfile.svelte';
</script>

<div class="">
	<div class="container min-h-screen mx-auto flex flex-col items-stretch bg-gray-200">
		<header class="m-5 p-2 rounded-lg shadow-xl bg-gray-400 flex flex-row justify-between items-center">
			<h1 class="text-4xl font-medium text-red-700">Kamod news</h1>

            <UserProfile/>
		</header>
		<main class="grow">
			<slot />
		</main>
		<footer class="m-5 p-2 rounded-lg shadow-xl bg-gray-400">
			<p>This a footer</p>
		</footer>
	</div>
</div>

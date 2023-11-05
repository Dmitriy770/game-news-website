<script>
	import { browser } from '$app/environment';
	import {saveToken, getToken} from '$lib/store/store';

	async function login() {
		try {
			if (!browser) {
				throw new Error('render not in browser');
			}

			let urlSeatchParams = new URLSearchParams(location.search);
			let code = urlSeatchParams.get('code');
			urlSeatchParams.delete('code');
			const url = new URL(location.href);
			url.search = urlSeatchParams.toString();
			history.replaceState(null, '', url);

			if (code !== null) {

				var response = await fetch(`http://localhost:8080/api/auth/token?code=${code}`);
				if (!response.ok) {
					throw new Error(await response.text());
				}
				var token = await response.json();

				saveToken(token);
			}

			var token = getToken();
			if(token.accessToken === null){
				throw new Error("token not found")
			}

			response = await fetch(`http://localhost:8080/api/auth/users/me`, {
				headers: {
					Authorization: `Bearer ${token.accessToken}`
				}
			});
			if (!response.ok) {
				throw new Error(await response.text());
			}
			var user = await response.json();
			
			return user;
		} catch (error) {
			console.log({ error });
			return Promise.reject(error);
		}
	}

	const userProfile = login();
</script>

<div class="flex flex-row justify-end items-center h-10 w-80">
	{#await userProfile}
		<div class="w-full h-full animate-pulse flex flex-row justify-end items-center">
			<svg
				class="w-9 h-9 text-gray-200 me-2"
				aria-hidden="true"
				xmlns="http://www.w3.org/2000/svg"
				fill="currentColor"
				viewBox="0 0 20 20"
			>
				<path
					d="M10 0a10 10 0 1 0 10 10A10.011 10.011 0 0 0 10 0Zm0 5a3 3 0 1 1 0 6 3 3 0 0 1 0-6Zm0 13a8.949 8.949 0 0 1-4.951-1.488A3.987 3.987 0 0 1 9 13h2a3.987 3.987 0 0 1 3.951 3.512A8.949 8.949 0 0 1 10 18Z"
				/>
			</svg>
			<div class="animate-pulse h-5 bg-gray-200 rounded-full w-40 me-5" />
			<div class="animate-pulse h-5 bg-gray-200 rounded-full w-20" />
		</div>
	{:then user}
		<img src={user.avatarUrl} alt="автарка" class="object-contain h-9 me-2" />
		<p class="text-white text-lg me-5">{user.globalName}</p>
		<button class="rounded-lg w-30 px-6 py-1 bg-red-600 text-white text-lg">Выйти</button>
	{:catch error}
		<a
			class="block rounded-lg w-30 px-6 py-1 bg-indigo-500 hover:bg-indigo-600 active:bg-indigo-700 text-white text-lg"
			href="http://localhost:8080/api/auth/login"
		>
			Войти
		</a>
	{/await}
</div>

<script>
	import DescCard from "./DescCard.svelte";

    export let data;

    const {descriptions} = data;

    async function handleClick()
    {
        var res = await fetch('http://localhost:5242/v1/oauth/login');
        сonsole.log(res);
    }
</script>

<div>
    <button on:click={handleClick}>Это кнопка</button>
    <a href="http://localhost:5242/v1/oauth/login">Это ссылка</a>
</div>
<div class="grid grid-cols-2 gap-5 mx-5">
    {#each descriptions as description (description.id)}
        <DescCard {...description}/>
    {/each}
</div>

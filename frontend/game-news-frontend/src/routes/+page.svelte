<script>
	import DescCard from "./DescCard.svelte";

    export let data;

    const {descriptions} = data;
</script>

<div>
    <a href="http://localhost:8080/api/v1/oauth2/authorize">Это ссылка</a>
    <a href="/test">Ссылка на страницу тест</a>
</div>
<div class="grid grid-cols-2 gap-5 mx-5">
    {#each descriptions as description (description.id)}
        <DescCard {...description}/>
    {/each}
</div>

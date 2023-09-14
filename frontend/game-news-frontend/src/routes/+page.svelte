<script>
	import { onMount } from "svelte";
	import DescriptionCard from "./descriptionCard.svelte";

    let descriptions = []

    onMount(async () => {
        const res = await fetch(`http://localhost:5123/v1/article/descriptions?take=10&skip=0`);
        descriptions = (await res.json()).descriptions
    });
</script>

<div>
    <header class="header">
        KAMOD GAMES
    </header>
    <main>
        {#each descriptions as description (description.id)}
            <DescriptionCard description={description}></DescriptionCard>
        {/each}
    </main>
    <footer></footer>
</div>

<style>
    .header{
        border: 2px solid red;
        border-radius: 5px;
        text-align: center;
    }
</style>
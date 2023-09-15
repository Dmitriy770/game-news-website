<script>
	import DescCard from "./DescCard.svelte";

    export let data;

    const {descriptions} = data;
</script>

<div class="grid grid-cols-2 gap-5 mx-5">
    {#each descriptions as description (description.id)}
        <DescCard {...description}/>
    {/each}
</div>

<script>
    import { Img } from 'flowbite-svelte';
    import { Hr } from 'flowbite-svelte';

    export let description;
</script>

<div>
    <h1>{description.title}</h1>
    <Img src="data:image/png;base64,{description.image}" size="max-w-xs"/>
    <span>{description.publicationDate}</span>
    <Hr/>
</div>
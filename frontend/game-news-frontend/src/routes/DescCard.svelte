<script>
    export const id = 0
    export let title = "";
    export let publicationDate = "";
    export let image = "";

</script>

<div class="relative h-96 p-2 rounded-lg shadow-xl flex flex-col items-center background-image bg-gray-300" style="--image-url: url(data:image/jpeg;base64,{image})">
    <h1 class="bg-red-500 p-1 rounded-lg">{title}</h1>
    <span class="bg-red-500 p-1 rounded-lg absolute bottom-0 right-0 m-4">{publicationDate}</span>
</div>

<style>
    .background-image{
        background-image: var(--image-url);
        background-repeat: no-repeat;
        background-position: center;
        background-size: cover;
    }
</style>
export const load = async() => {
    const fetchDescriptions = async () => {
        // const res = await fetch('http://localhost:8080/api/v1/article/descriptions?take=10&skip=0');
        // const data = await res.json();
        return data.descriptions;
    }

    return {
        descriptions: fetchDescriptions(),
    }
}
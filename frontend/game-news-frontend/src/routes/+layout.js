import { goto } from '$app/navigation';

export const load = async() => {
    const login = async () => {
        var urlSeatchParams = new URLSearchParams(location.search);
        var code = urlSeatchParams.get('code')
        
        if(code){
            urlSeatchParams.delete('code');
            var url = location.pathname + urlSeatchParams.toString();
            console.log(url)
            await goto(`${url}`, {replaceState: true})

            const res = await fetch(`http://localhost:8080/api/v1/oauth2/token?code=${code}`);
            const data = await res.json();
            console.log(data)
        }
    }

    return {
        login: login(),
    }
}
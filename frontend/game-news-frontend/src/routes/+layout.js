export const load = async() => {
    const login = async () => {
        let urlSeatchParams = new URLSearchParams(location.search);
        let code = urlSeatchParams.get('code')

        console.log({code})
        
        if(code){
            const res = await fetch(`http://localhost:8080/api/v1/oauth2/token?code=${code}`);
            const data = await res.json();
            
            return data
        }

        return null
    }

    return {
        login: login(),
    }
}
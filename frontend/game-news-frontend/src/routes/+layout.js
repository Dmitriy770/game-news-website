export const load = async({params}) => {
    const login = async () => {
        console.log("code: " + params.code)
    }

    return {
        login: login(),
    }
}
export const saveToken = function(token){
    localStorage.setItem('accessToken', token.accessToken);
    localStorage.setItem('expiresIn', token.expiresIn);
    localStorage.setItem('refreshToken', token.refreshToken);
}

export const deleteToken = function(){
    localStorage.removeItem('accessToken');
    localStorage.removeItem('expiresIn');
    localStorage.removeItem('refreshToken');
}

export const getToken = function(){
    return {
        accessToken: localStorage.getItem('accessToken'),
        expiresIn: localStorage.getItem('expiresIn'),
        refreshToken: localStorage.getItem('refreshToken')
    }
}
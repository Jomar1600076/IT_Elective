import { authHeader } from '../services/auth_header';

const apiUrl = 'http://localhost:3000'
let userToken = JSON.parse(localStorage.getItem('user'));

export const userServices = {
    getAll,
    getConvo,
    login,
    logout,
    register,
    sendMessages,
    getMessages,
    getOtherUser
}

function login(user) {
    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(user)
    };

    return fetch(`${apiUrl}/user/authenticate`, requestOptions)
        .then(handleResponse)
        .then(user => {
            // login successful if there's a jwt token in the response
            if (user.token) {
                // store user details and jwt token in local storage to keep user logged in between page refreshes
                localStorage.setItem('user', JSON.stringify(user));
            }
            return user;
        });
}

function logout() {
    // remove user from local storage to log user out
    localStorage.removeItem('user');
}

function register(user) {
    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(user)
    };
    return fetch(`${apiUrl}/user/register`, requestOptions).then(handleResponse);
}

function getAll() {
    const requestOptions = {
        method: 'GET',
        headers: authHeader()
    };
    return fetch(`${apiUrl}/message/chatlist`, requestOptions).then(handleResponse);
}

function getConvo() {
    const requestOptions = {
        method: 'GET',
        headers: authHeader()
    };
    return fetch(`${apiUrl}/message/getConversation`, requestOptions).then(handleResponse);
}

function sendMessages(message, reciever, sender){
    const requestOptions= {
        method: 'POST',
        headers: { 
            Authorization : 'Bearer '+ userToken.token,
            'Content-Type': 'application/json'
            },
        body: JSON.stringify({message, reciever, sender})
    };
    return fetch(`${apiUrl}/message/addmessage`, requestOptions).then(handleResponse);
}

function getMessages(otherUserId) {
    const requestOptions = {
        method: "GET",
        headers: authHeader(),
    };
    return fetch(`${apiUrl}/message/getmessage/${otherUserId}`, requestOptions).then(handleResponse);
}

function getOtherUser(otherUserId){
    const requestOptions = {
        method: "GET",
        headers: authHeader(),
    };
    return fetch(`${apiUrl}/message/getotheruser/${otherUserId}`, requestOptions).then(handleResponse);
}

function handleResponse(response) {
    return response.text().then(text => {
        const data = text && JSON.parse(text);
        if (!response.ok) {
            if (response.status === 401) {
                // auto logout if 401 response returned from api
                logout();
                location.reload(true);
            }
            const error = (data && data.message) || response.statusText;
            return Promise.reject(error);
        }

        return data;
    });
}
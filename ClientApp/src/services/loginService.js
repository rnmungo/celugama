import axios from 'axios';
import config from '../configs/config.json';

const newClient = () => {
    return axios.create({
        baseURL: config.baseURL,
        timeout: config.timeout,
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        }
    });
}

const newSecureClient = () => {
    return axios.create({
        baseURL: config.baseURL,
        timeout: config.timeout,
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json',
            'Authorization': `Bearer ${localStorage.getItem('celugama-token')}`,
        }
    });
}

export const tryLogin = async (username, password) => {
    const restclient = newClient();
    return await restclient.post('/login', { username, password })
        .then(response => {
            if (response.status === 200) {
                return response.data;
            } else {
                throw new Error('Usuario o contraseña incorrectos');
            }
        }).catch(e => { throw new Error('Usuario o contraseña incorrectos'); });
}

export const changePassword = async (password) => {
    const secureclient = newSecureClient();
    return await secureclient.put(`/login/${localStorage.getItem('celugama-username')}`, { password })
        .then(response => {
            if (response.status === 204) {
                return;
            } else {
                throw new Error('No fue posible cambiar la contraseña, intente nuevamente más tarde.');
            }
        }).catch((e) => { throw new Error('No fue posible cambiar la contraseña, intente nuevamente más tarde.'); });
}
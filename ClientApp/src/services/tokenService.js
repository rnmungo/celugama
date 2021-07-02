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

export const postToken = async token => {
    const restclient = newClient();
    return await restclient.post('/tokens', { ...token });
}

import { saveAs } from 'file-saver';
import { objectToQueryString } from '../functions';
import axios from 'axios';
import config from '../configs/config.json';

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

export const getOrder = async (orderId) => {
    const secureclient = newSecureClient();
    return await secureclient.get(`/orders/${orderId}`)
        .then(response => {
            if (response.status === 200) {
                return response.data;
            } else {
                return null;
            }
        }).catch(e => { throw new Error('Ocurrió un error al buscar los artículos'); });
};
    

export const searchOrders = async filters => {
    const secureclient = newSecureClient();
    return await secureclient.get(`/orders${objectToQueryString(filters)}`)
        .then(response => {
            if (response.status === 200) {
                return response.data;
            } else if (response.status === 401) {
                throw new Error('No está autorizado a visualizar los pedidos');
            } else {
                return null;
            }
        }).catch(e => {
            throw new Error('Ocurrió un error al buscar los pedidos');
        });
};

export const downloadOrders = async filters => {
    const secureclient = newSecureClient();
    return await secureclient.get(`/documents/orders${objectToQueryString(filters)}`, { responseType: 'arraybuffer' })
        .then(response => {
            if (response.status === 200) {
                const filename = response.headers['content-disposition']
                    .split(';')
                    .find((n) => n.includes('filename='))
                    .replace('filename=', '')
                    .trim();
                const url = window.URL.createObjectURL(new Blob([response.data], { type: response.headers['content-type'] }));
                saveAs(url, filename);
                return true;
            } else {
                return null;
            }
        }).catch((e) => {
            throw new Error('El Excel no pudo ser descargado');
        });
}

export const getLabels = async shippingIds => {
    const secureclient = newSecureClient();
    return await secureclient.get(`/shipments/${shippingIds.join(',')}/labels`, { responseType: 'arraybuffer' })
        .then(response => {
            if (response.status === 200) {
                const filename = response.headers['content-disposition'].split(';').find((n) => n.includes('filename=')).replace('filename=', '').trim();
                const url = window.URL.createObjectURL(new Blob([response.data], { type: response.headers['content-type'] }));
                saveAs(url, filename);
                return true;
            } else {
                throw new Error('El estado del envío no permite esta acción');
            }
        }).catch((e) => { throw new Error('Las etiquetas no pudieron ser descargadas'); });
};
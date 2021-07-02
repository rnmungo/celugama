import React, { useEffect, useReducer } from 'react';
import TokenContext from './TokenContext';
import { useSnackbar } from 'notistack';
import { TokenReducer, initialState, SET_IS_AUTH } from './TokenReducer';
import { isAuth } from '../services/tokenService';

const GlobalState = props => {

    const [state, dispatch] = useReducer(TokenReducer, initialState);
    const { enqueueSnackbar } = useSnackbar();

    const sendNotification = (variant, message) => {
        enqueueSnackbar(message, { variant });
    }

    useEffect(() => {
        isAuth()
            .then(authenticated => setIsAuth(authenticated))
            .catch(e => {
                sendNotification('warning', 'Ya no se encuentra autenticado en MercadoLibre')
                setIsAuth(false);
            });
    }, []);

    const setIsAuth = isAuth => {
        dispatch({ type: SET_IS_AUTH, isAuth });
    }

    return (
        <TokenContext.Provider value={{
            state,
            setIsAuth
        }}>
            {props.children}
        </TokenContext.Provider>
    );

}

export default GlobalState;
import React from 'react';
import { Redirect, Route } from 'react-router-dom';

const AuthRoute = ({ component: Component, ...rest }) => {

    const token = localStorage.getItem('celugama-token');
    const expires = localStorage.getItem('celugama-expires');
    const utcNow = new Date();
    const expiresUtc = new Date(expires);
    const validToken = token && utcNow.getTime() < expiresUtc.getTime();

    if (!validToken) {
        localStorage.removeItem('celugama-token');
        localStorage.removeItem('celugama-username');
        localStorage.removeItem('celugama-expires');
    }

    return (
        <Route
            {...rest}
            render={props =>
                validToken ? (
                    <Component {...props} />
                ) : (
                    <Redirect to={{ pathname: '/login', state: { from: props.location } }} />
                )
            }
        />
    );
};

export default AuthRoute;
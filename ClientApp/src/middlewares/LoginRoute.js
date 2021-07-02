import React from 'react';
import { Redirect, Route } from 'react-router-dom';

const LoginRoute = ({ component: Component, ...rest }) => {

    const token = localStorage.getItem('celugama-token');

    return (
        <Route
            {...rest}
            render={props =>
                !token ? (
                    <Component {...props} />
                ) : (
                    <Redirect to={{ pathname: '/', state: { from: props.location } }} />
                )
            }
        />
    );
};

export default LoginRoute;
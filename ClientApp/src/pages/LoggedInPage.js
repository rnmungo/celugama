import React, { useState, useEffect } from 'react';

// Components
import Alert from '@material-ui/lab/Alert';
import AlertTitle from '@material-ui/lab/AlertTitle';
import Collapse from '@material-ui/core/Collapse';
import Grid from '@material-ui/core/Grid';

import Layout from '../components/Layout';
import CircularLoadingBackdrop from '../components/animations/loading/CircularLoadingBackdrop';

// Styles
import { makeStyles } from '@material-ui/core/styles';

// Services
import { postToken } from '../services/tokenService';

import queryString from 'query-string'

const useStyles = makeStyles(theme => ({
    grid: {
        textAlign: 'center'
    },
    collapse: {
        textAlign: 'left'
    },
    alert: {
        marginTop: theme.spacing(2),
        marginBottom: theme.spacing(2)
    },
    button: {
        marginLeft: theme.spacing(1),
        marginRight: theme.spacing(1)
    },
    loading: {
        marginBottom: theme.spacing(2)
    }
}));

const defaultInfo = {
    loading: false,
    type: 'info',
    message: '',
    title: '',
};

const LoggedInPage = props => {

    const [info, setInfo] = useState(defaultInfo);

    const classes = useStyles();

    const authenticate = async () => {

        const { code } = queryString.parse(props.location.search);

        if (!code) {
            props.history.push('/');
            return;
        }

        try {
            const response = await postToken({ code });
            if (response.status === 200) {
                setInfo({
                    ...info,
                    type: 'success',
                    title: 'Perfecto',
                    message: 'Se autenticó correctamente',
                    loading: false
                });
            }
            else {
                setInfo({
                    type: 'warning',
                    title: 'Ups...',
                    message: 'Ocurrio un error en la autenticación con MercadoLibre, corrobore si las credenciales son correctas',
                    loading: false
                });
            }
        } catch (e) {
            setInfo({
                type: 'error',
                title: 'Error interno',
                message: 'Ocurrio un error en la autenticación con MercadoLibre, corrobore si las credenciales son correctas',
                loading: false
            });
        }
    };

    useEffect(() => {
        setInfo({ ...info, loading: true });
        authenticate();
    }, []);

    return (
        <Layout {...props}>
            <Grid container justify="center">
                <Grid item xs={8} className={classes.grid}>
                    <CircularLoadingBackdrop show={info.loading} label="Finalizando proceso..." />
                    <Collapse in={info.message !== ""} className={classes.collapse}>
                        <Alert severity={info.type} className={classes.alert}>
                            <AlertTitle>{info.title}</AlertTitle>
                            {info.message}
                        </Alert>
                    </Collapse>
                </Grid>
            </Grid>
        </Layout>
    );
};

export default LoggedInPage;
import React from 'react';

// Components
import Button from '@material-ui/core/Button';
import Grid from '@material-ui/core/Grid';
import Typography from '@material-ui/core/Typography';

import Layout from '../components/Layout';

// Styles
import { makeStyles } from '@material-ui/core/styles';

// Configurations
import Config from '../configs/config.json';

const useStyles = makeStyles(theme => ({
    gridButton: {
        textAlign: 'center'
    },
    button: {
        marginTop: theme.spacing(1)
    },
    padding: {
        padding: theme.spacing(3)
    },
    img: {
        height: 'auto',
        width: 100,
        margin: theme.spacing(2)
    }
}));

const baseURL = "https://auth.mercadolibre.com.ar";
const URL = `${baseURL}/authorization?response_type=code&client_id=${Config.applicationID}`;

const MeliPage = props => {

    const classes = useStyles();

    const redirect = () => {
        window.location = URL;
    }

    return (
        <Layout {...props}>
            <Grid container justify="center" alignItems="center">
                <Grid item xs={12}>
                    <Typography variant="h3" align="center" gutterBottom>
                        Vamos a iniciar sesión en MercadoLibre
                    </Typography>
                </Grid>
                <Grid item xs={12}>
                    <Typography variant="subtitle1" align="center" gutterBottom>
                        Precioná el botón <b>INGRESAR</b> para comenzar con el proceso
                    </Typography>
                </Grid>
                <Grid item xs={12} className={classes.gridButton}>
                    <Button
                        variant="contained"
                        className={classes.button}
                        color="secondary"
                        size="large"
                        onClick={redirect}
                    >
                        INGRESAR
                    </Button>
                </Grid>
            </Grid>
        </Layout>
    );
}

export default MeliPage;
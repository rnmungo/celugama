import React from 'react';

// Components
import Grid from '@material-ui/core/Grid';
import Typography from '@material-ui/core/Typography';

import Layout from '../components/Layout';

const HomePage = props => {

    return (
        <Layout { ...props }>
            <Grid container justify="center">
                <Grid item xs={12}>
                    <Typography align="center" variant="h3" gutterBottom>
                        BIENVENIDO
                    </Typography>
                </Grid>
                <Grid item xs={12}>
                    <Typography align="center" variant="subtitle1" gutterBottom>
                        Comience a administrar sus pedidos de MercadoLibre
                    </Typography>
                </Grid>
                <Grid item xs={12}>
                    <Typography align="center" variant="subtitle1" gutterBottom>
                        Dirijase a la barra de navegación, haga clic en el menú e ingrese al Link de <b>Pedidos</b>
                    </Typography>
                </Grid>
            </Grid>
        </Layout>
    );
}

export default HomePage;
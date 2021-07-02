import React from 'react';

// Components
import Grid from '@material-ui/core/Grid';
import Typography from '@material-ui/core/Typography';
import Button from '@material-ui/core/Button';
import { Link } from 'react-router-dom';
import NotFoundRobot from '../components/animations/notfound/NotFoundRobot';

import Layout from '../components/Layout';

// Styles
import { makeStyles } from '@material-ui/core/styles';

const useStyles = makeStyles(theme => ({
    img: {
        height: 'auto',
        width: '100%',
        opacity: 0.7
    },
    typography: {
        marginTop: theme.spacing(1)
    },
    grid: {
        textAlign: 'center'
    }
}));

const NotFoundPage = props => {

    const classes = useStyles();

    return (
        <Layout {...props}>
            <Grid container justify="center" alignItems="center">
                <Grid item xs={6}>
                    <Grid container justify="center">
                        <Grid item xs={12} className={classes.grid}>
                            <NotFoundRobot />
                        </Grid>
                        <Grid item xs={12}>
                            <Typography
                                variant="subtitle1"
                                align="center"
                                className={classes.typography}
                                gutterBottom
                            >
                                La página no existe o no se encuentra habilitada
                            </Typography>
                        </Grid>
                        <Grid item xs={12} className={classes.grid}>
                            <Button
                                color="secondary"
                                component={Link}
                                to="/"
                            >
                                Volver al inicio
                            </Button>
                        </Grid>
                    </Grid>
                </Grid>
            </Grid>
        </Layout>
    );
}

export default NotFoundPage;
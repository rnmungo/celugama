import React from 'react';

// Components
import Grid from '@material-ui/core/Grid';
import Backdrop from '@material-ui/core/Backdrop';
import Typography from '@material-ui/core/Typography';
import CircularProgress from '@material-ui/core/CircularProgress';

// Styles
import { makeStyles } from '@material-ui/core/styles';

const useStyles = makeStyles(theme => ({
    backdrop: {
        zIndex: theme.zIndex.drawer + 1,
        color: '#fff',
        textAlign: 'center'
    }
}));

const CircularLoadingBackdrop = ({ show, label }) => {

    const classes = useStyles();

    return (
        <Backdrop className={classes.backdrop} open={show}>
            <Grid container justify="center" alignItems="center">
                <Grid item xs={12}>
                    <CircularProgress color="inherit" size={60} />
                </Grid>
                <Grid item xs={12}>
                    <Typography variant="subtitle1" align="center">{label}</Typography>
                </Grid>
            </Grid>
        </Backdrop>  
    );
}

export default CircularLoadingBackdrop;